using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button SingleplayerBtn;
    [SerializeField] private Button MultiplayerBtn;
    [SerializeField] private Button ExitBtn;


    private void Awake()
    {
        MultiplayerBtn.onClick.AddListener(() => {

        });
        SingleplayerBtn.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.SampleScene);
        });
        ExitBtn.onClick.AddListener(() => {
            Application.Quit();
        });

        Time.timeScale = 1f;
    }
}
