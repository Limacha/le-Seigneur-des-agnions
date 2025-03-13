using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

namespace interaction
{
    public abstract class InteractionObject : MonoBehaviour
    {
        [SerializeField] protected bool shine = true;
        // Start is called before the first frame update
        void Start()
        {
            if (shine)
            {
                //Debug.Log("I'm chining");
            }
        }

        public virtual void InteractionPlayer()
        {
            Debug.Log("no interaction");
        }

    }
}