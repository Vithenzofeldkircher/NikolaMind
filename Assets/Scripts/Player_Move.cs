using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Move : MonoBehaviour, Jump
{
    [Header("Movement")]
    public float _speed = 2.0f;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private int _Jump_Force = 17f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"));
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _movement * _speed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision)
    }
}
