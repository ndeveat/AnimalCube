using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource main;
    public AudioSource ingame;
    public AudioSource bang;
    public AudioSource button;
    public AudioSource sum;
    public AudioSource highnoon;

    void Awake ()
    {
        instance = this;
    }

    public void SetBGM(AudioSource audio)
    {
        if(audio == main)
        {
            main.Play();
            ingame.Stop();
        }
        else
        {
            ingame.Play();
            main.Stop();
        }
    }

    public void PlayButtonClick()
    {
        button.Play();
    }
}
