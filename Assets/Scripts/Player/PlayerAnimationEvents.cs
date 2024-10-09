using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerGunAmmo ammo;

    public void ResetAmmo()
    {
        print("reset ammo");
        ammo.ResetAmmo();
        print("done reloading");
    }
}
