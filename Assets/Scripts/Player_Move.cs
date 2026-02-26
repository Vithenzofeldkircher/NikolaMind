using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Move : MonoBehaviour
{
    [Header("Configuração do Player")]
    public float _Speed_Player = 2f;
    private Rigidbody2D _rb;

    private Vector2 _Position;


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

        _Position = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _Position * _Speed_Player;
    }

}
