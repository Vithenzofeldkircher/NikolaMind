using System.Collections;
using UnityEngine;

public class FadeInEffect : MonoBehaviour, ITransitionEfect
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1.0f;

    public IEnumerator PlayEffect()
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f; // Começa invisível

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}