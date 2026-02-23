using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Move : MonoBehaviour, IJump
{
    [Header("Movement")]
    public float _speed = 2.0f;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private int Count_Jump = 0;

    [SerializeField] private float _Jump_Force = 17f;
    [SerializeField] private int Max_Jump = 1;


    private bool Is_Ground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector2(Input.GetAxis("Horizontal"), 0f);

       
    }

    private void FixedUpdate()
    {
        //Aplica a velocidade Horizontal no player.
        _rb.linearVelocity = new Vector2(_movement.x * _speed,_rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && Is_Ground)
        {
            _rb.AddForce(Vector2.up * _Jump_Force, ForceMode2D.Impulse);
                Count_Jump++; //adiciona mais um no contador de pontos

            //(teste de mecanica de pulo)
            //_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _Jump_Force);
        }
        
    }

    public bool Can_Jump()
    {
        return Count_Jump < Max_Jump;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //verifica se há um chão
        //if(collision.gameObject. && !Is_Ground)
        
            Is_Ground = true;
            Debug.Log("Há chão");
        
    }

    public void Jumps()
    {
        throw new System.NotImplementedException();
    }
}
