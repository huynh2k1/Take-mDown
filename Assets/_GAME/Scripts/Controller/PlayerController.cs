using DG.Tweening;
using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    private Camera cam;
    private Animator animator;

    [SerializeField] RigBuilder rigBuilder;
    [SerializeField] MultiAimConstraint legUpLeftConstraint;
    [SerializeField] MultiAimConstraint bodyUpLeftConstraint;
    [SerializeField] MultiAimConstraint rightUpLeftConstraint;

    [SerializeField] Transform target;
    [SerializeField] Rigidbody[] _rbRagdoll;
    [SerializeField] Collider[] _colliders;

    [Header("Drag")]
    [SerializeField] float followSpeed = 15f;

    [Header("Roll")]
    const float CAM_ROTATE_LEFT = 0f;
    const float CAM_ROTATE_RIGHT = 0f;

    const float CAM_POSITION_LEFT = -0.5f;
    const float CAM_POSITION_RIGHT = 0.5f;

    const float POS_LEFT = -1.8f;
    const float POS_RIGHT = 1.5f;

    Vector2 swipeStartPos;
    bool isSwipeDetected = false;
    bool isSwipeHorizontal = false;

    Vector3 initTargetPos;
    bool isLeft = true;
    bool isRolling;
    bool isDragging;
    bool isDead = false;

    float dragOffsetY;
    Plane dragPlane;

    public static Action OnPlayerDead;

    [Button("Setup Ragdoll")]
    public void SetupRagdoll()
    {
        _rbRagdoll = transform.GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();

        foreach (var r in _rbRagdoll)
        {
            var old = r.transform.GetComponents<RagdollPart>();
            foreach (var o in old)
            {
                DestroyImmediate(o); // dùng trong Editor / Setup
            }

            r.transform.AddComponent<RagdollPart>();
        }

        foreach (var c in _colliders)
        {
            c.isTrigger = true;
        }
    }

    void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!CanHandleInput())
            return;

        HandleMouseInput();
        HandleKeyboardInput();
    }

    void Dead()
    {
        SoundCtrl.I.PlaySFXByType(TypeSFX.DEAD);

        animator.enabled = false;

        isDead = true;

        ResetConstraints();

        EnableRagdoll();

        OnPlayerDead?.Invoke();
    }

    #region Init
    [Button("Init")]
    public void Init()
    {
        animator.enabled = true;
        isDead = false;
        isLeft = true;

        float camX = cam.transform.eulerAngles.x;
        cam.transform.rotation = Quaternion.Euler(
            camX,
            CAM_ROTATE_LEFT,
            0
        );
        cam.transform.DOMoveX(CAM_POSITION_LEFT, 0);

        target.position = Vector3.zero;
        initTargetPos = target.position;
        transform.position = new Vector3(POS_LEFT, 0, 0);


        dragPlane = new Plane(-cam.transform.forward, target.position);

        ResetConstraints();
        var parts = GetComponentsInChildren<RagdollPart>();

        foreach (var part in parts)
        {
            part.onTriggerEnter += (other, part) => OnRagdollTrigger(other, part);
        }
        DisableRagdoll();
    }

    #endregion

    #region Input

    bool CanHandleInput()
    {
        return GameController.I.CurState == GameController.State.PLAYING && !isRolling && !isDead;
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            OnPointerDown();
        else if (Input.GetMouseButton(0))
            OnPointerDrag();
        else if (Input.GetMouseButtonUp(0))
            OnPointerUp();
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SwitchSide(true);
        else if (Input.GetKeyDown(KeyCode.D))
            SwitchSide(false);
    }

    #endregion

    #region Pointer

    void OnPointerDown()
    {
        if (IsClickOnUI())
            return;

        swipeStartPos = Input.mousePosition;
        isSwipeDetected = false;
        isSwipeHorizontal = false;

        isDragging = true;
        CalculateOffset();
        ApplyConstraint();
    }

    void OnPointerDrag()
    {
        if (!isDragging)
            return;

        // detect swipe
        Vector2 delta = (Vector2)Input.mousePosition - swipeStartPos;

        if (!isSwipeDetected)
        {
            if (Mathf.Abs(delta.x) > 200f)
            {
                isSwipeDetected = true;
                isSwipeHorizontal = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
            }
        }

        if (isSwipeDetected && isSwipeHorizontal)
        {
            if (delta.x > 0)
                SwitchSide(false); // right
            else
                SwitchSide(true);  // left

            isDragging = false;
            ResetConstraints();
            return;
        }

        // NORMAL vertical drag logic
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            float targetY = ray.GetPoint(enter).y + dragOffsetY;
            Vector3 newPos = target.position;
            newPos.y = targetY;

            target.position = Vector3.Lerp(
                target.position,
                newPos,
                Time.deltaTime * followSpeed
            );
        }
    }

    void OnPointerUp()
    {
        isDragging = false;
        ResetConstraints();
        //target.DOLocalMoveY(initTargetPos.y, 0.15f).SetEase(Ease.OutQuad);
        target.position = initTargetPos;
    }

    #endregion

    #region UI Check

    bool IsClickOnUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
#else
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return false;
#endif
    }

    #endregion

    #region Drag Offset

    void CalculateOffset()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            float hitY = ray.GetPoint(enter).y;
            dragOffsetY = target.position.y - hitY;
        }
    }

    #endregion

    #region Rig

    void ApplyConstraint()
    {
        bodyUpLeftConstraint.weight = 0.4f;
        legUpLeftConstraint.weight = isLeft ? 1f : 0f;
        rightUpLeftConstraint.weight = isLeft ? 0f : 1f;

        BuildRig();
    }

    void ResetConstraints()
    {
        bodyUpLeftConstraint.weight = 0f;
        legUpLeftConstraint.weight = 0f;
        rightUpLeftConstraint.weight = 0f;

        BuildRig();
    }

    void BuildRig()
    {
        if (rigBuilder != null)
            rigBuilder.Build();
    }

    #endregion

    #region Switch & Roll

    public void SwitchSide(bool toLeft)
    {
        if (isLeft == toLeft || isRolling)
            return;

        isLeft = toLeft;
        ResetConstraints();
        Roll();
    }

    void Roll()
    {
        isRolling = true;

        float targetX = isLeft ? POS_LEFT : POS_RIGHT;
        float camTargetY = isLeft ? CAM_ROTATE_LEFT : CAM_ROTATE_RIGHT;
        float camTargetX = isLeft ? CAM_POSITION_LEFT : CAM_POSITION_RIGHT;
        float playerStartY = isLeft ? 270f : 90f;

        animator.SetBool("Roll", true);

        cam.transform.DOKill();
        transform.DOKill();

        DG.Tweening.Sequence seq = DOTween.Sequence();

        seq.Append(cam.transform.DORotate(
            new Vector3(cam.transform.eulerAngles.x, camTargetY, 0),
            0.4f
        ).SetEase(Ease.Linear));
        seq.Join(cam.transform.DOLocalMoveX(
            camTargetX,
            0.4f
        ).SetEase(Ease.Linear));

        seq.Join(transform.DORotate(
            new Vector3(0, playerStartY, 0),
            0.1f
        ).SetEase(Ease.Linear));

        seq.Append(transform.DOMove(new Vector3(targetX, 0, 0), 0.4f).SetEase(Ease.Linear));

        seq.Append(transform.DORotate(
            new Vector3(0, 180f, 0),
            0.1f
        ).SetEase(Ease.Linear));

        seq.OnComplete(() =>
        {
            animator.SetBool("Roll", false);
            isRolling = false;
        });
    }

    #endregion

    #region Ragdoll
    public void EnableRagdoll()
    {
        foreach (var c in _colliders)
        {
            c.isTrigger = false;
        }
        foreach (Rigidbody rb in _rbRagdoll)
        {
            rb.isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        foreach (Rigidbody rb in _rbRagdoll)
        {
            rb.isKinematic = true;
        }
    }

    private void OnRagdollTrigger(Collider other, RagdollPart part)
    {
        if (other.CompareTag("Obstacle") && !isDead)
        {
            Dead();
            Vector3 forceDir = (transform.position - other.transform.position).normalized;

            part.rb.AddForce(forceDir * 100f, ForceMode.Impulse);
        }
    }
    #endregion

}
