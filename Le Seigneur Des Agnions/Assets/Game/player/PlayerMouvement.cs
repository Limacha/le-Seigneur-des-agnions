using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

namespace player
{
    public class PlayerMouvement : MonoBehaviour
    {
        [Header("boolControl")]
        [SerializeField] private bool fly;//fait volez le jouer

        [Header("Movement")]
        [SerializeField, ReadOnly] private float moveSpeed; //vitesse du joueur
        [SerializeField] private float walkSpeed; //vitesse quand il marche
        [SerializeField] private float sprintSpeed; //vitesse quand il sprint
        [SerializeField] private float crouchSpeed; //vitesse acroupi
        [SerializeField, ReadOnly] private bool sprinting; //si il court

        [SerializeField] private float groundDrag; //acrochage du sol

        [Header("jump")]
        [SerializeField] private float jumpForce; //puissance de saut
        [SerializeField] private float jumpCooldown; //delay minimum avant de resauter
        [SerializeField] private float airMultiplier; //changer la vitesse en la multipliant
        [SerializeField, ReadOnly] private bool readyToJump; //si il est pret a sauter ou pas

        [Header("Crouching")]
        [SerializeField] private bool crouch; //si acroupit ou non
        [SerializeField] private float crouchYScale; //taille acroupi
        [SerializeField, ReadOnly] private float startYScale; //taille de basse
        [SerializeField, ReadOnly] private float centerY; //position en y du centre du collider

        [Header("Slope Handling")]
        [SerializeField] private float maxSlopeAngle; //angle max de la pente
        private RaycastHit slopeHit; //l'object sur le quel on marche
        [SerializeField, ReadOnly] private bool exitingSlope; //si on ne veut plus etre en slope

        [Header("Gravity")]
        [SerializeField] private float gravity; //la puissance de la graviter
        [SerializeField] private float mass; //la masse du joueur
        [SerializeField, ReadOnly] private bool useGravity; //si on utilise la graviter
        /*
        [Header("Keybinds")]
        [SerializeReference] private KeyBiding upKey; //touche pour avance
        [SerializeReference] private KeyBiding bottomKey; //touche pour reculer
        [SerializeReference] private KeyBiding leftKey; //touche pour allez a gauche
        [SerializeReference] private KeyBiding rightKey; //touche pour allez a droite
        [SerializeReference] private KeyBiding jumpKey; //touche pour sauter
        [SerializeReference] private KeyBiding sprintKey; //touche courir
        [SerializeReference] private KeyBiding crouchKey; //touche s'accroupir*/

        [Header("Ground Check")]
        [SerializeField] private float maxDistance; //distance a verifier sous le joueur
        [SerializeField] private float boxCastWidth; //distance a verifier sous le joueur
        [SerializeField] private LayerMask whatIsGround; //se qui est un sol
        [SerializeField, ReadOnly] private bool grounded; //si il est au sol

        private float horizontalInput; //poussez horizontal
        private float verticalInput; //poussez vertical

        [SerializeField, ReadOnly] private Vector3 moveDirection; //la direction dans le quel le joueur va

        private Rigidbody rb; //le rigidbody
        private CapsuleCollider capsuleCollider; //le collider
        private Animator animator; //l'animateur
        [SerializeReference, ReadOnly] private Player player; //le joueur

        //[SerializeReference] private TextMeshProUGUI textspeed; //texte pour afficher le mouvement du joueur 
        //[SerializeReference] private TextMeshProUGUI text2; //deuxiemme texte pour afficher qq chose

        [SerializeField, ReadOnly] private MovementState state = MovementState.stop; //l'etat du joueur

        public bool Sprinting { get { return sprinting; } set { sprinting = value; } } //poussez horizontal
        public float HorizontalInput { get { return horizontalInput; } set { horizontalInput = Mathf.Clamp(value, -1, 1); } } //poussez horizontal
        public float VerticalInput { get { return verticalInput; } set { verticalInput = Mathf.Clamp(value, -1, 1); } } //poussez vertical

        private enum MovementState
        {
            walking, //si il marche
            sprinting, //si il cour
            crouching, //si il est acroupi
            air, //si il est dans les air
            stop, //si il est freeze
            fly, //si il vol
            wearBigThings //si il porte qq chose de gros (comme un agnion)
        }

        private void Start()
        {
            player = gameObject.GetComponent<Player>();
            if (player == null)
            {
                Destroy(gameObject);
            }
            //set toute les ref des script a soit
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            moveSpeed = walkSpeed;

            capsuleCollider = GetComponent<CapsuleCollider>();
            centerY = capsuleCollider.center.y;
            startYScale = capsuleCollider.height;

            readyToJump = true;

            player = GetComponent<Player>();
            animator = GetComponent<Animator>();
        }

        void OnDrawGizmos()
        {
            //dessine le cube dans la scene
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + (Vector3.down * maxDistance / 2), new Vector3(boxCastWidth, maxDistance, boxCastWidth));
        }

        private void Update()
        {
            //verifie si il est au sol ou pas
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(boxCastWidth, maxDistance, boxCastWidth) / 2, Vector3.down, Quaternion.identity, maxDistance, whatIsGround);
            grounded = hits.Length > 0;

            //MyInput();
            StateHandler();
            SpeedControl();

            // defini le drag (le frotement du sol)
            if (grounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }

            /*//affiche les text de debug
            if (textspeed != null && text2 != null)
            {
                var vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                textspeed.text = vel.magnitude.ToString();
                text2.text = state.ToString();
            }*/
        }

        private void FixedUpdate()
        {
            //verifie si on peut bouger
            if (player.CanMouve)
            {
                MovePlayer();
            }

            //pour gerer nous meme la graviter
            if (useGravity)
            {
                rb.AddForce(Vector3.down * gravity * mass);
            }


        }
        /*
        /// <summary>
        /// gere toute les entres utilisateur
        /// </summary>
        private void MyInput()
        {
            horizontalInput = 0;
            verticalInput = 0;

            //obtien la direction de deplacement en fonction des inputs
            horizontalInput += (Input.GetKey(rightKey.key)) ? 1 : 0;
            horizontalInput -= (Input.GetKey(leftKey.key)) ? 1 : 0;
            verticalInput += (Input.GetKey(upKey.key)) ? 1 : 0;
            verticalInput -= (Input.GetKey(bottomKey.key)) ? 1 : 0;

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
                //reduit et deplace la capsule
                capsuleCollider.center = new Vector3(capsuleCollider.center.x, capsuleCollider.center.y / (startYScale / crouchYScale), capsuleCollider.center.z);
                capsuleCollider.height = crouchYScale;
            }

            // stop crouch
            if (Input.GetKeyUp(crouchKey.key))
            {
                //reset la capsule
                capsuleCollider.center = new Vector3(capsuleCollider.center.x, centerY, capsuleCollider.center.z);
                capsuleCollider.height = startYScale;

            }
        }*/
        /// <summary>
        /// permet d'obtennir l'etat du joueur
        /// </summary>
        private void StateHandler()
        {
            var oldState = state;

            if (player.AnimationPlayed)
            {
                state = MovementState.stop;
            }
            else
            {
                // Mode - vol
                if (fly)
                {
                    state = MovementState.fly;
                    moveSpeed = walkSpeed;
                }
                // Mode - Crouching
                else if (grounded && crouch)
                {
                    state = MovementState.crouching;
                    moveSpeed = crouchSpeed;
                }
                // Mode - Sprinting
                else if (grounded && sprinting)
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
            if (oldState != state)
            {
                //CallAnnimatorTrigger();
            }
        }

        /// <summary>
        /// deplace le joueur
        /// </summary>
        private void MovePlayer()
        {
            // calculate movement direction
            moveDirection = transform.forward * verticalInput + transform.right * horizontalInput; //creez la direction en fonction de la rotation de la camera et des resulta des input user
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z); //l'empeche de voler si regarde en hauteurs

            // on slope
            if (OnSlope() && !exitingSlope)
            {
                //deplace la direction de la force pour aller dans le sens de la pente
                rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

                //plaque le joueur au sol
                if (rb.velocity.y < 0)
                {
                    rb.AddForce(Vector3.down * 80, ForceMode.Force);
                }
            }

            //applique la force
            if (grounded)
            {
                //sans modification
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                //avec la vitesse dans les air
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
            //si il vol
            if (state == MovementState.fly)
            {
                //pas de gravite
                useGravity = false;
            }
            else
            {
                // sinon en fonction de si il glisse ou pas
                useGravity = !OnSlope();
            }
        }
        /// <summary>
        /// control la vitesse du joueur
        /// </summary>
        private void SpeedControl()
        {
            // limiting speed on slope car la coordonner y change donc peut pas la reset en 0
            if (OnSlope() && !exitingSlope)
            {
                //si il est trop rapide
                if (rb.velocity.magnitude > moveSpeed)
                {
                    //raplique en fonction de la vitesse
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

        public void Crouch()
        {
            if (crouch)
            {
                //reduit et deplace la capsule
                capsuleCollider.center = new Vector3(capsuleCollider.center.x, capsuleCollider.center.y / (startYScale / crouchYScale), capsuleCollider.center.z);
                capsuleCollider.height = crouchYScale;
                crouch = false;
            }
            else
            {
                //reset la capsule
                capsuleCollider.center = new Vector3(capsuleCollider.center.x, centerY, capsuleCollider.center.z);
                capsuleCollider.height = startYScale;
                crouch = true;
            }
        }

        public void TryJump()
        {
            if (readyToJump && grounded) //si le joueur veut et peut sauter
            {
                readyToJump = false; //ne peut plus sauter

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown); //lui permet de resauter au bout de x second
            }
        }

        /// <summary>
        /// fait sauter le joueur
        /// </summary>
        private void Jump()
        {
            //quiter une pente
            exitingSlope = true;

            //ajoute un force vers le haut
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        /// <summary>
        /// permet de reset les parametre de saut
        /// </summary>
        private void ResetJump()
        {
            readyToJump = true; //pret a sauter
            exitingSlope = false; //ne quite plus une pente
        }
        /// <summary>
        /// verifie si on glisse
        /// </summary>
        /// <returns>vrai si pente dif 0 et plus petit que maxSlopeAngle</returns>
        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, maxDistance, whatIsGround)) //permet d'obtenir l'object sur le quel on marche
            {
                //obtient l'angle de la pente
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
            //creer la projection
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
        /// <summary>
        /// a faire lors de l'ajout des animation
        /// </summary>
        private void CallAnnimatorTrigger()
        {
            if (state == MovementState.walking)
            {
                animator.SetTrigger("triggerWalk");
            }
            else if (state == MovementState.sprinting)
            {
                animator.SetTrigger("triggerJogging");
            }
            else if (state == MovementState.crouching)
            {
                animator.SetTrigger("triggerCrouchWalk");
            }
            else if (state == MovementState.air)
            {
            }
            else if (state == MovementState.stop)
            {
                animator.SetTrigger("triggerIdling");
            }
        }
    }
}