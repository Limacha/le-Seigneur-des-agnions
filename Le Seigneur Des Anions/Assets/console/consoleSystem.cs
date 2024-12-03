using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace debugCommand
{
    public class ConsoleSystem : MonoBehaviour
    {
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
        [SerializeField] private List<DebugCommand> commandList;

        public bool ShowConsole { get { return showConsole; } set { showConsole = value; } }
        public List<DebugCommand> CommandList { get { return commandList; } }
        public string UserInput { get { return input; } }
        public string Label { get { return label; } set { label = value; } }

        public void Update()
        {

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
            for (int i = 0; i < commandList.Count; i++)
            {
                if (properties[0] == '/' + commandList[i].Name)
                {
                    commandList[i].Effect.Invoke(input);
                    return;
                }
            }
            label = "Commande pas trouver.";
        }
    }
}
