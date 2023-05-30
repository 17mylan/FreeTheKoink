using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float RunTimer = 10f;
    public Transform NativeDuckPosition;
    public GameObject PlayerObject, testObject;
    public int CurrentIndexOfRun = 1;
    private Teleportation teleportation;

    public bool canWalk = true;

    public void Start()
    {
        teleportation = FindObjectOfType<Teleportation>();

        Debug.Log("Game has started!");
        StartCoroutine(RunTimerClock());
    }

    IEnumerator RunTimerClock()
    {
        yield return new WaitForSeconds(RunTimer);
        CurrentIndexOfRun++;
        teleportation.TeleportSystem();
        Debug.Log("New run incoming: " + CurrentIndexOfRun);
        StartCoroutine(RunTimerClock());
        if(CurrentIndexOfRun >= 3)
        {
            print("Game has finished!");
        }
    }

    private KinematicCharacterController.KinematicCharacterMotor kinematicMotor;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            teleportation.TeleportSystem();
        }
    }
}
