using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UiScript : MonoBehaviour
{
    [SerializeReference] TMP_Text currentTime;
    [SerializeReference] TMP_Text currentName;
    [SerializeReference] TMP_Text currentDate;

    [SerializeReference] DayNightCycle cycle;
    [SerializeReference] GameManager manager;

    // Update is called once per frame
    void Update()
    {
        currentTime.text = $"{cycle.Hours}:{cycle.Minutes}";
        currentName.text= manager.Save;
        currentDate.text = $"{manager.ThisDate.Date.Day}:{manager.ThisDate.Date.Month}:{manager.ThisDate.Date.Year}";
    }
}
