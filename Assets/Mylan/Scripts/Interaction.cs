using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public AudioSource audioSource;
    public AudioClip doorSound;
    public AudioClip cageDoorSound;
    public AudioClip getKeyCageSound;
    public AudioClip passValid;
    public AudioClip zipSound;
    public AudioClip electricityShutDown;
    public AudioSource fireSound;
    public GameObject imageKeyCageAsset, imageKeyDisjoncteur, imagePassDoor, FXFirePrefab;
    public ParticleSystem FXFire;
    private GameManager gameManager;

    [Header("Key Counts")]

    public bool hasKitchenKey = false;
    public bool hasBedroomKey = false;
    public bool hasCaveKey = false;

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
    public GameObject GlaconPrefab;
    public GameObject KeyPrefab;
    public GameObject MirorGlass;
    public GameObject GlaconPrefabUI;
    public GameObject VerreMiroir;
    public GameObject narrativeTextObject;
    public TextMeshProUGUI narrativeText;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        narrativeText.text = "Il faut que je trouve un moyen de sortir d’ici ! Je crois que le propriétaire à laissé la clé de la cage sur les cartons à côté";
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
                Outline outlineComponent = hitInfo.collider.gameObject.GetComponent<Outline>();
                if (outlineComponent != null)
                    outlineComponent.enabled = true;

                if (objectName.StartsWith("Narrative-"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                        interactObj.Interact();
                }
                else if (objectName.StartsWith("Interactive-"))
                {
                    nameText.text = "Appuyer sur [E] pour intéragir";
                    if (Input.GetKeyDown(KeyCode.E))
                        interactObj.Interact();
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
                    }
                }
                else if (objectName.StartsWith("Clé Cage"))
                {
                    nameText.text = "Appuyer sur [E] pour prendre la clé";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(getKeyCageSound);
                    }
                }
                else if (objectName.StartsWith("Clé Camera"))
                {
                    nameText.text = "Appuyer sur [E] pour prendre la clé";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                        audioSource.PlayOneShot(getKeyCageSound);
                    }
                }
                else if (objectName.StartsWith("Disjoncteur"))
                {
                    if(!hasCameraKey)
                        nameText.text = "Le disjoncteur est cadenassé ! La clé ne devrait pas être loin";
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
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                    }
                }
                else if(objectName.StartsWith("DetecteurCavePorte"))
                {
                    if(!hasPassCaveDoor)
                        nameText.text = "Le pass doit être inséré";
                    else if(hasPassCaveDoor)
                        nameText.text = "Appuyer sur [E] pour donner le pass";
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                            audioSource.PlayOneShot(passValid);
                        }
                    else if(hasPassCaveDoor && hasGivePassDoor)
                        nameText.text = "Le pass à été donné";
                }
                else if(objectName.StartsWith("Frigo"))
                {
                    nameText.text = "Appuyer sur [E] pour ouvrir le frigo";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
                    }
                }
                else if(objectName.StartsWith("PorteBacAGlacon"))
                {
                    nameText.text = "Appuyer sur [E] pour ouvrir le bac a glacons";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
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
                    if(hasIcedGlace)
                        nameText.text = "Appuyer sur [E] pour lancer le feu";
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            interactObj.Interact();
                        }
                    else if(!hasIcedGlace)
                        nameText.text = "Le glacon doit être trouvé";
                    else if(hasStartedFire)
                        nameText.text = "Le feu est allumé";
                }
                else if(objectName.StartsWith("Cheminée"))
                {
                    if(!hasStartedFire)
                        nameText.text = "Le feu doit être allumé avant";
                    if(hasFireCaqueteFireOne)
                        {
                            nameText.text = "Appuyer sur [A] pour refroidir la clé et la prendre";
                            if(Input.GetKeyDown(KeyCode.A))
                            {
                                hasFireCaqueteFireTwo = true;
                                hasKitchenKey = true;
                                gameManager.numberOfKey = gameManager.numberOfKey + 1;
                                audioSource.PlayOneShot(zipSound);
                                gameManager.UpdateKeyNumberInUI();
                                KeyPrefab.SetActive(false);
                            }   
                        }
                    if(hasFireCaqueteFireTwo)
                        nameText.text = "La clé à été récupérée";
                    if(hasStartedFire)
                        if(isWaitingForIceInFire)
                            nameText.text = "Le glacon est en train fondre";
                        else if(!isWaitingForIceInFire && !hasIcedFinishFired)
                        {
                            nameText.text = "Appuyer sur [E] pour mettre le glacon";
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                interactObj.Interact();
                            }
                        }
                        else if(!isWaitingForIceInFire && hasIcedFinishFired)
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
                else if(objectName.StartsWith("N-Miroir"))
                {
                    nameText.text = "Appuyer sur [E] pour inspecter";
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactObj.Interact();
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
                        }
                    if(hasCrackedMirror)
                    {
                        nameText.text = "Le miroir est cassé";
                    }
                }
                else if(objectName.StartsWith("BouDeMiroir"))
                {
                    nameText.text = "Appuyer sur [E] pour récuperer le bou du miroir";
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
                    }
                }
                else if(objectName.StartsWith("Oreiller"))
                {
                    if(hasBedroomKey)
                        nameText.text = "Une clé a été récupérée";
                    else if(hasCrackedMirror)
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
                        nameText.text = "Un objet coupant doit être trouvé";
                    }
                }
                else if(objectName.StartsWith("PivotPortePrincipale"))
                {
                    if(hasCameraKey && hasBedroomKey && hasCaveKey)
                    {
                        nameText.text = "Appuyer sur [E] pour vous échapper";
                    }
                    else
                    {
                        nameText.text = "Il vous faut 3 clés pour sortir (" + gameManager.numberOfKey.ToString() + " / 3)";
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
