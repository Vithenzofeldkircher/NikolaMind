using UnityEngine;

public class VisualEGameObject : MonoBehaviour, IVisualE
{
    [SerializeField] private GameObject target;

    // Se não atribuído no Inspector, usa o próprio GameObject
    private GameObject Target => target != null ? target : gameObject;

    public bool IsVisible => Target != null && Target.activeSelf;

    public void Show()
    {
        if (Target != null) Target.SetActive(true);
    }

    public void Hide()
    {
        if (Target != null) Target.SetActive(false);
    }

    // Ajuda ao adicionar o componente via menu: presumir o próprio objeto como alvo.
    private void Reset()
    {
        target = gameObject;
    }
}