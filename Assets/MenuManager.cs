using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameState
{
    Transition,
    InGame,
    GameEnd
}

public class MenuManager : MonoBehaviour
{
    public int score;
    public GameObject scorePanel;
    public Text scoreText;
    
    public GameObject hpPanel;
    public GameObject hpBar;
    public float hpBarTransitionTime = 0.5f;
    public float transitionTime = 0.5f;

    public StartMessage startMessage;
    public GameOverPanel gameOverPanel;
    
    public GameState gameState = GameState.Transition;
    
    public int killCount;
    
    public string menuSceneName = "Menu";

    private void Start()
    {
        score = 0;
        if (scoreText)
        {
            scoreText.text = $"{score}";
        }

        startMessage.Display();
        StartCoroutine(SmoothScaleY(scorePanel, 0f, 1f, transitionTime));
        StartCoroutine(SmoothScaleY(hpPanel, 0f, 1f, transitionTime));

        StartCoroutine(GameStateTransitionDelay(GameState.InGame, transitionTime));
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
            scoreText.text = $"{value}";
        }
    }
    
    public void AddKill(int value = 1)
    {
        killCount += value;
    }
    
    
    public void EndGame()
    {
        StartCoroutine(GameStateTransitionDelay(GameState.GameEnd, transitionTime));
        gameOverPanel.Display(score, killCount);
    }

    public void RestartGame()
    {
        StartCoroutine(SmoothScaleY(scorePanel, 1f, 0f, transitionTime));
        StartCoroutine(SmoothScaleY(hpPanel, 1f, 0f, transitionTime));
        StartCoroutine(SmoothScaleY(gameOverPanel.gameObject, 1f, 0f, transitionTime));
        
        StartCoroutine(DelayRestart(transitionTime));
    }
    
    public void BackToMenu()
    {
        StartCoroutine(SmoothScaleY(scorePanel, 1f, 0f, transitionTime));
        StartCoroutine(SmoothScaleY(hpPanel, 1f, 0f, transitionTime));
        StartCoroutine(SmoothScaleY(gameOverPanel.gameObject, 1f, 0f, transitionTime));
        
        StartCoroutine(DelayBackToMenu(transitionTime));
    }

    private System.Collections.IEnumerator SmoothScaleY(GameObject obj, float from, float to,
        float duration)
    {
        float elapsed = 0f;
        Vector3 scale = obj.transform.localScale;
        while (elapsed < duration)
        {
            float y = Mathf.Lerp(from, to, elapsed / duration);
            scale.y = y;
            obj.transform.localScale = scale;
            elapsed += Time.deltaTime;
            yield return null;
        }

        scale.y = to;
        obj.transform.localScale = scale;
    }
    
    
    System.Collections.IEnumerator GameStateTransitionDelay(GameState state, float delay)
    {
        yield return new WaitForSeconds(transitionTime + 0.1f);
        gameState = state;
    }
    
    
    System.Collections.IEnumerator DelayBackToMenu(float delay)
    {
        yield return new WaitForSeconds(transitionTime + 0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneName);
    }
    
    System.Collections.IEnumerator DelayRestart(float delay)
    {
        yield return new WaitForSeconds(transitionTime + 0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}