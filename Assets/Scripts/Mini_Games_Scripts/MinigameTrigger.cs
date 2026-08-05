using UnityEngine;

public class MinigameTrigger : MonoBehaviour, IInteractable
{
    [Header("Configuração da Cena")]
    [SerializeField] private string nomeDaCenaMinigame;

    // Chamado pelo seu PlayerInteraction quando o botão é pressionado
    public void Active()
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.IrParaMinigame(nomeDaCenaMinigame);
        }
        else
        {
            Debug.LogError("SceneTransitionManager não foi encontrado na cena!");
        }
    }
}