using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;
    private Animator _animator;

    private const string IS_DIE = "IsDie";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";

    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyAI.OnEnemyAttack += EnemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += EnemyEntity_OnTakeHit;
        enemyEntity.OnDeath += EnemyEntity_OnDeath;
    }

    private void EnemyEntity_OnDeath(object sender, EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = -1;
        enemyShadow.SetActive(false);
    }

    private void EnemyEntity_OnTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(TAKE_HIT);
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, enemyAI.IsRunning);
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, enemyAI.GetRoamingAnimationSpeed());
    }


    public void TriggerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }

    private void EnemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }

    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= EnemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit -= EnemyEntity_OnTakeHit;
        enemyEntity.OnDeath -= EnemyEntity_OnDeath;
    }
    
}
