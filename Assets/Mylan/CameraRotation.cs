using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("Camera")]
    public Camera cameraObject;
    public GameObject targetObject;
    
    [Header("Floats Number")]
    public float rotationSpeed = 5f;
    public float raycastDistance = 3f;

    [Header("Wall Check")]
    private GameObject lastHitObject;
    private bool hasHitObject = false, hasDoneFirstWallCheck = false;

    private void Update()
    {
        if(!hasDoneFirstWallCheck)
        {
            WallCheck();                
            hasDoneFirstWallCheck = true;
        }
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            cameraObject.transform.RotateAround(targetObject.transform.position, Vector3.up, -mouseX * rotationSpeed);
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
                        MeshRenderer meshRend = lastHitObject.GetComponent<MeshRenderer>();
                        if (meshRend != null)
                        {
                            meshRend.enabled = true;
                        }
                    }
                    MeshRenderer meshRenderer = hitObject.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.enabled = false;
                        lastHitObject = hitObject;
                        hasHitObject = true;
                    }
                }
            }
        }
    }
}
