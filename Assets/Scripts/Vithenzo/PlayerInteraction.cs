using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private string botaoInteracao = "Interact";
    private IInteractable _currentSelection;
    private MostrarE _currentVisualFeedback; // Guarda o script de feedback do objeto atual

    void Update()
    {
        if (_currentSelection != null && Input.GetButtonDown(botaoInteracao))
        {
            _currentSelection.Active();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verifica se o objeto é interagível
        if (collision.TryGetComponent(out IInteractable target))
        {
            _currentSelection = target;

            // 2. Procura o script "MostrarE" APENAS neste objeto
            if (collision.TryGetComponent(out MostrarE visual))
            {
                _currentVisualFeedback = visual;
                _currentVisualFeedback.Show(); // Ativa apenas o "E" deste objeto
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable target))
        {
            if (target == _currentSelection)
            {
                // Desativa o feedback visual antes de limpar a seleção
                if (_currentVisualFeedback != null)
                {
                    _currentVisualFeedback.Hide();
                    _currentVisualFeedback = null;
                }

                _currentSelection = null;
            }
        }
    }
}