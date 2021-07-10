using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainNode : MonoBehaviour
{
    public TrainNode next;
    public Transform head, tail;
    public TrainSpring spring;
    public Rigidbody2D rigidbody_2D;
    public BoxCollider2D collider_2D;
    public SpringJoint2D spring_2D;

    private void Awake()
    {
        head = transform.Find("Actor/Head");
        tail = transform.Find("Actor/Tail");
        spring = transform.Find("Spring").GetComponent<TrainSpring>();
        rigidbody_2D = GetComponent<Rigidbody2D>();
        collider_2D = GetComponent<BoxCollider2D>();
        spring_2D = GetComponent<SpringJoint2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collider_2D.isTrigger && other.CompareTag("TrainNode"))
        {
            var last = other.GetComponent<TrainNode>();
            if (last.collider_2D.isTrigger) return;
            while (last.next) last = last.next;
            last.next = this;
            collider_2D.isTrigger = false;
            rigidbody_2D.rotation = last.rigidbody_2D.rotation;
            rigidbody_2D.position = last.tail.position +
                                         (last.tail.position - last.head.position).normalized;
            ConnectToNode(last);
        }
    }

    public void ConnectToNode(TrainNode other)
    {
        spring_2D.connectedBody = other.rigidbody_2D;
        spring_2D.enabled = true;
        spring.ConnectToNode(other);
    }
}
