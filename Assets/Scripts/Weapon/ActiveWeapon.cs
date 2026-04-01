using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }



    [SerializeField] private Sword sword;

    public void Awake()
    {
        Instance = this;
    }


    public Sword GetActiveWeapon()
    {
        return sword;
    }

    private void Update()
    {
        if (Player.Instance.IsAlive())
        FollowMousePosition();
    }

    private void FollowMousePosition()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //! Quaternion для поворота оружия влево-вправо, так как там есть спрайт, который нужно отзеркалить, а не просто повернуть на 180 градусов по оси y
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


}
