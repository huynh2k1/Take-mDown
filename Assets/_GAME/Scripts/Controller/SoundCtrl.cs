using System.Collections.Generic;
using UnityEngine;

public class SoundCtrl : MonoBehaviour
{
    public static SoundCtrl I;

    [Header("MUSIC")]
    [SerializeField] AudioSource _musicSource;

    [Header("SOUNDS")]
    [SerializeField] AudioSource[] _soundSources;
    private Queue<AudioSource> _queueSounds;

    [Header("AUDIO CLIPS")]
    [SerializeField] AudioClip _bgMusic;
    [SerializeField] AudioClip _buttonClick, _win, _lose, _dead;

    bool _isStopMusic = true;
    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        _queueSounds = new Queue<AudioSource>(_soundSources);
        PlayMusic();
    }

    public void OnVolumeSoundChange()
    {
        foreach (var sound in _queueSounds)
        {
            sound.volume = PrefData.Sound;
        }
    }

    public void OnVolumeMusicChange()
    {
        _musicSource.volume = PrefData.Music;
    }

    public void PlayMusic()
    {
        if (!_isStopMusic)
            return;
        _isStopMusic = false;
        _musicSource.clip = _bgMusic;
        _musicSource.volume = PrefData.Music;
        _musicSource.Play();
    }

    public void PlaySFXByType(TypeSFX type)
    {
        switch (type)
        {
            case TypeSFX.CLICK:
                PlaySound(_buttonClick);
                break;
            case TypeSFX.WIN:
                PlaySound(_win);
                break;
            case TypeSFX.LOSE:
                PlaySound(_lose);
                break;
            case TypeSFX.DEAD:
                PlaySound(_dead);
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (_queueSounds.Count == 0) return;

        AudioSource source = _queueSounds.Dequeue();
        source.volume = PrefData.Sound;
        source.PlayOneShot(clip);
        StartCoroutine(ReturnToQueueWhenFinished(source));
    }

    private System.Collections.IEnumerator ReturnToQueueWhenFinished(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        _queueSounds.Enqueue(source);
    }

    public void StopMusic()
    {
        _isStopMusic = true;
        _musicSource.Stop();
    }
}
public enum TypeSFX
{
    MUSIC,
    CLICK,
    WIN,
    LOSE,
    DEAD,
}