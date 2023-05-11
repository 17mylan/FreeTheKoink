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
        RaycastHit hit;
        Debug.DrawRay(cameraObject.transform.position, cameraObject.transform.forward * raycastDistance, Color.red);
        if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out hit))
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
        foreach (Transform child in parentObject.transform)
        {
            child.gameObject.SetActive(false);
            DisableChildrenRecursively(child.gameObject);
        }
    }

    private void EnableChildrenRecursively(GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            child.gameObject.SetActive(true);
            EnableChildrenRecursively(child.gameObject);
        }
    }
}
