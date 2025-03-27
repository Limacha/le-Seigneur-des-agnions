using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mob;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using System.Linq;
using System;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace entreprise.venteAgnion
{
    public class VenteUiSetContent : MonoBehaviour
    {
        [SerializeReference] private TextMeshProUGUI nomTMP; //le TMP ou afficher le nom
        [SerializeReference] private VenteAgnionSystem venteSystem; //le system
        [SerializeReference] private SiteVente site; //le site
        [SerializeReference] private GameObject sellPanel; //le panel de vente
        [SerializeReference] private GameObject buyPanel; //le panel d'achat
        [SerializeReference] private GameObject prefab; //la prefab
        [SerializeReference] private GameObject prefAgnion; //la prefab des agnion
        [SerializeReference] private Sprite casse; //texture de la case
        [SerializeReference] private Sprite select; //texture selected
        [SerializeField] private bool refresh; //refresh

        public bool Refresh { get { return refresh; } set { refresh = value; } }
        public SiteVente Site { get { return site; } set { site = value; Clear(); Refresh = true; } }

        public void Start()
        {
            Open();
        }
        private void FixedUpdate()
        {
            if (refresh)
            {
                RefreshFunc();
                refresh = false;
            }
        }
        private void RefreshFunc()
        {
            if (site != null)
            {
                if (nomTMP != null)
                {
                    nomTMP.text = site.Nom;
                }
                if (site.SellConteneur != null)
                {
                    //Debug.Log("eqfszfzesf");
                    //Debug.Log("pokpk" + site.SellConteneur.GetComponents<Agnion>().Length);
                    //Debug.Log("gferf");
                    //Debug.Log(sellPanel.transform.childCount);
                    Agnion[] agnions = site.SellConteneur.GetComponents<Agnion>(); //recuperation des agnions
                    RectTransform rect = sellPanel.GetComponent<RectTransform>(); //recuperation du panel
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, (agnions.Length * prefab.GetComponent<RectTransform>().sizeDelta.y) + 10 + (agnions.Length * 5)); //defini la taille du panel pour scrollbar
                    while (sellPanel.transform.childCount != agnions.Length) //tant qu'il y a une difference
                    {
                        if (sellPanel.transform.childCount > agnions.Length) //si trop
                        {
                            Destroy(sellPanel.transform.GetChild(0)); //supprime
                        }
                        else
                        {
                            GameObject pref = Instantiate(prefab); //instancie
                            //defini l'affichage
                            //defini parent
                            pref.transform.SetParent(sellPanel.transform);
                            //defini la taille
                            pref.transform.localScale = Vector3.one;
                            pref.GetComponent<Button>().onClick.AddListener(() => Select(pref, true));
                        }
                    }

                    for (int i = 0; i < sellPanel.transform.childCount; i++)
                    {
                        sellPanel.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Quality: " + agnions[i].Quality;
                        sellPanel.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Prix: " + venteSystem.CalcSellPrice(new Agnion[1] { agnions[i] }, site);
                        sellPanel.transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite = (agnions[i].Selected) ? select : casse;
                    }
                }
                if (site.BuyConteneur != null)
                {
                    Agnion[] agnions = site.BuyConteneur.GetComponents<Agnion>(); //recuperation des agnions
                    RectTransform rect = buyPanel.GetComponent<RectTransform>(); //recuperation du panel
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, (agnions.Length * prefab.GetComponent<RectTransform>().sizeDelta.y) + 10 + (agnions.Length * 5)); //defini la taille du panel pour scrollbar
                    while (buyPanel.transform.childCount != agnions.Length) //tant qu'il y a une difference
                    {
                        if (buyPanel.transform.childCount > agnions.Length) //si trop
                        {
                            Destroy(buyPanel.transform.GetChild(0)); //supprime
                        }
                        else
                        {
                            GameObject pref = Instantiate(prefab); //instancie
                            //defini l'affichage
                            pref.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Quality: " + agnions[buyPanel.transform.childCount].Quality;
                            pref.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Prix: " + venteSystem.CalcBuyPrice(new Agnion[1] { agnions[buyPanel.transform.childCount] }, site);
                            //defini parent
                            pref.transform.SetParent(buyPanel.transform);
                            //defini la taille
                            pref.transform.localScale = Vector3.one;
                            pref.GetComponent<Button>().onClick.AddListener(() => Select(pref, false));
                        }
                    }

                    for (int i = 0; i < buyPanel.transform.childCount; i++)
                    {
                        buyPanel.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Quality: " + agnions[i].Quality;
                        buyPanel.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Prix: " + venteSystem.CalcBuyPrice(new Agnion[1] { agnions[i] }, site);
                        buyPanel.transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite = (agnions[i].Selected) ? select : casse;
                    }
                }
            }
        }

        private void Clear()
        {
            if (site.SellConteneur != null)
            {
                RectTransform rect = sellPanel.GetComponent<RectTransform>(); //recuperation du panel
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
                while (sellPanel.transform.childCount > 0)
                {
                    DestroyImmediate(sellPanel.transform.GetChild(0).gameObject); //supprime
                }
            }
            if (site.BuyConteneur != null)
            {
                RectTransform rect = buyPanel.GetComponent<RectTransform>(); //recuperation du panel
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
                while (buyPanel.transform.childCount > 0)
                {
                    DestroyImmediate(buyPanel.transform.GetChild(0).gameObject); //supprime
                }
            }
        }

        /// <summary>
        /// permet d'ouvrir la fenetre
        /// </summary>
        void Open()
        {
            gameObject.SetActive(true);
            site.Change = false;
            refresh = true;
        }
        /// <summary>
        /// ferme la fenetre
        /// </summary>
        void Close()
        {
            gameObject.SetActive(false);
            site.Change = true;
        }
        /// <summary>
        /// change le site lier
        /// </summary>
        /// <param name="siteV">le nouveau site</param>
        void ChangeSite(SiteVente siteV)
        {
            site.Change = true;
            site = siteV;
            site.Change = false;
        }

        public void SellSelected()
        {
            Agnion[] agnions = site.SellConteneur.GetComponents<Agnion>();
            Agnion[] agnionselect = new Agnion[agnions.Count(item => item.Selected == true)];
            int i = 0;
            foreach (Agnion agnion in agnions)
            {
                if (agnion.Selected)
                {
                    agnionselect[i] = agnion;
                    i++;
                }
            }
            if (venteSystem.Sell(agnionselect, site))
            {
                for (i = 0; i < agnionselect.Length; i++)
                {
                    int index = Array.IndexOf(site.SellConteneur.GetComponents<Agnion>(), agnionselect[i]);
                    agnionselect[i] = null;
                    DestroyImmediate(site.SellConteneur.GetComponents<Agnion>()[index]);
                    DestroyImmediate(sellPanel.transform.GetChild(index).gameObject);
                }
            }

            refresh = true;
        }

        public void BuySelected()
        {
            Agnion[] agnions = site.BuyConteneur.GetComponents<Agnion>();
            Agnion[] agnionselect = new Agnion[agnions.Count(item => item.Selected == true)];
            int i = 0;
            foreach (Agnion agnion in agnions)
            {
                if (agnion.Selected)
                {
                    agnionselect[i] = agnion;
                    i++;
                }
            }
            if (venteSystem.Buy(agnionselect, site))
            {
                for (i = 0; i < agnionselect.Length; i++)
                {
                    int index = Array.IndexOf(site.BuyConteneur.GetComponents<Agnion>(), agnionselect[i]);
                    SpawnAgnion(agnionselect[i]);
                    agnionselect[i] = null;
                    DestroyImmediate(site.BuyConteneur.GetComponents<Agnion>()[index]);
                    DestroyImmediate(buyPanel.transform.GetChild(index).gameObject);
                }
            }

            refresh = true;
        }

        public void RecupSelected()
        {
            //obtien tout les agnion select
            Agnion[] agnions = site.SellConteneur.GetComponents<Agnion>();
            Agnion[] agnionselect = new Agnion[agnions.Count(item => item.Selected == true)];
            //remplit le tableau
            int i = 0;
            foreach (Agnion agnion in agnions)
            {
                if (agnion.Selected)
                {
                    agnionselect[i] = agnion;
                    i++;
                }
            }

            for (i = 0; i < agnionselect.Length; i++)
            {
                int index = Array.IndexOf(site.SellConteneur.GetComponents<Agnion>(), agnionselect[i]);
                SpawnAgnion(agnionselect[i]);
                agnionselect[i] = null;
                DestroyImmediate(site.SellConteneur.GetComponents<Agnion>()[index]);
                DestroyImmediate(sellPanel.transform.GetChild(index).gameObject);
            }


            refresh = true;
        }

        private void SpawnAgnion(Agnion agnion)
        {
            Debug.Log("spawn");
            GameObject agn = Instantiate(prefAgnion);
            agn.GetComponent<Agnion>().Quality = agnion.Quality;
            agn.GetComponent<Transform>().position = site.SpawnPoint.position;
        }

        public void Select(GameObject panel, bool sell)
        {
            // Assurer que l'index est valide
            int index = panel.transform.GetSiblingIndex();

            // Vérifier qu'il y a suffisamment d'éléments dans le tableau
            Agnion[] agnions = (sell) ? site.SellConteneur.GetComponents<Agnion>() : site.BuyConteneur.GetComponents<Agnion>();

            if (index >= 0 && index < agnions.Length)
            {
                Agnion agn = agnions[index];
                agn.Selected = !agn.Selected; // Marquer l'élément comme sélectionné
            }
            else
            {
                Debug.LogWarning("Index hors limites pour les Agnions.");
            }
            refresh = true;
        }
    }
}