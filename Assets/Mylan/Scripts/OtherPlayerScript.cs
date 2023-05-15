using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip caqueteSound;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            audioSource.PlayOneShot(caqueteSound);
    }
}
