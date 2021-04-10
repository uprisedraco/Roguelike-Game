using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource levelMusic;

    public AudioSource gameOverMusic;

    public AudioSource winMusic;

    public AudioSource[] Sfx;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();
        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        levelMusic.Stop();
        winMusic.Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        Sfx[sfxToPlay].Stop();
        Sfx[sfxToPlay].Play();
    }
}
