using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenu : ManagerBehaviour
{
    [SerializeField] private GameObject _options, _mainMenu, _camera;
    public void NewGame()
    {
        _mainMenu.SetActive(false);
        GameManager.background.SetActive(false);
        SceneManager.LoadSceneAsync(1);
        GameManager.Prompt.SetActive(true);
        GameManager.IsGameStart = true;
    }

    public void Options()
    {
        _mainMenu.SetActive(false);
        _options.SetActive(true);
    }

    public void Back()
    {
        _mainMenu.SetActive(true);
        _options.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
