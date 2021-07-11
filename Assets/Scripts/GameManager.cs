using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject bgm;

    public AudioClip clip;

    private void Awake()
    {
        if (bgm is null)
        {
            bgm = new GameObject("BGM");
            var source = bgm.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = 0.2f;
            source.Play();
            DontDestroyOnLoad(bgm);
        }
    }

    public GameObject ui_defeat;
    public GameObject ui_vitory;
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}