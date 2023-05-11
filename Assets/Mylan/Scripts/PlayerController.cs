using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 5f;
    public Transform cameraTransform; // Référence au transform de la caméra

    private bool isJumping = false;
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
    /*public float movementSpeed = 5f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private void Update()
    {
        // Déplacement au clic de la souris
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Collider[] colliders = hit.transform.GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    Physics.IgnoreCollision(GetComponent<Collider>(), collider);
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y; // Maintient la hauteur du joueur

                    isMoving = true;
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
                }

                // Ignore les collisions avec les box colliders des objets touchés par le raycast
            }
        }

        // Déplacement vers la position cible
        if (isMoving)
        {
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            if (distance > 0.1f)
            {
                direction.Normalize();
                Vector3 movement = direction * movementSpeed * Time.deltaTime;
                transform.position += movement;
            }
            else
            {
                isMoving = false;
            }
        }
    }*/
}
