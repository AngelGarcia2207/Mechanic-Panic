using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movimientoX;
    private float movimientoZ;
    private Vector3 inputJugador;
    private Vector3 direccionJugador;
    public bool inmovilizado = false;

    public CharacterController player;
    public float velocidad;
    public float gravedad = 9.8f;
    public float velocidadCaida;
    public float jumpForce;
    public float floorRaycastDistance;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    private Transform platformTransform = null;
    private Vector3 lastPlatformPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inmovilizado) {
            movimientoX = Input.GetAxis("Horizontal");
            movimientoZ = Input.GetAxis("Vertical");
        }
        else {
            movimientoX = 0;
            movimientoZ = 0;
        }

        direccionarCamara();

        inputJugador = Vector3.ClampMagnitude(new Vector3(movimientoX, 0, movimientoZ), 1);
        direccionJugador = inputJugador.x * camRight + inputJugador.z * camForward;
        direccionJugador = direccionJugador * velocidad;
        player.transform.LookAt(player.transform.position + direccionJugador);

        aplicarGravedad();

        inputsEspeciales();

        if (platformTransform != null)
        {
            Vector3 platformMovement = platformTransform.position - lastPlatformPosition;
            player.Move(platformMovement);
            lastPlatformPosition = platformTransform.position;
        }

        player.Move(direccionJugador * Time.deltaTime);
    }

    void inputsEspeciales() {
        if ((player.isGrounded || raycastFloor()) && Input.GetButtonDown("Jump")) {
            velocidadCaida = jumpForce;
            direccionJugador.y = velocidadCaida;
        }
    }

    void direccionarCamara() {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        
        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void aplicarGravedad() {
        if (player.isGrounded) {
            velocidadCaida = -gravedad * 0.1f;
            direccionJugador.y = velocidadCaida;
        }
        else {
            velocidadCaida -= gravedad * Time.deltaTime;
            direccionJugador.y = velocidadCaida;
        }
    }

    bool raycastFloor() {
        Vector3 origin = transform.position;
        RaycastHit hit;
        Vector3 direction = -transform.up;

        if (Physics.Raycast(origin, direction, out hit, floorRaycastDistance)) {
            return true;
        }
        else {
            return false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit collision) {
        Transform collisionTransform = collision.collider.transform;

        if (collisionTransform.CompareTag("MovingPlatform")) {
            if (platformTransform == null) {
                platformTransform = collisionTransform.GetComponentInParent<Transform>();
                lastPlatformPosition = platformTransform.position;
            }
        }
        else {
            platformTransform = null;
        }
    }
}