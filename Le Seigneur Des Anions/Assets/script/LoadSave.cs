using JetBrains.Annotations;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSave : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private StartLoad startLoad;
    // Start is called before the first frame update
    void Start()
    {
        if (Directory.Exists(SaveSystem.DefaultPath))
        {
            
            foreach(string file in Directory.GetDirectories(SaveSystem.DefaultPath))
            {
                //Debug.Log(file.Split('\\')[1]);

                var button = Instantiate(buttonPrefab);
                button.name = file.Split('\\')[1];
                button.transform.SetParent(content.transform, false);
                button.transform.GetComponentInChildren<TMP_Text>().text = file.Split('\\')[1];
                button.transform.GetComponentInChildren<TMP_Text>().margin = new Vector4(10, 0, 10, 0);
                button.GetComponent<Button>().onClick.AddListener(() => { startLoad.startButton = button.GetComponent<Button>(); });
            }
        }
    }
}
