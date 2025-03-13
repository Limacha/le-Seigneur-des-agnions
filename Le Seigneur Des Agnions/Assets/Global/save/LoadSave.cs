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
    [SerializeField] private int textMarginLeft = 5; //marge a gauche du texte
    // Start is called before the first frame update
    void Start()
    {
        if (Directory.Exists(SaveSystem.DefaultPath))
        {
            
            foreach(string file in Directory.GetDirectories(SaveSystem.DefaultPath))
            {
                //Debug.Log(file.Split('\\')[1]);
                GameObject button = Instantiate(buttonPrefab);
                button.name = file.Split('\\')[1];
                button.transform.SetParent(content.transform, false);
                button.transform.GetComponentInChildren<TMP_Text>().text = file.Split('\\')[1];
                button.transform.GetComponentInChildren<TMP_Text>().margin = new Vector4(textMarginLeft, 0, 0, 0);
                button.GetComponent<Button>().onClick.AddListener(() => { startLoad.startButton = button.GetComponent<Button>(); });
                if (overwrite && delete)
                {
                    //texte
                    Fonction.SetRightRt(button.transform.GetChild(0).GetComponent<RectTransform>(), overwriteSize + deleteSize + spaceBetween * 3);

                    //overwrite
                    button.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(overwriteSize/2 + spaceBetween + deleteSize + spaceBetween), 0);
                    button.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta += new Vector2(-button.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.x + overwriteSize, 0);

                    //delete
                    button.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(deleteSize / 2 + spaceBetween), 0);
                    button.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta += new Vector2(-button.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta.x + deleteSize, 0);
                } else if (delete)
                {
                    //texte
                    Fonction.SetRightRt(button.transform.GetChild(0).GetComponent<RectTransform>(),deleteSize + spaceBetween * 3);

                    //overwrite
                    button.transform.GetChild(1).gameObject.SetActive(false);

                    //delete
                    button.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(deleteSize/2 + spaceBetween*2) , 0);
                    button.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta += new Vector2(-button.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta.x + deleteSize, 0);
                } else if (overwrite)
                {
                    //texte
                    Fonction.SetRightRt(button.transform.GetChild(0).GetComponent<RectTransform>(), overwriteSize + spaceBetween * 3);

                    //ovrerwrite
                    button.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(overwriteSize/2 + spaceBetween*2), 0);
                    button.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta += new Vector2(-button.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta.x + overwriteSize, 0);
                    //delete
                    button.transform.GetChild(2).gameObject.SetActive(false);
                }

            }
        }
    }
}
