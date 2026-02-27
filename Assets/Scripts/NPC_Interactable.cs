using UnityEngine;

public class NPC_Interactable : MonoBehaviour, IInteractable
{
    [Header("Dados do Diálogo")]
    public DialogueData meuDialogo;

    public void Interagir()
    {
        // Busca o sistema de diálogo e inicia
        var sistema = Object.FindFirstObjectByType<Dialogue_System>();

        if (sistema != null && !sistema.EstaEmDialogo())
        {
            sistema.IniciarDialogo(meuDialogo);
        }
    }
}