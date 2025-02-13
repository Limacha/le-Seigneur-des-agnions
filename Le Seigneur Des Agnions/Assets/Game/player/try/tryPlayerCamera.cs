using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class tryPlayerCamera : MonoBehaviour
    {
        [SerializeField] private float senX = 350f; //sensibiliter du deplacement de la camera sur l'axe x
        [SerializeField] private float senY = 250f; //sensibiliter du deplacement de la camera sur l'axe y

        [SerializeField, Range(-180, 0)] private float minX; //le min en x
        [SerializeField, Range(0, 180)] private float maxX; //le max en x

        [SerializeReference] private Transform cameraTransform; // La camera du joueur
        [SerializeReference] private Transform headBone; // mixamorig:Head
        [SerializeReference] private Transform neckBone; // mixamorig:Neck
        [SerializeReference] private Transform playerBody; //le transform du joueur
        [SerializeReference] private Transform oritentation; //l'orientation du joueur

        [SerializeField, ReadOnly] private float xRotation = 0f; //la rotation de la tete du joueur

        [SerializeReference] private Player player; //le joueur


        void Start()
        {
            //desactive et block le curser
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        void Update()
        {
            if (player.CanLookAround)
            {
                // Rotation horizontale du corps
                RotatteBodyForLooking();

                // Contr�le de la t�te et nuque (inclinaison et avancement sur Z)
                RotateHeadAndNeckt();
            }
        }
        /// <summary>
        /// rotation orizontal du corps pour tourner la vue
        /// </summary>
        void RotatteBodyForLooking()
        {
            // Mouvement horizontal de la cam�ra et du corps (rotation du joueur)
            float mouseX = Input.GetAxis("Mouse X") * senX * Time.deltaTime;
            // Rotation du joueur sur l'axe Y (horizontal)
            playerBody.rotation = Quaternion.Euler(0, playerBody.rotation.eulerAngles.y + mouseX, 0);
            oritentation.rotation = Quaternion.Euler(0, playerBody.rotation.eulerAngles.y, 0);
        }

        void RotateHeadAndNeckt()
        {
            if (headBone && neckBone)
            {
                // Rotation de la t�te sur l'axe X en fonction de la souris
                float mouseY = Input.GetAxis("Mouse Y") * senY * Time.deltaTime;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, minX, maxX); // Limite de l'angle de la t�te

                // Appliquer la rotation sur l'os de la t�te
                headBone.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                // Appliquer la m�me rotation � la nuque
                neckBone.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                // Calculer l'avance de la t�te sur l'axe Z
                float zOffset = xRotation * 0.001f; // 0.001 par degr� d'inclinaison
                headBone.localPosition = new Vector3(headBone.localPosition.x, headBone.localPosition.y, zOffset);

                // Appliquer le m�me d�placement de Z � la nuque
                neckBone.localPosition = new Vector3(neckBone.localPosition.x, neckBone.localPosition.y, zOffset);
            }
        }
    }
}