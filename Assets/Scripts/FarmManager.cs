using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public PlnatItem selectPlant;
    public bool isPlanting = false;
    public int money = 100;
    public Text moneyTxt;
    public PlotManager PM;

    public Color buyColor = Color.red;
    public Color cancelColor = Color.green;

    public bool isSelecting = false;
    public int selectedTool = 0;

    public Image[] buttonsImg;
    public Sprite normalButton;
    public Sprite selectedButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyTxt.text = "$" + money;
    }

    // Update is called once per frame
    
    public void SelectPlant(PlnatItem newPlan)
    {
        if (selectPlant == newPlan)
        {
            CheckSelection();

        }
        else
        {
            CheckSelection();
            selectPlant = newPlan;
            selectPlant.btnImage.color = cancelColor;
            selectPlant.btnTxt.text = "Cancel";
            Debug.Log("Selected" + selectPlant.plant.plantName);
            isPlanting = true;
        }
    }

    public void SelectTool(int toolNumber)
    {
        if (toolNumber == selectedTool)
        {
            CheckSelection();
        }
        else
        {
            CheckSelection();
            isSelecting = true;
            selectedTool = toolNumber;
            buttonsImg[toolNumber-1].sprite = selectedButton;
        }
    }

    void CheckSelection()
    {
        isPlanting = false ;
        if (selectPlant != null)
        {
            selectPlant.btnImage.color = buyColor;
            selectPlant.btnTxt.text = "Buy";
            selectPlant = null;
        }
        if(isSelecting)
        {
            if(selectedTool>0)
            {
                buttonsImg[selectedTool - 1].sprite = normalButton;
            }
            isSelecting = false ;
            selectedTool = 0 ;
        }
    }

    public void Transaction(int value)
    {
        Debug.Log("Transaction");
        money += value;
        moneyTxt.text = "$" + money;

    }
}
