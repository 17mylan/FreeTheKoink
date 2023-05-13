using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAnimationFinshed : MonoBehaviour
{
    public float animationDelay = 1f;
    void Start()
    {
        StartCoroutine(FinishedAnimation());
    }
    IEnumerator FinishedAnimation()
    {
        yield return new WaitForSeconds(animationDelay);
        this.gameObject.SetActive(false);
    }
}
