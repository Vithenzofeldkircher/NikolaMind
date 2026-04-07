using UnityEngine;

public class Porta : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private Vector3 deslocamentoAoAbrir = new Vector3(0, 3f, 0);
    [SerializeField] private float velocidade = 3f;

    private Vector3 _posicaoFechada;
    private Vector3 _posicaoAberta;
    private bool _deveAbrir = false;

    void Start()
    {
        _posicaoFechada = transform.position;
        _posicaoAberta = _posicaoFechada + deslocamentoAoAbrir;
    }

    void Update()
    {
        // Define para onde a porta deve ir no momento
        Vector3 destino = _deveAbrir ? _posicaoAberta : _posicaoFechada;

        // Move suavemente a porta frame a frame
        transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime * velocidade);
    }

    // Método chamado pela alavanca
    public void SetAberta(bool status)
    {
        _deveAbrir = status;
    }
}