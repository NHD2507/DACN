using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ItemPickup : MonoBehaviour
{
    public GameObject inttext, Item;
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
            inttext.SetActive(true);
            inReach = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inttext.SetActive(false);
            inReach = false;
        }
    }

    private void Start()
    {
        // Ensure the AudioSource component is attached to the same GameObject
        pickup = GetComponent<AudioSource>();
        if (pickup == null)
        {
            Debug.LogError("AudioSource component is missing from the GameObject.");
        }

        // Initialize the position and flags
        pos = hand.transform.position;
        followhand = false;

        // Ensure EquipSlot is found on RightHand GameObject
        Slots = GameObject.FindGameObjectWithTag("RightHand").GetComponent<EquipSlot>();
        if (Slots == null)
        {
            Debug.LogError("EquipSlot component is missing from the RightHand GameObject.");
        }

        // Ensure ReachItem is found on Reach GameObject
        CurrentItem = GameObject.FindGameObjectWithTag("Reach").GetComponent<ReachItem>();
        if (CurrentItem == null)
        {
            Debug.LogError("ReachItem component is missing from the Reach GameObject.");
        }
    }


    void Update()
    {
        if (inReach == true && !followhand)
        {           
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (GetItemIntoSlot())
                {
                    inttext.SetActive(false);
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