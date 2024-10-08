using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AllFonction;

public class LoadSave : MonoBehaviour
{
    [SerializeField] private GameObject content; //le conteneur des bouton
    [SerializeField] private GameObject buttonPrefab; //la prefab des boutons
    [SerializeField] private StartLoad startLoad; //script de lancement de game
    [SerializeField] private bool overwrite; //si on peut overwrite les sauvegardes
    [SerializeField] private bool delete; //si on peut suprimer les sauvegardes
    [SerializeField] private int overwriteSize = 30; //taille du button overwrite
    [SerializeField] private int deleteSize = 30; //taille button delete
    [SerializeField] private int spaceBetween = 10; //espace entre les 2 bouton
    // Start is called before the first frame update
    void Start()
    {
        Fonction func = new Fonction();
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
                Debug.Log(content.transform.parent.transform.parent.transform.parent.name);
                Debug.Log(content.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<RectTransform>().offsetMin.x);
                Debug.Log(content.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<RectTransform>().offsetMax.x);
                Debug.Log(content.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<RectTransform>().offsetMax.x - content.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<RectTransform>().offsetMin.x);
                Debug.Log(content.gameObject.GetComponent<RectTransform>().offsetMax.x + content.gameObject.GetComponent<RectTransform>().offsetMin.x + content.GetComponent<VerticalLayoutGroup>().padding.left + content.GetComponent<VerticalLayoutGroup>().padding.right);
                if (overwrite && delete)
                {
                    func.SetRightRt(button.transform.GetChild(0).GetComponent<RectTransform>(), overwriteSize + spaceBetween + deleteSize + spaceBetween);

                    func.SetRightRt(button.transform.GetChild(1).GetComponent<RectTransform>(), spaceBetween + deleteSize + spaceBetween);
                    func.SetLeftRt(button.transform.GetChild(1).GetComponent<RectTransform>(), 224 - overwriteSize - spaceBetween - deleteSize - spaceBetween);

                    func.SetRightRt(button.transform.GetChild(2).GetComponent<RectTransform>(), spaceBetween);
                    func.SetLeftRt(button.transform.GetChild(2).GetComponent<RectTransform>(), 200 - deleteSize - spaceBetween);
                } else if (delete)
                {
                    func.SetRightRt(button.transform.GetChild(0).GetComponent<RectTransform>(), deleteSize + spaceBetween);

                    func.SetRightRt(button.transform.GetChild(2).GetComponent<RectTransform>(), spaceBetween);
                    func.SetLeftRt(button.transform.GetChild(2).GetComponent<RectTransform>(), 200 - deleteSize - spaceBetween);
                } else if (overwrite)
                {
                    func.SetRightRt(button.transform.GetChild(0).GetComponent<RectTransform>(), overwriteSize + spaceBetween);

                    func.SetRightRt(button.transform.GetChild(1).GetComponent<RectTransform>(), spaceBetween);
                    func.SetLeftRt(button.transform.GetChild(1).GetComponent<RectTransform>(), 200 - overwriteSize - spaceBetween);
                }

            }
        }
    }
}
