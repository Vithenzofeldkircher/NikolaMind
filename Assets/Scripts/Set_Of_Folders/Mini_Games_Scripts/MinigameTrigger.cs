using UnityEngine;

public class MinigameTrigger : MonoBehaviour, IInteractable
{
    [Header("Configuração do Minigame UI")]
    [SerializeField] private GameObject minigamePanel;

    // Chamado pelo seu PlayerInteraction quando o botão de interação é pressionado
    public void Active()
    {
        if (minigamePanel != null)
        {
            // Ativa o Panel na tela
            minigamePanel.SetActive(true);

            // Opcional: Desativa a movimentação ou controles do Player aqui, se necessário
        }
        else
        {
            Debug.LogError("O minigamePanel não foi atribuído no Inspector do GameObject: " + gameObject.name);
        }
    }
}