using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string save = "new game"; //nom de la save
    public string Save { get { return save; } set { save = value; } }
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void DestroySave()
    {
        SaveSystem.DeleteSave();
    }
}
