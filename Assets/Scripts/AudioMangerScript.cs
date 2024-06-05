using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class AudioMangerScript : MonoBehaviour
{
    public static AudioMangerScript Instance{get;private set;}
    [SerializeField] private AudioClip shotgunShoot;
    [SerializeField] private AudioClip hammerSwing;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PlayerShootScript.OnShoot += PlayerShootScript_OnShoot;
        PlayerShootScript.OnHammerSwing += PlayerShootScript_OnHammerSwing;
    }

    private void PlayerShootScript_OnHammerSwing(object sender, EventArgs e)
    {
         PlaySound(hammerSwing,PlayerScript.LocalInstance.transform.position);
    }

    private void PlayerShootScript_OnShoot(object sender, EventArgs e)
    {
        PlaySound(shotgunShoot,PlayerScript.LocalInstance.transform.position);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 2f){
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
