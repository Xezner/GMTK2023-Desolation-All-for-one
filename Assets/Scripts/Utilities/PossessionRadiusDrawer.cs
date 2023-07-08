using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PossessionRadiusDrawer : MonoBehaviour
{
    [SerializeField] private CharacterController _charactercontroller;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private float _radius;
    private float _fadeDuration = 1f;
    private float _currentAlphaValue;
    private float _fadeTimer;
    public void StartDrawing()
    {
        _radius = _charactercontroller.PossessionRadius;
        Vector3 scale = new Vector3(_radius, _radius, 1f);
        _spriteRenderer.transform.localScale = scale;
        _currentAlphaValue = 0.5f;
        _fadeTimer = _fadeDuration;
    }

    private void SetAlpha(float alpha)
    {
        Color color = _spriteRenderer.color;
        color.a = alpha;
        _spriteRenderer.color = color;
    }

    private void FadeIn()
    {
        SetAlpha(0f);
    }

    private void Update()
    {
        _spriteRenderer.transform.position = _charactercontroller.Character.transform.position;

        _fadeTimer -= Time.deltaTime;

        if (_fadeTimer <= _fadeDuration)
        {
            // Calculate the current alpha value based on the fade timer
            float targetAlpha = (_fadeTimer / _fadeDuration) * _currentAlphaValue;
            SetAlpha(targetAlpha);
        }
    }

}
