using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour, IInteractable
{
    public Material newMaterial;
    public void Interact()
    {
        GetComponent<Renderer>().material = newMaterial;
        if(gameObject.name == "TestInteractionCube")
        {
            print("J'ai intéragit avec: " + gameObject.name);
        }
    }
}

