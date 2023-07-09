using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using TMPro;
using UnityEngine.UI;

public class SoundManager : ManagerBehaviour
{
    public static SoundManager sm;

    [SerializeField, Space(5), Header("Text for Audio"), Tooltip("Put the UI for the Music and Audio here.")]
    private TMP_Text musicOptionText, sfxText;

    //removedAmbience
    [SerializeField, Space(5), Header("Buttons for Audio"), Tooltip("Place the Buttons for the Music and Audio here.")]
    private Button musicButton, sfxButton;

    [SerializeField, Space(5), Header("Audio Source"), Tooltip("Assign the Scene's Dedicated Audio Sources into these game objects.")]
    private AudioSource sfxSource, musicSource, ambienceSource;

    [SerializeField, Space(2), Header("Sound Effects"), Tooltip("Place the sound effects of the game here")]
    private AudioClip[] grunts, meleeWeapons, meleeWeaponHit, magic, magicProjectileHit, possession;

    [SerializeField, Space(2), Header("Music"), Tooltip("Place the music of the game here")]
    private AudioClip[] music;

    //Move this to the Custom Scene Manager
    private Scene currentScene;

    //Actions available by the player
    public enum actionTypes { possession, melee, range, voice }

    //Weapon Types available by for use by the player
    public enum weaponTypes { none, sword, axe, magic }

    private void Awake()
    {
        //Initialize the current scene
       currentScene = SceneManager.GetActiveScene();

        //Initialize the Sound Manager
        if (sm ==null)
        {
            sm = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Initialize the Buttons on the Menu
        musicButton.onClick.AddListener(() => AudioOptions(musicSource, musicOptionText));
        sfxButton.onClick.AddListener(() => AudioOptions(sfxSource, sfxText));
        //ambienceButton.onClick.AddListener(() => AudioOptions(ambienceSource));
    }

    void Start()
    {
        BGMPlayer();
        //For Testing
        //PlaySoundEffect("possession");
    }

    //Triggers for the Sound, Collosion Hit, Raycast Status
    public bool PlaySoundEffect(string actionTypeString, bool isEnemyHit = false, string weaponInUseString = "none")
    {
        //Convert the String Parameter to Enum
        actionTypes actionType  = (actionTypes)Enum.Parse(typeof(actionTypes), actionTypeString);
        weaponTypes weaponType = (weaponTypes)Enum.Parse(typeof(weaponTypes), weaponInUseString);

        //Sound Effect for possession
        if (actionType == actionTypes.possession)
        {
            sfxSource.PlayOneShot(possession[UnityEngine.Random.Range(0,3)]);
            Debug.Log("Possessing Target " + sfxSource.isPlaying);
            return true;
        }

        //Sound Effect when striking with Melee or firing in Range
        if (actionType == actionTypes.melee && isEnemyHit != true)
        {
            sfxSource.PlayOneShot(meleeWeapons[UnityEngine.Random.Range(0, 3)]);
            Debug.Log("Weapon is Swinging " + sfxSource.isPlaying);
            return true;
        }
        else if (actionType == actionTypes.range && isEnemyHit != true)
        {
            sfxSource.PlayOneShot(magic[UnityEngine.Random.Range(0, 3)]);
            Debug.Log("Magic Missile is Firing " + sfxSource.isPlaying);
            return true;
        }

        //Sound Effect for when the melee lands or the projectile hits
        if (isEnemyHit != false && actionType == actionTypes.melee)
        {
            sfxSource.PlayOneShot(meleeWeaponHit[UnityEngine.Random.Range(0, 3)]);
            Debug.Log("Weapon Hits! " + sfxSource.isPlaying);
            return true;
        }
        else if (isEnemyHit != false && actionType == actionTypes.range)
        {
            sfxSource.PlayOneShot(magicProjectileHit[UnityEngine.Random.Range(0, 3)]);
            Debug.Log("Magic Projectile Hits! " + sfxSource.isPlaying);
            return true;
        }

        return false;
    }

    //shuffle is not in use as of the moment.
    public void BGMPlayer (bool shuffle = false)
    {
        //Detect if player is in Combat Area
        if (currentScene.name != "Main")
        {
            //index one is combat music
            musicSource.clip = music[0];
            musicSource.Play();
        }
        else if (currentScene.name == "Main")
        {
            //Index zero is main theme
            musicSource.clip = music[1];
            musicSource.Play();
        }

    }

    //Must Refactor later, iterate over the buttons, add button change sprite
    public void AudioOptions(AudioSource source, TMP_Text optionText, bool enable = true)
    {
        source.mute = !source.mute;
        if (source.mute == false)
        {
            optionText.text = "On";
        }
        else
        {
            optionText.text = "Off";
        }

    }




}
