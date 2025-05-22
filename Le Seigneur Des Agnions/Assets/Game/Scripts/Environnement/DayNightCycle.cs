using entreprise;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static entreprise.fisc.FiscControlSystem;

public class DayNightCycle : MonoBehaviour
{
    [SerializeReference] private GameObject sun;
    [SerializeReference] private GameObject moon;
    [SerializeField] private float TimeSpan = 1;
    float actualTime;
    [SerializeField, Range(0, 59)] private int minutes = 0;
    [SerializeField, Range(0, 23)] private int hours = 6;
    [SerializeField] private int day = 0;
    int EntreTemps = 0;
    bool OnceOnATime = true;

    [Header("new day")]
    [SerializeField, Range(0, 1000)] private int chanceControl = 1;//chance de se faire choper sur 1000


    public int Minutes { get { return minutes; } }
    public int Hours { get { return hours; } }
    public int Day { get { return day; } }

    void Start()
    {
        sun.transform.rotation = Quaternion.Euler(0, 19, 0);
        StartCoroutine(DayNightLoop());
    }

    IEnumerator DayNightLoop()
    {
        while (true)
        {
            EntreTemps++;
            if (EntreTemps == 4)
            {
                actualTime++;

                if (hours > 5 && hours < 18)
                {
                    if (OnceOnATime)
                    {
                        actualTime = 0;
                        moon.SetActive(false);
                        sun.SetActive(true);
                        OnceOnATime = false;
                    }
                    StartCoroutine(SmoothRotation(sun.transform));
                }
                else
                {
                    if (!OnceOnATime)
                    {
                        actualTime = 0;
                        moon.SetActive(true);
                        sun.SetActive(false);
                        OnceOnATime = true;
                    }
                    StartCoroutine(SmoothRotation(moon.transform));
                }

                EntreTemps = 0;
            }
            minutes++;
            if (minutes == 60)
            {
                minutes = 0;
                hours++;
                if (hours == 24)
                {
                    hours = 0;
                    day++;
                    NewDay();
                }
            }

            yield return new WaitForSeconds(TimeSpan);
        }
    }
    IEnumerator SmoothRotation(Transform obj)
    {
        float startAngle = actualTime;
        float targetAngle = actualTime + 1;
        float duration = TimeSpan * 4f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration);
            obj.rotation = Quaternion.Euler(newAngle, 19, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.rotation = Quaternion.Euler(targetAngle, 19, 0);
    }

    /// <summary>
    /// lance une nouvelle journer
    /// </summary>
    private void NewDay()
    {
        GameManager manager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        Entreprise ent = GameObject.FindWithTag("Entreprise")?.GetComponent<Entreprise>();
        if (ent)
        {
            foreach (Dette dette in ent.Dettes)
            {
                dette.NewDay();
            }
            ent.ControlDette();
        }
        else
        {
            Debug.LogError("no entreprise set in cycle");
        }
        if (manager)
        {
            manager.ThisDate = new DateTime().AddDays(day);
            if (ent)
            {
                if (UnityEngine.Random.Range(0, 1000) < chanceControl) LaunchCorruption(ent, manager);
            }
            manager.VerifAllGame(ent);
        }
        else
        {
            Debug.LogError("no gamemanager set in cycle");
        }
    }
}
