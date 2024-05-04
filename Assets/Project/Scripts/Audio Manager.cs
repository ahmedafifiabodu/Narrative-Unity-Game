using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _masterVolume;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] internal AudioSource _sFXSource;
    [SerializeField] private AudioSource _walkSFXSource;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip _background;

    [SerializeField] internal AudioClip _walk;
    [SerializeField] internal AudioClip _jump;
    [SerializeField] internal AudioClip _interact;

    internal AudioSource WalkSFXSource => _walkSFXSource;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterServiceDontDestoryOnLoad(this);

        _masterVolume.volume = PlayerPrefs.GetFloat("MasterVolume", 1);
        _musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        _sFXSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        _walkSFXSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
    }

    private void Start() => PlayBackground();

    internal void PlaySFX(AudioClip clip) => _sFXSource.PlayOneShot(clip);

    internal void PlayWalkSFX(AudioClip clip)
    {
        if (!_walkSFXSource.isPlaying)
        {
            _walkSFXSource.clip = clip;
            _walkSFXSource.loop = true;
            _walkSFXSource.Play();
        }
    }

    internal void StopWalkSFX()
    {
        _walkSFXSource.Stop();
        _walkSFXSource.loop = false;
    }

    internal void PlayBackground()
    {
        _musicSource.clip = _background;
        _musicSource.Play();
    }

    internal void StopAllAudio()
    {
        _sFXSource.Stop();
        _walkSFXSource.Stop();
    }
}