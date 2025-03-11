using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateInteraction : Interactable
{
    [SerializeField] private GateControl airlockGate;
    [SerializeField] private GateControl entryGate;
    
    void Start()
    {
        interactAction = Move;
    }

    private void Move(Collider other = null)
    {
        airlockGate.MoveGate(false);
        entryGate.MoveGate(true, false);
    }
}
