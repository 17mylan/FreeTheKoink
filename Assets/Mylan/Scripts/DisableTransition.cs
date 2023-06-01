using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTransition : MonoBehaviour
{
    public GameObject thisObject;
    private GameManager gameManager;
    private Timer timer;
    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        gameManager = FindObjectOfType<GameManager>();
    }
    public void DisableTransitionEvent()
    {
        gameManager.StartCoroutine(gameManager.RunTimerClock());
        timer.ResetTimer();
        thisObject.SetActive(false);
    }
}
