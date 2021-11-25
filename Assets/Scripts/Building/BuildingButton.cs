using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public BuildingPlacer BuildingPlacer;
    [SerializeField] GameObject BuildingPrefab;
    [SerializeField] Text _buildText;


    private void Start()
    {
        int price = BuildingPrefab.GetComponent<Building>().Price;
        _buildText.text = price.ToString();
    }
    public void TryBuy()
    {
        int price = BuildingPrefab.GetComponent<Building>().Price;
        if(FindObjectOfType<Resources>().Money >= price)
        {
            FindObjectOfType<Resources>().Money -= price;
            BuildingPlacer.CreateBuilding(BuildingPrefab);

        }
        else
        {
            Debug.Log("Мало денег");
        }

    }
    public void BuyBuild()
    {
        int price = BuildingPrefab.GetComponent<Building>().Price;
        _buildText.text = price.ToString();
    }
}
