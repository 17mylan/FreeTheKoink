using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform[] targets; // Liste des points sur lesquels la cam�ra peut se focaliser
    public float rotationSpeed = 5f; // Vitesse de rotation de la cam�ra
    public float distance = 10f; // Distance entre la cam�ra et le point de focalisation

    private int _currentTargetIndex = 0; // Index du point de focalisation actuel

    private float _yaw = 0f;
    private float _pitch = 0f;

    private void Update()
    {
        // R�cup�rer les mouvements de la souris
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Ajouter les mouvements de la souris � la rotation actuelle de la cam�ra
        _yaw += mouseX * rotationSpeed;
        _pitch -= mouseY * rotationSpeed;

        // Limiter l'angle de rotation vertical de la cam�ra entre -90� et 90�
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);

        // Appliquer la rotation � la cam�ra
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);

        // V�rifier si le joueur a cliqu� avec le bouton gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            // Changer le point de focalisation
            _currentTargetIndex = (_currentTargetIndex + 1) % targets.Length;
        }

        // D�placer la cam�ra pour qu'elle regarde le point de focalisation actuel
        transform.position = targets[_currentTargetIndex].position - transform.forward * distance;
    }
}
