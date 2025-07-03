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
    private RectTransform rect;
    
    public void Start()
    {
        rect = GetComponent<RectTransform>();
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
    }

    private System.Collections.IEnumerator SmoothChangeHeight(float from, float to, float duration)
    {
        float elapsed = 0f;
        if (!rect) yield break;
        Vector2 size = rect.sizeDelta;
        float startHeight = from;
        float endHeight = to;
        while (elapsed < duration)
        {
            float h = Mathf.Lerp(startHeight, endHeight, elapsed / duration);
            size.y = h;
            rect.sizeDelta = size;
            elapsed += Time.deltaTime;
            yield return null;
        }
        size.y = endHeight;
        rect.sizeDelta = size;
    }

    System.Collections.IEnumerator DelayEnumerator(System.Collections.IEnumerator function, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(function);
    }
    
    System.Collections.IEnumerator DelayDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}