using UnityEngine;

public class MostrarE : MonoBehaviour
{
    // Arraste aqui apenas o objeto do Texto, NÃO o Canvas inteiro!
    public GameObject visualE;

    void Start()
    {
        // Desativa apenas o visual, o objeto pai continua vivo
        if (visualE != null) visualE.SetActive(false);
    }

    public void Show()
    {
        if (visualE != null) visualE.SetActive(true);
    }

    public void Hide()
    {
        if (visualE != null) visualE.SetActive(false);
    }
}