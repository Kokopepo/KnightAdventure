using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState; //выбираем состояние моба
    [SerializeField] private float roamingDistanceMin = 3f; //минимальное расстояние которое объект может пройти
    
    [SerializeField] private float roamingDistanceMax = 7f; //максимальное расстояние которое объект может пройти
    [SerializeField] private float roamingTimerMax = 2f; //время в течении которого он будет двигаться (если в течении этого времени
    //он не дошел до нужной точки, то он выбирает себе новую точку) 2 секунды короче говоря

    private NavMeshAgent _navMeshAgent; //непосредственно тот объект с которым мы будем работать
    private State _currentState; //переменная текущее состояние объекта
    private float _roamingTimer; //переменная текущее время передвижения
    private Vector3 _roamPosition; //точка в которую мы будем направлятся
    private Vector3 _startingPosition; //начальная позиция моба

    //? Атака игрока начало
    public event EventHandler OnEnemyAttack;

    [SerializeField] private bool isAttackingEnemy = false; 
    [SerializeField] private float attackingDistance = 2f; 
    [SerializeField] private float attackRate = 2f;
    private float _nextAttackTime = 0f;

    //? Атака игрока конец



    //? Преследование игрока начало
    [SerializeField] private bool isChasingEnemy = false; //преследует ли игрока агент
    [SerializeField] private float chasingDistance = 4f; //расстояние обнаружения игрока агентом

    [SerializeField] private float chasingSpeedMultiplier = 2f; // ускорение при преследовании игрока

    private float _roamingSpeed;
    private float _chasingSpeed;
    //? Преследование игрока конец


    //? Поворот скелета начало

    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition;

    //? Поворот скелета конец



    public bool IsRunning
    {
        get
        {

            if (_navMeshAgent.velocity == Vector3.zero) //если скорость агента равна 0 то он не движется
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        
    }


    private enum State //перечисление C#
    {
        Idle, //моб стоит (я убрал ибо он не нужен (нет анимации под это) )
        Roaming, //моб двигается
        Chasing, //преследование
        Attacking, //атака
        Death

    }

    //? РАЗДЕЛЕНИЕ ЛОГИКИ И ВИЗУАЛА

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false; //измение вращения отключаем
        _navMeshAgent.updateUpAxis = false; //измение ориентации тоже отключаем 
        _currentState = startingState;

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * chasingSpeedMultiplier;
    }


    private void Update() //метод Roaming происходит 1 раз в 60 фпс, длится он 2 секунды. при 144 фпс длится о
    {
        StateHandler();
        MovementDirectionHandler();

        // if (Player.Instance.IsAlive() == false)
        // {
        //     _isChasingEnemy = false;
        //     _currentState = State.Roaming;
        // }
    }

    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming: 
                _roamingTimer -= Time.deltaTime; // roamingTime = roamingTime - Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = roamingTimerMax;                   
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                    ChasingTarget();
                    CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }
    }


    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);

            _nextAttackTime = Time.time + attackRate; 
        }
        
    }


    private void CheckCurrentState()
    {   
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position); 
        
        State newState = State.Roaming;    

        if (isChasingEnemy)
        {
            if (distanceToPlayer <= chasingDistance)
            {
                isChasingEnemy = true;
                newState = State.Chasing;
            }
            
        }

        if (isAttackingEnemy)
        {
            if (distanceToPlayer <= attackingDistance)
            {
                newState = State.Attacking;
                // if (Player.Instance.IsAlive())
                // {
                //     newState = State.Attacking;
                // }                   
                // else
                // {
                //     newState = State.Roaming;
                // }                
            }
        }

        if (Player.Instance.IsAlive() == false)
        {
            isAttackingEnemy = false;
            isChasingEnemy = false;
            newState = State.Roaming;
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath(); 
                _navMeshAgent.speed = _chasingSpeed;

            }

            else if (newState == State.Roaming)
            {
                _roamingTimer = 0f; 
                _navMeshAgent.speed = _roamingSpeed;            
            }

            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath(); 
            }
            _currentState = newState;
        }

    }

    private void ChasingTarget()
    {
        ChangeFacingDirection(_startingPosition, Player.Instance.transform.position); 
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }


    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning)
            {
                ChangeFacingDirection(_lastPosition, transform.position);
            }
            else if (_currentState == State.Attacking)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);   
            }

            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }    

    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        //ChangeFacingDirection(_startingPosition, _roamPosition); 
        //Debug.Log(roamPosition);
        _navMeshAgent.SetDestination(_roamPosition);
    }

    private Vector3 GetRoamingPosition() //получаем новое направление для движения
    {
        return _startingPosition + Utils.GetRandomDir() * Utils.GetRandomDistance(roamingDistanceMin, roamingDistanceMax); //у юнити свой рандом (UnityEngine.Random.Range)
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition) //source - позиция моба, target - куда он пойдет 
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0); //Quaternion - единичный вектор, вращаем объект только по y (x и z не трогаем) 
        }                                                      //Euler - метод крутения
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}





