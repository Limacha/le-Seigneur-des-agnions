using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using sc.terrain.proceduralpainter;

public class PlayerMouvement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, ReadOnly] private float moveSpeed; //vitesse du joueur
    [SerializeField] private float walkSpeed; //vitesse quand il marche
    [SerializeField] private float sprintSpeed; //vitesse quand il sprint
    [SerializeField] private float crouchSpeed; //vitesse acroupi

    [SerializeField] private float groundDrag;

    [Header("jump")]
    [SerializeField] private float jumpForce; //puissance de saut
    [SerializeField] private float jumpCooldown; //delay minimum avant de resauter
    [SerializeField] private float airMultiplier; //changer la vitesse en la multipliant
    [SerializeField, ReadOnly] private bool readyToJump; //si il est pret a sauter ou pas

    [Header("Crouching")]
    [SerializeField] private float crouchYScale; //taille acroupi
    [SerializeField, ReadOnly] private float startYScale; //taille de basse
    [SerializeField, ReadOnly] private float centerY; //position en y du centre du collider

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle; //angle max de la pente
    private RaycastHit slopeHit; //l'object sur le quel on marche
    [SerializeField, ReadOnly] private bool exitingSlope; //si on ne veut plus etre en slope

    [Header("Gravity")]
    [SerializeField] private float gravity;
    [SerializeField] private float mass;
    [SerializeField] private bool useGravity;

    [Header("Keybinds")]
    [SerializeReference] private KeyBiding upKey; //touche pour avance
    [SerializeReference] private KeyBiding bottomKey; //touche pour reculer
    [SerializeReference] private KeyBiding leftKey; //touche pour allez a gauche
    [SerializeReference] private KeyBiding rightKey; //touche pour allez a droite
    [SerializeReference] private KeyBiding jumpKey; //touche pour sauter
    [SerializeReference] private KeyBiding sprintKey; //touche courir
    [SerializeReference] private KeyBiding crouchKey; //touche s'accroupir

    [Header("Ground Check")]
    [SerializeField] private float maxDistance; //distance a verifier sous le joueur
    [SerializeField] private LayerMask whatIsGround; //se qui est un sol
    [SerializeField, ReadOnly] private bool grounded; //si il est au sol

    [SerializeReference] private Transform orientation; //orientation du joueur

    private float horizontalInput; //poussez horizontal
    private float verticalInput; //poussez vertical

    [SerializeField, ReadOnly] private Vector3 moveDirection; //la direction dans le quel le joueur va

    private Rigidbody rb; //le rigidbody
    private CapsuleCollider capsuleCollider; //le collider
    [SerializeReference] private Player player;

    [SerializeReference] private TextMeshProUGUI textspeed;
    [SerializeReference] private TextMeshProUGUI text2;

    [SerializeField, ReadOnly] private MovementState state;
    private enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = walkSpeed;

        capsuleCollider = GetComponent<CapsuleCollider>();
        centerY = capsuleCollider.center.y;
        startYScale = capsuleCollider.height;

        readyToJump = true;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        // ground check
        grounded = Physics.Raycast(ray, maxDistance, whatIsGround);

        //Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        MyInput();
        StateHandler();
        SpeedControl();

        // defini le drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (textspeed != null && text2 != null)
        {
            var vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            textspeed.text = vel.magnitude.ToString();
            text2.text = state.ToString();
        }
    }

    private void FixedUpdate()
    {
            MovePlayer();

        //pour gerer nous meme la graviter
        if (useGravity)
        {
            rb.AddForce(Vector3.down * gravity * mass);
        }
    }
    /// <summary>
    /// gere toute les entres utilisateur
    /// </summary>
    private void MyInput()
    {
        horizontalInput = 0;
        verticalInput = 0;

        horizontalInput += (Input.GetKey(rightKey.key)) ? 1 : 0;
        horizontalInput -= (Input.GetKey(leftKey.key)) ? 1 : 0;
        verticalInput += (Input.GetKey(upKey.key)) ? 1 : 0;
        verticalInput -= (Input.GetKey(bottomKey.key)) ? 1 : 0;

        // when to jump
        if (Input.GetKey(jumpKey.key) && readyToJump && grounded) //si le joueur veut et peut sauter
        {
            readyToJump = false; //ne peut plus sauter

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); //lui permet de resauter au bout de x second
        }

        //a changer pour animation
        // start crouch
        if (Input.GetKeyDown(crouchKey.key))
        {
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, capsuleCollider.center.y / (startYScale / crouchYScale), capsuleCollider.center.z);
            capsuleCollider.height = crouchYScale;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey.key))
        {
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, centerY, capsuleCollider.center.z);
            capsuleCollider.height = startYScale;
        }
    }
    /// <summary>
    /// permet d'obtennir l'etat du joueur
    /// </summary>
    private void StateHandler()
    {
        // Mode - Crouching
        if (grounded && Input.GetKey(crouchKey.key))
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

    /// <summary>
    /// deplace le joueur
    /// </summary>
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; //creez la direction en fonction de la rotation de la camera et des resulta des input user
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z); //l'empeche de voler si regarde le dol

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 80, ForceMode.Force);
            }
        }

        //applique la force
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // turn gravity off while on slope
        useGravity = !OnSlope();
    }
    /// <summary>
    /// control la vitesse du joueur
    /// </summary>
    private void SpeedControl()
    {
        // limiting speed on slope car y change donc peut pas le reset en 0
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z); //obtenir la vitesse au sol

            //verifie si il faut limiter
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed; //recreez une velociter au max
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //applique la velociter limiter
            }
        }
    }
    /// <summary>
    /// fait sauter le joueur
    /// </summary>
    private void Jump()
    {
        exitingSlope = true;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    //set readyToJump true
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }
    /// <summary>
    /// verifie si on glisse
    /// </summary>
    /// <returns>vrai si pente dif 0 et plus petit que maxSlopeAngle</returns>
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, maxDistance, whatIsGround)) //permet d'obtenir l'object sur le quel on marche
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //Debug.Log(slopeHit.normal + " " + Vector3.up + " " + angle + " " + (angle < maxSlopeAngle && angle != 0));
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    /// <summary>
    /// creez une direction perpendiculaire au plan
    /// </summary>
    /// <returns>la direction pour avance normalement</returns>
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}