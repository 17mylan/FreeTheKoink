using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("Camera")]
    public Camera cameraObject;
    public GameObject cameraTargetObject;

    [Header("Floats Number")]
    public float rotationSpeed = 5f;
    public float raycastDistance = 3f;
    public float reappearDelay = 2f; // Temps d'attente avant que l'objet réapparaisse (en secondes)

    [Header("Wall Check")]
    private GameObject lastHitObject;
    private bool hasHitObject = false; 
    public bool hasDoneFirstWallCheck = false;

    private void Update()
    {
        if (!hasDoneFirstWallCheck)
        {
            WallCheck();
            hasDoneFirstWallCheck = true;
        }
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            cameraObject.transform.RotateAround(cameraTargetObject.transform.position, Vector3.up, -mouseX * rotationSpeed);
            WallCheck();
        }
    }

    private void WallCheck()
{
    Vector3 raycastOrigin = cameraObject.transform.position - cameraObject.transform.up;

    RaycastHit hit;
    Debug.DrawRay(raycastOrigin, cameraObject.transform.forward * raycastDistance, Color.red);
    if (Physics.Raycast(raycastOrigin, cameraObject.transform.forward, out hit))
    {
        GameObject hitObject = hit.collider.gameObject;
        if (hitObject.layer == 3)
        {
            if (hitObject != lastHitObject)
            {
                if (lastHitObject != null)
                {
                    StartCoroutine(ReappearObject(lastHitObject));
                }
                MeshRenderer meshRenderer = hitObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                    DisableChildrenRecursively(hitObject);
                    lastHitObject = hitObject;
                    hasHitObject = true;
                }
            }
        }
    }
}


    private IEnumerator ReappearObject(GameObject obj)
    {
        yield return new WaitForSeconds(reappearDelay);
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }
        EnableChildrenRecursively(obj);
    }

    private void DisableChildrenRecursively(GameObject parentObject)
    {
        Renderer parentRenderer = parentObject.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            parentRenderer.enabled = false;
        }

        foreach (Transform child in parentObject.transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = false;
            }
            DisableChildrenRecursively(child.gameObject);
        }
    }

    private void EnableChildrenRecursively(GameObject parentObject)
    {
        Renderer parentRenderer = parentObject.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            parentRenderer.enabled = true;
        }

        foreach (Transform child in parentObject.transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = true;
            }
            EnableChildrenRecursively(child.gameObject);
        }
    }
}
