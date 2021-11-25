using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    public int MoneyAdd = 5;
    public float MoneyPeriod = 10f;
    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= MoneyPeriod)
        {
            _timer = 0f;
            FindObjectOfType<Resources>().AddMoney(MoneyAdd);
        }
    }
}
