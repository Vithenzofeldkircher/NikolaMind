using UnityEngine;

public class ItemColetavel : MonoBehaviour, IInteractable
{
    private bool _estaSendoCarregado = false;
    private Transform _maoDoPlayer;
    private Rigidbody2D _rb;

    [Header("Configura��es")]
    [SerializeField] private float cooldownInteracao = 0.2f; // Tempo para evitar duplo clique
    private float _proximoTempoInteracao = 0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb != null) _rb.gravityScale = 0;
    }

    public void Active()
    {
        // Se ainda n�o passou o tempo de cooldown, ignora o comando
        if (Time.time < _proximoTempoInteracao) return;

        // Define quando poder� interagir novamente
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

            // F�sica e Hierarquia
            if (_rb != null) _rb.simulated = false;
            transform.SetParent(_maoDoPlayer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Debug.Log("Objeto coletado!");
        }
    }

    private void Soltar()
    {
        _estaSendoCarregado = false;

        // Retira do player e volta a simular f�sica
        transform.SetParent(null);
        if (_rb != null)
        {
            _rb.simulated = true;
            _rb.linearVelocity = Vector2.zero; // Evita que ele herde velocidade estranha
        }

        Debug.Log("Objeto solto!");
    }
}