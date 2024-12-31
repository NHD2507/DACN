using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnToNormal : MonoBehaviour
{
    public AudioSource test;
    public AudioClip sound;
    public bool inCameraReach;
    public Changed ch;
    public GameObject Camera;
    public GameObject anomaly;
    public bool ena;
    [SerializeField] private PhotoCD _photoCD;
    public SetTriggerNumber triggerNumber;
    // Start is called before the first frame update
    void Start()
    {

        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");
        }


        inCameraReach = false;                   //player not in zone       
        ena = false;
        _photoCD = Camera.GetComponent<Photograph>()._photoCD;
        triggerNumber = GameObject.Find("AllAnomalys").GetComponent<SetTriggerNumber>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraContact();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CameraReach")  //if player in zone
        {
            UIManager.Instance.showToggleUI();
            inCameraReach = true;
        }
    }


    private void OnTriggerExit(Collider other)     //if player exit zone
    {
        if (other.gameObject.tag == "CameraReach")
        {
            inCameraReach = false;
            UIManager.Instance.hideToggleUI();
        }
    }
    public void CameraContact()
    {
        if (Camera.activeSelf)
        {
            ena = true;

            if (inCameraReach == true && Input.GetKeyDown(KeyCode.F) && !_photoCD.IsOutOfUseTime)           //if in zone and press F key
            {
                ch.enabled = false;
                anomaly.SetActive(false);
                ch.normal.SetActive(true);
                test.PlayOneShot(sound);
                inCameraReach = false;
                UIManager.Instance.hideToggleUI();
                triggerNumber.Decrease();
                gameObject.GetComponent<AnomalyCount>().enabled = true;
            }

        }
        else ena = false;
    }

}
