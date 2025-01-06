using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomID : MonoBehaviour
{
    public GameObject room;
    public GameObject Switch;
    public AnomalyID[] Anomalys;
    public OpenCloseDoor1 doorscript;
    public int lastValue;
    public int NumOfAnomaly;
    public int AnomalyDifficultLevel;
    public int NumberOfAnomalyTrigger;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("started");
        Anomalys = room.GetComponentsInChildren<AnomalyID>();
        NumOfAnomaly = Anomalys.Length;
        NumberOfAnomalyTrigger = 1;
    }

    public void RoomTrigger()
    {
        if (doorscript != null)
            if (!doorscript.toggle == false)
                doorscript.DoorCloseed();

        Switch.GetComponent<LightOnOffMobile>().IsPower = false;

        // Check if there are any anomalies before triggering
        if (Anomalys.Length == 0)
        {
            Debug.LogWarning("No anomalies found in the room.");
            return; // Exit the function if no anomalies exist
        }

        // Ensure that NumberOfAnomalyTrigger does not exceed the number of anomalies
        int numberOfTriggers = Mathf.Min(NumberOfAnomalyTrigger, Anomalys.Length);

        for (int i = 0; i < numberOfTriggers; i++)
        {
            int random = Random.Range(0, Anomalys.Length);
            // chỗ này để random, đg test nên để 0 là cái đầu tiên (nhiều thì thành random)
            Anomalys[random].Active();
        }
    }

}
