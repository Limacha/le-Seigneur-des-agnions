using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace debugCommand
{
    [CreateAssetMenu(menuName = "console/new commande")]
    public class DebugCommand : ScriptableObject
    {
        [SerializeField, ReadOnly] private string _id = Guid.NewGuid().ToString();
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private string _format;
        [SerializeField] private bool _showHelp = true;

        [SerializeField] private DebugCommandEffect _effect;

        public string Id { get { return _id; } }
        public string Name { get { return _name; } }
        public string Description { get { return _description; } }
        public string Format { get { return _format; } }
        public bool ShowHelp { get { return _showHelp; } }
        public DebugCommandEffect Effect { get { return _effect; } }
    }

    [Serializable]
    public class DebugCommandEffect : UnityEvent<string> { }
}