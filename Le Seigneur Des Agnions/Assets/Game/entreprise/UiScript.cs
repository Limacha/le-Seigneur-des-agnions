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
        currentTime.text = $"{cycle.Hours:D2}:{cycle.Minutes:D2}";
        currentName.text = manager.Save;
        currentDate.text = $"{manager.ThisDate.Date.Day:D2}:{manager.ThisDate.Date.Month:D2}:{manager.ThisDate.Date.Year:D2}";
    }

    public void OpenInt(GameObject trans)
    {
        trans.SetActive(true);
    }

    public void CloseInt(GameObject trans)
    {
        trans.SetActive(false);
    }

    public void SwapInt(GameObject trans)
    {
        trans.SetActive(!trans.activeSelf);
    }
}
