using System.Collections;
using System.Collections.Generic;
using Stem;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SoundID, SerializeField]
    internal Stem.ID sound = Stem.ID.None;

    [SerializeField]
    internal Transform spawnTarget = null;


    public void Play()
    {
        if (spawnTarget) SoundManager.Play3D(sound, spawnTarget.position);
        else SoundManager.Play(sound);

    }
}//Closes SoundPlayer class
