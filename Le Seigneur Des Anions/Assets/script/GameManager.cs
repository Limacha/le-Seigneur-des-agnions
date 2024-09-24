using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string save = "new game";
    public string Save { get { return save; } set { save = value; } }
}
