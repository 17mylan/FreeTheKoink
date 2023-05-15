using UnityEngine;

public class Glide : MonoBehaviour
{
    public float glideSpeed = 5f; // Vitesse de glissade
    public float glideDrag = 5f; // Drag (résistance) lors de la glissade

    private bool isGliding = false; // Variable pour suivre si le personnage glisse ou non

    private Rigidbody rb; // Composant Rigidbody du personnage

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Détecte si le joueur appuie sur le bouton de glissade (par exemple, une touche ou un bouton de manette)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGliding = true; // Active la glissade
            rb.useGravity = false; // Désactive la gravité pour éviter les chutes
        }

        // Détecte si le joueur a relâché le bouton de glissade
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isGliding = false; // Désactive la glissade
            rb.useGravity = true; // Réactive la gravité
        }
    }

    private void FixedUpdate()
    {
        // Applique le mouvement de glissade si le personnage glisse
        if (isGliding)
        {
            // Calcule le vecteur de déplacement en fonction des entrées de mouvement (par exemple, les touches directionnelles)
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical);

            // Applique le mouvement de glissade en utilisant le vecteur de déplacement
            rb.AddForce(moveDirection * glideSpeed, ForceMode.VelocityChange);

            // Applique une résistance (drag) pour ralentir le personnage progressivement
            rb.drag = glideDrag;
        }
        else
        {
            // Réinitialise le drag à zéro lorsque le personnage ne glisse pas
            rb.drag = 0f;
        }
    }
}