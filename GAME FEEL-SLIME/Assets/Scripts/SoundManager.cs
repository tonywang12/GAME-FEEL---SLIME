using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SoundManager : MonoBehaviour
{
    public static AudioClip playerJump;
    static AudioSource audioSrc;
    void Start()
    {   
        playerJump = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Audio/Jump.mp3", typeof(AudioClip));
        audioSrc = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PlaySound (string clip)
    {
        switch (clip) 
        {
            case "jump":
                audioSrc.PlayOneShot(playerJump);
            break;
            case "run":
                audioSrc.PlayOneShot(playerJump);
            break;
        }
    }

    public static void pause()
    {   
        audioSrc.clip = playerJump;
        audioSrc.Pause();
    }
}
