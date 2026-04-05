using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))] //признак хорошего кода, говорит о том что если мы добавляем этот скрипт на объект, то у него обязательно
// должен быть компонент Rigidbody2D, если его нет, то он будет добавлен автоматически
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackForce = 3f;
    [SerializeField] private float knockBackMovingTimerMax = 0.3f;

    private float _knockBackMovingTimer;

    private Rigidbody2D _rb;

    public bool isGettingKnockedBack { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _knockBackMovingTimer -= Time.deltaTime;
        if(_knockBackMovingTimer < 0)
        {
            StopKnockBackMovement();
        }
    }

    public void GetKnockedBack(Transform damageSource)
    {
        isGettingKnockedBack = true;
        _knockBackMovingTimer = knockBackMovingTimerMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackForce / _rb.mass;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        _rb.velocity = Vector2.zero; //остановка движения от knockback
        isGettingKnockedBack = false;
    }
}
