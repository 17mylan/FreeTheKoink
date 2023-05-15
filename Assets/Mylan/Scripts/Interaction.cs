using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable{
    public void Interact();
}
public class Interaction : MonoBehaviour
{
    public Transform InteractionSource;
    public float InteractRange;
    public GameObject InteractionText;

    private CatchObjects catchObjects;

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
                InteractionText.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                    interactObj.Interact();
            }
            else
                InteractionText.SetActive(false);
        }
        else
            InteractionText.SetActive(false);
    }
}
