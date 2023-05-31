using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private GameManager gameManager;
    private KinematicCharacterController.KinematicCharacterMotor kinematicMotor;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        kinematicMotor = transform.GetComponent<KinematicCharacterController.KinematicCharacterMotor>();
        print("Bien instancié");
    }
    public void TeleportSystem()
    {
        StartCoroutine(tpSys());
    }
    IEnumerator tpSys()
    {
        DisableAllScripts();
        gameManager.canWalk = false;
        Vector3 teleportPosition = gameManager.NativeDuckPosition.position;
        //kinematicMotor.MoveCharacter(teleportPosition);
        kinematicMotor.SetPosition(teleportPosition);
        //this.transform.position = teleportPosition; Téléporte le joueur mais revient sur son acienne position
        yield return new WaitForSeconds(0.5f);
        EnableAddScripts();
        gameManager.canWalk = true;
    }
    private void DisableAllScripts()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }
    }
    private void EnableAddScripts()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = true;
            }
        }
    }
}
