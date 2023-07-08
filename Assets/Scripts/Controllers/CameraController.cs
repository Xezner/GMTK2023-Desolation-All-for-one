using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : ManagerBehaviour
{
    [SerializeField] private GameObject _character;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _smoothTime = 0.2f; // Smoothing time for camera movement
    private Vector3 _velocity = Vector3.zero; // Velocity for SmoothDamp

    public GameObject Character { get { return _character; } set { _character = value; } }

    private void Start()
    {
        Debug.Log(gameObject.name);
        GetCharacter();
    }

    private void LateUpdate()
    {
        if (!GameManager.IsGamePaused)
        {
            if (_character != null)
            {
                Vector3 targetPosition = _character.transform.position;
                Vector3 desiredPosition = new Vector3(targetPosition.x, targetPosition.y, -10f);

                _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, desiredPosition, ref _velocity, _smoothTime);
            }
        }
    }

    private void GetCharacter()
    {
        _character = gameObject.GetComponent<CharacterController>().Character;
    }
}
