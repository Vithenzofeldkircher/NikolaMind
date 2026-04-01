using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Move : MonoBehaviour
{
    [Header("Configuração do Player")]
    public float _Speed_Player = 2f;
    private Rigidbody2D _rb;
    private float movimentoHorizontal;
    private float movimentoVertical;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //teclas de uso dos comandos de movimento
        movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        movimentoVertical = Input.GetAxisRaw("Vertical");
    }
        
    private void FixedUpdate()
    {
        // Se o WireManager disser que NÃO pode mover, cancelamos a velocidade
        if (WireManager.Instance != null && !WireManager.Instance.podeMover)
        {
            _rb.linearVelocity = Vector2.zero; // Para o Rigidbody
            return; // Impede que o resto do código de andar execute
        }

        _rb.linearVelocity = new Vector2(movimentoHorizontal, movimentoVertical).normalized * _Speed_Player;
    }

}
