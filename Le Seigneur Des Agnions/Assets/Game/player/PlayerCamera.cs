using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float senX = 350f; //sensibiliter du deplacement de la camera sur l'axe x
        [SerializeField] private float senY = 250f; //sensibiliter du deplacement de la camera sur l'axe y

        [SerializeField, Range(-180, 0)] private float minX; //le min en x
        [SerializeField, Range(0, 180)] private float maxX; //le max en x

        [SerializeReference] private Transform headBone; // mixamorig:Head
        [SerializeReference] private Transform neckBone; // mixamorig:Neck
        [SerializeReference] private Transform playerBody; //le transform du joueur

        [SerializeField, ReadOnly] private float xRotation = 0f; //la rotation de la tete du joueur

        [SerializeReference, ReadOnly] private Player player; //le joueur

        [SerializeField, ReadOnly] private float mouseX; //mouvement de la souris sur l'axe x horizontal
        [SerializeField, ReadOnly] private float mouseY; //mouvement de la souris sur l'axe y

        public float MouseX {  get { return mouseX; } set { mouseX = Mathf.Clamp(value, -1, 1) * senX * Time.deltaTime; } }
        public float MouseY {  get { return mouseY; } set { mouseY = Mathf.Clamp(value, -1, 1) * senY * Time.deltaTime; } }
        /*
        [SerializeReference] private Camera playerCamera; //la camera du joueur
        public Camera CamPlayer { get { return playerCamera; } } //distance pour interagir*/


        public void Start()
        {
            player = gameObject.GetComponent<Player>();
            if (player == null)
            {
                Destroy(gameObject);
            }
            //desactive et block le curser
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        public void Update()
        {
            if (player.CanLookAround)
            {
                // Rotation horizontale du corps
                RotatteBodyForLooking();

                // Contrôle de la tête et nuque (inclinaison et avancement sur Z)
                RotateHeadAndNeckt();
            }
        }
        /// <summary>
        /// rotation orizontal du corps pour tourner la vue
        /// </summary>
        private void RotatteBodyForLooking()
        {
            // Mouvement horizontal de la caméra et du corps (rotation du joueur)
            //float mouseX = Input.GetAxis("Mouse X") * senX * Time.deltaTime;
            // Rotation du joueur sur l'axe Y (horizontal)
            //Debug.Log(playerBody.rotation.eulerAngles.y + mouseX);
            //playerBody.rotation = Quaternion.Euler(0, playerBody.rotation.eulerAngles.y + mouseX, 0);
            playerBody.Rotate(Vector3.up * mouseX);
            //oritentation.rotation = Quaternion.Euler(0, playerBody.rotation.eulerAngles.y, 0);
        }

        private void RotateHeadAndNeckt()
        {
            if (headBone && neckBone)
            {
                // Rotation de la tête sur l'axe X en fonction de la souris
                //float mouseY = Input.GetAxis("Mouse Y") * senY * Time.deltaTime;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, minX, maxX); // Limite de l'angle de la tête

                // Appliquer la rotation sur l'os de la tête
                headBone.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                // Appliquer la même rotation à la nuque
                neckBone.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                // Calculer l'avance de la tête sur l'axe Z
                float zOffset = xRotation * 0.001f; // 0.001 par degré d'inclinaison
                headBone.localPosition = new Vector3(headBone.localPosition.x, headBone.localPosition.y, zOffset);

                // Appliquer le même déplacement de Z à la nuque
                neckBone.localPosition = new Vector3(neckBone.localPosition.x, neckBone.localPosition.y, zOffset);
            }
        }
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float senX; //sensibiliter du deplacement de la camera sur l'axe x
    [SerializeField] private float senY; //sensibiliter du deplacement de la camera sur l'axe y

    [SerializeField, Range(-180, 0)] private float minX; //le min en x
    [SerializeField, Range(0, 180)] private float maxX; //le max en x
    [SerializeField, Range(-180, 0)] private float minY; //le min en y
    [SerializeField, Range(0, 180)] private float maxY; //le max en y

    [SerializeReference] private Transform orientation; //l'objet a rotate pour tourner la camera
    [SerializeReference] private Transform position; //l'objet a rotate pour tourner la camera
    [SerializeReference] private Transform cam; //l'objet a rotate pour tourner la camera
    [SerializeReference] private Transform head; //l'objet a rotate pour tourner la camera


    void Start()
    {
        //desactive et block le curser
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //reset la rotation
        orientation.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        //obtien le deplacement voulu
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX; //deplacement de la souris en x (inverser sur axe rotation)
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY; //deplacement de la souris en y (inverser sur axe rotation)

        //calcule la nouvelle position
        float axeX = orientation.rotation.eulerAngles.x - mouseY;
        float axeY = orientation.rotation.eulerAngles.y + mouseX;
        float axeZ = orientation.rotation.eulerAngles.z;

        //si plus que le max et plus petit que le min(vu range 0-360 alors on ajoute le min(vu que negatif))
        if (axeX > maxX && axeX < 360 + minX)
        {
            axeX = orientation.rotation.eulerAngles.x;
        }

        //si plus que le max et plus petit que le min(vu range 0-360 alors on ajoute le min(vu que negatif))
        if (axeY > maxY && axeY < 360 + minY)
        {
            axeY = orientation.rotation.eulerAngles.y;
        }

        //applique la rotation
        orientation.rotation = Quaternion.Euler(axeX, axeY, axeZ);
        cam.rotation = orientation.rotation;
        cam.position = position.position;

        head.rotation = orientation.rotation;
    }
}
*/