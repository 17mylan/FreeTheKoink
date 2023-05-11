using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float runSpeedMultiplier = 2f; // Multiplicateur de vitesse lors de la course
    public float jumpForce = 5f;
    public float rotationSpeed = 5f;
    public Transform cameraTransform; // Référence au transform de la caméra

    private bool isJumping = false;
    private bool isRunning = false; // Indique si le joueur est en train de courir
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Déplacement horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed;

        // Vérification de la course
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (isRunning)
        {
            movement *= runSpeedMultiplier; // Applique le multiplicateur de vitesse lors de la course
        }

        movement = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * movement; // Rotation du mouvement selon l'orientation de la caméra
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }

        if (movement.magnitude > 0.1f) // Vérifie si le joueur se déplace
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement); // Calcul de la rotation cible en fonction du mouvement
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
