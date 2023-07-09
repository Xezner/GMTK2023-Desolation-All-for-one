using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : ManagerBehaviour
{
    [SerializeField] private GameObject _pauseMenu, _mainMenu, _options, _camera, _eventSystem;
    public enum Scenes
    {
        SampleMainMenu,
        DialogueScene,
        GameScene,
        Options
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape) && GameManager.IsGameStart)
        //{
        //    PauseGame();
        //}
    }

    public void DialogueScene()
    {
        _camera.GetComponent<AudioListener>().enabled = false;
        _eventSystem.SetActive(false);
        SceneManager.LoadSceneAsync(Scenes.DialogueScene.ToString(), LoadSceneMode.Additive);
        _mainMenu.SetActive(false);
        _options.SetActive(false);
    }

    public void Continue()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(Scenes.GameScene.ToString());
    }

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void BackToMainMenu()
    {
        _pauseMenu.SetActive(false);
        _mainMenu.SetActive(true);
        SceneManager.UnloadSceneAsync(Scenes.DialogueScene.ToString());
        _camera.GetComponent<AudioListener>().enabled = true;
        _eventSystem.SetActive(true);
    }

}

