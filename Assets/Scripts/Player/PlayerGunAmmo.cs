using StarterAssets;
using TMPro;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

public class PlayerGunAmmo : MonoBehaviour
{
	[SerializeField] private int maxAmmo;
	[Space]
	[SerializeField] private InputActionReference actionRef;
	[SerializeField] private TextMeshProUGUI ammoText;
	[SerializeField] private GameObject ammoScreen;
	[SerializeField] private StarterAssetsInputs inputs;
	
	private int _curAmmo;
	public int curAmmo
	{
		get => _curAmmo;
		set
		{
			_curAmmo = value;
			ammoText.text = $"{_curAmmo}/{maxAmmo}";
		}
	}

	private void Start()
	{
		ammoScreen.SetActive(false);
		Reload();

		actionRef.action.performed += context =>
		{
			if (context.interaction is TapInteraction)
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
		curAmmo = maxAmmo;
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
