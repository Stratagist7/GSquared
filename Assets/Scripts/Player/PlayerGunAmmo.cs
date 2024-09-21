using System;
using TMPro;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif

public class PlayerGunAmmo : MonoBehaviour
{
	[SerializeField] private int maxAmmo;
	[SerializeField] private InputActionReference actionRef;
	[SerializeField] private TextMeshProUGUI ammoText;
	[SerializeField] private GameObject ammoScreen;
	
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
				ammoScreen.SetActive(true);
			}
		};
		actionRef.action.canceled += context =>
		{
			if (context.interaction is HoldInteraction)
			{
				ammoScreen.SetActive(false);
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

	private void Update()
	{
		
	}
	
	private void Reload()
	{
		curAmmo = maxAmmo;
	}
	
#if ENABLE_INPUT_SYSTEM
	public void OnReload(InputValue value)
	{
		
	}
#endif
}
