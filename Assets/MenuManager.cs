using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public int score;
    public Text scoreText;
    public Text lifeText;
    
    public GameObject hpBar;
    public float hpBarTransitionTime = 0.5f;

    public StartMessage StartMessage;
    public GameOverPanel gameOverPanel;
    
    public int killCount = 0;

    private void Start()
    {
        score = 0;
        if (scoreText)
        {
            // scoreText.text = $"Score: {score}";
            scoreText.text = $"{score}";
        }

        StartMessage.Display();
    }

    public void SetLife(int life)
    {
        if (lifeText)
        {
            lifeText.text = $"Life: {life}";
        }
    }

    public void SetHpBar(float percent)
    {
        if (hpBar)
        {
            Vector3 scale = hpBar.transform.localScale;
            StopAllCoroutines();
            StartCoroutine(SmoothScaleX(hpBar, scale.x, percent, hpBarTransitionTime));
        }
    }

    private System.Collections.IEnumerator SmoothScaleX(GameObject bar, float from, float to, float duration)
    {
        float elapsed = 0f;
        Vector3 scale = bar.transform.localScale;
        while (elapsed < duration)
        {
            float x = Mathf.Lerp(from, to, elapsed / duration);
            scale.x = x;
            bar.transform.localScale = scale;
            elapsed += Time.deltaTime;
            yield return null;
        }
        scale.x = to;
        bar.transform.localScale = scale;
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
            // scoreText.text = $"Score: {value}";
            scoreText.text = $"{score}";
        }
    }
    
    public void AddKill(int value = 1)
    {
        killCount += value;
    }
    
    
    public void EndGame()
    {
        gameOverPanel.Display(score, killCount);
    }

    public void RestartGame()
    {
        // todo
    }
    
    public void BackToMenu()
    {
        // todo
    }
}