using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

interface IInteractable
{
    void Interact();
}

public class Interaction : MonoBehaviour
{
    public Transform InteractionSource;
    public float InteractRange;
    public float RaycastHeight = 3f; // Hauteur du raycast
    public float RaycastHorizontalOffset = 0f; // Offset horizontal du raycast
    public GameObject InteractionText;
    public TextMeshProUGUI nameText;
    private GameObject lastInteractedObject;
    private InteractionSystem interactionSystem;
    private FinalEvent finalEvent;
    public AudioSource audioSource;
    public AudioClip doorSound;
    public AudioClip cageDoorSound;
    public AudioClip getKeyCageSound;
    public AudioClip passValid;
    public AudioClip zipSound;
    public AudioClip electricityShutDown;
    public AudioClip breakSound;
    public AudioClip binSound;
    public AudioClip toiletSound;
    public AudioClip survolObjectSound;
    public AudioClip interactObjectSound;
    public AudioSource fireSound;
    public GameObject imageKeyCageAsset, imageKeyDisjoncteur, imagePassDoor, FXFirePrefab;
    public ParticleSystem FXFire;
    private GameManager gameManager;

    [Header("Key Counts")]

    public bool hasKitchenKey = false;
    public bool hasBedroomKey = false;
    public bool hasCaveKey = false;

    [Header("Key To Open Doors")]
    public bool hasKeyToOpenBedroomDoor = false;
    public bool hasKeyToOpenOfficeDoor = false;

    [Header("Mission Sortir du nid")]
    public bool hasCageKey = false;
    public bool hasCageDoorOpen = false;
    
    [Header("Mission Eteindre les caméras")]
    public bool hasCameraKey = false;
    public bool hasCageDisjoncteurOpen = false;
    
    [Header("Mission Prendre le badge")]
    public bool hasPassCaveDoor = false;
    public bool hasGivePassDoor = false;
    public bool hasCaqueteToOpenDoor = false;
    public bool hasOpenCaveDoor = false;

    [Header("Mission Cuisine")]
    public bool hasIcedGlace = false;
    public bool hasStartedFire = false;
    public bool hasPutIcedInFire = false;
    public bool hasIcedFinishFired = false;
    public bool hasStartedIcedInFireForFirstTime = false;
    public bool hasFireCaqueteFireOne = false;
    public bool hasFireCaqueteFireTwo = false;
    public bool isWaitingForIceInFire = false;

    [Header("Mission Chambre")]
    public bool hasCheckedPillow = false;
    public bool hasCrackedMirror = false;
    public bool hasKeepUpCrackedMirror = false;
    public bool hasCrackedPillow = false;
    public bool hasKeepUpPlume = false;
    public bool hasPutPlumeInStatue = false;

    [Header("Mission Fin Chopper")]
    public bool hasCleanTraces = false;
    public bool hasBreakClock = false;
    public bool hasThrowClockInBin = false;
    public bool hasFinishChopperMission = false;

    [Header("Mission Fin Shooter")]
    public bool hasTakenMunitions = false;
    public bool hasPutMunitonsInToilets = false;
    public bool hasTakenMirrorGlasses = false;
    public bool hasThrowGlassesInBin = false;
    public bool hasFinishShooterMission = false;
    
    [Header("Mission Fin Shooter")]
    public BoxCollider plancheBoxCollider;
    
    [Header("Other")]
    public GameObject GlaconPrefab;
    public GameObject KeyPrefab;
    public GameObject MirorGlass;
    public GameObject GlaconPrefabUI;
    public GameObject VerreMiroir;
    public GameObject TracesDePasObject;
    public GameObject ReveilPrefab;
    public GameObject ReveilPrefabUI;
    public GameObject ShotgunAmmoUI;
    public GameObject UIbedroomKey;
    public GameObject UIofficeKey;
    public GameObject UIfeather;
    public GameObject secondKeyInCheminee;
    public GameObject narrativeTextObject;
    public GameObject fadeToBlackEndGame;
    public TextMeshProUGUI narrativeText;
    private void Start()
    {
        finalEvent = FindObjectOfType<FinalEvent>();
        gameManager = FindObjectOfType<GameManager>();
        narrativeText.text = "Je dois trouver un moyen de m'échapper d'ici ! Il faut que je trouve de quoi ouvrir la porte de la cage";
        narrativeTextObject.SetActive(true);
        narrativeText.color = Color.white;
    }
    void Update()
    {
        Vector3 raycastOrigin = InteractionSource.position + Vector3.up * RaycastHeight + InteractionSource.right * RaycastHorizontalOffset; // Utilise la hauteur et l'offset horizontal du raycast
        Ray r = new Ray(raycastOrigin, InteractionSource.forward);
        Debug.DrawRay(r.origin, r.direction * InteractRange, Color.red);
        
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, ~LayerMask.GetMask("Ignore Raycast")))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                string objectName = hitInfo.collider.gameObject.name;
                InteractionText.SetActive(true);
                //audioSource.PlayOneShot(survolObjectSound);
                Outline outlineComponent = hitInfo.collider.gameObject.GetComponent<Outline>();
                if (outlineComponent != null)
                    outlineComponent.enabled = true;

                if (objectName.StartsWith("Narrative-"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        audioSource.PlayOneShot(interactObjectSound);
                        interactObj.Interact();
                    }
                }
                else if (objectName.StartsWith("Interactive-"))
                {
                    nameText.text = "Appuyer sur [E] pour intéragir";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        audioSource.PlayOneShot(interactObjectSound);
                        interactObj.Interact();
                    }
                }
                else if (objectName.StartsWith("CaveDoorClose"))
                {
                    if(hasPassCaveDoor && hasGivePassDoor && hasCaqueteToOpenDoor)
                        nameText.text = "Appuyer sur [E] pour ouvrir";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(doorSound);
                        }
                    else if(!hasPassCaveDoor || !hasGivePassDoor)
                        nameText.text = "La porte est fermée";
                    else if(hasGivePassDoor && hasPassCaveDoor && !hasCaqueteToOpenDoor)
                        nameText.text = "Appuyer sur [A] pour caqueter";
                        if(Input.GetKeyDown(KeyCode.A))
                        {
                            hasCaqueteToOpenDoor = true;
                        }
                }
                else if (objectName.StartsWith("Door"))
                {
                    nameText.text = "Appuyer sur [E] pour intéragir";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(doorSound);
                    }
                }
                else if (objectName.StartsWith("I-PorteCage"))
                {
                    if(!hasCageKey)
                    {
                        nameText.text = "La porte est fermée";
                    }
                    else if(hasCageKey)
                    {
                        nameText.text = "Appuyer sur [E] pour ouvrir";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(cageDoorSound);
                            hasCageDoorOpen = true;
                        }
                    }
                    else if (hasCageKey && hasCageDoorOpen)
                        nameText.text = "La porte est ouverte";
                }
                else if (objectName.StartsWith("CartonCage"))
                {
                    nameText.text = "Appuyer sur [E] pour bouger la boite";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(interactObjectSound);
                    }
                }
                else if (objectName.StartsWith("Clé Cage"))
                {
                    nameText.text = "Appuyer sur [E] pour prendre la clé de la cage";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(getKeyCageSound);
                    }
                }
                else if (objectName.StartsWith("Clé Camera"))
                {
                    nameText.text = "Appuyer sur [E] pour prendre la clé des caméras";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(getKeyCageSound);
                    }
                }
                else if (objectName.StartsWith("Disjoncteur"))
                {
                    if(!hasCameraKey)
                        nameText.text = "Le disjoncteur requiert une clé";
                    else if(hasCameraKey)
                        nameText.text = "Appuyer sur [E] pour eteindre la caméra";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(electricityShutDown);
                        }
                    else if (hasCameraKey && hasCageDisjoncteurOpen)
                        nameText.text = "La caméra est éteinte";
                }
                else if(objectName.StartsWith("Pass Porte"))
                {
                    nameText.text = "Appuyer sur [E] pour prendre le pass de la porte";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("TirroirBloqué"))
                {
                    nameText.text = "La porte est verrouillée, il est possible que le propriétaire ait caché la clé quelque part dans la maison";
                }
                else if(objectName.StartsWith("DetecteurCavePorte"))
                {
                    if(!hasPassCaveDoor)
                        nameText.text = "Il est nécessaire d'insérer le pass.";
                    else if(hasPassCaveDoor)
                        nameText.text = "Appuyer sur [E] pour donner le pass";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(passValid);
                        }
                    else if(hasPassCaveDoor && hasGivePassDoor)
                        nameText.text = "Le pass à été inséré";
                }
                else if(objectName.StartsWith("Frigo"))
                {
                    nameText.text = "Appuyer sur [E] pour ouvrir le frigo";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(doorSound);
                    }
                }
                else if(objectName.StartsWith("PorteBacAGlacon"))
                {
                    nameText.text = "Appuyer sur [E] pour ouvrir le bac a glacons";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(doorSound);
                    }
                }
                else if(objectName.StartsWith("Glacon"))
                {
                    nameText.text = "Appuyer sur [E] pour prendre le glacon";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("Four"))
                {
                    nameText.text = "Appuyer sur [E] pour lancer le feu";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(interactObjectSound);
                    }
                    else if(hasStartedFire)
                        nameText.text = "Le feu est allumé";

                    /*if(hasIcedGlace)
                        nameText.text = "Appuyer sur [E] pour lancer le feu";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                        }
                    else if(!hasIcedGlace)
                        nameText.text = "Il faut trouver un glaçon";
                    else if(hasStartedFire)
                        nameText.text = "Le feu est allumé";*/
                }
                else if(objectName.StartsWith("SecondKeyInCheminee"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer la clé de mission";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("Cheminée"))
                {
                    if(!hasStartedFire)
                        nameText.text = "Il est nécessaire d'allumer le feu préalablement";
                    if(hasFireCaqueteFireOne)
                        {
                            nameText.text = "Appuyer sur [A] pour refroidir la clé";
                            if(Input.GetKeyDown(KeyCode.A))
                            {
                                hasFireCaqueteFireTwo = true;
                                if(!hasKitchenKey)
                                {
                                    secondKeyInCheminee.SetActive(true);
                                }
                            }   
                        }
                    if(hasFireCaqueteFireTwo)
                        nameText.text = "Une clé de mission à été récupérée";
                    if(hasStartedFire)
                    {
                        if(!isWaitingForIceInFire && !hasIcedFinishFired && !hasIcedGlace)
                        {
                            nameText.text = "Un glacon devrait être mit dans ce feu";
                        }
                        else if(isWaitingForIceInFire)
                            nameText.text = "Le glacon est en train fondre";
                        else if(!isWaitingForIceInFire && !hasIcedFinishFired && hasIcedGlace)
                        {
                            nameText.text = "Appuyer sur [E] pour mettre le glacon";
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                interactObj.Interact();
                            }
                        }
                        else if(!isWaitingForIceInFire && hasIcedFinishFired && hasIcedGlace)
                        {
                            nameText.text = "Appuyer sur [A] pour caqueter et arreter le feu";
                            if(Input.GetKeyDown(KeyCode.A))
                            {
                                hasStartedFire = false;
                                FXFire.Stop();
                                fireSound.enabled = false;
                                hasFireCaqueteFireOne = true;
                            }
                        }
                    }
                }
                else if(objectName.StartsWith("N-Miroir"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(interactObjectSound);
                    }
                }
                else if(objectName.StartsWith("Miroir"))
                {
                    if(!hasCrackedMirror)
                        nameText.text = "Appuyer sur [A] pour caqueter et casser le miroir";
                        if(Input.GetKeyDown(KeyCode.A))
                        {
                            hasCrackedMirror = true;
                            MirorGlass.SetActive(true);
                            audioSource.PlayOneShot(breakSound);
                        }
                    if(hasCrackedMirror)
                    {
                        nameText.text = "Le miroir a été brisé";
                    }
                }
                else if(objectName.StartsWith("BouDeMiroir"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer le fragment du miroir";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("N-Oreiller"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(interactObjectSound);
                    }
                }
                else if(objectName.StartsWith("Oreiller"))
                {
                    /*if(hasBedroomKey)
                        nameText.text = "Une clé de mission a été récupérée";*/
                    if(hasCrackedMirror)
                    {
                        nameText.text = "Appuyer sur [E] pour couper l'oreiller";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(zipSound);
                        }
                    }
                    else if(!hasCrackedMirror)
                    {
                        nameText.text = "Il est nécessaire de trouver un objet tranchant";
                    }
                }
                else if(objectName.StartsWith("PivotPortePrincipale"))
                {
                    if(hasKitchenKey && hasBedroomKey && hasCaveKey)
                    {
                        nameText.text = "Appuyer sur [E] pour tenter de vous échapper";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            if(hasFinishChopperMission && hasFinishShooterMission)
                            {
                                StartCoroutine(endPlayerWin());
                            }
                            else if(!hasFinishChopperMission && hasFinishShooterMission)
                            {
                                StartCoroutine(endPlayerChopper());
                            }
                            else if(hasFinishChopperMission && !hasFinishShooterMission)
                            {
                                StartCoroutine(endPlayerShooter());
                            }
                            else if(!hasFinishShooterMission && !hasFinishChopperMission)
                            {
                                StartCoroutine(endPlayerNoMission());
                            }
                        }
                    }
                    else
                    {
                        nameText.text = "Il vous faut 3 clés de mission pour sortir (" + gameManager.numberOfKey.ToString() + " / 3)";
                    }
                }
                else if(objectName.StartsWith("TracesDePas"))
                {
                    nameText.text = "Veuillez appuyer sur [E] pour effacer les traces de pas";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(interactObjectSound);
                    }
                }
                else if(objectName.StartsWith("PoubelleChambre"))
                {
                    if(!hasBreakClock)
                        nameText.text = "Le réveil doit être mis à la poubelle";
                    else if(hasBreakClock && !hasThrowClockInBin)
                        nameText.text = "Appuyer sur [E] pour jeter le reveil";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(binSound);
                        }
                    else if(hasBreakClock && hasThrowClockInBin)
                        nameText.text = "Le reveil à été jeté";
                }
                else if(objectName.StartsWith("Reveil"))
                {
                    nameText.text = "Appuyer sur [E] pour casser le reveil et prendre les morceaux";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(breakSound);
                    }
                }
                else if(objectName.StartsWith("Munitions"))
                {
                    nameText.text = "Appuyer sur [E] pour prendre les munitions";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("Toilets"))
                {
                    if(!hasTakenMunitions)
                        nameText.text = "Des munitions doivent être jeté dans ces toilettes";
                    else if(hasTakenMunitions && !hasPutMunitonsInToilets)
                    {
                        nameText.text = "Appuyer sur [E] pour jeter les munitions";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(toiletSound);
                        }
                    }
                    else if(hasTakenMunitions && hasPutMunitonsInToilets)
                        nameText.text = "Les munitions ont été jetées";
                }
                else if(objectName.StartsWith("PoubelleCuisine"))
                {
                    if(!hasTakenMirrorGlasses)
                        nameText.text = "Des morceaux de verre doivent être jetés dans cette poubelle";
                    else if(hasTakenMirrorGlasses && !hasThrowGlassesInBin && hasCrackedPillow)
                    {
                        nameText.text = "Appuyer sur [E] pour jeter les morceaux de verre dans la poubelle";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(binSound);
                        }
                    }
                    else if(hasTakenMirrorGlasses && !hasThrowGlassesInBin && !hasCrackedPillow)
                        nameText.text = "Les morceaux de verre doivent servir avant de le jeter";
                    else if(hasThrowGlassesInBin && hasTakenMirrorGlasses)
                        nameText.text = "Les morceaux de verre ont été jetés dans la poubelle";
                }
                else if(objectName.StartsWith("Clé Chambre"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer la clé de la chambre";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                        UIbedroomKey.SetActive(true);
                    }
                }
                else if(objectName.StartsWith("PivotPorteChambre"))
                {
                    if(!hasKeyToOpenBedroomDoor)
                        nameText.text = "Chambre\nLa porte nécessite une clé";
                    else if(hasKeyToOpenBedroomDoor)
                    {
                        nameText.text = "Chambre\nAppuyer sur [E] pour ouvrir";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(doorSound);
                            UIbedroomKey.SetActive(false);
                        }
                    }
                }
                else if(objectName.StartsWith("Clé Bureau"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer la clé du bureau";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                        UIofficeKey.SetActive(true);
                    }
                }
                else if(objectName.StartsWith("PivotPorteBureau"))
                {
                    if(!hasKeyToOpenOfficeDoor)
                        nameText.text = "Bureau\nLa porte nécessite une clé";
                    else if(hasKeyToOpenOfficeDoor)
                    {
                        nameText.text = "Bureau\nAppuyer sur [E] pour ouvrir";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(doorSound);
                            UIofficeKey.SetActive(false);
                        }
                    }
                }
                else if(objectName.StartsWith("MaitrePiece"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        audioSource.PlayOneShot(interactObjectSound);
                        interactObj.Interact();
                    }
                }
                else if(objectName.StartsWith("PlanchePivot"))
                {
                    nameText.text = "Appuyer sur [A] pour détacher les planches";
                    if(Input.GetKeyDown(KeyCode.A))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(doorSound);
                    }
                }
                else if(objectName.StartsWith("Clé Cuisine"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer la clé de mission";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("PlumeDansLoreiller"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer la plume";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("StatueCanard"))
                {
                    if(!hasKeepUpPlume && !hasPutPlumeInStatue)
                        nameText.text = "Une plume devrait etre posée sur ce socle";
                    else if(hasKeepUpPlume && !hasPutPlumeInStatue)
                    {
                        nameText.text = "Appuyer sur [E] pour poser la plume";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                        }
                    }
                    else if(hasPutPlumeInStatue && hasKeepUpPlume)
                    {
                        nameText.text = "La plume à été correctement posée";
                    }
                }
                else if(objectName.StartsWith("Clé Canard"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer la clé de mission";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(zipSound);
                    }
                }
                else if(objectName.StartsWith("CouteauSurCuisine"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        audioSource.PlayOneShot(interactObjectSound);
                        interactObj.Interact();
                    }
                }
                else if(objectName.StartsWith("Board"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        audioSource.PlayOneShot(interactObjectSound);
                        interactObj.Interact();
                    }
                }
                else if(objectName.StartsWith("TableauCheminee"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        audioSource.PlayOneShot(interactObjectSound);
                        interactObj.Interact();
                    }
                }
                else if(objectName.StartsWith("Fusil"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        audioSource.PlayOneShot(interactObjectSound);
                        interactObj.Interact();
                    }
                }
                lastInteractedObject = hitInfo.collider.gameObject;
            }
            else
            {
                InteractionText.SetActive(false);
                DisableOutline(lastInteractedObject);
            }
        }
        else
        {
            InteractionText.SetActive(false);
            DisableOutline(lastInteractedObject);
        }
    }
    IEnumerator endPlayerWin()
    {
        fadeToBlackEndGame.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("End - Win");
    }
    IEnumerator endPlayerChopper()
    {
        fadeToBlackEndGame.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("End - Chopper");
    }
    
    IEnumerator endPlayerShooter()
    {
        fadeToBlackEndGame.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("End - Shooter");
    }
    IEnumerator endPlayerNoMission()
    {
        fadeToBlackEndGame.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("End - NoMission");
    }
    void DisableOutline(GameObject obj)
    {
        if (obj != null)
        {
            Outline outlineComponent = obj.GetComponent<Outline>();
            if (outlineComponent != null)
            {
                outlineComponent.enabled = false;
            }
        }
    }
}
