using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControl : MonoBehaviour
{
    private static readonly int stateKey = Animator.StringToHash("i_state_index");
    private static readonly int endKey = Animator.StringToHash("t_end");
    private static WeaponType _state;

    [SerializeField] private WeaponType initialState;
    [SerializeField] private Animator animator;
    [Space]
    [Header("Gun Controllers")]
    [SerializeField] private GameObject gun;
    [SerializeField] private PlayerGunAmmo ammo;
    [SerializeField] private Image[] gunImage;
    [Space]
    [Header("Melee Controllers")]
    [SerializeField] private MeleeCombat melee;
    [SerializeField] private Image[] fistImage;

    private void Awake()
    {
        animator.SetInteger(stateKey, (int)initialState);
    }

    public void OnSwapToGun()
    {
        if (_state == WeaponType.Gun || _state == WeaponType.None || MenuUI.Paused)
        {
            return;
        }
        SwapWeapons(WeaponType.Gun);
    }
    
    public void OnSwapToMelee()
    {
        if (_state == WeaponType.Melee || _state == WeaponType.None || MenuUI.Paused)
        {
            return;
        }
        SwapWeapons(WeaponType.Melee);
    }

    private void SwapWeapons(WeaponType argWeapon)
    {
        _state = WeaponType.None;
        animator.SetTrigger(endKey);
        animator.SetInteger(stateKey, (int)argWeapon);
    }

    public void EnableMelee()
    {
        _state = WeaponType.Melee;
        gun.SetActive(false);
        ammo.enabled = false;
        ChangeColor(gunImage, Color.black);
        
        // enable melee stuff
        melee.enabled = true;
        ChangeColor(fistImage, Color.white);
    }
    
    public void EnableGun()
    {
        // disable melee stuff
        melee.enabled = false;
        ChangeColor(fistImage, Color.black);
        
        _state = WeaponType.Gun;
        gun.SetActive(true);
        ammo.enabled = true;
        ChangeColor(gunImage, Color.white);
    }

    private static void ChangeColor(Image[] argImages, Color argColor)
    {
        foreach (Image i in argImages)
        {
            i.color = argColor;
        }
    }
}

public enum WeaponType {None = -1, Melee = 0, Gun = 6};
