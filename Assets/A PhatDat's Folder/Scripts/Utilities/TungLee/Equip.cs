//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Equip : MonoBehaviour
//{
//    public EquipSlot Slots;
//    public GameObject CurrentObj;
//    public void Start()
//    {
//        Slots = GameObject.FindGameObjectWithTag("RightHand").GetComponent<EquipSlot>();
//        CurrentObj = new GameObject();
//    }
//    public void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            if (CurrentObj != null)
//                CurrentObj.SetActive(false);
//            if(Slots.ItemInSlot[0] != null)
//            {
//                CurrentObj = Slots.ItemInSlot[0];
//                CurrentObj.SetActive(true);
//            }        
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            if (CurrentObj != null)
//                CurrentObj.SetActive(false);
//            if (Slots.ItemInSlot[1] != null)
//            {
//                CurrentObj = Slots.ItemInSlot[1];
//                CurrentObj.SetActive(true);
//            }
//        }
//        else if(Input.GetKeyDown(KeyCode.Alpha3))
//        {
//            if (CurrentObj != null)
//                CurrentObj.SetActive(false);
//            if (Slots.ItemInSlot[2] != null)
//            {
//                CurrentObj = Slots.ItemInSlot[2];
//                CurrentObj.SetActive(true);
//            }
//        }
//    }
//}

//Code bên trên là PC
//Code cho mobile
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public EquipSlot Slots;
    public GameObject CurrentObj;
    private int currentIndex = -1;

    public void Start()
    {
        Slots = GameObject.FindGameObjectWithTag("RightHand").GetComponent<EquipSlot>();
        CurrentObj = null;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToItem(2);
        }
    }

    public void SwitchToNextItem()
    {
        int totalItems = Slots.ItemInSlot.Length;

        // Tìm item tiếp theo có trong các slot
        for (int i = 1; i <= totalItems; i++)
        {
            currentIndex = (currentIndex + 1) % totalItems;

            if (Slots.ItemInSlot[currentIndex] != null)  // Nếu tìm thấy item hợp lệ
            {
                SwitchToItem(currentIndex);  // Chuyển sang item đó
                return;  // Kết thúc hàm
            }
        }

        Debug.LogWarning("Không có item nào để chuyển đổi!"); // Nếu không có item hợp lệ
    }



    public void SwitchToItem(int index)
    {
        if (CurrentObj != null)
            CurrentObj.SetActive(false);

        if (Slots.ItemInSlot[index] != null)
        {
            CurrentObj = Slots.ItemInSlot[index];
            CurrentObj.SetActive(true);
        }
    }
}


