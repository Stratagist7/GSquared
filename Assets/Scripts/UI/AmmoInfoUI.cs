using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private Image elementIcon;

    public void SetAmmoCount(int argAmmoCount, int argExtraAmmo)
    {
        ammoCountText.text = $"{argAmmoCount}|{argExtraAmmo}";
    }

    public void SetElementIcon(DamageType argDamageType)
    {
        GameObject elementUI = ElementManager.instance.GetElement(argDamageType).ammoUI;
        elementIcon.sprite = elementUI.GetComponent<Image>().sprite;
        elementIcon.rectTransform.sizeDelta = elementUI.GetComponent<RectTransform>().sizeDelta;
    }
}
