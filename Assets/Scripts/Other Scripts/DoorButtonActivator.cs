using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonActivator : MonoBehaviour
{
    public bool openDoor;
    private bool opened;
    public GameObject door;
    private float timer;
    private float maxTime;

    private void Start()
    {
        timer = 0;
        maxTime = 15.0f;
        openDoor = false;
        opened = false;
    }
    void Update()
    {
        if (openDoor)
        {
            timer =+ Time.deltaTime;
            if (!opened)
            {
                door.SetActive(true);
                opened = true;
            }
            if(timer >= maxTime)
            {
                door.SetActive(false);
                openDoor = false;
            }
        }
    }
}
