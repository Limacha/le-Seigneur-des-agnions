using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using debugCommand;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string save = "new game"; //nom de la save
    [SerializeReference] private KeyBiding openConsole; //key pour ouvrir/fermer la console
    [SerializeReference] private ConsoleSystem consoleSystem; //console

    public string Save { get { return save; } set { save = value; } }
    public ConsoleSystem ConsoleSystem { get { return consoleSystem; } }
    public void Awake()
    {
        DontDestroyOnLoad(this);
        //Cursor.lockState = CursorLockMode.Locked; 
        //Cursor.visible = false;
    }
    public void Update()
    {
        if (Input.GetKeyDown(openConsole.key))
        {
            consoleSystem.ShowConsole = !consoleSystem.ShowConsole;
        }
    }
    /*
    public void DestroySave()
    {
        SaveSystem.DeleteSave(save);
    }*/
}
