using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpring : MonoBehaviour
{
    public Player player;
    public TrainNode node, other;
    public LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        node = transform.parent.GetComponent<TrainNode>();
    }

    private void Update()
    {
        if (node && other)
        {
            line.SetPosition(0, node.head.position);
            line.SetPosition(1, other.tail.position);
        }
        else if (node && player)
        {
            line.SetPosition(0, node.head.position);
            line.SetPosition(1, player.transform.position);
        }
        else
        {
            line.enabled = false;
        }
    }

    public void ConnectToNode(TrainNode other)
    {
        if (other)
        {
            this.other = other;
            player = null;
            line.enabled = true;
            if (node) node.head.GetComponent<SpriteRenderer>().enabled = true;
            other.tail.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    
    public void ConnectToPlayer(Player player)
    {
        this.player = player;
        line.enabled = true;
        other = null;
        if (node) node.head.GetComponent<SpriteRenderer>().enabled = true;
    }
}