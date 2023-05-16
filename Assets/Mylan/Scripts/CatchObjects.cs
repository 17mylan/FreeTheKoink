using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObjects : MonoBehaviour
{
    public bool isPickuping = false;
    public bool checkIsCrouching = false;
    /*private GameObject objectInHand;
    private bool canPickup = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !checkIsCrouching)
        {
            if (objectInHand == null && canPickup)
            {
                TryPickupObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    private void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Pickupable"))
            {
                objectInHand = hit.collider.gameObject;
                Rigidbody objectRigidbody = objectInHand.GetComponent<Rigidbody>();
                objectRigidbody.isKinematic = true;
                objectRigidbody.useGravity = false;
                objectInHand.transform.SetParent(transform);
                objectInHand.transform.localPosition = Vector3.zero;
                Vector3 newPosition = objectInHand.transform.localPosition;
                newPosition += new Vector3(0f, 1f, 1.3f);
                objectInHand.transform.localPosition = newPosition;
                isPickuping = true;
            }
        }
    }

    private void DropObject()
    {
        Rigidbody objectRigidbody = objectInHand.GetComponent<Rigidbody>();
        objectRigidbody.isKinematic = false;
        objectRigidbody.useGravity = true;
        objectInHand.transform.SetParent(null);
        objectInHand = null;
        isPickuping = false;
    }
    void OnCollisionEnter(Collision other)
    {
        print("Je touche");
        if(isPickuping)
            DropObject();
    }*/

    public bool estEnTrainDeTenir = false; // Variable pour indiquer si le joueur tient l'objet ou non
    private Rigidbody objetTenu; // Référence au Rigidbody de l'objet tenu
    private float distanceObjetTenu = 1.2f; // Distance à laquelle l'objet tenu doit être placé devant le personnage
    private float hauteurObjetTenu = 0.5f; // Hauteur à laquelle l'objet tenu doit être placé par rapport au personnage

    public BoxCollider boxCollider;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Vérifie si le clic droit de la souris est enfoncé
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward); // Crée un rayon à partir de la position du GameObject sur lequel le script est attaché

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Pickupable")) // Vérifie si l'objet touché est l'objet spécial (utilisez le tag approprié)
                {
                    estEnTrainDeTenir = true;

                    objetTenu = hit.collider.GetComponent<Rigidbody>(); // Récupère le Rigidbody de l'objet spécial
                    objetTenu.useGravity = false; // Désactive la gravité de l'objet pour le maintenir en l'air
                    objetTenu.isKinematic = true; // Active le mode cinématique pour éviter les interactions physiques avec d'autres objets
                    boxCollider.isTrigger = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(1)) // Vérifie si le clic droit de la souris est relâché
        {
            if (estEnTrainDeTenir)
            {
                estEnTrainDeTenir = false;

                objetTenu.useGravity = true; // Réactive la gravité de l'objet
                objetTenu.isKinematic = false; // Désactive le mode cinématique de l'objet pour rétablir les interactions physiques
                objetTenu = null; // Réinitialise la référence de l'objet tenu
                boxCollider.isTrigger = false;
            }
        }

        if (estEnTrainDeTenir)
        {
            // Récupérer la position et la rotation du personnage
            Vector3 positionPersonnage = transform.position;
            Quaternion rotationPersonnage = transform.rotation;

            // Calculer la position finale de l'objet tenu en ajoutant un décalage
            Vector3 positionObjetTenu = positionPersonnage + (rotationPersonnage * Vector3.forward * distanceObjetTenu) + (rotationPersonnage * Vector3.up * hauteurObjetTenu);

            // Déplacer l'objet tenu vers la position finale en utilisant la rotation du personnage
            objetTenu.MovePosition(positionObjetTenu);
            objetTenu.MoveRotation(rotationPersonnage);
        }
    }
}