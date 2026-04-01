using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
/*using UnityEngine.TestTools.Constraints; */
[SelectionBase]
public class Player : MonoBehaviour
{   
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;

    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Vector2 inputVector;
    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private int _currentHealth;

    private bool _canTakeDamage;
    private bool _isAlive;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (_knockBack.isGettingKnockedBack)
            return;
        HandleMovement();

        /* Debug.Log(inputVector); */
    }

    public bool IsAlive() => _isAlive;
    
    public void TakeDamage(Transform damageSource, int damage)
    {        
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage); //чтоб здоровье не уходило в минус. можно было написать if, но так короче
            Debug.Log(_currentHealth);
            _knockBack.GetKnockedBack(damageSource);

            StartCoroutine(DamageRecoveryRoutine());
        }  

        DeteckDeath();   
    }

    private void DeteckDeath()
    {
        if (_currentHealth == 0 && _isAlive)
        {
            _isAlive = false;

            _knockBack.StopKnockBackMovement();

            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
        
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position); //«Камера, скажи мне, где в пикселях экрана находится этот объект мира»
        return playerScreenPosition;
    }

    private void HandleMovement()
    {        
        //inputVector = inputVector.normalized; - из-за input action это не нужно
        //Debug.Log(inputVector);
        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }
    
    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }
}
