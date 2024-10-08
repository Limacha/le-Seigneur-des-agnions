using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform orientation;

    float sensitivityX;
    float sensitivityY;
    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        sensitivityX = player.CameraSensibilityX;
        sensitivityY = player.CameraSensibilityY;
    }

    // Update is called once per frame
    void Update()
    {
        //obtenir les infso souris
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate la cam
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
