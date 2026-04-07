using UnityEngine;

public class MostrarE : MonoBehaviour
{
    public GameObject textoE;

    // Começa escondido
    void Start()
    {
        if (textoE != null) textoE.SetActive(false);
    }

    // Agora são funções simples que o Player vai chamar
    public void Show()
    {
        if (textoE != null) textoE.SetActive(true);
    }

    public void Hide()
    {
        if (textoE != null) textoE.SetActive(false);
    }
}