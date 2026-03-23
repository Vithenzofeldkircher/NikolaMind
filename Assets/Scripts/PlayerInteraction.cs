using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private string botaoInteracao = "Interact";

    // Armazena o alvo atual que está dentro do Circle Collider
    private IInteractable _currentSelection;

    void Update()
    {
        // Se houver alguém no raio e o jogador apertar o botão
        if (_currentSelection != null && Input.GetButtonDown(botaoInteracao))
        {
            _currentSelection.Active();
        }
    }

    // Chamado automaticamente pelo Unity quando algo entra no Circle Collider (Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o que entrou tem o script/interface de interação
        if (collision.TryGetComponent(out IInteractable target))
        {
            _currentSelection = target;
            Debug.Log("Pode interagir com: " + collision.name);
        }
    }

    // Chamado quando o objeto sai do alcance do Circle Collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable target))
        {
            // Só limpa se o objeto que saiu for o mesmo que estamos focando
            if (target == _currentSelection)
            {
                _currentSelection = null;
                Debug.Log("Saiu do alcance de interação.");
            }
        }
    }
}