using System;
using System.Collections;
using System.Collections.Generic;
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

    private int _waveCounter;
    void Start()
    {
        _waveCounter = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SpawnCharacter()
    {
        //CleanSpawn();

        Debug.Log("HELLO");
        int spawnNumber = _waveCounter == 1 ? 5 : Random.Range(5, 11);
        for(int i = 0; i < spawnNumber; i++)
        {
            int enemyType = Random.Range(0, 2);
            int randomIndex = Random.Range(0, _characterNames.NameList.Count);

            var controller = FindObjectOfType<CharacterController>();
            _characterController = controller.Character ? controller.Character.transform : controller.transform;

            Vector2 spawnPoint = GetSpawnPoint();

            _spawnList[enemyType].Name = _characterNames.NameList[randomIndex];
            Instantiate(_spawnList[enemyType].Character, spawnPoint, Quaternion.identity, transform);
        }

        _waveCounter++;
    }


    private Vector2 GetSpawnPoint()
    {
        Vector2 spawnPoint = Vector2.zero;
        for (int i = 0; i <= 10; i++)
        {
            _spawnRadius = _waveCounter == 1 ? (int)_playerNoSpawnRadius + 5 : 30;
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
