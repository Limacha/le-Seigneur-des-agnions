using debugCommand;
using inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    //ExcMenuController
    //Inventory
    //PlayerInteraction
    //PlayerMouvement
    //PlayerCamera
    //ConsoleSystem

    [SerializeField] private Dictionary<string, string> keys = new Dictionary<string, string>();
    [SerializeReference, ReadOnly] private ConsoleSystem console;
    [SerializeReference, ReadOnly] private Inventory inv;


    public Dictionary<string, string> Keys { get { return keys; } }

    void Awake()
    {
        KeyBiding[] keyBidings = Resources.LoadAll<KeyBiding>("keyBiding");
        foreach (KeyBiding key in keyBidings)
        {
            keys.Add(key.name, key.key);
        }
    }

    private void Start()
    {
        InitComponent();
    }

    public void InitComponent()
    {
        console = gameObject.GetComponent<ConsoleSystem>();
        inv = GameObject.FindWithTag("inventory")?.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (console != null && Input.GetKeyDown(keys["consoleKey"]))
        {
            console.OpenClose();
        }

        if (inv != null && Input.GetKeyDown(keys["openInvKey"]) && !inv.AnimationSacEnCours)
        {
            inv.OpenCloseInventory();   
        }
    }
}
