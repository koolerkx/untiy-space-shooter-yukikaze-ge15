using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemMessage : MonoBehaviour
{
    public Text text;
    public Image image;

    public string message;
    public float showDuration;
    public Sprite sprite;

    public float transitionInTime = 0.5f;
    public float transitionOffTime = 0.5f;

    public int height = 100;
    private RectTransform _rect;

    public GameObject progressBar;

    public void Start()
    {
        _rect = GetComponent<RectTransform>();
        display(transitionInTime, transitionOffTime);
    }

    public void display(float transitionInTime = 0.5f, float transitionOffTime = 0.5f)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        text.text = message;
        image.sprite = sprite;

        StartCoroutine(SmoothChangeHeight(0f, height, transitionInTime));
        StartCoroutine(DelayEnumerator(SmoothChangeHeight(height, 0f, transitionOffTime), showDuration));
        StartCoroutine(DelayDestroy(showDuration + transitionOffTime));
        
        StartCoroutine(SmoothScaleX(progressBar, 1f, 0f, showDuration));
    }

    private IEnumerator SmoothChangeHeight(float from, float to, float duration)
    {
        float elapsed = 0f;
        if (!_rect) yield break;
        Vector2 size = _rect.sizeDelta;
        float startHeight = from;
        float endHeight = to;
        while (elapsed < duration)
        {
            float h = Mathf.Lerp(startHeight, endHeight, elapsed / duration);
            size.y = h;
            _rect.sizeDelta = size;
            elapsed += Time.deltaTime;
            yield return null;
        }

        size.y = endHeight;
        _rect.sizeDelta = size;
    }

    IEnumerator DelayEnumerator(IEnumerator function, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(function);
    }

    IEnumerator DelayDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }


    private IEnumerator SmoothScaleX(GameObject bar, float from, float to, float duration)
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
}