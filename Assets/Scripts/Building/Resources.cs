using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{

    public Text MoneyText;
    public int Money;

    private void Update()
    {
        MoneyText.text = Money.ToString();
    }
    public void AddMoney(int money)
    {
        Money += money;
    }


}
