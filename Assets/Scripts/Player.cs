using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rigidbody_2D;

    private void Awake()
    {
        rigidbody_2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        rigidbody_2D.velocity = new Vector2(x, y) * speed;
    }
}
