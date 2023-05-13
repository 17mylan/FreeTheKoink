using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHover : MonoBehaviour
{
    public float upScaleValue = 1.2f, normalScaleValue = 1f;
    public void PointerEnter()
    {
        transform.localScale = new Vector2(upScaleValue, upScaleValue);
    }
    public void PointerExit()
    {
        transform.localScale = new Vector2(normalScaleValue, normalScaleValue);
    }
}
