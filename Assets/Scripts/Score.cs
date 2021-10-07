using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{

    int score;
    int highscore;

    private void Awake()
    {
        var arr = FindObjectsOfType<Score>();
        if (arr.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        FindObjectOfType<GameSession>().UpdateDisplay();
        highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    public void AddScore()
    {
        score++;
        FindObjectOfType<GameSession>().UpdateDisplay();
    }

    public void Lose()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
    }

    public void ResetScore()
    {
        score = 0;
        FindObjectOfType<GameSession>().UpdateDisplay();
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighscore()
    {
        return highscore;
    }
}
