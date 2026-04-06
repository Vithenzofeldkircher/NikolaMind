using UnityEngine;

public class MostrarE : MonoBehaviour
{
    public GameObject textoE;

    private void OnEnable()
    {
        // Se inscreve nos avisos do PlayerInteraction
        PlayerInteraction.OnTargetEnter += Show;
        PlayerInteraction.OnTargetExit += Hide;
    }

    private void OnDisable()
    {
        // Se desinscreve para evitar erros de memória e de "Missing Reference"
        PlayerInteraction.OnTargetEnter -= Show;
        PlayerInteraction.OnTargetExit -= Hide;
    }

    private void Show()
    {
        if (textoE != null) textoE.SetActive(true);
    }

    private void Hide()
    {
        // O "if != null" aqui mata o erro de "Object Destroyed" na troca de cena!
        if (textoE != null) textoE.SetActive(false);
    }
}