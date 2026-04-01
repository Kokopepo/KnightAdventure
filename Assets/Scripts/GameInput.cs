using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
public class GameInput : MonoBehaviour
{

    private PlayerInputActions _playerInputActions;

    public static GameInput Instance {get; private set;}

    public event EventHandler OnPlayerAttack;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();

        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
    }

    public void OnEnable()
    {
        _playerInputActions.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }    

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
}
