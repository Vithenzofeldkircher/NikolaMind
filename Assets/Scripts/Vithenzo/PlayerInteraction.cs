using UnityEngine;
using System; // Necessário para o Action

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private string botaoInteracao = "Interact";
    private IInteractable _currentSelection;

    // Eventos para avisar outros scripts (como o MostrarE)
    public static event Action OnTargetEnter;
    public static event Action OnTargetExit;

    void Update()
    {
        if (_currentSelection != null && Input.GetButtonDown(botaoInteracao))
        {
            _currentSelection.Active();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable target))
        {
            _currentSelection = target;
            OnTargetEnter?.Invoke(); // Avisa que alguém entrou
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable target))
        {
            if (target == _currentSelection)
            {
                _currentSelection = null;
                OnTargetExit?.Invoke(); // Avisa que alguém saiu
            }
        }
    }
}