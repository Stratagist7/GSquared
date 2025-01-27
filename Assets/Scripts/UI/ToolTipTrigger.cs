using UnityEngine;
using UnityEngine.InputSystem;

public class ToolTipTrigger : MonoBehaviour
{
    [SerializeField] private ToolTip tooltip;
    [SerializeField] private InputActionReference dismissInput;
    private void OnTriggerEnter(Collider other)
    {
        tooltip.DisplayTooltip("Hold TAB to change ammo type", dismissInput);
    }
}
