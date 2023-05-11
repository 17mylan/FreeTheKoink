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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractionSource.position, InteractionSource.forward);
            if(Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
