using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using mob;
using TMPro;
using UnityEngine;

public class TouchesSettings : MonoBehaviour
{
    [SerializeReference] private GameObject PanelTouches; //le panel d'achat
    [SerializeReference] private GameObject PrefabTouche; //la prefab
    [SerializeField] private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null && inputManager.Keys != null)
        {
            int i = 0;
            KeyBiding[] keyBidings = Resources.LoadAll<KeyBiding>("keyBiding");
            foreach (KeyBiding key in keyBidings)
            {
                GameObject prefab = Instantiate(PrefabTouche);
                prefab.transform.SetParent(PanelTouches.transform);
                prefab.transform.localScale = Vector3.one;
                PanelTouches.gameObject.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = key.nom;
                PanelTouches.gameObject.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = key.key;
                PanelTouches.gameObject.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = key.description;
                PanelTouches.gameObject.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = key.name;
                i++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
