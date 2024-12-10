using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationEvents : MonoBehaviour
{
    private static readonly int reloadKey = Animator.StringToHash("t_reload");
    
    [SerializeField] private PlayerGunAmmo ammo;
    [SerializeField] private Animator gunAnim;
    [Space]
    [SerializeField] private Transform ammoCartridge;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform handLBone;
    [SerializeField] private Vector3 gunOffset;
    [SerializeField] private Vector3 handOffset;

    public void ResetAmmo()
    {
        ammo.ResetAmmo();
    }
    
    public void ReloadStart()
    {
        gunAnim.SetTrigger(reloadKey);
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
}
