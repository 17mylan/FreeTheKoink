using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

interface IInteractable
{
    void Interact();
}

public class Interaction : MonoBehaviour
{
    public Transform InteractionSource;
    public float InteractRange;
    public float RaycastHeight = 3f; // Hauteur du raycast
    public float RaycastHorizontalOffset = 0f; // Offset horizontal du raycast
    public GameObject InteractionText;
    public TextMeshProUGUI nameText;
    private GameObject lastInteractedObject;
    private InteractionSystem interactionSystem;
    public AudioSource audioSource;
    public AudioClip doorSound;
    public AudioClip cageDoorSound;
    public AudioClip getKeyCageSound;

    public bool hasCageKey = false;
    public bool hasCageDoorOpen = false;

    void Update()
    {
        Vector3 raycastOrigin = InteractionSource.position + Vector3.up * RaycastHeight + InteractionSource.right * RaycastHorizontalOffset; // Utilise la hauteur et l'offset horizontal du raycast
        Ray r = new Ray(raycastOrigin, InteractionSource.forward);
        Debug.DrawRay(r.origin, r.direction * InteractRange, Color.red);
        
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, ~LayerMask.GetMask("Ignore Raycast")))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                string objectName = hitInfo.collider.gameObject.name;
                InteractionText.SetActive(true);
                Outline outlineComponent = hitInfo.collider.gameObject.GetComponent<Outline>();
                if (outlineComponent != null)
                    outlineComponent.enabled = true;

                if (objectName.StartsWith("Narrative-"))
                {
                    nameText.text = "Press [E] to inspect";
                    if (Input.GetKeyDown(KeyCode.E))
                        interactObj.Interact();
                }
                else if (objectName.StartsWith("Interactive-"))
                {
                    nameText.text = "Press [E] to interact";
                    if (Input.GetKeyDown(KeyCode.E))
                        interactObj.Interact();
                }
                else if (objectName.StartsWith("Door"))
                {
                    nameText.text = "Press [E] to interact";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(doorSound);
                    }
                }
                else if (objectName.StartsWith("I-PorteCage"))
                {
                    if(!hasCageKey)
                    {
                        nameText.text = "Press [E] to try open";
                    }
                    else if(hasCageKey)
                    {
                        nameText.text = "Press [E] to open";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(cageDoorSound);
                            hasCageDoorOpen = true;
                        }
                    }
                }
                else if (objectName.StartsWith("CartonCage"))
                {
                    nameText.text = "Press [E] to move the box";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                    }
                }
                else if (objectName.StartsWith("Clé Cage"))
                {
                    nameText.text = "Press [E] to take key";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(getKeyCageSound);
                    }
                }
                lastInteractedObject = hitInfo.collider.gameObject;
            }
            else
            {
                InteractionText.SetActive(false);
                DisableOutline(lastInteractedObject);
            }
        }
        else
        {
            InteractionText.SetActive(false);
            DisableOutline(lastInteractedObject);
        }
    }

    void DisableOutline(GameObject obj)
    {
        if (obj != null)
        {
            Outline outlineComponent = obj.GetComponent<Outline>();
            if (outlineComponent != null)
            {
                outlineComponent.enabled = false;
            }
        }
    }
}
