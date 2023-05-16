using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCatchedObjects : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        print("Je touche" + other.name);
    }
}
