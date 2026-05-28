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
            if (Pickup_Manager.Instance != null && Pickup_Manager.Instance.estaCarregandoItem)
            {
                Debug.Log("Não pode pegar o fio! Suas mãos estão ocupadas com uma caixa.");
                return; // Para o código aqui e impede de pegar o fio
            }

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