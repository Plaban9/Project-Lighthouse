using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CurrencyBar : MonoBehaviour
{
    [SerializeField] CurrencyType currencyType = CurrencyType.Gold;
    [SerializeField] TMPro.TextMeshProUGUI amountText;

    // Start is called before the first frame update
    void Start()
    {
        CurrencyManager.Instance.SubscribeCurrency(CurrencyType.Gold).Subscribe(x =>
        {
            amountText.text = x.ToString();
        }).AddTo(this);

    }
    
}
