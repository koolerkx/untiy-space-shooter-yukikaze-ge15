using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    private int _killCount;
    private int _score;

    public Text killCountText;
    public Text scoreText;

    public Button restartButton;
    public Button menuButton;

    private int _buttonSelectedIndex;

    private float _lastHorizontalInputTime;
    private readonly float _horizontalInputCooldown = 0.3f;

    public void Display(int score, int killCount)
    {
        _score = score;
        _killCount = killCount;

        scoreText.text = $"ìæì_ÅF{_score}";
        scoreText.text = $"åÇíƒêîÅF{_killCount}";

        restartButton.Select();
        _buttonSelectedIndex = 0;

        Vector3 scale = transform.localScale;
        scale.y = 0f;
        transform.localScale = scale;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(SmoothStartMessageSequence(gameObject));
    }

    private System.Collections.IEnumerator SmoothStartMessageSequence(GameObject obj)
    {
        yield return StartCoroutine(SmoothStartMessageScaleY(obj, 0f, 1f, 0.5f));
    }

    private System.Collections.IEnumerator SmoothStartMessageScaleY(GameObject obj, float from, float to,
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetButtonDown("Fire1") ||
            Input.GetKeyDown(KeyCode.Space)
           )
        {
            if (_buttonSelectedIndex == 0)
            {
                restartButton.onClick.Invoke();
            }
            else
            {
                menuButton.onClick.Invoke();
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        bool left = Input.GetKeyDown(KeyCode.LeftArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);
        float now = Time.unscaledTime;
        bool canInput = (now - _lastHorizontalInputTime) > _horizontalInputCooldown;

        if (Input.GetKeyDown(KeyCode.Tab) ||
            (canInput && (right || horizontal > 0.5f)) ||
            (canInput && (left || horizontal < -0.5f)))
        {
            if (right || horizontal > 0.5f || Input.GetKeyDown(KeyCode.Tab))
            {
                _buttonSelectedIndex = (_buttonSelectedIndex + 1) % 2;
            }
            else if (left || horizontal < -0.5f)
            {
                _buttonSelectedIndex = (_buttonSelectedIndex + 1) % 2;
            }
            _lastHorizontalInputTime = now;
        }
        
        if (_buttonSelectedIndex == 0)
        {
            restartButton.Select();
        }
        else
        {
            menuButton.Select();
        }
    }
}