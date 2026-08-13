using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class SceneTransition : MonoBehaviour
{
    [Header("Efeitos da Transição")]
    [SerializeField] private SlideUpEffect titleSlideEffect;
    [SerializeField] private FadeInEffect gameFadeEffect;

    [Header("Configuração de Cena")]
    [SerializeField] private string nextSceneName = "SampleScene"; // Nome da cena de destino

    // Chame este método quando o jogador clicar no botão "Iniciar"
    public void StartTransition()
    {
        StartCoroutine(ExecuteSceneSequence());
    }

    private IEnumerator ExecuteSceneSequence()
    {
        // 1. Executa o efeito de Slide Up na Tela de Título
        if (titleSlideEffect != null)
        {
            yield return StartCoroutine(titleSlideEffect.PlayEffect());
        }

        // 2. Executa o efeito de Fade In (se houver painel de escurecimento)
        if (gameFadeEffect != null)
        {
            yield return StartCoroutine(gameFadeEffect.PlayEffect());
        }

        // 3. Carrega a próxima cena ("SampleScene")
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("O nome da próxima cena não foi configurado!");
        }
    }
}