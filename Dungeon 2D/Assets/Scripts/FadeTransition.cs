using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    public static FadeTransition instance;

    public Image blackImage; 
    public float fadeDuration = 1f; // Duración del desvanecimiento

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeToBlack()
    {
        StartCoroutine(Fade(1f)); // Fade out
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Fade(0f)); // Fade in
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Color color = blackImage.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            blackImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        blackImage.color = color;
    }
}
