using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("Efeitos da Transição")]
    [SerializeField] private SlideUpEffect titleSlideEffect;
    [SerializeField] private FadeInEffect gameFadeEffect;

    [Header("Elementos de UI (Opcional)")]
    [SerializeField] private GameObject titleScreenGameObject;
    [SerializeField] private GameObject gameContentGameObject;

    private void Start()
    {
        StartCoroutine(ExecuteSceneSequence());
    }

    private IEnumerator ExecuteSceneSequence()
    {
        // 1. Executa o efeito de Slide Up no Título
        if (titleSlideEffect != null)
        {
            yield return StartCoroutine(titleSlideEffect.PlayEffect());
        }

        // Desativa a tela de título se necessário
        if (titleScreenGameObject != null)
        {
            titleScreenGameObject.SetActive(false);
        }

        // Ativa o conteúdo do jogo
        if (gameContentGameObject != null)
        {
            gameContentGameObject.SetActive(true);
        }

        // 2. Executa o efeito de Fade In do Jogo
        if (gameFadeEffect != null)
        {
            yield return StartCoroutine(gameFadeEffect.PlayEffect());
        }
    }
}