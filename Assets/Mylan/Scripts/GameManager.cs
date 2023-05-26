using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float RunTimer = 20f;
    public Transform NativeDuckPosition;
    public GameObject PlayerObject;
    public int CurrentIndexOfRun = 1;
    private Teleportation teleportation;
    public void Start()
    {
        teleportation = FindObjectOfType<Teleportation>();

        Debug.Log("Game has started!");
        teleportation.TeleportSystem();
        StartCoroutine(RunTimerClock());
    }

    IEnumerator RunTimerClock()
    {
        yield return new WaitForSeconds(RunTimer);
        CurrentIndexOfRun++;
        teleportation.TeleportSystem();
        Debug.Log("New run in coming " + CurrentIndexOfRun);
        StartCoroutine(RunTimerClock());
        if(CurrentIndexOfRun >= 3)
        {
            print("Game has finished!");
        }
    }
}
