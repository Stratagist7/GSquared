using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationEvents : MonoBehaviour
{    
    [SerializeField] private PlayerGunAmmo ammo;
    [SerializeField] private Animator gunAnim;
    [Space]
    [SerializeField] private Transform ammoCartridge;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform handLBone;
    [SerializeField] private WeaponControl weaponController;
    [SerializeField] private MeleeCombat melee;

    public void ResetAmmo()
    {
        ammo.ResetAmmo();
    }
    
    public void ReloadStart()
    {
        //gunAnim.SetTrigger(reloadKey);
        PlayerSoundManager.instance.PlayReload();
    }

    public void ParentGun()
    {
        ammoCartridge.parent = gun;
    }

    public void ParentHand()
    {
        ammoCartridge.parent = handLBone;
    }
    
    public void EnableMelee()
    {
        weaponController.EnableMelee();
    }
    
    public void EnableGun()
    {
        weaponController.EnableGun();
    }

    public void StartAttack()
    {
        melee.SetIsAttacking(true);
    }
    
    public void EndAttack()
    {
        melee.SetIsAttacking(false);
    }

    public void EnableHit()
    {
        melee.SetHitCollider(true);
    }
    
    public void DisableHit()
    {
        melee.SetHitCollider(false);
    }
}
