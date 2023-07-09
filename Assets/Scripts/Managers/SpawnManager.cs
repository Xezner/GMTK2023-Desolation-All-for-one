using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : ManagerBehaviour
{
    [SerializeField] private List<CharacterData> _spawnList;
    [SerializeField] private CharacterNames _characterNames;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private LayerMask _enemyMask;

    private int _spawnRadius = 50;
    private float _collisionRadius = 2f;
    private float _playerNoSpawnRadius = 10f;
    private Transform _characterController;

    public int WaveCounter;

    public static SpawnManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        WaveCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            CheckWave();
        }
    }

    public void CheckWave()
    {
        if(gameObject.transform.childCount == 2 && WaveCounter != 1 /*&& gameObject.transform.GetChild(0).name == "Player"*/)
        {
            SpawnCharacter();
        }
    }
    
    public void CleanSpawn()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponentInChildren<WeaponController>().IsPlayer)
                continue;
            Destroy(child.gameObject);
        }
    }

    public void DestroyAll()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SpawnCharacter()
    {
        int spawnNumber = WaveCounter == 1 ? 5 : Random.Range(5, 11);
        for(int i = 0; i < spawnNumber; i++)
        {
            int enemyType = Random.Range(0, _spawnList.Count);
            int randomIndex = Random.Range(0, _characterNames.NameList.Count);

            var controller = FindObjectOfType<CharacterController>();
            _characterController = controller.Character ? controller.Character.transform : controller.transform;

            Vector2 spawnPoint = GetSpawnPoint();
            _spawnList[enemyType].Name = _characterNames.NameList[randomIndex];
            Debug.Log(_spawnList[enemyType].Name);
            var spawn = Instantiate(_spawnList[enemyType].Character, spawnPoint, Quaternion.identity, transform);
            SetCharacterData(spawn, _spawnList[enemyType], _characterNames.NameList[randomIndex]);
        }
        WaveCounter++;
    }

    public void SetCharacterData(GameObject spawn, CharacterData characterData, string name)
    {
        spawn.gameObject.GetComponent<CharacterDataHolder>().AssignStats(characterData, name);
    }

    private Vector2 GetSpawnPoint()
    {
        Vector2 spawnPoint = Vector2.zero;
        for (int i = 0; i <= 10; i++)
        {
            _spawnRadius = WaveCounter == 1 ? (int)_playerNoSpawnRadius + 5 : 30;
            int randomizedRadius = Random.Range((int)_playerNoSpawnRadius, _spawnRadius);
            
            Vector2 randomSpawnPoint = Random.insideUnitCircle.normalized * randomizedRadius;
            Vector2 calculatedSpawnPoint = (Vector2)transform.position + randomSpawnPoint;

            if (!Physics2D.CircleCast(calculatedSpawnPoint, _collisionRadius, Vector2.zero, 0f, _obstacleMask))
            {
                if (Vector2.Distance(calculatedSpawnPoint, _characterController.position) > _playerNoSpawnRadius)
                {
                    Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(calculatedSpawnPoint, _collisionRadius, _enemyMask);
                    if (enemyColliders.Length == 0)
                    {
                        spawnPoint = calculatedSpawnPoint;
                        if (spawnPoint == Vector2.zero)
                        {
                            continue;
                        }
                        break;
                    }
                }
            }
        }
        return spawnPoint;
    }
}
