using System.Collections;
using UnityEngine;

public class SlideUpEffect : MonoBehaviour, ITransitionEfect
{
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float moveDistance = 1080f; // Distância para subir

    public IEnumerator PlayEffect()
    {
        Vector2 startPosition = targetTransform.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(0, moveDistance);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Suavização do movimento (Ease-InOut)
            t = Mathf.SmoothStep(0, 1, t);

            targetTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        targetTransform.anchoredPosition = endPosition;
    }
}