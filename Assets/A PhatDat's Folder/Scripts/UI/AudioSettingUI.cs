using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Gán giá trị ban đầu cho các slider từ PlayerPrefs
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", AudioManager.Instance.GetMasterVolume());
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", AudioManager.Instance.GetMusicVolume());
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", AudioManager.Instance.GetSFXVolume());

        // Cập nhật âm lượng ban đầu
        AudioManager.Instance.SetMasterVolume(masterSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);

        // Đăng ký sự kiện khi slider thay đổi
        masterSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetMasterVolume(value));
        musicSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetMusicVolume(value));
        sfxSlider.onValueChanged.AddListener((value) => AudioManager.Instance.SetSFXVolume(value));
    }

    void OnDestroy()
    {
        // Lưu giá trị khi GameObject bị phá hủy
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }
}
