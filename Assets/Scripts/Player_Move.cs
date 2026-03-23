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
        _rb.linearVelocity = new Vector2(movimentoHorizontal, movimentoVertical).normalized * _Speed_Player;
    }

}
