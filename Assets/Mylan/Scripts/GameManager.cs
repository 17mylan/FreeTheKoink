using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Teleportation")]
    public Transform NativeDuckPosition;
    public Teleportation teleportation;
    public Interaction interaction;
    public Timer timer;
    public float TransitionBetweenRunTimer = 7f;
    public GameObject TransitionObject;
    public TextMeshProUGUI numberOfKeysText;
    public int numberOfKey = 0;

    
    [Header("Run Manager")]
    public float RunTimer = 10f;
    public int CurrentIndexOfRun = 1, maxRun = 10;
    public TextMeshProUGUI playerDollar;
    public int maxPlayerDollar = 3500;
    public int currentPlayerDollar;
    public Slider dollarSlider;

    [Header("Player")]
    public bool canWalk = true;

    public void Start()
    {
        interaction = FindObjectOfType<Interaction>();
        teleportation = FindObjectOfType<Teleportation>();
        timer = FindObjectOfType<Timer>();
        Debug.Log("Game has started!");
        StartCoroutine(RunTimerClock());
        currentPlayerDollar = maxPlayerDollar;
        playerDollar.text = currentPlayerDollar.ToString() + "$";
        dollarSlider.value = (float)currentPlayerDollar / maxPlayerDollar;
        timer.StartTimer();
    }

    public IEnumerator RunTimerClock()
    {
        print("Coroutine started");
        yield return new WaitForSeconds(RunTimer);
        TransitionObject.SetActive(true);
        CurrentIndexOfRun++;
        teleportation.TeleportSystem();
        Debug.Log("New run incoming: " + CurrentIndexOfRun);
        if(CurrentIndexOfRun > maxRun) // pour pouvoir jouer la derniere
        {
            print("Game has finished!");
        }
        else
        {
            //StartCoroutine(RunTimerClock());
            // Fait dans l'animator
        }
    }

    public void UpdateKeyNumberInUI()
    {
        /*if(interaction.hasBedroomKey)
            numberOfKey = numberOfKey + 1;
        if(interaction.hasCaveKey)
            numberOfKey = numberOfKey + 1;
        if(interaction.hasKitchenKey)
            numberOfKey = numberOfKey + 1;*/
        numberOfKeysText.text = numberOfKey.ToString() + " / 3"; 
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
