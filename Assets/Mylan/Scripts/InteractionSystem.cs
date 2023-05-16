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
    public float NarrativeWaitTimer = 5f;
    public void Interact()
    {
        GetComponent<Renderer>().material = newMaterial;
        // INTERACTIVE


        // NARRATIVE 
        
        StopAllCoroutines();
        if(gameObject.name == "Narrative-Chaise")
        {
            StartCoroutine(NarrativeWaiter("Cette chaise est très belle."));
        }
        else if(gameObject.name == "Narrative-Meuble")
        {
            StartCoroutine(NarrativeWaiter("Cette meuble est moche."));
        }
    }

    IEnumerator NarrativeWaiter(string narrationText)
    {
        narrativeText.text = narrationText;
        narrativeTextObject.SetActive(true);
        yield return new WaitForSeconds(NarrativeWaitTimer);
        narrativeTextObject.SetActive(false);
    }
}

