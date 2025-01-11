using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

public class PlayerGunAmmo : MonoBehaviour
{
	private static readonly int reloadKey = Animator.StringToHash("t_reload");
	
	[SerializeField] private InputActionReference reloadRef;
	[SerializeField] private InputActionReference actionRef;
	[SerializeField] private AmmoInfoUI ammoUI;
	[SerializeField] private AmmoWheel ammoScreen;
	[SerializeField] private StarterAssetsInputs inputs;
	[SerializeField] private Animator playerAnim;
	[SerializeField] private Animator gunAnim;
	[SerializeField] private int[] startingAmmo = new int[6];

	private int[] ammoAmounts = new int[6];
	private int extraAmmo;
	private int maxAmmo;
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
			ammoAmounts[(int)_damageType] = curAmmo + extraAmmo;
			ammoUI.SetAmmoCount(curAmmo, extraAmmo);
		}
	}
	
	private DamageType _damageType = DamageType.Fire;
	public DamageType damageType
	{
		get => _damageType;
		set
		{
			if (value == _damageType)
			{
				return;
			}
			_damageType = value;

			SetCurrentAmmo();
			
			ammoUI.SetElementIcon(damageType);
		}
	}

	public bool isReloading { get; private set; }

	private void Start()
	{
		for (int i = 0; i < ammoAmounts.Length; i++)
		{
			ammoAmounts[i] = startingAmmo[i];
		}
		
		ammoScreen.gameObject.SetActive(false);
		ResetAmmo();

		reloadRef.action.performed += OnPerformReload;
		actionRef.action.performed += OnPerformAmmoMenu;
		actionRef.action.canceled += OnCancelAmmoMenu;
	}

	private void OnPerformAmmoMenu(InputAction.CallbackContext context)
	{
		if (context.interaction is HoldInteraction)
		{
			ammoScreen.SetAmmo(ammoAmounts);
			DisplayAmmoScreen(true);
		}
	}

	private void OnCancelAmmoMenu(InputAction.CallbackContext context)
	{
		if (context.interaction is HoldInteraction && PlayerHealth.playerIsDead == false)
		{
			DisplayAmmoScreen(false);
		}
	}

	private void OnPerformReload(InputAction.CallbackContext context)
	{
		if (context.interaction is TapInteraction)
		{
			Reload();
		}
	}

	private void OnEnable()
	{
		actionRef.action.Enable();
	}

	private void OnDisable()
	{
		actionRef.action.Disable();
	}

	private void OnDestroy()
	{
		reloadRef.action.performed -= OnPerformReload;
		actionRef.action.performed -= OnPerformAmmoMenu;
		actionRef.action.canceled -= OnCancelAmmoMenu;
	}

	public void Reload(bool force = false)
	{
		if ((curAmmo == maxAmmo || CanReload() == false) && force == false)
		{
			return;
		}
		
		isReloading = true;
		playerAnim.SetTrigger(reloadKey);
	}

	public void ResetAmmo()
	{
		maxAmmo = ElementManager.instance.GetElement(damageType).maxAmmo;
		SetCurrentAmmo();
		ammoUI.SetElementIcon(damageType);
		isReloading = false;
	}

	private void SetCurrentAmmo()
	{
		if (ammoAmounts[(int)damageType] >= maxAmmo)
		{
			extraAmmo = ammoAmounts[(int)damageType] - maxAmmo;
			curAmmo = maxAmmo;
		}
		else
		{
			extraAmmo = 0;
			curAmmo = ammoAmounts[(int)damageType];
		}
	}

	public bool CanShoot()
	{
		return curAmmo > 0 && isReloading == false && MenuUI.Paused == false;
	}

	public bool CanReload()
	{
		return extraAmmo > 0 && isReloading == false && MenuUI.Paused == false;
	}

	private void DisplayAmmoScreen(bool shouldDisplay)
	{
		ammoScreen.gameObject.SetActive(shouldDisplay);
		Cursor.visible = shouldDisplay;
		inputs.LookInput(Vector2.zero);  // Fixes camera spinning if the look input left as non-zero number
		inputs.SetCursorLocked(shouldDisplay == false);
		inputs.cursorInputForLook = shouldDisplay == false;
	}
}
