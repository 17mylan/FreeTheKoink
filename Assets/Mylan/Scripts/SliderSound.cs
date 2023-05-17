using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderSound : MonoBehaviour
{
    public Slider slider;
    public List<Sprite> levelSprites;
    public Image levelImage;

    private void Start()
    {
        if (levelSprites.Count > 0)
            UpdateLevelImage();
    }

    public void OnSliderValueChanged()
    {
        UpdateLevelImage();
    }

    private void UpdateLevelImage()
    {
        if (slider.value == 0)
            levelImage.sprite = levelSprites[0];
        else
            levelImage.sprite = levelSprites[1];
    }
}
