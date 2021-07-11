using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TrainNode : MonoBehaviour
{
    public Player player;
    public TrainNode prev, next;
    public Transform head, tail;
    public TrainSpring spring;
    public Rigidbody2D rigidbody_2D;
    public BoxCollider2D collider_2D;
    public SpringJoint2D spring_2D;
    public AudioClip drop_clip;
    public AudioClip connect_clip;

    public TrainNode last
    {
        get
        {
            var node = this;
            while (node.next) node = node.next;
            return node;
        }
    }

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
            var other_node = other.GetComponent<TrainNode>();
            if (other_node.collider_2D.isTrigger) return;
            var other_last = other_node.last;
            other_last.next = this;
            prev = other_last;
            collider_2D.isTrigger = false;
            // rigidbody_2D.rotation = last.rigidbody_2D.rotation;
            rigidbody_2D.position = other_last.tail.position +
                                    (other_last.tail.position - other_last.head.position).normalized;
            ConnectToNode(other_last);
            FindObjectOfType<AudioSource>().PlayOneShot(connect_clip);
        }

        if (!collider_2D.isTrigger && other.CompareTag("Abyss"))
        {
            spring_2D.enabled = false;
            spring.node = null;
            rigidbody_2D.drag = 0;
            collider_2D.enabled = false;
            transform.DOScale(0, 1).OnComplete(() => Destroy(gameObject));
            if (next)
            {
                if (prev) next.ConnectToNode(prev);
                if (player) next.ConnectToPlayer(player);
            }

            FindObjectOfType<AudioSource>().PlayOneShot(drop_clip, 0.4f);
        }

        if (collider_2D.isTrigger && other.CompareTag("Player"))
        {
            var p = other.GetComponent<Player>();
            if (p is {locomotive: null})
            {
                ConnectToPlayer(p);
                FindObjectOfType<AudioSource>().PlayOneShot(connect_clip);
            }
        }
    }

    public void ConnectToNode(TrainNode other)
    {
        spring_2D.connectedBody = other.rigidbody_2D;
        spring_2D.enabled = true;
        spring.ConnectToNode(other);
    }

    public void ConnectToPlayer(Player player)
    {
        collider_2D.isTrigger = false;
        this.player = player;
        player.locomotive = this;
        spring_2D.connectedBody = player.rigidbody_2D;
        spring_2D.enabled = true;
        spring_2D.connectedAnchor = Vector2.zero;
        spring_2D.frequency = 0.6f;
        spring.ConnectToPlayer(player);
        foreach (var light2D in GetComponentsInChildren<Light2D>(true))
        {
            light2D.enabled = true;
        }
    }
}