using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SymbioteController : ManagerBehaviour
{
    [SerializeField] private GameObject _ftuePrompt;
    [SerializeField] private Animator _animator;
    private float _possessionDuration = 3f;
    private bool IsAnimatingPossession = false;

    private CharacterController _character;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _ftuePrompt = GameManager.FTUEPrompt;
        Debug.Log("AIM + RMB to Possess a Human");
        _ftuePrompt.SetActive(true);
        GameManager.IsWaitingForFirstPossession = true;
    }

    public void Update()
    {
        
    }

    public void AnimatePossession()
    {
        _character = FindObjectOfType<CharacterController>();
        _character.RigidBody.bodyType = RigidbodyType2D.Static;
        _character.GetComponentInChildren<Collider2D>().enabled = false;
        transform.position = _character.Character.transform.position;
        _possessionDuration = 3f;
        IsAnimatingPossession = true;
        StartCoroutine(PossessionCoroutine());
    }

    IEnumerator PossessionCoroutine()
    {
        while (_possessionDuration > 0f)
        {
            if (IsAnimatingPossession)
            {
                _possessionDuration -= Time.deltaTime;
                Debug.Log(_possessionDuration);
            }
            _character.RigidBody.velocity = Vector2.zero;
            yield return null;
        }
        if (_possessionDuration <= 0f)
        {
            IsAnimatingPossession = false;
            gameObject.SetActive(false);
            GameManager.IsWaitingForFirstPossession = false;
            GameManager.IsControllable = true;
            _character.RigidBody.bodyType = RigidbodyType2D.Dynamic;
            _character.GetComponentInChildren<Collider2D>().enabled = true;
        }
    }
}
