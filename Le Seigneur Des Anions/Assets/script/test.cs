using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class MyEvent : UnityEvent { }


public class test : MonoBehaviour
{


        public MyEvent OnEvent;
    public void ttt() {
        Debug.Log("fbhkkjdfljkfdkjl");
    }
    public void tttt(string t)
    {
        Debug.Log(t);
    }
}
public class t
{

}

public abstract class ttt
{

}

public interface tt
{

}

public class yoyo : MyEvent, tt
{

}
