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



    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
