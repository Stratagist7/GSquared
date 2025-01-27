using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolTip : MonoBehaviour
{
	private readonly string POP_IN = "Tooltip Pop In";
	private readonly string POP_OUT = "Tooltip Pop Out";
	
    [SerializeField] private TextMeshProUGUI toolTipText;
    [SerializeField] private Animation anim;
	private InputActionReference dismissInput;
	bool isDisplaying = false;

	private void Update()
	{
		if (isDisplaying == false || dismissInput == null)
		{
			return;
		}

		if (dismissInput.action.phase == InputActionPhase.Performed)
		{
			DismissTooltip();
		}
	}

	public void DisplayTooltip(string argText, InputActionReference argDismissInput)
	{
		if (isDisplaying)
		{
			print("Already displaying message");
			return;
		}
		isDisplaying = true;
		toolTipText.text = argText;
		dismissInput = argDismissInput;
		anim.Play(POP_IN);
	}

	private void DismissTooltip()
	{
		isDisplaying = false;
		anim.Play(POP_OUT);
	}

}
