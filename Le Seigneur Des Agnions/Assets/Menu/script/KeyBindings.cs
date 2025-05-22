using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class KeyBindings : MonoBehaviour
{
    [SerializeField] GameObject waitingForKey;
    private string[] OkKeys = new string[] {
        "delete",
        "tab",
        "clear",
        "pause",
        "space",
        "up",
        "down",
        "right",
        "left",
        "insert",
        "home",
        "end",
        "page up",
        "page down",
        "f1",
        "f2",
        "f3",
        "f4",
        "f5",
        "f6",
        "f7",
        "f8",
        "f9",
        "f10",
        "f11",
        "f12",
        "f13",
        "f14",
        "f15",
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "!",
        "\"",
        "#",
        "$",
        "&",
        "'",
        "(",
        ")",
        "*",
        "+",
        ",",
        "-",
        ".",
        "/",
        ":",
        ";",
        "<",
        "=",
        ">",
        "?",
        "@",
        "[",
        "\\",
        "]",
        "^",
        "_",
        "`",
        "a",
        "b",
        "c",
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "w",
        "x",
        "y",
        "z",
        "numlock",
        "caps lock",
        "scroll lock",
        "right shift",
        "left shift",
        "right ctrl",
        "left ctrl",
        "right alt",
        "left alt"
    };
    void Start()
    {
        Button ButChangeTouche = transform.GetChild(1).gameObject.GetComponent<Button>();
        ButChangeTouche.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        waitingForKey.SetActive(true);

        if (VerifKeys() != null)
        {
            string Id = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text;
            waitingForKey.SetActive(false);
        }
    }

    void Update()
    {
        // Vérifie constamment si une touche a été pressée
        if (waitingForKey != null)
        {
            if (waitingForKey.activeSelf && VerifKeys() != null)
            {
                waitingForKey.SetActive(false); // Ferme le menu si la touche est pressée
            }
        }
    }

    private string VerifKeys()
    {
        foreach (var keys in OkKeys)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                waitingForKey.SetActive(false); // Ferme le menu si la touche est pressée
                return null;
            }
            else
            {
                if (Input.GetKeyDown(keys))
                {
                    return keys;
                }
            }
            
        }
        return null;
    }
}