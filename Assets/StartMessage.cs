using UnityEngine;

public class StartMessage : MonoBehaviour
{
    public void Display()
    {
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
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(SmoothStartMessageScaleY(obj, 1f, 0f, 0.5f));
        obj.SetActive(false);
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
}