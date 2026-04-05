using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damageableObject; 
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.2f;

    private float _blinkTimer;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
        _isBlinking = true;      
    }

    private void Start()
    {
        if (_damageableObject is Player)
        {
            (_damageableObject as Player).OnFlashBlink += DamagableObject_OnFlashBlink;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isBlinking)
        {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
        }

    }

    public void StopBlinking()
    {      
        SetDefaultMaterial();
        _isBlinking = false;
    }

    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    private void SetBlinkMaterial()
    {
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }

    private void DamagableObject_OnFlashBlink(object sender, System.EventArgs e)
    {
        SetBlinkMaterial();
    }

    private void OnDestroy()
    {
        if (_damageableObject is Player)
        {
            (_damageableObject as Player).OnFlashBlink -= DamagableObject_OnFlashBlink;
        }
    }
}
