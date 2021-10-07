using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSession : MonoBehaviour
{

    [SerializeField] Canvas loseCanvas;
    [SerializeField] Canvas gameCanvas;
    [SerializeField] Canvas winCanvas;
    [SerializeField] Text gameScoreText;
    [SerializeField] Text loseScoreText;
    [SerializeField] Text loseHighcoreText;

    // Start is called before the first frame update
    void Start()
    {
        loseCanvas.enabled = false;
        winCanvas.enabled = false;
        UpdateDisplay();
    }

    // Update is called once per frame
    public void UpdateDisplay()
    {
        gameScoreText.text = FindObjectOfType<Score>().GetScore().ToString();
    }

    public void Lose()
    {
        loseCanvas.enabled = true;
        gameCanvas.enabled = false;
        FindObjectOfType<Score>().Lose();
        loseScoreText.text = FindObjectOfType<Score>().GetScore().ToString();
        loseHighcoreText.text = FindObjectOfType<Score>().GetHighscore().ToString();
        FindObjectOfType<Score>().ResetScore();
    }

    public void Win()
    {
        winCanvas.enabled = true;
        gameCanvas.enabled = false;
        FindObjectOfType<Score>().AddScore();
    }
}
