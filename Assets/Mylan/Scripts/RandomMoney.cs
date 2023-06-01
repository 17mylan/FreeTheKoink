using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoney : MonoBehaviour
{
    public ParticleSystem[] particles;  // Tableau contenant les systèmes de particules à contrôler
    public float minActiveTime = 2f;   // Temps minimum d'activation
    public float maxActiveTime = 5f;   // Temps maximum d'activation

    private float timer;               // Compteur pour suivre le temps écoulé
    private float activeTime;          // Temps actif pour chaque système de particules

    void Start()
    {
        // Initialisation des temps actifs pour chaque système de particules
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();  // On s'assure que les particules sont désactivées au départ
            activeTime = Random.Range(minActiveTime, maxActiveTime);
        }

        // Démarre le compteur
        timer = 0f;
    }

    void Update()
    {
        // Incrémente le compteur
        timer += Time.deltaTime;

        // Vérifie si le temps actif est écoulé
        if (timer >= activeTime)
        {
            // Active ou désactive aléatoirement chaque système de particules
            foreach (ParticleSystem particle in particles)
            {
                if (Random.Range(0, 4) == 0)
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }

                // Génère un nouveau temps actif aléatoire pour le système de particules
                activeTime = Random.Range(minActiveTime, maxActiveTime);
            }

            // Réinitialise le compteur
            timer = 0f;
        }
    }
}
