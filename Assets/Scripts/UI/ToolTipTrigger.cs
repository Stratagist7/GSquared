using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToolTipTrigger : MonoBehaviour
{
    [SerializeField] private ToolTip tooltip;
    [SerializeField] private string tooltipText;
    [SerializeField] private InputActionReference dismissInput;
    private void OnTriggerEnter(Collider other)
    {
        tooltip.DisplayTooltip(tooltipText, dismissInput);
        Destroy(this);
    }
}
