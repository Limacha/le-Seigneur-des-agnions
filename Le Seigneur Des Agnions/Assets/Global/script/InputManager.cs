using debugCommand;
using inventory;
using System.Collections.Generic;
using UnityEngine;
using player;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.InputSystem;
using System.Linq;

public class InputManager : MonoBehaviour
{
    //ExcMenuController
    //Inventory
    //PlayerInteraction
    //PlayerMouvement
    //PlayerCamera
    //ConsoleSystem

    [SerializeField] private Dictionary<string, string> keys = new Dictionary<string, string>();
    [SerializeReference, ReadOnly] private ConsoleSystem console;
    [SerializeReference, ReadOnly] private Inventory inv;
    [SerializeReference, ReadOnly] private ExcMenuController excMenu;
    [SerializeReference, ReadOnly] private Player player;
    [SerializeReference, ReadOnly] private PlayerMouvement playerMouvement;
    [SerializeReference, ReadOnly] private PlayerCamera playerCamera;
    [SerializeReference, ReadOnly] private PlayerInteraction playerInteract;
    [SerializeReference, ReadOnly] private KeyBiding[] keyBidings;


    public Dictionary<string, string> Keys { get { return keys; } }

    void Awake()
    {
         keyBidings = Resources.LoadAll<KeyBiding>("keyBiding");
        foreach (KeyBiding key in keyBidings)
        {
            keys.Add(key.name, key.key.ToLower().Trim());
        }
    }

    private void Start()
    {
        InitComponent();
    }

    public void InitComponent()
    {
        gameObject.TryGetComponent(out console);
        GameObject.FindWithTag("inventory")?.TryGetComponent(out inv);
        GameObject.Find("MenuEscape")?.TryGetComponent(out excMenu);
        GameObject.FindWithTag("Player")?.TryGetComponent(out player);
        GameObject.FindWithTag("Player")?.TryGetComponent(out playerMouvement);
        GameObject.FindWithTag("Player")?.TryGetComponent(out playerCamera);
        GameObject.FindWithTag("Player")?.TryGetComponent(out playerInteract);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject[] allInterface = GameObject.FindGameObjectsWithTag("Interface");
            IEnumerable<GameObject> allActiveInterface = GameObject.FindGameObjectsWithTag("Interface").Where(inter => inter.activeSelf);
            if (allActiveInterface.Count() > 0)
            {
                foreach (GameObject activeInt in allActiveInterface)
                {
                    activeInt.SetActive(false);
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (player)
                {
                    player.CanInteract = true;
                    player.CanMouve = true;
                    player.CanLookAround = true;
                }
                else
                {
                    Debug.LogError("pas de player a unfreeze");
                }
            }
            else
            {
                if (excMenu != null)
                {
                    excMenu.ToggleMenu();
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (console != null && Input.GetKeyDown(keys["consoleKey"]))
        {
            console.OpenClose();
        }
        if (inv != null)
        {
            if (Input.GetKeyDown(keys["rotateKey"]) && inv.SlotDrag)
            {
                //inv.ItemDrag.Rotate();
                inv.SlotDrag.Rotate();
            }

            if (Input.GetKeyDown(keys["equipKey"]) && inv.SlotDrag)
            {
                inv.SlotDrag.TryEquip();
            }

            if (Input.GetKeyDown(keys["dropKey"]))
            {
                if (inv.SlotDrag)
                {
                    inv.SlotDrag.Drop();
                }
                else
                {
                    inv.DropAll();
                }
            }

            if (Input.GetKeyDown(keys["openCloseInvKey"]) && !inv.AnimationSacEnCours)
            {
                if (inv.SlotDrag == null)
                {
                    inv.OpenCloseInventory();
                }
            }
        }
        if (player != null) {
            if (playerMouvement != null && player.CanMouve)
            {
                int horizontalInput = 0;
                int verticalInput = 0;

                //obtien la direction de deplacement en fonction des inputs
                horizontalInput += Input.GetKey(keys["rightKey"]) ? 1 : 0;
                horizontalInput -= Input.GetKey(keys["leftKey"]) ? 1 : 0;
                verticalInput += Input.GetKey(keys["upKey"]) ? 1 : 0;
                verticalInput -= Input.GetKey(keys["bottomKey"]) ? 1 : 0;

                playerMouvement.HorizontalInput = horizontalInput;
                playerMouvement.VerticalInput = verticalInput;

                if (Input.GetKey(keys["jumpKey"])) //si le joueur veut et peut sauter
                {
                    playerMouvement.TryJump();
                }

                if (Input.GetKeyDown(keys["crouchKey"]))
                {
                    playerMouvement.Crouch();
                }

                playerMouvement.Sprinting = Input.GetKey(keys["sprintKey"]);
            }

            if (playerCamera != null && player.CanMouve)
            {
                playerCamera.MouseX = Input.GetAxis("Mouse X");
                playerCamera.MouseY = Input.GetAxis("Mouse Y");
            }

            if (playerInteract != null && Input.GetKeyDown(keys["interactKey"]) && player.CanInteract)
            {
                playerInteract.LaunchInteract();
            }
        }
    }
}
