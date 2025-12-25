using UnityEngine;
using UnityEngine.UI;

public class UISetting : BasePopup
{
    public override UIType Type => UIType.SETTING;

    [SerializeField] Slider _sliderSound;
    [SerializeField] Slider _sliderMusic;

    protected override void Awake()
    {
        base.Awake();
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
}
