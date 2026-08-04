using UnityEngine;

public class NPC_Interactable : MonoBehaviour, IInteractable
{
    [Header("Dados do Diálogo")]
    public DialogueData meuDialogo;

    private Dialogue_System sistemaDialogo;

    void Start()
    {
        sistemaDialogo = Object.FindFirstObjectByType<Dialogue_System>();
    }

    public void Active()
    {
        if (sistemaDialogo != null && !sistemaDialogo.EstaEmDialogo())
        {
            sistemaDialogo.IniciarDialogo(meuDialogo);
        }
        CursorManager.Instance.UnlockCursor();
    }

    // Detecta quando o Player sai do Trigger do NPC
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Se o player se afastar, manda o sistema fechar o painel
            if (sistemaDialogo != null)
            {
                sistemaDialogo.FecharDialogoManualmente();
            }
        }
    }
}