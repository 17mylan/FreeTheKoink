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
    public Animator disjoncteurAnimator;
    public Animator frigoPortePrincipale;
    public Animator frigoPorteGlacon;
    public Animator plancheOpen;
    public ParticleSystem ElectricitySystem;
    public bool isDoorOpen = false;
    public GameObject CameraCollider;
    private Interaction interaction;
    public GameObject keyInPillowPrefab;
    public GameObject plumeInPillow;
    public GameObject plumeInStatue;
    public GameObject keyPlumeStatue;
    public ParticleSystem pillowCut;
    public BoxCollider mirroirNarrativeBeforeInteraction;
    public BoxCollider pillowNarrativeBeforeInteraction;
    private GameManager gameManager;

    private void Start()
    {
        interaction = FindObjectOfType<Interaction>();
        gameManager = FindObjectOfType<GameManager>();
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
                interaction.narrativeText.color = Color.green;
                interaction.narrativeText.text = "Aide - Maintenant, je dois désactiver les caméras de surveillance de la maison afin de sortir discrètement.";           
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
            if(interaction.hasCameraKey)
            {
                Destroy(CameraCollider);
                interaction.hasCageDisjoncteurOpen = true;
                interaction.imageKeyDisjoncteur.SetActive(false);
                interaction.narrativeText.text = "Astuce - Vous pouvez effectuer des sauts propulsés en maintenant les touches CTRL + Espace pour atteindre des hauteurs supérieures"; 
                disjoncteurAnimator.SetBool("open", true);
                ElectricitySystem.Stop();
            }
        }
        else if(gameObject.name == "Pass Porte")
        {
            Destroy(gameObject);
            interaction.hasPassCaveDoor = true;
            interaction.imagePassDoor.SetActive(true);
            interaction.narrativeTextObject.SetActive(false);    
        }
        else if(gameObject.name == "DetecteurCavePorte" && interaction.hasPassCaveDoor && !interaction.hasGivePassDoor)
        {
            interaction.hasGivePassDoor = true;
            interaction.narrativeTextObject.SetActive(true);
            interaction.narrativeText.text = "Il manque la reconnaissance vocale, je réalise que mon cri puissant peut briser des objets. Je pourrais l'utiliser pour perturber les systèmes de sécurité.";
            interaction.narrativeText.color = Color.white;    
            interaction.imagePassDoor.SetActive(false);
        }
        else if(gameObject.name == "CaveDoorClose" && interaction.hasPassCaveDoor && interaction.hasGivePassDoor && interaction.hasCaqueteToOpenDoor)
        {
            interaction.hasOpenCaveDoor = true;
            interaction.narrativeTextObject.SetActive(false);
            if (isDoorOpen)
            {
                DoorClose();
            }
            else
                DoorOpen();
        }
        else if(gameObject.name == "FrigoPortePrincipale")
        {
            //Destroy(gameObject);
            // faire animation d'ouverture de la porte
            frigoPortePrincipale.SetBool("open", true);

        }
        else if(gameObject.name == "PorteBacAGlacon")
        {
            //Destroy(gameObject);
            // faire animation d'ouverture de la porte
            frigoPorteGlacon.SetBool("open", true);

        }
        else if(gameObject.name == "Glacon")
        {
            Destroy(gameObject);
            interaction.hasIcedGlace = true;
            interaction.GlaconPrefabUI.SetActive(true);
        }
        else if(gameObject.name == "Four")
        {
            if(!interaction.hasStartedFire) //&& interaction.hasIcedGlace
            {
                interaction.hasStartedFire = true;
                interaction.FXFirePrefab.SetActive(true);
                interaction.FXFire.Play();
            }
        }
        else if(gameObject.name == "Cheminée")
        {
            if(interaction.hasStartedFire && !interaction.hasFireCaqueteFireOne && !interaction.hasFireCaqueteFireTwo && interaction.hasIcedGlace)
            {
                if(!interaction.hasStartedIcedInFireForFirstTime)
                {
                    interaction.hasPutIcedInFire = true;
                    interaction.GlaconPrefab.SetActive(true);
                    interaction.GlaconPrefabUI.SetActive(false);
                    StartCoroutine(IcedInFire());
                    interaction.hasStartedIcedInFireForFirstTime = true;
                }
            }
        }
        else if(gameObject.name == "BouDeMiroir")
        {
            interaction.MirorGlass.SetActive(false);
            interaction.hasKeepUpCrackedMirror = true;
            interaction.VerreMiroir.SetActive(true);
            interaction.hasTakenMirrorGlasses = true;
        }
        else if(gameObject.name == "Oreiller")
        {
            pillowCut.Play();
            interaction.hasCrackedPillow = true;
            plumeInPillow.SetActive(true);
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;
            BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
            boxCollider.enabled = false;
        }
        else if(gameObject.name == "PlumeDansLoreiller")
        {
            interaction.hasKeepUpPlume = true;
            interaction.UIfeather.SetActive(true);
            print("jai recuperer la plume");
            Destroy(gameObject);
        }
        else if(gameObject.name == "StatueCanard")
        {
            interaction.hasPutPlumeInStatue = true;
            interaction.UIfeather.SetActive(false);
            plumeInStatue.SetActive(true);
            keyPlumeStatue.SetActive(true);
        }
        else if(gameObject.name == "Clé Canard")
        {
            if(!interaction.hasBedroomKey)
            {
                interaction.hasBedroomKey = true;
                gameManager.numberOfKey = gameManager.numberOfKey + 1;
                gameManager.UpdateKeyNumberInUI();
            }
            Destroy(gameObject);
        }
        else if(gameObject.name == "TracesDePas")
        {
            interaction.TracesDePasObject.SetActive(false);
            interaction.hasCleanTraces = true;
            if(interaction.hasThrowClockInBin)
                interaction.hasFinishChopperMission = true;
        }
        else if(gameObject.name == "Reveil")
        {
            interaction.hasBreakClock = true;
            interaction.ReveilPrefab.SetActive(false);
            interaction.ReveilPrefabUI.SetActive(true);
        }
        else if(gameObject.name == "PoubelleChambre")
        {
            interaction.hasThrowClockInBin = true;
            interaction.ReveilPrefabUI.SetActive(false);
            if(interaction.hasCleanTraces)
                interaction.hasFinishChopperMission = true;
        }
        else if(gameObject.name == "Munitions")
        {
            interaction.hasTakenMunitions = true;
            interaction.ShotgunAmmoUI.SetActive(true);
            Destroy(gameObject);
        }
        else if(gameObject.name == "Toilets")
        {
            interaction.hasPutMunitonsInToilets = true;
            interaction.ShotgunAmmoUI.SetActive(false);
            if(interaction.hasThrowGlassesInBin)
                interaction.hasFinishShooterMission = true;
        }
        else if(gameObject.name == "PoubelleCuisine")
        {
            interaction.hasThrowGlassesInBin = true;
            interaction.VerreMiroir.SetActive(false);
            if(interaction.hasPutMunitonsInToilets)
                interaction.hasFinishShooterMission = true;
        }
        else if(gameObject.name == "Clé Chambre")
        {
            Destroy(gameObject);
            interaction.hasKeyToOpenBedroomDoor = true;
        }
        else if(gameObject.name == "PivotPorteChambre")
        {
            if (isDoorOpen)
            {
                DoorClose();
            }
            else
                DoorOpen();
        }
        else if(gameObject.name == "Clé Bureau")
        {
            Destroy(gameObject);
            interaction.hasKeyToOpenOfficeDoor = true;
        }
        else if(gameObject.name == "PivotPorteBureau")
        {
            if (isDoorOpen)
            {
                DoorClose();
            }
            else
                DoorOpen();
        }
        else if(gameObject.name == "PlanchePivot")
        {
            plancheOpen.SetBool("open", true);
            interaction.plancheBoxCollider.enabled = false;
            //Destroy(gameObject);
        }
        else if(gameObject.name == "Clé Cuisine")
        {
            Destroy(gameObject);
            interaction.hasCaveKey = true;
            gameManager.numberOfKey = gameManager.numberOfKey + 1;
            gameManager.UpdateKeyNumberInUI();
        }
        else if(gameObject.name == "SecondKeyInCheminee")
        {
            interaction.hasKitchenKey = true;
            gameManager.numberOfKey = gameManager.numberOfKey + 1;
            gameManager.UpdateKeyNumberInUI();
            interaction.KeyPrefab.SetActive(false);
            interaction.TracesDePasObject.SetActive(true);
            Destroy(gameObject);
        }


        // NARRATIVE 
        StopCoroutine(NarrativeWaiter(NarrationText));
        if (gameObject.name == "CouteauSurCuisine")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        if (gameObject.name == "MaitrePiece")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        if (gameObject.name == "Board")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        if (gameObject.name == "TableauCheminee")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        if (gameObject.name == "Fusil")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        if (gameObject.name == "Narrative-Chaise")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        else if (gameObject.name == "Narrative-Meuble")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }
        /*else if (gameObject.name == "TirroirBloqué")
        {
            StartCoroutine(NarrativeWaiter(NarrationText));
        }*/
        else if(gameObject.name == "N-Miroir")
        {
            if(!interaction.hasCheckedPillow)
                StartCoroutine(NarrativeWaiter("Je devrais faire attention à ne pas le briser mais je pourrais obtenir des morceaux de verre tranchant."));
            else if(interaction.hasCheckedPillow)
                StartCoroutine(NarrativeWaiter("Je devrais faire attention à ne pas le briser"));
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

    public IEnumerator NarrativeWaiter(string narrationText)
    {
        narrativeText.text = narrationText;
        narrativeTextObject.SetActive(true);
        yield return new WaitForSeconds(NarrativeWaitTimer);
        narrativeTextObject.SetActive(false);
    }
}