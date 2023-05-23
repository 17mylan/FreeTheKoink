using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

interface IInteractable{
    public void Interact();
}
public class Interaction : MonoBehaviour
{
    public Transform InteractionSource;
    public float InteractRange;
    public GameObject InteractionText;
    public TextMeshProUGUI nameText;
    private CatchObjects catchObjects;
    private GameObject lastInteractedObject;

    public void Start()
    {
        catchObjects = FindObjectOfType<CatchObjects>();
    }

    void Update()
    {
        Ray r = new Ray(InteractionSource.position, InteractionSource.forward);
        Debug.DrawRay(r.origin, r.direction * InteractRange, Color.red);
        if(Physics.Raycast(r, out RaycastHit hitInfo, InteractRange) && !catchObjects.isPickuping)
        {
            if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                string objectName = hitInfo.collider.gameObject.name;
                InteractionText.SetActive(true);
                Outline outlineComponent = hitInfo.collider.gameObject.GetComponent<Outline>();
                if (outlineComponent != null)
                    outlineComponent.enabled = true;

                if(objectName.StartsWith("Narrative-"))
                {
                    nameText.text = "Press [E] to inspect";
                    if(Input.GetKeyDown(KeyCode.E))
                        interactObj.Interact();
                }
                else if(objectName.StartsWith("Interactive-"))
                {
                    nameText.text = "Press [E] to interact";
                    if(Input.GetKeyDown(KeyCode.E))
                        interactObj.Interact();
                }
                else if(objectName.StartsWith("Door"))
                {
                    nameText.text = "Door";
                    if(Input.GetKeyDown(KeyCode.E))
                        interactObj.Interact();
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
        if(obj != null)
        {
            Outline outlineComponent = obj.GetComponent<Outline>();
            if (outlineComponent != null)
            {
                outlineComponent.enabled = false;
            }
        }
    }
}
