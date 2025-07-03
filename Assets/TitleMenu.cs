using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public GameObject titleLogo;
    public GameObject menuPanel;
    
    public Button startButton;
    public Button quitButton;
    
    public string gameScene = "GameScene";
    
    private int _buttonSelectedIndex;

    private float _lastHorizontalInputTime;
    private readonly float _horizontalInputCooldown = 0.3f;

    public float transitionTime = 0.5f;

    public AudioSource bgmAudioSource;
    public AudioSource menuClickAudioSource;
    public AudioSource buttonSelectAudioSource;

    private void Start()
    {
        Cursor.visible = false;
        InputSystem.DisableDevice(Mouse.current); 
        
        StartSequence();
    }
    
    private void StartSequence()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothScaleY(titleLogo, 0f, 1f, transitionTime));
        StartCoroutine(SmoothScaleY(menuPanel, 0f, 1f, transitionTime));
    }

    public void StartGameSequence()
    {
        StartCoroutine(SmoothScaleY(titleLogo, 1f, 0f, transitionTime));
        StartCoroutine(SmoothScaleY(menuPanel, 1f, 0f, transitionTime));
        if (bgmAudioSource != null)
        {
            StartCoroutine(FadeOutAudio(bgmAudioSource, transitionTime));
        }
        StartCoroutine(WaitAndLoadScene());
    }

    public void ExitGame()
    {
        Application.Quit();
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
    
    System.Collections.IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(transitionTime + 0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene);
    }
    
    private System.Collections.IEnumerator FadeOutAudio(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetButtonDown("Fire1") ||
            Input.GetKeyDown(KeyCode.Space)
           )
        {
            menuClickAudioSource.Play();
            if (_buttonSelectedIndex == 0)
            {
                startButton.onClick.Invoke();
            }
            else
            {
                quitButton.onClick.Invoke();
            }
        }

        float horizontal = Input.GetAxisRaw("Vertical");
        bool left = Input.GetKeyDown(KeyCode.UpArrow);
        bool right = Input.GetKeyDown(KeyCode.DownArrow);
        float now = Time.unscaledTime;
        bool canInput = (now - _lastHorizontalInputTime) > _horizontalInputCooldown;

        if (Input.GetKeyDown(KeyCode.Tab) ||
            (canInput && (right || horizontal > 0.5f)) ||
            (canInput && (left || horizontal < -0.5f)))
        {
            buttonSelectAudioSource.Play();
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
            startButton.Select();
        }
        else
        {
            quitButton.Select();
        }
    }
}
