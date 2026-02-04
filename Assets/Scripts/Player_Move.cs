using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Move : MonoBehaviour
{

    private float _speed = 2.0f;
    private Vector2 _movement;
    private Rigidbody2D _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _movement * _speed;
    }
}
