using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using debugCommand;
using entreprise;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string save = "new game"; //nom de la save
    [SerializeReference] private bool isTheController; //si s'est un second gameManager
    [SerializeReference] private ConsoleSystem consoleSystem; //console
    [SerializeField] private DateTime thisDate = new DateTime(); //le jour actuel
    [SerializeField] private bool loose = false; //si le joueur a perdu

    [SerializeReference, ReadOnly]  private Entreprise entreprise;

    public string Save { get { return save; } set { save = value; } }
    public ConsoleSystem ConsoleSystem { get { return consoleSystem; } }
    public DateTime ThisDate { get { return thisDate; } set { thisDate = value; } }

    public void Awake()
    {
        if (isTheController)
        {
            DeleteOther();
        }
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        //Cursor.lockState = CursorLockMode.Locked; 
        //Cursor.visible = false;
    }

    /// <summary>
    /// appeler les du chargement de scene
    /// </summary>
    /// <param name="arg0">la scene</param>
    /// <param name="arg1">le mode</param>
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (isTheController)
        {
            DeleteOther();
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            entreprise = GameObject.FindWithTag("Entreprise")?.GetComponent<Entreprise>();
            if (entreprise != null)
            {
                entreprise.Nom = save;
            }
            else
            {
                SceneManager.LoadScene("Menu");
            }
            if (gameObject.TryGetComponent(out InputManager inputManager))
            {
                inputManager.InitComponent();
            }
        }
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            DestroyAllDontDestroyOnLoad();
        }
    }

    /// <summary>
    /// detroi tout les objoct dont lors du chargement du menu sauf debug et 1 gamemanager
    /// </summary>
    void DestroyAllDontDestroyOnLoad()
    {
        // Trouver tous les objets de la scène active
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        List<GameObject> dontDestroyObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Vérifie si l'objet est dans la scène spéciale "DontDestroyOnLoad"
            if (obj.scene.buildIndex == -1)
            {
                dontDestroyObjects.Add(obj);
            }
        }
        bool gameManeged = false;
        // Détruire les objets trouvés
        foreach (GameObject obj in dontDestroyObjects)
        {
            if (obj.name != "[Debug Updater]")
            {
                if (obj.name == "GameManager" && !gameManeged)
                {
                    gameManeged |= true;
                }
                else
                {
                    Destroy(obj);
                    Debug.Log("Objet détruit : " + obj.name);
                }
            }
        }
    }

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }*/
    }

    /// <summary>
    /// suprime tout les object tager SecondGameController
    /// </summary>
    public void DeleteOther()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("SecondGameController");
        foreach (var gameObject in gameObjects)
        {
            //Debug.Log(gameObject.name);
            Destroy(gameObject);
        }
    }

    #region saveManager
    public bool ChangeSaveName(string nom)
    {
        string oldSave = save.ToLower();
        save = nom.ToLower();
        if (GameObject.FindWithTag("Entreprise"))
        {
            GameObject.FindWithTag("Entreprise").GetComponent<Entreprise>().Nom = save;
        }
        return SaveSystem.RenameSave(oldSave, save);
    }
    #endregion

    public void VerifAllGame(Entreprise ent)
    {
        if (ent)
        {
            if(ent.Argent < 0)
            {
                loose = true;
            }
        }
    }
}
