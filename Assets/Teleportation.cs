using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void TeleportSystem()
    {
        transform.position = gameManager.NativeDuckPosition.position;
    }
}
