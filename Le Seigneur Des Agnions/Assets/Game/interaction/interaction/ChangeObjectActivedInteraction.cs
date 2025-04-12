using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interaction
{
    public class ChangeObjectActivedInteraction : InteractionObject
    {
        [SerializeField] private GameObject obj;
        [SerializeField, ReadOnly] private bool actived;

        public void Start()
        {
            if(obj == null)
            {
                Debug.Log($"{gameObject.name}: pas d'obj");
                Destroy(gameObject);
            }
        }

        public override void InteractionPlayer()
        {
            actived = !obj.activeSelf;
            obj.SetActive(actived);
        }
    }
}