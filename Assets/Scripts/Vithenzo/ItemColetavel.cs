using UnityEngine;

public class ItemColetavel : MonoBehaviour, IInteractable
{
    private bool _estaSendoCarregado = false;
    private Transform _maoDoPlayer;
    private Rigidbody2D _rb;
    private Collider2D _col;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();

        // Garante que não tenha gravidade
        if (_rb != null) _rb.gravityScale = 0;
    }

    // O PlayerInteraction chama isso ao apertar "E"
    public void Active()
    {
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
        // Encontra o player e a mão dele
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        _maoDoPlayer = player.transform.Find("Mao");

        if (_maoDoPlayer != null)
        {
            _estaSendoCarregado = true;

            // Faz o objeto seguir a mão
            transform.SetParent(_maoDoPlayer);
            transform.localPosition = Vector3.zero;

            // Desativa a física para não bugar o movimento do player
            if (_rb != null) _rb.simulated = false;

            Debug.Log("Objeto coletado!");
        }
    }

    private void Soltar()
    {
        _estaSendoCarregado = false;
        transform.SetParent(null);

        // Reativa a física (sem gravidade, ele fica parado onde soltou)
        if (_rb != null) _rb.simulated = true;

        Debug.Log("Objeto solto!");
    }
}