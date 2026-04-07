using UnityEngine;

public class Alavanca : MonoBehaviour
{
    [Header("Arraste a Porta da Hierarchy para cá")]
    [SerializeField] private GameObject portaObjeto;

    private bool _portaEstaAtiva = true;

    // Este método o Unity chama sozinho quando você clica no objeto (se ele tiver Collider)
    private void OnMouseDown()
    {
        TogglePorta();
    }

    // Mantemos o Active caso você queira usar o "E" no futuro também
    public void Active()
    {
        TogglePorta();
    }

    private void TogglePorta()
    {
        if (portaObjeto != null)
        {
            _portaEstaAtiva = !_portaEstaAtiva;

            // Desliga/Liga o objeto da porta
            portaObjeto.SetActive(_portaEstaAtiva);

            Debug.Log("Clique detectado! Porta ativa: " + _portaEstaAtiva);
        }
        else
        {
            Debug.LogError("Erro: Arraste a porta para o campo 'Porta Objeto' no Inspector!");
        }
    }
}