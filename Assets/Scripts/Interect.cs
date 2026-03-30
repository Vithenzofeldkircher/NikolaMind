using UnityEngine;
using UnityEngine.SceneManagement;

public class Interect : MonoBehaviour
{
    public GameObject textoE;

    [Header("Configuração de Cena")]
    public string nomeDaCena; 

    private bool playerPerto = false;

    void Start()
    {
        if (textoE != null)
            textoE.SetActive(false);
    }

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(nomeDaCena))
            {
                SceneManager.LoadScene(nomeDaCena);
            }
            else
            {
                Debug.LogWarning("Nenhuma cena foi definida!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textoE != null)
                textoE.SetActive(true);

            playerPerto = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textoE != null)
                textoE.SetActive(false);

            playerPerto = false;
        }
    }
}
