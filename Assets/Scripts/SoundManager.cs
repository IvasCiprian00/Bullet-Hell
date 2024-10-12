using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    ENEMY_SHOOT,
    PLAYER_SHOOT,
    DEATH,
    PLAYER_DEATH,
    DODGE
}

[ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    public void Awake()
    {
        //DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        _musicSource = transform.Find("Music Source").GetComponent<AudioSource>();
        _sfxSource = transform.Find("SFX Source").GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.AddPitchVariance();
        instance._sfxSource.PlayOneShot(randomClip, volume);
    }

    public void AddPitchVariance()
    {
        float newPitch = UnityEngine.Random.Range(0.8f, 1.2f);
        instance._sfxSource.pitch = newPitch;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for(int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
    public void ToggleMusic()
    {
        _musicSource.enabled = !_musicSource.isActiveAndEnabled;
    }

    public void ToggleSFX()
    {
        _sfxSource.enabled = !_sfxSource.isActiveAndEnabled;
    }

    [Serializable]
    public struct SoundList
    {
        public AudioClip[] Sounds { get => sounds; }
        [HideInInspector] public string name;
        [SerializeField] private AudioClip[] sounds;
    }
}
