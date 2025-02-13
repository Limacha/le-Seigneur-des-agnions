using System.Collections;
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
