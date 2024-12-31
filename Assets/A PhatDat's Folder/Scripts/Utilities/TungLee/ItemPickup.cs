using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ItemPickup : MonoBehaviour
{
    public GameObject Item;
    public AudioSource pickup;
    public AudioClip pickupSound;
    public Transform hand;
    public Vector3 pos;
    public Quaternion rot;
    public bool inReach;
    public bool followhand;
    public EquipSlot Slots;
    public ReachItem CurrentItem;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            UIManager.Instance.showToggleUI();
            inReach = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            UIManager.Instance.hideToggleUI();
            inReach = false;
        }
    }

    private void Start()
    {

        if (UIManager.Instance != null)
        {
            UIManager.Instance.hideToggleUI();
        }
        else
        {
            Debug.LogWarning("UIManager không có trong ứng dụng");
        }

        pos = hand.transform.position;
        followhand = false;
        Slots = GameObject.FindGameObjectWithTag("RightHand").GetComponent<EquipSlot>();
        CurrentItem = GameObject.FindGameObjectWithTag("Reach").GetComponent<ReachItem>();
    }
    void Update()
    {
        if (inReach == true && !followhand)
        {           
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (GetItemIntoSlot())
                {
                    UIManager.Instance.hideToggleUI();
                    inReach = false;
                    PlayPickUpSound();
                    Item.transform.localPosition = pos;
                    followhand = true;
                    OnHand temp = CurrentItem.CurrentItem.GetComponent<OnHand>();
                    if (temp != null)
                    {
                        temp.onHand = followhand;
                    }
                    Item.SetActive(false);
                }               
            }
        }
        else if(followhand)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) { Item.SetActive(true);}
            transform.position = hand.position;
            transform.rotation = hand.rotation;
        }
    }
    private bool GetItemIntoSlot()
    {
        for (int i = 0; i < Slots.ItemInSlot.Length; i++)
        {
            if (!Slots.IsFull[i])
            {                
                Slots.ItemInSlot[i] = Item;
                Slots.IsFull[i] = true;
                return true;
            }
        }
        return false;
    }
    void PlayPickUpSound()
    {
        pickup.PlayOneShot(pickupSound);
    }
}