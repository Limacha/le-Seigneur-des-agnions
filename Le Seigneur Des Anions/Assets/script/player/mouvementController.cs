using UnityEngine;


public class mouvementController : MonoBehaviour
{
    //vitesse de déplacement
    public float walkSpeed; //vitese du jouer
    public float runSpeed; //vitesse lors de la course

    //Imputs
    public KeyBiding inputFront; //touche avancer
    public KeyBiding inputBack; //touche reculer
    public KeyBiding inputLeft; //touche vers la gauche
    public KeyBiding inputRight; //touche vers la droite

    public Vector3 jumSpeed; //direction du saut

    CapsuleCollider playerCollider;
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        float movez = (Input.GetKey(inputFront.key)? 1 : 0) - (Input.GetKey(inputBack.key) ? 1 : 0);
        float movex = (Input.GetKey(inputRight.key) ? 1 : 0) - (Input.GetKey(inputLeft.key) ? 1 : 0);

        Vector3 move = new Vector3(movex, 0, movez);

        if (move != new Vector3(0, 0, 0))
        {
            rigidBody.MovePosition(rigidBody.position + move.normalized * walkSpeed * Time.deltaTime); //deplacement du jouer
        }

        /*
        //si on avance
        if (Input.GetKey(inputFront))
        {
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
            animator.SetBool("walk", true);
        }

        //si on recule

        if (Input.GetKey(inputBack))
        {
            transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);
            animator.SetBool("walk", true);
        }

        //roatation à gauche
        if (Input.GetKey(inputLeft))
        {
            transform.Translate(-walkSpeed * Time.deltaTime, 0, 0);
            animator.SetBool("walk", true);
        }

        //rotaion à droite
        if (Input.GetKey(inputRight))
        {
            transform.Translate(walkSpeed * Time.deltaTime, 0, 0);
            animator.SetBool("walk", true);
        }*/
    }
}
