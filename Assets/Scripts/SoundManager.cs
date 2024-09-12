using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private SoundManager _soundManager;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (_soundManager == null)
        {
            _soundManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip sound)
    {
        _sfxSource.PlayOneShot(sound);
    }

    public void Play3DSound(AudioClip sound, Vector2 location)
    {
        _sfxSource.spatialBlend = 1;
        _sfxSource.transform.position = location;
        _sfxSource.PlayOneShot(sound);
    }

    public void ToggleMusic()
    {
        _musicSource.enabled = !_musicSource.isActiveAndEnabled;
    }

    public void ToggleSFX()
    {
        _sfxSource.enabled = !_sfxSource.isActiveAndEnabled;
    }
}
