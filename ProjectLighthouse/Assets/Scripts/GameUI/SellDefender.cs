using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SellDefender : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI amountText;
    [SerializeField] Button sellButton;

    Subject<bool> OnSell;
    public Subject<bool> Show(Transform pos, DefenderData data)
    {
        OnSell = new();
        gameObject.SetActive(true);
        gameObject.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos.transform.TransformPoint(new Vector3(5, 3, 0)));
        amountText.text = ((int)(data.cost * 0.5f)).ToString();

        return OnSell;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        OnSell?.Dispose();
    }

    public void OnClickSell()
    {
        OnSell?.OnNext(true);
        Hide();
    }
}
