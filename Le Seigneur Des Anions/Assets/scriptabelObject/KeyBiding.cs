using UnityEngine;

[CreateAssetMenu(fileName = "scriptabelObject/keyBiding", menuName = "Keys/New key")]
[System.Serializable]
public class KeyBiding : ScriptableObject
{
    public string nom; //nom de l'item
    public string description; //description de l'item
    public string key; //input a presser
}
