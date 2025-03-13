using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace debugCommand
{
    public class ConsoleSystem : MonoBehaviour
    {
        [SerializeReference] private KeyBiding openConsole; //key pour ouvrir/fermer la console
        [SerializeField] private bool showConsole = false;
        [SerializeField] private string input;
        [SerializeField] private string label;

        [Header("Style")]
        [SerializeField] private GUISkin defaultskin;
        [SerializeField] private float posX;
        [SerializeField] private float posY;
        [SerializeField] private int fontSize;

        [Header("style input")]
        [SerializeField] private int inputWidth;
        [SerializeField] private int inputHeight;
        [SerializeField] private float inputMargX;
        [SerializeField] private float inputMargY;
        [SerializeField] private Color inputBorderColor;
        [SerializeField] private Color inputTextColor;
        [SerializeField] private Color inputBackColor;

        [Header("style text area")]
        [SerializeField] private int labelWidth;
        [SerializeField] private int labelHeight;
        [SerializeField] private float labelMargX;
        [SerializeField] private float labelMargY;
        [SerializeField] private float labelSpaceInput;
        [SerializeField] private Color labelBorderColor;
        [SerializeField] private Color labelTextColor;
        [SerializeField] private Color labelBackColor;

        [Header("commande")]
        [SerializeField] private DebugCommand[] commands;

        public bool ShowConsole { get { return showConsole; } set { showConsole = value; } }
        public DebugCommand[] Commands { get { return commands; } }
        public string UserInput { get { return input; } }
        public string Label { get { return label; } set { label = value; } }

        void Awake()
        {
            commands = Resources.LoadAll<DebugCommand>("Commands");
        }

        public void Start()
        {
            var gameObjects = GameObject.FindGameObjectsWithTag("GameController");
            if (gameObjects.Length == 1 && gameObject.tag != "GameController")
            {
                Destroy(gameObject);
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if (Input.GetKeyDown(openConsole.key))
            {
                showConsole = !showConsole;
            }
            if (!showConsole) { return; }
            if (Input.GetKeyDown("return"))
            {
                HandleInput();
                input = "";
            }
        }

        private void OnGUI()
        {
            if (!showConsole) { return; }

            CanvasScaler canvasreso = GameObject.Find("Canvas").GetComponent<CanvasScaler>(); //le canvas
            float ratioX = Screen.width / canvasreso.referenceResolution.x;

            GUI.skin = defaultskin;
            GUI.backgroundColor = inputBackColor;
            GUI.Box(new Rect(posX, Screen.height - posY - (inputHeight * ratioX), inputWidth * ratioX, inputHeight * ratioX), "");
            GUI.backgroundColor = inputBorderColor;
            GUI.contentColor = inputTextColor;
            input = GUI.TextField(new Rect(posX + inputMargX, Screen.height - posY - (inputHeight) * ratioX + inputMargY, (inputWidth - (inputMargX * 2)) * ratioX, (inputHeight - (inputMargY * 2)) * ratioX), input);
            GUI.skin.textField.fontSize = (int)(fontSize * ratioX);

            GUI.skin = defaultskin;
            GUI.backgroundColor = labelBackColor;
            GUI.Box(new Rect(posX, Screen.height - posY - (inputHeight * ratioX) - (labelHeight * ratioX) - labelSpaceInput, labelWidth * ratioX, labelHeight * ratioX), "");
            
            Rect labelRect = new Rect(posX + labelMargX, Screen.height - posY - (inputHeight * ratioX) - (labelHeight * ratioX) - labelSpaceInput + labelMargY, (labelWidth - (labelMargX * 2)) * ratioX, (labelHeight - (labelMargY * 2)) * ratioX);

            GUI.backgroundColor = Color.white;
            GUI.backgroundColor = labelBorderColor;
            GUI.contentColor = labelTextColor;
            GUI.Label(labelRect, label);
            GUI.skin.label.fontSize = (int)(fontSize * ratioX);
        }

        public void HandleInput()
        {
            string[] properties = input.Split(' ');
            for (int i = 0; i < commands.Length; i++)
            {
                if (properties[0] == '/' + commands[i].Name)
                {
                    commands[i].Effect.Invoke(input);
                    return;
                }
            }
            label = "Commande pas trouver.";
        }
    }
}
