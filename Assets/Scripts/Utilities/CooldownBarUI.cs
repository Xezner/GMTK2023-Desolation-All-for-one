using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CooldownBarUI : ManagerBehaviour
{
    public Transform TargetObject;

    private RectTransform _uiRectTransform;
    public static CooldownBarUI Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _uiRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            UpdateCooldownBarUI();
        }
    }

    private void UpdateCooldownBarUI ()
    {
        if (TargetObject != null)
        {
            // Update the UI position to match the target object

            Vector3 targetPosition = TargetObject.position;
            Vector3 screenPosition = targetPosition + new Vector3(0f, -0.604f, 0f);
            _uiRectTransform.position = screenPosition;
        }
    }
}
