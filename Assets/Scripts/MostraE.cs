using UnityEngine;

public class MostrarE : MonoBehaviour
{
    public GameObject textoE;

    private void Start()
    {
        if (textoE != null)
            textoE.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textoE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textoE.SetActive(false);
        }
    }
}
