using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private FlashBlink _flashBlink;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>();

    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        _flashBlink.StopBlinking();
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());

        if (Player.Instance.IsAlive())
            AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
        {
            _spriteRenderer.flipX = true;
            //*ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _spriteRenderer.flipX = false;
            //*ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            
        }
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
}
