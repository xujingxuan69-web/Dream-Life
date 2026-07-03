using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int currency;    //¾ÖÍâ»õ±Ò

    #region Extra Judgement
    public bool dashExtra;
    public bool jumpExtra;
    #endregion

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughCurrency(int _price)
    {
        if (currency >= _price)
        {
            currency -= _price;
            currency = Mathf.Clamp(currency, 0, int.MaxValue);
            return true;
        }
        return false;
    }
}
