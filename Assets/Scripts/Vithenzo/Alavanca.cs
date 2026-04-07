using UnityEngine;

public class Alavanca : MonoBehaviour, IInteractable
{
    [Header("Arraste a Porta da Hierarchy para cá")]
    [SerializeField] private GameObject portaObjeto;

    private bool _portaEstaAtiva = true;

    // Este é o método que o seu PlayerInteraction chama
    public void Active()
    {
        if (portaObjeto != null)
        {
            // Inverte o estado atual (se era true, vira false / se era false, vira true)
            _portaEstaAtiva = !_portaEstaAtiva;

            // Faz a porta sumir ou aparecer
            portaObjeto.SetActive(_portaEstaAtiva);

            Debug.Log(_portaEstaAtiva ? "Porta Apareceu!" : "Porta Sumiu!");
        }
        else
        {
            Debug.LogWarning("Esqueceu de arrastar a porta para o campo Porta Objeto no Inspector!");
        }
    }
}