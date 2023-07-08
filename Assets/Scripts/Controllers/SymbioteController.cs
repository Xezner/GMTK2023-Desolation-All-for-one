using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbioteController : ManagerBehaviour
{
    [SerializeField] private GameObject _ftuePrompt;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("AIM + RMB to Possess a Human");
        _ftuePrompt.SetActive(true);
        GameManager.IsWaitingForFirstPossession = true;
    }
}
