using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField] GameObject _unitPrefab;
    [SerializeField] Text _priceText;
    [SerializeField] Barack _barack;

    private void Start()
    {
        _priceText.text = _unitPrefab.GetComponent<Unit>().Price.ToString();
    }

    public void TryBuy()
    {
        int price = _unitPrefab.GetComponent<Unit>().Price;
        if (FindObjectOfType<Resources>().Money >= price)
        {
            FindObjectOfType<Resources>().Money -= price;

            //Создать юнита
            _barack.CreateUnit(_unitPrefab);
        }
        else
        {
            Debug.Log("Мало денег");
        }

    }
}
