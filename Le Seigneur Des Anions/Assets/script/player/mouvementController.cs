using UnityEngine;


public class mouvementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed; //vitesse de deplacement
    [SerializeField] private float walkSpeed; //vitesse de marche
    [SerializeField] private float sprintSpeed; //vitesse de course

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed; //vitesse lorsque on est acroupit
    [SerializeField, Range(0.1f, 5)] private float crouchYScale; //taille du jouer acroupit
    [SerializeField, Range(0.1f, 5)] private float startYScale; //taille du jouer par default

    [Header("Jumping")]
    [SerializeField] private float jumpForce; //puissance du saut
    [SerializeField, Range(0.1f, 60)] private float jumpCooldown; //delay entre chaque saut
    [SerializeField] private float airMultiplier; //vitesse dans les airs
    bool readyToJump = true; //si il peut sauter

    [Header("Keybinds")]
    [SerializeReference] private KeyBiding jumpKey; //touche pour sauter
    [SerializeReference] private KeyBiding crouchKey; //touche pour s'acroupir
    [SerializeReference] private KeyBiding sprintKey; //touche pour courir
    [SerializeReference] private KeyBiding inputFront; //touche avancer
    [SerializeReference] private KeyBiding inputBack; //touche reculer
    [SerializeReference] private KeyBiding inputLeft; //touche vers la gauche
    [SerializeReference] private KeyBiding inputRight; //touche vers la droite

    [Header("Ground Check")]
    [SerializeField] LayerMask whatIsGround; //tout se qui est considerer comme un sol
    [SerializeField]bool grounded = true; //si il est sur le sol
    float playerHeight; //taille du jouer

    [Header("autre")]
    [SerializeReference] private Transform orientation; //orientation du jouer
    [SerializeField] MovementState state; //etat actuel du jouer

    float movez; //deplacement horizontal
    float movex; //deplacement vertical

    Vector3 moveDirection; //direction de deplacement

    Rigidbody rb; //rigide body

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //attribution du rigidebody
        rb.freezeRotation = true; //block la rotation
    }

    private void Update()
    {
        playerHeight = GetComponent<Transform>().GetComponentInChildren<CapsuleCollider>().height;
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);

        GetInput();
        StateHandler();
        MovePlayer();
    }

    private void GetInput()
    {
        movex = (Input.GetKey(inputFront.key) ? 1 : 0) - (Input.GetKey(inputBack.key) ? 1 : 0); //vertical
        movez = (Input.GetKey(inputRight.key) ? 1 : 0) - (Input.GetKey(inputLeft.key) ? 1 : 0); //horizontal

        // when to jump
        if (Input.GetKey(jumpKey.key) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey.key))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey.key))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey.key))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey.key))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * movex + orientation.right * movez;

        // on ground
        if (grounded)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.deltaTime);
        }
        // in air
        else if (!grounded)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.deltaTime * airMultiplier);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void DebugState()
    {
        Debug.Log("Le joueur est entraint de " + state.ToString());
    }
}
