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
    private Animator anim;

    public void Interact()
    {
        // INTERACTIVE

        if (gameObject.tag == "Door")
        {
            print("J'interagis avec une porte.");
            anim = GetComponentInParent<Animator>();
            anim.Play("DoorOpen");
            // Appeler la fonction OpenDoor pour ouvrir ou fermer la porte
            OpenDoor();
        }


        // NARRATIVE 

        StopAllCoroutines();
        if (gameObject.name == "Narrative-Chaise")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        else if (gameObject.name == "Narrative-Meuble")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
    }

    IEnumerator NarrativeWaiter(string narrationText)
    {
        narrativeText.text = narrationText;
        narrativeTextObject.SetActive(true);
        yield return new WaitForSeconds(NarrativeWaitTimer);
        narrativeTextObject.SetActive(false);
    }

    private void OpenDoor()
    {
        anim = GetComponentInParent<Animator>();
        // Vérifier l'état actuel de la porte
        bool isOpen = anim.GetBool("isOpen");

        // Inverser l'état de la porte
        isOpen = !isOpen;

        // Mettre à jour le paramètre "isOpen" de l'Animator
        anim.SetBool("isOpen", isOpen);
    }
}