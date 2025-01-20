using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlotManager : MonoBehaviour
{
    bool isPlanted = false;
    bool isCoined = false;
    bool isWormed =false;
    bool isEnd =false;
    public bool isBlood = false;
    public SpriteRenderer myplant;
    public SpriteRenderer coin;
    public SpriteRenderer worm;
    public GameOverScreen gameOverScreen;
    public BloodBathGD BloodBathGD;
    int plantStage = 0;

    float timer;
    float countD;
    float RaidTime;
    float RaidGoing;
    float ToRot;
    float BloodBathT;

    bool isDry = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;

    float speed = 1f;

    public bool isBought=true;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

    SpriteRenderer plot;

    public ScoreController _scoreController;

    public PlantObject selectedPlant;

    FarmManager fm;

    // Start is called before the first frame update
    void Start()
    {
        _scoreController = FindFirstObjectByType<ScoreController>();
        fm=transform.parent.GetComponent<FarmManager>();
        plot = GetComponent<SpriteRenderer>();
        if (isBought)
        {
            plot.sprite = drySprite;
        }
        else
        {
            plot.sprite = unavailableSprite;
        }


    }

    public void GameOver()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_scoreController.Score<0)
        {
            isEnd = true;
            Debug.Log("Ended");
            gameOverScreen.Setup();
        }
        if (isBlood)
        {
            BloodBathT += Time.deltaTime;
            if (BloodBathT >= 30)
            {
                isBlood = false;
                BloodBathDis();
            }
        }
        if (isPlanted && !isDry)
        {
            //https://medium.com/star-gazers/understanding-time-deltatime-6528a8c2b5c8

            timer -= speed*Time.deltaTime;
            RaidGoing += Time.deltaTime;
            

            if (timer < 0 && plantStage < selectedPlant.plantStages.Length - 1)
            {
                timer = selectedPlant.timeBtwStages;
                plantStage++;
                UpdatePlant();
            }
            if (RaidGoing >= RaidTime )
            {
                RaidAttack();
            }
            if (isWormed)
            {
                ToRot += Time.deltaTime;
                if (ToRot >= 5)
                {
                    isPlanted = false;
                    myplant.gameObject.SetActive(false);
                    Debug.Log("Corupted");
                }
            }
        }
        else if (isDry && !isPlanted)
        {
            countD -= Time.deltaTime;
            if (countD < 0)
            {
                countD = 10;
                DisablePlot();
            }
        }
        
    }

    private void OnMouseDown()
    {
        if (isPlanted)
        {
            if (!fm.isPlanting)
            {
                Ground();
            }
        }
        else
        {
            if (!isCoined)
            {
                if (fm.isPlanting && fm.selectPlant.plant.BuyPrice <= fm.money && isBought)
                {
                    Plant(fm.selectPlant.plant);
                }
            }
            else
            {
                KeepCoin();
            }
        }
        if (fm.isSelecting)
        {
            switch(fm.selectedTool)
            {
                case 1:
                    if(fm.money>=2 && isBought)
                    {
                        fm.Transaction(-2);
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted)
                        {
                            myplant.sprite = selectedPlant.plantStages[plantStage];
                        }
                    }
                    break;
                case 2:
                    if(fm.money >= 20 && isBought)
                    {
                        fm.Transaction(-20);
                        if (speed < 100) speed += 2f;
                    }
                    break;
                case 3:
                    if(fm.money >= 200 && !isBought)
                    {
                        fm.Transaction(-200);
                        isBought = true;
                        plot.sprite = drySprite;
                    }
                    break;
                case 4:
                    if( fm.money >= 15 && isWormed)
                    {
                        fm.Transaction(-15);
                        isWormed = false;
                        worm.gameObject.SetActive(false);
                    }
                    break;
                case 5:
                    if(fm.money >=100 && _scoreController.Score>=10)
                    {
                        fm.Transaction(-100);
                        _scoreController.AddScore(-10);
                        BloodBath();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseOver()
    {
        if(fm.isPlanting)
        {
            if(isPlanted || fm.selectPlant.plant.BuyPrice > fm.money || !isBought)
            {
                plot.color = unavailableColor;
            }
            else
            {
                plot.color = availableColor;
            }
        }
    }

    private void OnMouseExit()
    {
        plot.color = Color.white;
    }

    void Ground()
    {
        if (plantStage==3)
        {
            isPlanted = false;
            myplant.gameObject.SetActive(false);
            coin.gameObject.SetActive(true);
            isCoined = true;
            isDry = true;
            plot.sprite = drySprite;
            speed = 1f;
        } 
        else if (plantStage==4)
        {
            isPlanted = false;
            myplant.gameObject.SetActive(false);
            fm.Transaction(selectedPlant.DiePrice);
            _scoreController.AddScore(-20);
            isDry = true;
            plot.sprite = drySprite;
            speed = 1f;
        }
    }
    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;
        fm.Transaction(-selectedPlant.BuyPrice);
        plantStage = 0;
        RaidTime = Random.Range(20, 100);
        UpdatePlant();
        timer = selectedPlant.timeBtwStages;
        myplant.gameObject.SetActive(true);
        //coin.gameObject.SetActive(false);

    }
    void KeepCoin()
    {
        isCoined = false;
        fm.Transaction(selectedPlant.SellPrice);
        _scoreController.AddScore(10);
        coin.gameObject.SetActive(false);

    }

    void DisablePlot()
    {
        isPlanted = false;
        plot.sprite = unavailableSprite;
        isBought = false;
    }

    void RaidAttack()
    {
        isWormed = true;
        worm.gameObject.SetActive(true);
        RaidGoing = 0;
        //Debug.Log("Worm");
    }

    public void BloodBath()
    {
        isBlood = true;
        BloodBathGD.Setup();
        speed = 10f;
    }

    public void BloodBathDis()
    {
        BloodBathGD.Dis();
    }
    void UpdatePlant()
    {
        if (isDry && isBought)
        {
            myplant.sprite = selectedPlant.dryPlanted;
        }
        else
        {
            myplant.sprite = selectedPlant.plantStages[plantStage];
        }
    }

}
