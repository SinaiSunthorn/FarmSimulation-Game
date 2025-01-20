using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public int Score { get; private set; }
    public int Start_score = 0;
    public GameObject _MyScore;
    TextMeshProUGUI MyScore_text;

    private void Start()
    {
        Score = Start_score;
        MyScore_text = _MyScore.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        MyScore_text.text = "Score:" + Score;
    }
    public void AddScore(int amount)
    {
        Score += amount;

    }

}







//public ScoreController _scoreController;

//void Start()
//{
//    _scoreController = FindObjectOfType<ScoreController>();
//}
