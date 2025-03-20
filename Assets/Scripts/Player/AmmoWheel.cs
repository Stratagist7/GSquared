using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class AmmoWheel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] ammoTexts;
    [SerializeField] private RectTransform[] ammoObjects;
    [SerializeField] private RectTransform mask;
    [SerializeField] private PlayerGunAmmo ammo;
    [Space]
    [SerializeField] private Vector3 bigMaskScale;
    [SerializeField] private Vector3 smallMaskScale;
    [SerializeField] private Vector3 smallBgScale;
    [SerializeField] private Vector3 bigBgScale;
    [Space]
    [SerializeField] private float shrinkDuration = 0.1f;
    [SerializeField] private float growDuration = 0.2f;
    
    private readonly Dictionary<int, DamageType> ammoAngle = new Dictionary<int, DamageType>()
    {
        {0, DamageType.Wind},
        {60, DamageType.Earth},
        {120, DamageType.Fire},
        {180, DamageType.Lightning},
        {-180, DamageType.Lightning},
        {-120, DamageType.Water},
        {-60, DamageType.Ice}
    };

    private DamageType curType = DamageType.None;
    private DamageType lastType = DamageType.None;
    private Vector2 mouseLoc;
	
    private void OnEnable()
    {
        lastType = DamageType.None;
    }

    private void OnDisable()
    {
        ammo.damageType = curType;
    }
    
    private void Update()
    {
        if (Mathf.Approximately(mouseLoc.x, Input.mousePosition.x) && Mathf.Approximately(mouseLoc.y, Input.mousePosition.y))
        {
            return;
        }
		
        mouseLoc.x = Input.mousePosition.x - Screen.width * 0.5f;
        mouseLoc.y = Input.mousePosition.y - Screen.height * 0.5f;
        mouseLoc.Normalize();

        if (mouseLoc == Vector2.zero)
        {
            return;
        }
        
        float angle = Mathf.Atan2(mouseLoc.y, mouseLoc.x) * Mathf.Rad2Deg + 30;
        int closeAngle = GetClosestAngle(angle);

        if (curType == ammoAngle[closeAngle])
        {
            return;
        }

        lastType = curType;
        SetSize((int)ammoAngle[closeAngle]);
        curType = ammoAngle[closeAngle];
    }

    private int GetClosestAngle(float argAngle)
    {
        int closestAngle = 0;
        double minDif = Double.MaxValue;

        foreach (int angle in ammoAngle.Keys)
        {
            double dif = Math.Abs(argAngle - angle);
            if (dif < minDif)
            {
                closestAngle = angle;
                minDif = dif;
            }
        }
		
        return closestAngle;
    }

    private void SetSize(int growIndex)
    {
        // Shrink the last one
        if (lastType != DamageType.None)
        {
            StartCoroutine(ChangeSize((int)lastType, true));
        }

        // grow the intended one
        StartCoroutine(ChangeSize(growIndex));
    }
    
    private IEnumerator ChangeSize(int ammoIndex, bool isShrink = false)
    {
        float time = 0;
        float duration = growDuration;
        Vector3 maskStartValue = bigMaskScale;
        Vector3 maskEndValue = smallMaskScale;
        Vector3 ammoStartValue = ammoObjects[ammoIndex].localScale;
        Vector3 ammoEndValue = bigBgScale;
        ammoObjects[ammoIndex].SetSiblingIndex(2);
        
        if (isShrink)
        {
            duration = shrinkDuration;
            maskStartValue = smallMaskScale;
            maskEndValue = bigMaskScale;
            // ammoStartValue = bigBgScale;
            ammoEndValue = smallBgScale;
            ammoObjects[ammoIndex].SetSiblingIndex(7);
        }
        
        while (time < duration)
        {
            mask.localScale = Vector3.Lerp(maskStartValue, maskEndValue, time / duration);
            ammoObjects[ammoIndex].localScale = Vector3.Lerp(ammoStartValue, ammoEndValue, time / duration);
            
            time += Time.deltaTime;
            yield return null;
        }
        mask.localScale = maskEndValue;
        ammoObjects[ammoIndex].localScale = ammoEndValue;
    }

    public void SetAmmo(int[] argAmmoAmounts)
    {
        for (int i = 0; i < ammoTexts.Length; i++)
        {
            ammoTexts[i].text = argAmmoAmounts[i].ToString();
        }
    }
}
