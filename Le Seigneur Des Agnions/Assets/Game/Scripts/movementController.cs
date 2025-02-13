using UnityEngine;

public class MovementController : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    // Vitesse et paramètres
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 2f;

    private float gravity = -20.00f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;

    // Accroupissement
    private float initialHeight;
    private Vector3 initialCenter;
    private float crouchHeight = 1.2f;

    // Caméra
    public Transform cameraTransform; // La caméra ne sera plus contrôlée ici
    public Transform headBone; // mixamorig:Head
    public Transform neckBone; // mixamorig:Neck
    public Transform playerBody;
    public float mouseSensitivityX = 350f; // Sensibilité horizontale
    public float mouseSensitivityY = 250f; // Sensibilité verticale

    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        initialHeight = controller.height;
        initialCenter = controller.center;

        Cursor.lockState = CursorLockMode.Locked; // Cache le curseur
    }

    void Update()
    {
        // Gestion de la gravité et du sol
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Mouvement horizontal
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.Z)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.Q)) move -= transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;

        // Sprint et accroupissement
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            speed = crouchSpeed;
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * 10f);
            controller.center = Vector3.Lerp(controller.center, new Vector3(initialCenter.x, crouchHeight / 2, initialCenter.z), Time.deltaTime * 10f);
        }
        else
        {
            isCrouching = false;
            controller.height = Mathf.Lerp(controller.height, initialHeight, Time.deltaTime * 10f);
            controller.center = Vector3.Lerp(controller.center, initialCenter, Time.deltaTime * 10f);
        }

        // Déplacer le personnage
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Animation
        float animationSpeed = move.magnitude > 0 ? speed : 0;
        animator.SetFloat("Speed", animationSpeed);
        animator.SetBool("IsCrouching", isCrouching);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Appliquer la gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Rotation horizontale du corps
        HandleMouseLook();

        // Contrôle de la tête et nuque (inclinaison et avancement sur Z)
        HandleHeadAndNeckMovement();
    }

    void HandleMouseLook()
    {
        // Mouvement horizontal de la caméra et du corps (rotation du joueur)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;

        // Rotation du joueur sur l'axe Y (horizontal)
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleHeadAndNeckMovement()
    {
        if (headBone && neckBone)
        {
            // Rotation de la tête sur l'axe X en fonction de la souris
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -25f, 45f); // Limite de l'angle de la tête

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
