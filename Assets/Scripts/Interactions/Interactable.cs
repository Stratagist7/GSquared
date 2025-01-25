using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject interactText;
    [SerializeField] private InputActionReference interactInput;
    protected Action<Collider> interactAction;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
            interactText.SetActive(false);
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")){
            if (interactInput.action.phase == InputActionPhase.Performed)
            {
                interactAction?.Invoke(other);
            }
        }
    }
}
