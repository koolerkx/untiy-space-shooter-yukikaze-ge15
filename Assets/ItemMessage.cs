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

    public int height;

    public void Start()
    {
        display(transitionInTime, transitionOffTime);
    }

    public void display(float transitionInTime = 0.5f, float transitionOffTime = 0.5f)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        text.text = message;
        image.sprite = sprite;

        StartCoroutine(SmoothScaleY(gameObject, 0f, 1f, transitionInTime));
        StartCoroutine(DelayEnumerator(SmoothScaleY(gameObject, 1f, 0f, transitionOffTime), showDuration));
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

    System.Collections.IEnumerator DelayEnumerator(System.Collections.IEnumerator function, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(function);
    }
}