using UnityEngine;

public class MostrarE : MonoBehaviour
{
    // Atribua aqui um Component que implemente IVisualE (ex: VisualEGameObject)
    [SerializeField] private MonoBehaviour visualProvider;
    [SerializeField] private bool startVisible = false;

    private IVisualE _visual;

    private void Awake()
    {
        // Injeção por referência: aceita um MonoBehaviour que implemente IVisualE
        _visual = visualProvider as IVisualE ?? GetComponent<IVisualE>();

        if (_visual == null)
        {
            Debug.LogWarning($"{name}: nenhum IVisualE encontrado. A visibilidade não será controlada.");
        }
    }

    private void Start()
    {
        if (_visual == null) return;

        if (startVisible) _visual.Show();
        else _visual.Hide();
    }

    public void Show() => _visual?.Show();

    public void Hide() => _visual?.Hide();
}