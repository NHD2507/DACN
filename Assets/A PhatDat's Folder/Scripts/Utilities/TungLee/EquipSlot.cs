using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipSlot : MonoBehaviour
{
    public GameObject[] ItemInSlot;
    public bool[] IsFull;
    public void Awake()
    {
        ItemInSlot = new GameObject[3];
        IsFull = new bool[3];
    }
    public void DebugSlots()
    {
        for (int i = 0; i < ItemInSlot.Length; i++)
        {
            Debug.Log($"Slot {i}: {(ItemInSlot[i] != null ? ItemInSlot[i].name : "Trống")}");
        }
    }

}
