using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControl : MonoBehaviour
{
    private static readonly int stateKey = Animator.StringToHash("i_state_index");
    private static readonly int indexKey = Animator.StringToHash("i_attack_index");
    private static readonly int endKey = Animator.StringToHash("t_end");
    public static WeaponType _state;

    [SerializeField] private WeaponType initialState;
    [SerializeField] private bool hasGun = false;
    [SerializeField] private Animator animator;
    [Space]
    [Header("Gun Controllers")]
    [SerializeField] private GameObject gun;
    [SerializeField] private PlayerGunAmmo ammo;
    [SerializeField] private GameObject ammoUI;
    [SerializeField] private Image[] gunImage;
    [Space]
    [Header("Melee Controllers")]
    [SerializeField] private MeleeCombat melee;
    [SerializeField] private Image[] fistImage;
    

    private void Awake()
    {
        // Backup if forgot to change
        if (hasGun == false)
        {
            if (initialState == WeaponType.Gun)
            {
                hasGun = true;
            }
            else
            {
                foreach (Image i in gunImage)
                {
                    i.gameObject.SetActive(false);
                }
            }
        }
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
        if (argWeapon == WeaponType.Gun && hasGun == false)
        {
            return;
        }
        
        _state = WeaponType.None;
        animator.SetTrigger(endKey);
        animator.SetInteger(stateKey, (int)argWeapon);
    }

    public void EnableMelee()
    {
        _state = WeaponType.Melee;
        gun.SetActive(false);
        ammo.enabled = false;
        ammoUI.SetActive(false);
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
        
        animator.SetInteger(indexKey, -1); // Ensures no lingering attack triggers cause weird animations 
        _state = WeaponType.Gun;
        gun.SetActive(true);
        ammo.enabled = true;
        ammoUI.SetActive(true);
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
