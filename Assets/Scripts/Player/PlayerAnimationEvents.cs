using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private static readonly int reloadKey = Animator.StringToHash("t_reload");
    
    [SerializeField] private PlayerGunAmmo ammo;
    [SerializeField] private Animator gunAnim;

    public void ResetAmmo()
    {
        ammo.ResetAmmo();
    }
    
    public void ReloadStart()
    {
        gunAnim.SetTrigger(reloadKey);
        PlayerSoundManager.instance.PlayReload();
    }
}
