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
    private CharacterDataHolder _targetCharactrer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _ftuePrompt = GameManager.FTUEPrompt;
        Debug.Log("Hold RMB and AIM.\n Release RMB to Possess.");
        _ftuePrompt.SetActive(true);
        GameManager.IsWaitingForFirstPossession = true;
    }

    public void Update()
    {
        
    }

    public void AnimateTransfer()
    {
        Debug.Log("START TRANSFER");
        GetComponent<Collider2D>().enabled = false;
        _animator.ResetTrigger("Possess");
        _animator.SetTrigger("Leave");
        transform.position = _targetCharactrer.transform.position;
    }

    public void ResetTransfer()
    {
        Debug.Log("END TRANSFER");
        StopCoroutine(PossessionCoroutine());
        ResetSymbiote();
        _animator.ResetTrigger("Leave");
    }

    public void AnimatePossession(CharacterDataHolder targetCharacter)
    {
        _ftuePrompt = GameManager.FTUEPrompt;
        Debug.Log("START POSSESSION");
        StopCoroutine(PossessionCoroutine());
        _ftuePrompt.SetActive(false);
        _animator = GetComponent<Animator>();
        _animator.ResetTrigger("Leave");
        _targetCharactrer = targetCharacter;
        _animator.SetTrigger("Possess");
        _character = FindObjectOfType<CharacterController>();
        if (_character.Character != null)
        {
            _character.RigidBody.bodyType = RigidbodyType2D.Static;
            _character.GetComponentInChildren<Collider2D>().enabled = false;
        }
        transform.position = _character.Character ? _character.Character.transform.position : _character.transform.position;
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
            }

            if (_character.Character != null)
            {
                _character.RigidBody.velocity = Vector2.zero;
            }

            yield return null;
        }
    }

    private void ResetSymbiote()
    {
        Debug.Log("HERE2");
        IsAnimatingPossession = false;
        GameManager.IsWaitingForFirstPossession = false;
        GameManager.IsControllable = true;

        if (_character.Character != null)
        {
            _character.RigidBody.bodyType = RigidbodyType2D.Dynamic;
            _character.Character.GetComponentInChildren<Collider2D>().enabled = true;
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Debug.Log("Huli ka");
    }


}
