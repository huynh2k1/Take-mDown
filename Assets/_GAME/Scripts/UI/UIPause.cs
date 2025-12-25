using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : BasePopup
{
    public override UIType Type => UIType.PAUSE;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnResume;
    [SerializeField] Slider _sliderSound;
    [SerializeField] Slider _sliderMusic;


    public static Action OnHomeClicked;
    public static Action OnResumeClicked;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);
        _btnResume.onClick.AddListener(OnClickResume);

        _sliderMusic.onValueChanged.AddListener((v) =>
        {
            OnVolumeMusicChange(v);
        });
        _sliderSound.onValueChanged.AddListener((v) =>
        {
            OnVolumeSoundChange(v);
        });
    }

    public override void Show()
    {
        base.Show();
        Load();
    }

    void Load()
    {
        _sliderSound.value = PrefData.Sound;
        _sliderMusic.value = PrefData.Music;
    }

    void OnVolumeSoundChange(float value)
    {
        PrefData.Sound = value;
        SoundCtrl.I.OnVolumeSoundChange();
    }

    void OnVolumeMusicChange(float value)
    {
        PrefData.Music = value;
        SoundCtrl.I.OnVolumeMusicChange();
    }

    void OnClickHome()
    {
        Hide(() => OnHomeClicked?.Invoke());

    }

    void OnClickResume()
    {
        Hide(() => OnResumeClicked?.Invoke());
    }

}
