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
    public BoxCollider mirroirNarrativeBeforeInteraction;
    public BoxCollider pillowNarrativeBeforeInteraction;

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
            {
                cageDoorAnimator.SetBool("doorCageAnimationOpen", true);
                interaction.imageKeyCageAsset.SetActive(false);
            }
        }
        else if(gameObject.name == "CartonCage")
        {
            Destroy(gameObject);
        }
        else if(gameObject.name == "Clé Cage")
        {
            Destroy(gameObject);
            interaction.hasCageKey = true;
            interaction.imageKeyCageAsset.SetActive(true);
        }
        else if(gameObject.name == "Clé Camera")
        {
            Destroy(gameObject);
            interaction.hasCameraKey = true;
            interaction.imageKeyDisjoncteur.SetActive(true);
        }
        else if(gameObject.name == "Disjoncteur")
        {
            Destroy(CameraCollider);
            interaction.hasCageDisjoncteurOpen = true;
            interaction.imageKeyDisjoncteur.SetActive(false);
        }
        else if(gameObject.name == "Pass Porte")
        {
            Destroy(gameObject);
            interaction.hasPassCaveDoor = true;
            interaction.imagePassDoor.SetActive(true);
        }
        else if(gameObject.name == "DetecteurCavePorte" && interaction.hasPassCaveDoor && !interaction.hasGivePassDoor)
        {
            interaction.hasGivePassDoor = true;
        }
        else if(gameObject.name == "CaveDoorClose" && interaction.hasPassCaveDoor && interaction.hasGivePassDoor && interaction.hasCaqueteToOpenDoor)
        {
            interaction.imagePassDoor.SetActive(false);
            interaction.hasOpenCaveDoor = true;
            if (isDoorOpen)
            {
                DoorClose();
            }
            else
                DoorOpen();
        }
        else if(gameObject.name == "FrigoPortePrincipale")
        {
            Destroy(gameObject);
            // faire animation d'ouverture de la porte

        }
        else if(gameObject.name == "PorteBacAGlacon")
        {
            Destroy(gameObject);
            // faire animation d'ouverture de la porte

        }
        else if(gameObject.name == "Glacon")
        {
            Destroy(gameObject);
            interaction.hasIcedGlace = true;
        }
        else if(gameObject.name == "Four")
        {
            if(interaction.hasIcedGlace && !interaction.hasStartedFire)
            {
                interaction.hasStartedFire = true;
                interaction.FXFirePrefab.SetActive(true);
                interaction.FXFire.Play();
            }
        }
        else if(gameObject.name == "Cheminée")
        {
            if(interaction.hasStartedFire && !interaction.hasFireCaqueteFireOne && !interaction.hasFireCaqueteFireTwo)
            {
                if(!interaction.hasStartedIcedInFireForFirstTime)
                {
                    interaction.hasPutIcedInFire = true;
                    interaction.GlaconPrefab.SetActive(true);
                    StartCoroutine(IcedInFire());
                    interaction.hasStartedIcedInFireForFirstTime = true;
                }
            }
        }
        else if(gameObject.name == "BouDeMiroir")
        {
            interaction.MirorGlass.SetActive(false);
            interaction.hasKeepUpCrackedMirror = true;
        }
        else if(gameObject.name == "Oreiller")
        {
            interaction.hasCrackedPillow = true;
            interaction.hasBedroomKey = true;
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
        else if(gameObject.name == "N-Miroir")
        {
            if(!interaction.hasCheckedPillow)
                StartCoroutine(NarrativeWaiter("Je devrais faire attention à ne pas le casser, Je pourrais casser ce miroir pour récupérer un bout de verre pour ouvrir l’oreiller"));
            else if(interaction.hasCheckedPillow)
                StartCoroutine(NarrativeWaiter("Je devrais faire attention à ne pas le casser"));
            mirroirNarrativeBeforeInteraction.enabled = false;
        }
        else if(gameObject.name == "N-Oreiller")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
            pillowNarrativeBeforeInteraction.enabled = false;
        }
    }
    IEnumerator IcedInFire()
    {
        interaction.isWaitingForIceInFire = true;
        yield return new WaitForSeconds(3f);
        print("Le glacon a fondu");
        interaction.isWaitingForIceInFire = false;
        interaction.hasIcedFinishFired = true;
        interaction.GlaconPrefab.SetActive(false);
        interaction.KeyPrefab.SetActive(true);
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