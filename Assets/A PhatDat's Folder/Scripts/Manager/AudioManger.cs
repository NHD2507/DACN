using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Đối tượng duy nhất tồn tại trong toàn bộ ứng dụng, giúp quản lý âm thanh tập trung và tránh xung đột
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource jumpScareSound;
    public AudioSource backGroundSound;

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

    // Chơi nhạc win
    public void PlayWinSound()
    {
        if (winSound != null)
        {
            winSound.Play();
        }
    }

    // Chơi nhạc lose
    public void PlayLoseSound()
    {
        if (loseSound != null)
        {
            loseSound.Play();
        }
    }

    // Chơi âm thanh jump scare
    public void PlayJumpScareSound()
    {
        if (jumpScareSound != null)
        {
            jumpScareSound.Play();
        }
    }

    // Chơi nhạc nền
    public void PlayBackgroundSound()
    {
        if (backGroundSound != null && !backGroundSound.isPlaying)
        {
            backGroundSound.Play();
        }
    }

    // Dừng nhạc nền
    public void StopBackgroundSound()
    {
        if (backGroundSound != null && backGroundSound.isPlaying)
        {
            backGroundSound.Stop();
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
