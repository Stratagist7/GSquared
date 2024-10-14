using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public static ElementManager instance;
    
    [SerializeField] private Element[] elements;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }  else
            Destroy(gameObject);
    }
    
    public Element GetElement(DamageType argType)
    {
        return elements[(int)argType];
    }
}
