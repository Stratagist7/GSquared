using UnityEngine;

public class GateInteraction : Interactable
{
    [SerializeField] private GateControl airlockGate;
    [SerializeField] private GateControl entryGate;
    [SerializeField] private GameObject navLink;
    
    void Start()
    {
        interactAction = Move;
    }

    private void Move(Collider other = null)
    {
        airlockGate.MoveGate(false);
        entryGate.MoveGate(true, false);
        interactText.SetActive(false);
        navLink.SetActive(false);
        Destroy(gameObject);
    }
}
