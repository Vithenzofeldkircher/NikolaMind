using UnityEngine;

public class Porta : MonoBehaviour
{
    [SerializeField] private Vector3 deslocamentoAberta = new Vector3(0, 2, 0); // Onde ela vai parar
    [SerializeField] private float velocidade = 2f;

    private Vector3 _posicaoInicial;
    private Vector3 _posicaoDestino;
    private bool _deveAbrir = false;

    void Start()
    {
        _posicaoInicial = transform.position;
        _posicaoDestino = _posicaoInicial + deslocamentoAberta;
    }

    void Update()
    {
        // Move suavemente em direção ao objetivo
        Vector3 alvo = _deveAbrir ? _posicaoDestino : _posicaoInicial;
        transform.position = Vector3.Lerp(transform.position, alvo, Time.deltaTime * velocidade);
    }

    public void SetAberta(bool estado)
    {
        _deveAbrir = estado;
    }
}