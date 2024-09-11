using UnityEngine;


public class mouvementController : MonoBehaviour
{
    //vitesse de déplacement
    public float walkSpeed;
    public float runSpeed;

    //Imputs
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    public Vector3 jumSpeed;
    CapsuleCollider playerCollider;
    Rigidbody rigidBody;




    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        float movez = (Input.GetKey(inputFront)? 1 : 0) - (Input.GetKey(inputBack)? 1 : 0);
        float movex = (Input.GetKey(inputLeft)? 1 : 0) - (Input.GetKey(inputRight)? 1 : 0);

        Vector3 move = new Vector3(movex, 0, movez);

        if (move != new Vector3(0, 0, 0))
        {
            rigidBody.MovePosition(rigidBody.position + move.normalized * walkSpeed * Time.deltaTime);
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
