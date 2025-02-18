using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteVente : MonoBehaviour
{
    private int demande; //le nombre d'agnion demande dans le commerce
    private int ratio; //le ratio d'achat en fonctiion du prix global sur le marche
    public int Demande { get { return demande; } set { demande = value; } }
    public int Ratio { get { return Ratio; } set { ratio = value; } }

}
