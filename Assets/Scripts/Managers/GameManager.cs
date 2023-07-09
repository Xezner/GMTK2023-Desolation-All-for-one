using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : ManagerBehaviour
{
    [Header("GameOver")]
    [SerializeField] private GameObject _gameOver;

    [Header("FTUE Prompt")]
    [SerializeField] public GameObject FTUEPrompt;

    
    [Header("Camera")]
    [SerializeField] public Camera Camera;

    public bool IsGameStart = false;
    public bool IsGamePaused = false;
    public bool IsWaitingForFirstPossession = false;
    public bool IsControllable = true;
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
        IsGamePaused = false;
        IsWaitingForFirstPossession = false;
        IsControllable = true;
        SpawnManager.Instance.DestroyAll();
        SpawnManager.Instance.WaveCounter = 1;
        SpawnManager.Instance.SpawnCharacter();
    }

    public float zoomOutDuration = 1f;
    public float targetSize = 4f;

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
        IsWaitingForFirstPossession = false;
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
