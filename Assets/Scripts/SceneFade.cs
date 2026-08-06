using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    [SerializeField] private Image fadePanel;
    [SerializeField] private float tempoFade = 1.5f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color cor = fadePanel.color;

        while (cor.a > 0)
        {
            cor.a -= Time.deltaTime / tempoFade;
            fadePanel.color = cor;

            yield return null;
        }

        cor.a = 0;
        fadePanel.color = cor;

        fadePanel.raycastTarget = false;
    }
}