using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

public class PlayerGunAmmo : MonoBehaviour
{
	private static readonly int reloadKey = Animator.StringToHash("t_reload");
	
	[SerializeField] private InputActionReference actionRef;
	[SerializeField] private TextMeshProUGUI ammoText;
	[SerializeField] private GameObject ammoScreen;
	[SerializeField] private Image ammoType;
	[SerializeField] private StarterAssetsInputs inputs;
	[SerializeField] private Animator playerAnim;
	[SerializeField] private Animator gunAnim;

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
			ammoText.text = $"{_curAmmo}/{maxAmmo}";
		}
	}
	private DamageType _damageType = DamageType.Fire;
	public DamageType damageType
	{
		get => _damageType;
		set
		{
			if (value != _damageType)
			{
				_damageType = value;
				Reload(true);
			}
		}
	}

	public bool isReloading { get; private set; }

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
	
	public void Reload(bool force = false)
	{
		if (curAmmo == maxAmmo && force == false)
		{
			return;
		}
		
		isReloading = true;
		playerAnim.SetTrigger(reloadKey);
	}

	public void ResetAmmo()
	{
		maxAmmo = ElementManager.instance.GetElement(damageType).maxAmmo;
		curAmmo = maxAmmo;
		ammoType.color = ElementManager.instance.GetElement(damageType).color;
		isReloading = false;
	}

	public bool CanShoot()
	{
		return curAmmo > 0 && CanReload();
	}

	public bool CanReload()
	{
		return isReloading == false && PauseMenu.Paused == false;
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
