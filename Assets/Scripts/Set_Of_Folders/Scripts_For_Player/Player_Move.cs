using UnityEngine;

public class Player_Move : MonoBehaviour
{
    [Header("Configuração do Player")]
    public float _Speed_Player = 2f;

    [Header("Som dos Passos")]
    public AudioSource audioPassos;
    public AudioClip[] sonsPassos;
    public float intervaloPasso = 0.4f;

    private Rigidbody2D _rb;
    private float movimentoHorizontal;
    private float movimentoVertical;
    private float contadorPasso;

    private int ultimoPasso = -1;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.gravityScale = 0f;
    }

    void Update()
    {
        movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        movimentoVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 direcaoDesejada =
            new Vector2(movimentoHorizontal, movimentoVertical).normalized;

        Vector3 proximaPosicao =
            transform.position +
            (Vector3)direcaoDesejada *
            _Speed_Player *
            Time.fixedDeltaTime;

        if (WireManager.Instance != null &&
            !WireManager.Instance.PodeMoverPara(proximaPosicao))
        {
            _rb.linearVelocity = Vector2.zero;
        }
        else
        {
            _rb.linearVelocity =
                direcaoDesejada * _Speed_Player;
        }

        // SOM DOS PASSOS
        if (direcaoDesejada != Vector2.zero &&
            _rb.linearVelocity != Vector2.zero)
        {
            contadorPasso -= Time.fixedDeltaTime;

            if (contadorPasso <= 0f)
            {
                TocarPasso();
                contadorPasso = intervaloPasso;
            }
        }
        else
        {
            contadorPasso = 0f;
        }
    }

    private void TocarPasso()
    {
        if (sonsPassos.Length == 0)
            return;

        int novoPasso;

        do
        {
            novoPasso = Random.Range(0, sonsPassos.Length);
        }
        while (sonsPassos.Length > 1 &&
               novoPasso == ultimoPasso);

        ultimoPasso = novoPasso;

        audioPassos.PlayOneShot(sonsPassos[novoPasso]);
    }
}