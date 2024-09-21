using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGunAmmo : MonoBehaviour
{
    [SerializeField] private int maxAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    
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
        Reload();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Reload();
        }
    }

    private void Reload()
    {
        curAmmo = maxAmmo;
    }
}
