using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIPSection : MonoBehaviour
{
    [SerializeField] private GameObject wipText;

    private void OnTriggerEnter(Collider other)
    {
        wipText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        wipText.SetActive(false);
    }
}
