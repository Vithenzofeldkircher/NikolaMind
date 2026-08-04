using UnityEngine;

public class HandFollow : MonoBehaviour
{
    public Transform pontoDaMao;
    public float distanciaDoPlayer = 0.6f;
    public float velocidadeSuavizacao = 15f;

    private Vector2 _direcaoOlhar;

    void Start()
    {
        // Define a direção inicial baseada na posição atual da mão em relação ao player
        // Isso evita o "pulo" inicial
        _direcaoOlhar = (pontoDaMao.position - transform.position).normalized;
        if (_direcaoOlhar == Vector2.zero) _direcaoOlhar = Vector2.down;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Atualiza a direção apenas se houver movimento
        if (moveX != 0 || moveY != 0)
        {
            _direcaoOlhar = new Vector2(moveX, moveY).normalized;
        }

        // Se o player estiver parado mas segurando o item, a mão mantém a última direção
        PosicionarMao();
    }

    private void PosicionarMao()
    {
        Vector3 posicaoAlvo = new Vector3(_direcaoOlhar.x, _direcaoOlhar.y, 0) * distanciaDoPlayer;

        // Move a mão suavemente para a frente do personagem
        pontoDaMao.localPosition = Vector3.Lerp(pontoDaMao.localPosition, posicaoAlvo, Time.deltaTime * velocidadeSuavizacao);
    }

    // Função pública que o Pickup_Manager pode chamar para forçar a direção ao pegar
    public void ResetarDirecaoAoPegar()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
            _direcaoOlhar = new Vector2(moveX, moveY).normalized;

        // Posiciona instantaneamente para não ter delay no primeiro frame
        pontoDaMao.localPosition = new Vector3(_direcaoOlhar.x, _direcaoOlhar.y, 0) * distanciaDoPlayer;
    }
}