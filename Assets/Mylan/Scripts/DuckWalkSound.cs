using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckWalkSound : MonoBehaviour
{
    public List<AudioClip> footstepsSounds;
    public AudioSource audioSource;
    public float cooldown = 0.5f;
    private bool canPlaySound = true;
    private int currentFootstepIndex = 0;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("Veuillez attacher un AudioSource au GameObject du personnage.");
            }
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canPlaySound = true;
    }

    private void PlayFootstepSound()
    {
        if (!canPlaySound)
            return;

        if (footstepsSounds.Count == 0)
        {
            Debug.LogWarning("La liste des clips audio des pas est vide.");
            return;
        }

        audioSource.clip = footstepsSounds[currentFootstepIndex];
        audioSource.Play();

        currentFootstepIndex = (currentFootstepIndex + 1) % footstepsSounds.Count;

        canPlaySound = false;
        StartCoroutine(ResetCooldown());
    }
    public void OnCharacterMove()
    {
        PlayFootstepSound();
    }
}
