using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private string botaoInteracao = "Interact";

    [Header("UI de Feedback")]
    [SerializeField] private GameObject iconeInteracao; // Arraste o Canvas do "E" para cá

    private IInteractable _currentSelection;

    void Start()
    {
        // Garante que o ícone comece desligado
        if (iconeInteracao != null) iconeInteracao.SetActive(false);
    }

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

            // ATIVA o ícone "E" na tela
            if (iconeInteracao != null) iconeInteracao.SetActive(true);

            Debug.Log("Pode interagir com: " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable target))
        {
            if (target == _currentSelection)
            {
                _currentSelection = null;

                // DESATIVA o ícone "E" na tela
                if (iconeInteracao != null) iconeInteracao.SetActive(false);

                Debug.Log("Saiu do alcance.");
            }
        }
    }
}