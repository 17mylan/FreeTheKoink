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
        Vector3 teleportPosition = gameManager.NativeDuckPosition.position;
        kinematicMotor.MoveCharacter(teleportPosition);
        yield return new WaitForSeconds(0.5f);
        EnableAddScripts();
        gameManager.canWalk = true;
    }
    private void DisableAllScripts()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // Vérifie si le script est différent du script Teleportation lui-même
            {
                script.enabled = false; // Désactive le script
            }
        }
    }
    private void EnableAddScripts()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // Vérifie si le script est différent du script Teleportation lui-même
            {
                script.enabled = true; // Désactive le script
            }
        }
    }
}
