using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private GameManager gameManager;
    private Timer timer;
    private KinematicCharacterController.KinematicCharacterMotor kinematicMotor;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        timer = FindObjectOfType<Timer>();
        kinematicMotor = transform.GetComponent<KinematicCharacterController.KinematicCharacterMotor>();
    }
    public void TeleportSystem()
    {
        StartCoroutine(tpSys());
    }
    IEnumerator tpSys()
    {
        gameManager.canWalk = false;
        yield return new WaitForSeconds(1.2f);
        Vector3 teleportPosition = gameManager.NativeDuckPosition.position;
        kinematicMotor.SetPosition(teleportPosition);
        yield return new WaitForSeconds(0.5f);
        gameManager.canWalk = true;
    }
}
