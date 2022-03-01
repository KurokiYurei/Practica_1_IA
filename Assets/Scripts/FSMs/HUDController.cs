using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    private float maxTime;
    private float currentTime;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI savedMiceText;
    private int savedMice;

    public static HUDController HUDinstance;
    private void Awake()
    {
        if (HUDinstance == null)
        {
            HUDinstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        maxTime = 0;
        currentTime = 60;
        timerText.text = "Time: " + maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime > maxTime)
        {
            timerText.text = "Time: " + Mathf.Round(currentTime);
            savedMiceText.text = "Saved Mice: " + savedMice;
        }

    }
    public void AddSaved()
    {
        savedMice++;
    }
}
