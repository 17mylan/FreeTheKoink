using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OtherPlayerScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip caqueteSound;
    public Animator animator;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            audioSource.PlayOneShot(caqueteSound);
            StartCoroutine(WaitCaquete());
        }
    }
    IEnumerator WaitCaquete()
    {
        animator.SetFloat("Caquete", 1f);
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("Caquete", 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Camera"))
        {
            SceneManager.LoadScene("GO-Camera");
        }
    }
}
