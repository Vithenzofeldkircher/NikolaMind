using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Move : MonoBehaviour, Jump
{
    [Header("Movement")]
    public float _speed = 2.0f;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private int Count_Jump;

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
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Jump"));

       
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _movement * _speed;

        if (Input.GetButtonDown("Jump") && Is_Ground)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _Jump_Force);
        }
        
    }

    public bool Can_Jump()
    {
        return Max_Jump < Count_Jump;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //verifica se há um chão
        if(collision != null && !Is_Ground)
        {
            Is_Ground = true;
            Debug.Log("Há chão");
        }
    }

    public void Jumps()
    {
        throw new System.NotImplementedException();
    }
}
