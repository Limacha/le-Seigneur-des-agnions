using inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntrepriseVisuel : MonoBehaviour
{
    [Header("Taille des elements ref(800, 600)")]
    [SerializeField] private Vector2 sizePanel;

    private CanvasScaler canvasreso; //le canvasScaler pour voir les dimention d'origine
    private float ratioX; //proportion taille/reference
    private GameObject panel; //le panel de l'entriprise

    [Header("GUI")]
    public Texture texture;
    public GUISkin skin;
    [SerializeField] private int labelWidth;
    [SerializeField] private int labelHeight;
    [SerializeField] private float labelMargX;
    [SerializeField] private float labelMargY;
    [SerializeField] private Color labelBorderColor;
    [SerializeField] private Color labelTextColor;
    [SerializeField] private Color labelBackColor;

    void Awake()
    {
        canvasreso = GetComponent<CanvasScaler>();
        ratioX = Screen.width / canvasreso.referenceResolution.x;
    
        panel = transform.GetChild(0).gameObject;

        InitEntreprise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitEntreprise()
    {
        panel.GetComponent<RectTransform>().sizeDelta = sizePanel;
    }

    public void OnGUI()
    {
        string label = "";
        var ratioX = Screen.width / canvasreso.referenceResolution.x;
        //var ratioY = Screen.height / canvasreso.referenceResolution.y;

        //label += $"panel x{rt.position.x} y{rt.position.y} \n";
        //label += $"panel W{rt.rect.width} H{rt.rect.height} \n";

        //label += $"panel 0 x{rt.position.x - rt.rect.width/2} y{rt.position.y - rt.rect.height/2} \n";
        //label += $"panel 0 + pad x{rt.position.x - rt.rect.width/2 + paddingLeft} y{rt.position.y - rt.rect.height/2 + paddingTop} \n";

        label += $"ecran W{Screen.width} H{Screen.height} \n";
        label += $"origi W{canvasreso.referenceResolution.x} H{canvasreso.referenceResolution.y} \n";
        label += $"ratio X{ratioX} YratioY \n";

        GUI.skin = skin;
        GUI.backgroundColor = labelBackColor;
        GUI.Box(new Rect(0, 0, labelWidth, labelHeight), "");
        GUI.backgroundColor = labelBorderColor;
        GUI.contentColor = labelTextColor;
        GUI.Label(new Rect(labelMargX, labelMargY, (labelWidth - (labelMargX * 2)) * ratioX, (labelHeight - (labelMargY * 2)) * ratioX), label);
        GUI.skin.label.fontSize = 10;


    }
}
