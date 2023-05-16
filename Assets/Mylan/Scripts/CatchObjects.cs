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
    }*/
}