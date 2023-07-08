using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBehaviour : MonoBehaviour
{
    protected CachedComponent<GameManager> _gameManager = new();
    protected GameManager GameManager
    {
        get { return _gameManager.Instance(this); }
    }

    protected CachedComponent<SpawnManager> _spawnManager = new();
    protected SpawnManager SpawnManager
    {
        get { return _spawnManager.Instance(this); }
    }
}
