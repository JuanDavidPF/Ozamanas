using System.Collections;
using System.Collections.Generic;
using Stem;
using UnityEngine;


namespace JuanPayan.Helpers
{
public class SoundPlayer : MonoBehaviour
{
    [SoundID, SerializeField]
    internal Stem.ID sound = Stem.ID.None;

    [SoundID, SerializeField]
    internal Stem.ID sound2 = Stem.ID.None;

    [SerializeField]
    internal Transform spawnTarget = null;

    [SerializeField] bool playOnStart = false;

    void Start()
    {
        if(playOnStart) Play();
    }


    public void Play()
    {
        if (spawnTarget) SoundManager.Play3D(sound, spawnTarget.position);
        else SoundManager.Play(sound);

    }

    public void Play2()
    {
        if (spawnTarget) SoundManager.Play3D(sound2, spawnTarget.position);
        else SoundManager.Play(sound2);

    }
}//Closes SoundPlayer class
}