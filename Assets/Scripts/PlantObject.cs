using UnityEngine;

[CreateAssetMenu(fileName ="New Plant", menuName ="Plant")]

public class PlantObject : ScriptableObject
{
    public string plantName;
    public Sprite[] plantStages;
    public float timeBtwStages;
    public int BuyPrice;
    public int SellPrice;
    public int DiePrice;
    public Sprite dryPlanted;
    //public Sprite icon;
}
