using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ManagerBehaviour
{
    public bool IsGameStart = false;
    public bool IsGamePaused = false;
    public bool IsWaitingForFirstPossession = false;

    // Start is called before the first frame update
    void Start()
    {
        IsGamePaused = true;
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameStart();
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            SpawnManager.CleanSpawn();
        }
    }

    public void DebugHere()
    {
        Debug.Log("HERE");
    }

    public void GameStart()
    {
        IsGameStart = true;
        IsGamePaused = false;
        IsWaitingForFirstPossession = false;
        Debug.Log("HERE");
        SpawnManager.SpawnCharacter();
    }
}
