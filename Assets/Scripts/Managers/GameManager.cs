using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : ManagerBehaviour
{
    [Header("MainMenu")]
    [SerializeField] private GameObject _mainMenu;

    [Header("GameOver")]
    [SerializeField] private GameObject _gameOver;

    [Header("FTUE Prompt")]
    [SerializeField] public GameObject FTUEPrompt;

    
    [Header("Camera")]
    [SerializeField] public Camera Camera;

    private int _minimumLifeforce = 5;

    public bool IsGameStart = false;
    public bool IsGamePaused = false;
    public bool IsWaitingForFirstPossession = false;
    public bool IsControllable = true;
    public bool IsPossessed = false;
    public int PossessionGauge = 0;
    public int PlayerHP = 0;
    // Start is called before the first frame update
    void Start()
    {
        IsGamePaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameStart();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SpawnManager.Instance.CleanSpawn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
        }
    }

    public void DebugHere()
    {
        Debug.Log("HERE");
    }

    public void GameStart()
    {
        _mainMenu.SetActive(false);
        IsGamePaused = false;
        IsWaitingForFirstPossession = false;
        IsControllable = true;
        PossessionGauge = _minimumLifeforce;
        IsPossessed = false;
        SpawnManager.Instance.DestroyAll();
        SpawnManager.Instance.WaveCounter = 1;
        SpawnManager.Instance.SpawnCharacter();
        StartCoroutine(ZoomOut());
    }

    public float zoomOutDuration = 1.5f;
    public float targetSize = 4.5f;

    private float initialSize;
    private float currentSize;

    IEnumerator ZoomOut()
    {
        initialSize = Camera.orthographicSize;
        currentSize = initialSize;
        float elapsedTime = 0f;

        while (elapsedTime < zoomOutDuration)
        {
            float t = elapsedTime / zoomOutDuration;

            currentSize = Mathf.Lerp(initialSize, targetSize, t);

            Camera.orthographicSize = currentSize;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        _mainMenu.SetActive(true);
        IsWaitingForFirstPossession = false;
        IsPossessed = false;
        IsControllable = true;
        FTUEPrompt.SetActive(false);
        _gameOver.SetActive(false);
        Camera.orthographicSize = 0.75f;
        Camera.transform.position = new Vector3(0f, 0f, -10f);
    }

    public void GameOverScreen()
    {
        IsGamePaused = true;
        _gameOver.SetActive(true);
    }
}
