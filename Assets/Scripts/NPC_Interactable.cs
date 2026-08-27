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
}