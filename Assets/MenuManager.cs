using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;

    public void AddScore(int value)
    {
        score += value;
        Debug.Log($"Score: {score}");
        if (scoreText)
        {
            Debug.Log("Hi");
            scoreText.text = $"Score: {score}";
        }
    }
}