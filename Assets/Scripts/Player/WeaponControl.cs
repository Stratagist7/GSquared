using UnityEngine;
using UnityEngine.UI;

public class WeaponControl : MonoBehaviour
{
    private static readonly int stateKey = Animator.StringToHash("i_state_index");
    private static readonly int endKey = Animator.StringToHash("t_end");
    private const int MIDDLE_STATE = -1;
    private const int MELEE_STATE = 0;
    private const int GUN_STATE = 6;
    private static int state = 6;
    
    [SerializeField] private Animator animator;
    [Header("Gun Controllers")]
    [SerializeField] private GameObject gun;
    [SerializeField] private PlayerGunAmmo ammo;
    [SerializeField] private Image[] gunImage;
    [Header("Melee Controllers")]
    [SerializeField] private Image[] fistImage;

    public void OnSwapToGun()
    {
        if (state == GUN_STATE || state == MIDDLE_STATE || MenuUI.Paused)
        {
            return;
        }
        SwapWeapons(true);
    }
    
    public void OnSwapToMelee()
    {
        if (state == MELEE_STATE || state == MIDDLE_STATE || MenuUI.Paused)
        {
            return;
        }
        SwapWeapons(false);
    }

    private void SwapWeapons(bool argSwapToGun)
    {
        state = MIDDLE_STATE;
        animator.SetTrigger(endKey);
        animator.SetInteger(stateKey, argSwapToGun ? GUN_STATE : MELEE_STATE);
    }

    public void EnableMelee()
    {
        state = MELEE_STATE;
        gun.SetActive(false);
        ammo.enabled = false;
        ChangeColor(gunImage, Color.black);
        // enable melee stuff
        ChangeColor(fistImage, Color.white);
    }
    
    public void EnableGun()
    {
        state = GUN_STATE;
        gun.SetActive(true);
        ammo.enabled = true;
        ChangeColor(gunImage, Color.white);
        // disable melee stuff
        ChangeColor(fistImage, Color.black);
    }

    private void ChangeColor(Image[] argImages, Color argColor)
    {
        foreach (Image i in argImages)
        {
            i.color = argColor;
        }
    }
}
