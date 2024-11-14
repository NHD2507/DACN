using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class soundbutton : MonoBehaviour
{
    public AudioSource sound;
    public void Playsound()
    {
        sound.Play();
    }
}
  
