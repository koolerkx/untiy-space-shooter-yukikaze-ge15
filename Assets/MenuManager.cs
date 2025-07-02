using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public int score;
    public Text scoreText;
    public Text lifeText;

    private void Start()
    {
        score = 0;
        if (scoreText)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void SetLife(int life)
    {
        if (lifeText)
        {
            lifeText.text = $"Life: {life}";
        }
    }

    public void AddScore(int value)
    {
        score += value;
        SetScore(score);
    }
    
    public void SetScore(int value)
    {
        if (scoreText)
        {
            scoreText.text = $"Score: {value}";
        }
    }
}