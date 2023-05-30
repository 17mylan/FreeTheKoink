using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionSystem : MonoBehaviour, IInteractable
{
    public Material newMaterial;
    public TextMeshProUGUI narrativeText;
    public GameObject narrativeTextObject;
    public float NarrativeWaitTimer = 10f;
    public string NarrationText;
    public Animator doorAnimator;
    public Animator cageDoorAnimator;
    public bool isDoorOpen = false;
    public GameObject CameraCollider;

    private Interaction interaction;

    private void Start()
    {
        interaction = FindObjectOfType<Interaction>();
    }

    public void Interact()
    {
        // INTERACTIVE

        if (gameObject.tag == "Door")
        {
            if (isDoorOpen)
                DoorClose();
            else
                DoorOpen();
        }
        else if(gameObject.name == "I-PorteCage")
        {
            if(interaction.hasCageKey)
                cageDoorAnimator.SetBool("doorCageAnimationOpen", true);
        }
        else if(gameObject.name == "CartonCage")
        {
            Destroy(gameObject);
        }
        else if(gameObject.name == "Clé Cage")
        {
            Destroy(gameObject);
            interaction.hasCageKey = true;
        }
        else if(gameObject.name == "Clé Camera")
        {
            Destroy(gameObject);
            interaction.hasCameraKey = true;
        }
        else if(gameObject.name == "Disjoncteur")
        {
            Destroy(CameraCollider);
            interaction.hasCageDisjoncteurOpen = true;
        }
        else if(gameObject.name == "Pass Porte")
        {
            Destroy(gameObject);
            interaction.hasPassCaveDoor = true;
        }
        else if(gameObject.name == "CaveDoorClose" && interaction.hasPassCaveDoor)
        {
            if (isDoorOpen)
                DoorClose();
            else
                DoorOpen();
        }


        // NARRATIVE 
        StopCoroutine(NarrativeWaiter(NarrationText));
        if (gameObject.name == "Narrative-Chaise")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        else if (gameObject.name == "Narrative-Meuble")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        else if (gameObject.name == "TirroirBloqué")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
    }

    public void DoorOpen()
    {
        doorAnimator.SetBool("open", true);
        doorAnimator.SetBool("closed", false);
        isDoorOpen = true;
    }
    public void DoorClose()
    {
        doorAnimator.SetBool("open", false);
        doorAnimator.SetBool("closed", true);
        isDoorOpen = false;
    }

    IEnumerator NarrativeWaiter(string narrationText)
    {
        narrativeText.text = narrationText;
        narrativeTextObject.SetActive(true);
        yield return new WaitForSeconds(NarrativeWaitTimer);
        narrativeTextObject.SetActive(false);
    }
}