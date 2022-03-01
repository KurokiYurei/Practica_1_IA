using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButtonActivator : MonoBehaviour
{
    public bool openDoor;
    private bool opened;
    public GameObject door;
    public float timer;
    private float maxTime;

    private void Start()
    {
        timer = 0;
        maxTime = 10.0f;
        openDoor = false;
        opened = false;
    }
    void Update()
    {
        
        if (openDoor)
        {
            timer += Time.deltaTime;
            if (!opened)
            {
                door.GetComponent<SpriteRenderer>().color = Color.green;
                door.tag = "DOOR_OPENED";
                opened = true;
            }
            if(timer >= maxTime)
            {
                door.GetComponent<SpriteRenderer>().color = Color.red;
                door.tag = "DOOR";
                openDoor = false;
                timer = 0;
            }
        }
    }
}
