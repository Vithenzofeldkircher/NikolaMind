using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configurações de Entrada")]
    [SerializeField] private string botaoInteracao = "Interact";

    [Header("Ponto de Encaixe do Item")]
    [SerializeField] private Transform holdPoint; // Traga um GameObject filho do Player para cá no Inspector

    private IInteractable _currentSelection;
    private MostrarE _currentVisualFeedback;

    // Armazena o item que está atualmente nas mãos do jogador
    private IHoldable _heldItem;

    void Update()
    {
        if (Input.GetButtonDown(botaoInteracao))
        {
            // 1. Se já estamos segurando um item, a prioridade do botão é LARGAR
            if (_heldItem != null)
            {
                DropItem();
            }
            // 2. Se não estamos segurando nada, mas estamos sobre um objeto interagível, ative-o ou pegue-o
            else if (_currentSelection != null)
            {
                // Se o objeto atual for um item pegável, nós o pegamos
                if (_currentSelection is IHoldable holdable)
                {
                    PickUpItem(holdable);
                }
                else
                {
                    // Caso contrário, apenas executa a ação normal (ex: abrir porta, falar com NPC)
                    _currentSelection.Active();
                }
            }
        }
    }

    private void PickUpItem(IHoldable item)
    {
        _heldItem = item;
        _heldItem.OnPickUp(holdPoint);

        // Esconde o ícone de interação enquanto carrega
        ClearVisualFeedback();
    }

    private void DropItem()
    {
        _heldItem.OnDrop();
        _heldItem = null;

        // Se ao soltar o item ainda estivermos dentro do Trigger dele, reativa o ícone
        if (_currentVisualFeedback != null)
        {
            _currentVisualFeedback.Show();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se já estiver carregando algo, ignora novas seleções para evitar conflitos
        if (_heldItem != null) return;

        if (collision.TryGetComponent(out IInteractable target))
        {
            _currentSelection = target;

            if (collision.TryGetComponent(out MostrarE visual))
            {
                _currentVisualFeedback = visual;
                _currentVisualFeedback.Show();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable target))
        {
            if (target == _currentSelection)
            {
                ClearVisualFeedback();
                _currentSelection = null;
            }
        }
    }

    private void ClearVisualFeedback()
    {
        if (_currentVisualFeedback != null)
        {
            _currentVisualFeedback.Hide();
            _currentVisualFeedback = null;
        }
    }
}

// Interface auxiliar para itens que podem ser carregados (opcional, mas boa prática)
public interface IHoldable : IInteractable
{
    void OnPickUp(Transform holdPoint);
    void OnDrop();
}