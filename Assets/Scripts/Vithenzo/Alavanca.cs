using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Alavanca : MonoBehaviour, IInteractable
{
    [SerializeField] private Porta portaAlvo; // Arraste a porta para cá no Inspector
    private bool _estaAtiva = false;

    // Se você quiser usar o Clique do Mouse diretamente:
    private void OnMouseDown()
    {
        Active();
    }

    public void Active()
    {
        _estaAtiva = !_estaAtiva; // Inverte o estado (liga/desliga)

        if (portaAlvo != null)
        {
            portaAlvo.SetAberta(_estaAtiva);
        }

        // Feedback visual da alavanca (opcional)
        transform.localScale = new Vector3(transform.localScale.x, _estaAtiva ? -1 : 1, 1);
        Debug.Log("Alavanca " + (_estaAtiva ? "Ativada" : "Desativada"));
    }
}