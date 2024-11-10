using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class settingMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Dropdown graphicsDropdown;
    public Slider MasterVol, MusicVol, SFXVol;
    public AudioMixer mainAudioMixer;
    // Start is called before the first frame update
    public void ChangeGraphicQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("Mastervol", MasterVol.value);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("Musicvol", MusicVol.value);
    }
    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("SFXvol", SFXVol.value);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
