using UnityEngine;
using UnityEngine.UI;

public class PlnatItem : MonoBehaviour
{
    public PlantObject plant;
    public Text nameTxt;
    public Text priceTxt;
    //public Image icon;

    public Image btnImage;
    public Text btnTxt;

    FarmManager fm;

    void Start()
    {
        fm = FindFirstObjectByType<FarmManager>();
        InitializeUI();
        
    }
    public void BuyPlant() 
    {
        Debug.Log("Bought" + plant.plantName);
        fm.SelectPlant(this);
    }

    void InitializeUI()
    {
        nameTxt.text = plant.plantName;
        priceTxt.text = "$" + plant.BuyPrice;
        //icon.sprite = plant.icon;
    }
}
