using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

public class PlayerGunAmmo : MonoBehaviour
{
	private static readonly int reloadKey = Animator.StringToHash("t_reload");
	
	[SerializeField] private int maxAmmo;
	[Space]
	[SerializeField] private InputActionReference actionRef;
	[SerializeField] private TextMeshProUGUI ammoText;
	[SerializeField] private GameObject ammoScreen;
	[SerializeField] private StarterAssetsInputs inputs;
	[SerializeField] private Animator playerAnim;
	[SerializeField] private Animator gunAnim;
	
	private int _curAmmo;
	public int curAmmo
	{
		get => _curAmmo;
		set
		{
			_curAmmo = value;
			if (value < 0)
			{
				_curAmmo = 0;
			}
			ammoText.text = $"{_curAmmo}/{maxAmmo}";
		}
	}

	private bool isReloading = false;

	private void Start()
	{
		ammoScreen.SetActive(false);
		ResetAmmo();

		actionRef.action.performed += context =>
		{
			if (context.interaction is TapInteraction && isReloading == false)
			{
				Reload();
			} else if (context.interaction is HoldInteraction)
			{
				DisplayAmmoScreen(true);
			}
		};
		actionRef.action.canceled += context =>
		{
			if (context.interaction is HoldInteraction)
			{
				DisplayAmmoScreen(false);
			}
		};
	}

	private void OnEnable()
	{
		actionRef.action.Enable();
	}

	private void OnDisable()
	{
		actionRef.action.Disable();
	}
	
	public void Reload()
	{
		if (curAmmo == maxAmmo)
		{
			return;
		}
		
		isReloading = true;
		playerAnim.SetTrigger(reloadKey);
		gunAnim.SetTrigger(reloadKey);
	}

	public void ResetAmmo()
	{
		curAmmo = maxAmmo;
		isReloading = false;
	}

	public bool CanShoot()
	{
		return curAmmo >= 0 && isReloading == false;
	}

	private void DisplayAmmoScreen(bool shouldDisplay)
	{
		ammoScreen.SetActive(shouldDisplay);
		Cursor.visible = shouldDisplay;
		inputs.LookInput(Vector2.zero);  // Fixes camera spinning if the look input left as non-zero number
		inputs.SetCursorLocked(shouldDisplay == false);
		inputs.cursorInputForLook = shouldDisplay == false;
	}
}
