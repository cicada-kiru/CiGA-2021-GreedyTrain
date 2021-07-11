using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool alive = true;
    public float total_second;
    public float camera_size_factor;
    public float speed = 5;
    public float max_delta_size = 0.1f;
    public Text time;
    public CinemachineVirtualCamera virtual_camera;
    public Transform camera_target;
    public Rigidbody2D rigidbody_2D;
    public TrainNode locomotive;
    public AudioClip drop_clip;

    private void Awake()
    {
        rigidbody_2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (alive)
        {
            total_second += Time.deltaTime;
            time.text = $"{Math.Round(total_second, 2):F2}s";
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            rigidbody_2D.velocity = new Vector2(x, y) * speed;
        }
        
        if (locomotive)
        {
            var last = locomotive.last;
            var target_size = 5 + (transform.position - last.transform.position).magnitude * camera_size_factor;
            var delta_size = target_size - virtual_camera.m_Lens.OrthographicSize;
            delta_size = Mathf.Clamp(delta_size, -max_delta_size, max_delta_size);
            virtual_camera.m_Lens.OrthographicSize += delta_size;
            camera_target.position = (transform.position + last.transform.position) / 2;
        }
        else
        {
            locomotive = null;
            camera_target.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Abyss"))
        {
            alive = false;
            locomotive.spring_2D.enabled = false;
            locomotive.spring.node = null;
            rigidbody_2D.drag = 1;
            transform.DOScale(0, 1).OnComplete(() =>
            {
                Destroy(gameObject);
                FindObjectOfType<GameManager>().ui_defeat.SetActive(true);
            });
            FindObjectOfType<AudioSource>().PlayOneShot(drop_clip);
        }

        if (other.CompareTag("End"))
        {
            time.gameObject.SetActive(false);
            var vitory = FindObjectOfType<GameManager>().ui_vitory;
            var node = locomotive;
            var count = node ? 1 : 0;
            while (node && node.next)
            {
                count++;
                node = node.next;
            }

            vitory.transform.Find("TrainCount").GetComponent<Text>().text += count;
            vitory.transform.Find("Time").GetComponent<Text>().text += Math.Round(total_second, 2) + "s";
            vitory.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
