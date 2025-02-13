using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiScript : MonoBehaviour
{
    [SerializeField] TMP_Text currentTime;
    [SerializeField] TMP_Text currentDate;
    // Update is called once per frame
    void Update()
    {
        currentTime.text = DateTime.Now.ToShortTimeString(); // date and time
        currentDate.text = DateTime.Now.ToShortDateString(); // date and time
    }
}
