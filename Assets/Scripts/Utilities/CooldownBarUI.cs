using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBarUI : MonoBehaviour
{
    [SerializeField] private Transform _targetObject;

    private RectTransform _uiRectTransform;

    private void Start()
    {
        _uiRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_targetObject != null)
        {
            // Update the UI position to match the target object
            Vector3 targetPosition = _targetObject.position;
            Vector3 screenPosition = targetPosition + new Vector3(0f,-0.604f,0f);
            _uiRectTransform.position = screenPosition;
        }
    }
}
