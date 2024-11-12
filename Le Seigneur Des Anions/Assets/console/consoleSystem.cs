using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GUI.skin = defaultskin;
            GUI.backgroundColor = inputBackColor;
            GUI.Box(new Rect(posX, Screen.height - posY - inputHeight, inputWidth, inputHeight), "");
            GUI.backgroundColor = inputBorderColor;
            GUI.contentColor = inputTextColor;
            input = GUI.TextField(new Rect(posX + inputMargX, Screen.height - posY - inputHeight + inputMargY, inputWidth - (inputMargX * 2), inputHeight - (inputMargY * 2)), input);
            GUI.skin.textField.fontSize = fontSize;

            GUI.skin = defaultskin;
            GUI.backgroundColor = labelBackColor;
            GUI.Box(new Rect(posX, Screen.height - posY - inputHeight - labelHeight - labelSpaceInput, labelWidth, labelHeight), "");
            GUI.backgroundColor = labelBorderColor;
            GUI.contentColor = labelTextColor;
            GUI.Label(new Rect(posX + labelMargX, Screen.height - posY - inputHeight - labelHeight - labelSpaceInput + labelMargY, labelWidth - (labelMargX * 2), labelHeight - (labelMargY * 2)), label);
            GUI.skin.label.fontSize = fontSize;
        }

        public void HandleInput()
        {
            string[] properties = input.Split(' ');
            for (int i = 0; i < commandList.Count; i++)
            {
                if (properties[0] == commandList[i].Name)
                {
                    commandList[i].Effect.Invoke(input);
                    return;
                }
            }
            label = "Commande pas trouver.";
        }
    }
}
