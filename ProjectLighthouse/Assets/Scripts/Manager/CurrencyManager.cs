using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum CurrencyType
{
    Gold,
    Lumber,
    Iron,
    Firelight
}

public class CurrencyManager
{
    Dictionary<CurrencyType, ReactiveProperty<int>> currencyDict = new();

    static CurrencyManager instance;

    public static CurrencyManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new CurrencyManager();
                instance.Init();
            }

            return instance;
        }
    }

    public void Init()
    {
        currencyDict.Add(CurrencyType.Gold, new ReactiveProperty<int>(500));
    }

    public ReactiveProperty<int> SubscribeCurrency(CurrencyType type)
    {
        try
        {
            return currencyDict[type];
        }
        catch 
        {
            Debug.Log("Currency not implemented");
            throw new System.Exception("Currency not implemented");
        }
    }

    public void Add(CurrencyType type, int amount)
    {
        if(currencyDict.ContainsKey(type))
        {
            currencyDict[type].Value += amount;
        }
    }

    public void AddCoin(int amount)
    {
        Add(CurrencyType.Gold, amount);
    }

    public bool CheckCost(CurrencyType type, int cost)
    {
        if (currencyDict.ContainsKey(type))
        {
            return currencyDict[type].Value >= cost;
        }

        return false;
    }
}
