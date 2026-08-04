using UnityEngine;

public class MostrarE : MonoBehaviour
{
    // Arraste aqui apenas o objeto do Texto, NÃO o Canvas inteiro!
    public GameObject visualE;

    void Start()
    {
        // Desativa apenas o visual no início
        if (visualE != null) visualE.SetActive(false);
    }

    // Detecta quando o Player entra na área do Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Show();
        }
    }

    // Detecta quando o Player sai da área do Trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Hide();
        }
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