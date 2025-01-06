using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Đối tượng duy nhất tồn tại trong toàn bộ ứng dụng, giúp quản lý âm thanh tập trung và tránh xung đột
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // Gắn Audio Mixer tại đây

    private void Awake()
    {
        // Đảm bảo mẫu thiết kế singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Điều chỉnh âm lượng Master
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", value == 0 ? -80f : Mathf.Log10(value) * 20);
    }

    // Điều chỉnh âm lượng nhạc nền
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", value == 0 ? -80f : Mathf.Log10(value) * 20);
    }

    // Điều chỉnh âm lượng hiệu ứng âm thanh
    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", value == 0 ? -80f : Mathf.Log10(value) * 20);
    }


    // Các phương thức lấy âm lượng hiện tại để sử dụng trong UI
    public float GetMasterVolume()
    {
        audioMixer.GetFloat("MasterVolume", out float value);
        return Mathf.Pow(10, value / 20);
    }

    public float GetMusicVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float value);
        return Mathf.Pow(10, value / 20);
    }

    public float GetSFXVolume()
    {
        audioMixer.GetFloat("SFXVolume", out float value);
        return Mathf.Pow(10, value / 20);
    }
}
