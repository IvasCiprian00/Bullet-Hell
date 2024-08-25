using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private SoundManager _soundManager;

    public AudioSource musicSource;
    public AudioSource SFXSource;

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
}
