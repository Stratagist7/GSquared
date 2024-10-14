using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Element : ScriptableObject
{
    public DamageType damageType;
    public int damage;
    public int maxAmmo;
    public Color color;
}
