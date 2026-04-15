using UnityEngine;
using UnityEngine.SceneManagement;

public class InteratvelGenerico : MonoBehaviour, IInteractable
{
    [Header("Tipo de Interação")]
    public bool ehParaPegarFio = false;
    public string nomeDaCena;

    public void Active()
    {
        // Se estiver configurado para ser o fio
        if (ehParaPegarFio)
        {
            if (WireManager.Instance != null)
            {
                WireManager.Instance.RetomarFio();
                Debug.Log("Fio recuperado via Interface!");
            }
        }
        // Se estiver configurado para mudar de cena
        else if (!string.IsNullOrEmpty(nomeDaCena))
        {
            SceneManager.LoadScene(nomeDaCena);
        }
    }
}