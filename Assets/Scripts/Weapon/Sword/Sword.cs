using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 2;

    public event EventHandler OnSwordSwing;
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start() {
        AttackColliderTurnOffOn();
        //! Включаем коллайдер, а потом сразу же выключаем, чтобы он не сработал при старте игры + при изменении скорости анимации всё будет корректно работать.
    }

    public void Attack()
    {
        AttackColliderTurnOn();
        
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    private void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOn();
        AttackColliderTurnOff();        
    }

    
}
