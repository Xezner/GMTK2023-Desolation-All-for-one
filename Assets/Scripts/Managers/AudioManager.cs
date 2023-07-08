using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private GameObject _musicOffText, _sfxOffText, _musicOnText, _sfxOnText;
    [SerializeField] private Sound[] _musicSounds, _sfxSounds;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    private void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            PlaySFX("Jump");
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(_musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not available");
        }

        else
        {
            _musicSource.clip = s.clip;
            _musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(_sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not available");
        }

        else
        {
            _sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
        MusicButtonText();
    }

    public void MusicButtonText()
    {
        if (_musicOffText.activeInHierarchy)
        {
            _musicOffText.SetActive(false);
            _musicOnText.SetActive(true);
        }

        else
        {
            _musicOffText.SetActive(true);
            _musicOnText.SetActive(false);
        }
    }

    public void ToggleSFX()
    {
        _sfxSource.mute = !_sfxSource.mute;
        SFXButtonText();
    }

    public void SFXButtonText()
    {
        if (_sfxOffText.activeInHierarchy)
        {
            _sfxOffText.SetActive(false);
            _sfxOnText.SetActive(true);
        }

        else
        {
            _sfxOffText.SetActive(true);
            _sfxOnText.SetActive(false);
        }
    }
}


