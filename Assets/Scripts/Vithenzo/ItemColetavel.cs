using UnityEngine;

public class ItemColetavel : MonoBehaviour, IInteractable
{
    private bool _estaSendoCarregado = false;
    private Transform _maoDoPlayer;
    private Rigidbody2D _rb;
    private Collider2D _col;

    [Header("Configurações")]
    [SerializeField] private float cooldownInteracao = 0.2f;
    private float _proximoTempoInteracao = 0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();

        if (_rb != null) _rb.gravityScale = 0;
    }

    public void Active()
    {
        if (Time.time < _proximoTempoInteracao) return;
        _proximoTempoInteracao = Time.time + cooldownInteracao;

        if (!_estaSendoCarregado)
        {
            Pegar();
        }
        else
        {
            Soltar();
        }
    }

    private void Pegar()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        _maoDoPlayer = player.transform.Find("Mao");

        if (_maoDoPlayer != null)
        {
            _estaSendoCarregado = true;

            // 1. Desativa a física e o collider para não interferir no Trigger do Player
            if (_rb != null) _rb.simulated = false;

            // 2. Gruda na mão
            transform.SetParent(_maoDoPlayer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Debug.Log("Objeto coletado!");
        }
    }

    private void Soltar()
    {
        _estaSendoCarregado = false;

        // 1. Tira da hierarquia do player
        transform.SetParent(null);

        // 2. Reativa a física imediatamente
        if (_rb != null)
        {
            _rb.simulated = true;
            _rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("Objeto solto!");
    }
}