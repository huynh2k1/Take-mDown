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
    [SerializeField] AudioClip _buttonClick, _win, _lose, _maleFall, _femaleFall, _roll;


    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        _queueSounds = new Queue<AudioSource>(_soundSources);
        PlayMusic(_bgMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
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
            case TypeSFX.ROLL:
                PlaySound(_roll);
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (_queueSounds.Count == 0) return;

        AudioSource source = _queueSounds.Dequeue();
        source.clip = clip;
        source.Play();
        StartCoroutine(ReturnToQueueWhenFinished(source));
    }

    private System.Collections.IEnumerator ReturnToQueueWhenFinished(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        _queueSounds.Enqueue(source);
    }

    public void StopMusic()
    {
       _musicSource.Stop();
    }
}
public enum TypeSFX
{
    MUSIC,
    CLICK,
    WIN,
    LOSE,
    ROLL,
}