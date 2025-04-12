using entreprise.venteAgnion;
using inventory;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using mob;
using System.Linq;

namespace debugCommand
{
    public class DebugCommandEvent : MonoBehaviour
    {
        [SerializeField] private ItemData agnion;
        /// <summary>
        /// commande qui affiche les autres commande
        /// </summary>
        public void Help()
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            string helpText = "";

            foreach (DebugCommand command in console.Commands)
            {
                if (command.ShowHelp)
                {
                    helpText += $"{command.Format}\n";
                }
            }
            console.Label = helpText;
        }
        /// <summary>
        /// affiche le texte taper
        /// </summary>
        /// <param name="input">text taper par l'utilisateur</param>
        public void ShowText(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            if (input.Split('\"').Length > 2)
            {
                console.Label = input.Split('\"')[1];
                return;
            }
            console.Label = "Merci de respecter la syntax.";
        }
        /// <summary>
        /// permet de se donner un object
        /// </summary>
        /// <param name="input">text de l'utilisateur avec le nom[1] puis le nombre[2]</param>
        public void Give(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            Inventory inventaire = GameObject.Find("Inventory").GetComponent<Inventory>();
            string retour = "Une erreur est survenu.";
            string[] properties = input.Split(' ');

            if (properties.Length == 3)
            {
                ItemData item = inventaire.FindItemWhitName(properties[1]);
                if (item != null && item.Restriction.Contains(ItemData.Restrict.inventory))
                {
                    if (int.TryParse(properties[2], out int number))
                    {
                        if (number != 0)
                        {
                            bool possible = true;
                            int i = 0;
                            if (number < 0)
                            {
                                retour = $"Supresion de {-number} {properties[1]}.\n";
                                console.Label = retour;
                                for (i = 0; i < -number && possible; i++)
                                {
                                    possible = inventaire.RemoveItem(item);
                                }
                                retour += $"{i} {properties[1]} ont été suprimer.";
                                console.Label = retour;
                            }
                            else
                            {
                                retour = $"Ajout de {number} {properties[1]}.";
                                console.Label = retour;
                                for (i = 0; i < number && possible; i++)
                                {
                                    possible = inventaire.AddItem(item);
                                }
                                if (!possible)
                                {
                                    i--;
                                }
                                retour += $"{i} {properties[1]} ont été ajouter.";
                                console.Label = retour;
                            }
                        }
                        else
                        {
                            retour = "Si tu veux rien tu n'aura rien";
                        }
                    }
                    else
                    {
                        retour = "La valeur n°2 n'est pas un entier!";
                    }
                }
                else
                {
                    retour = $"l'item avec comme nom {properties[1]} n'existe pas.";
                }
            }
            else
            {
                retour = "Nombre de proprieter invalid";
            }
            console.Label = retour;
        }
        /// <summary>
        /// sauvegarde une partie sur un nom donner
        /// </summary>
        /// <param name="input">l'entrer de lutilisateur avec le nom de la save[1]</param>
        public void Save(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            Inventory inventaire = GameObject.Find("Inventory").GetComponent<Inventory>();
            string save = input.Split(" ")[1];

            console.Label = $"sauvegarde de {save}:\n";

            console.Label += $"Tentative de SaveInventory dans {save}.\n";
            SaveSystem.SaveInventory(save, inventaire);
            console.Label += $"Inventaire sauver dans {save}.\n";

            console.Label += $"Fin de la sauvegarde de {save}.\n";
        }
        /// <summary>
        /// charge une partie sur un nom donner
        /// </summary>
        /// <param name="input">l'entrer de lutilisateur avec le nom de la save[1]</param>
        public void LoadSave(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            Inventory inventaire = GameObject.Find("Inventory").GetComponent<Inventory>();
            string save = input.Split(" ")[1];

            console.Label = $"chargement de {save}:\n";

            console.Label += $"Tentative de LoadInventory de {save}.\n";
            InventorySaveData inventorySaveData = SaveSystem.LoadInventory(save);
            console.Label += $"Inventaire charge de {save}.\n";
            //Debug.Log(inventorySaveData.ItemSaveDatas.Length);
            inventaire.ResetInventory();
            foreach (var item in inventorySaveData.ItemSaveDatas)
            {
                if(item != null)
                {
                    //Debug.Log($"n°{k++}");
                    //Debug.Log(item.id);
                    //Debug.Log(item.refX + " " + item.refY);
                    //Debug.Log(item.stack + "stack" + item.rotation + "°");
                    ItemData itemData = inventaire.FindItemWhitId(item._id);
                    itemData.Init();
                    itemData.RefX = item.refX;
                    itemData.RefY = item.refY;
                    itemData.Rotate = item.rotation;
                    itemData.Stack = item.stack;
                    if (itemData.Rotate != 360)
                    {
                        int rotate = 360;
                        do
                        {
                            itemData.rotatePatern();
                            rotate -= 90;
                        } while (itemData.Rotate != rotate || rotate == 0);
                    }
                    inventaire.PlaceItemInInventory(itemData, itemData.RefX, itemData.RefY);
                }
            }
            inventaire.RefreshInventory();

            console.Label += $"Fin du chargement de {save}.\n";
        }

        public void DeleteSave(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            string save = input.Split(" ")[1];
            console.Label = $"suppression de {save}:\n";
            SaveSystem.DeleteSave(save);
        }

        public void DuplicateSave(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            console.Label = $"info pas en ordre\n";
            if (input.Split(" ").Length == 3)
            {
                string save = input.Split(" ")[1];
                string newSave = input.Split(" ")[2];
                console.Label = $"duplicage de {save} en {newSave}\n";
                SaveSystem.DuplicateSave(save, newSave);
            }
        }

        public void NicoTest()
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<ConsoleSystem>();
            console.Label = $"chargement de la scene test";
            SceneManager.LoadScene("nicoTest");
            console.Label += $"la scene test est charger";
        }

        public void LoadScene(string input)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            ConsoleSystem console = gameManager.GetComponent<ConsoleSystem>();
            string scene = input.Split(" ")[1];
            if (scene != null)
            {
                console.Label = $"chargement de la scene {scene}";
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
                console.Label += $"la scene {scene} est charger";/*
                var gameObjects = GameObject.FindGameObjectsWithTag("GameController");
                Debug.Log(gameObjects.Length);
                if(gameObjects.Length == 1)
                {
                    gameObjects[0].GetComponent<GameManager>().DeleteOther();
                }*/
            }
            else
            {
                console.Label = $"Merci de mettre un nom de scene valide!";
            }
        }

        public void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void HipHop()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Animator>().SetTrigger("triggerHipHop");
        }
        public void VendreAgnion()
        {
            GameObject gameManager = GameObject.Find("GameManager");
            ConsoleSystem console = gameManager?.GetComponent<ConsoleSystem>();
            Inventory inv = GameObject.Find("Inventory")?.GetComponent<Inventory>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (console == null || inv == null || player == null)
            {
                console.Label = "Probleme lors de la recuperation";
                return;
            }

            if (inv.TwoHands == null && (inv.Hands == null || (inv.LeftHand == null && inv.RightHand == null)))
            {
                console.Label = "Rien en main";
                return;
            }

            byte quality;
            GameObject entreprise = GameObject.Find("Entreprise");
            VenteAgnionSystem venteSystem = entreprise?.GetComponent<VenteAgnionSystem>();
            SiteVente site = GameObject.Find("consoleSite")?.GetComponent<SiteVente>();

            if (venteSystem == null || site == null)
            {
                console.Label = "Pas de site ou de système de vente";
                return;
            }

            ItemData heldItem = inv.TwoHands ?? inv.LeftHand ?? inv.RightHand;
            if (heldItem == null || heldItem.ID != agnion.ID || !byte.TryParse(heldItem.PersonalData, out quality))
            {
                console.Label = "L'objet en main n'est pas un agnion valide";
                return;
            }

            site.Init();
            console.Label = "Initialisation du site...";

            Agnion agn = site.SellConteneur.AddComponent<Agnion>();
            agn.Quality = quality;

            Agnion[] agnionsToSell = new Agnion[1] { agn };
            float price = venteSystem.CalcSellPrice(agnionsToSell, site);

            console.Label = "Prix : " + price;

            if (venteSystem.Sell(agnionsToSell, site))
            {
                // Vérifie quelle main tenait l'agnion et la vide
                if (inv.TwoHands == heldItem)
                    inv.TwoHands = null;
                else if (inv.LeftHand == heldItem)
                    inv.LeftHand = null;
                else if (inv.RightHand == heldItem)
                    inv.RightHand = null;

                console.Label += "\nAgnion vendu";
            }
            else
            {
                heldItem.Drop(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                console.Label = "Agnion invendable";
            }
        }

    }
}