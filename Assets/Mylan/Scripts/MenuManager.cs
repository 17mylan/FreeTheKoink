using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public float playAnimationDelayTimer = 1f;
    public GameObject fadeTransition, creditsPanel, settingsPanel;
    public string sceneToLoad;
    public Animator creditsPanelAnimator, settingsPanelAnimator;
    public bool isCreditsPanelAnimationPlaying = false, isSettingsPanelAnimationPlaying = false;
    public bool isSettingsPanelOpen = false, isCreditsPanelOpen = false;
    public void Play()
    {
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
    {
        fadeTransition.SetActive(true);
        yield return new WaitForSeconds(playAnimationDelayTimer);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OpenCredits()
    {
        StartCoroutine(OpenCreditsPanel());
    }
    IEnumerator OpenCreditsPanel()
    {
        if (!isCreditsPanelAnimationPlaying && !isSettingsPanelOpen)
        {
            creditsPanel.SetActive(true);
            isCreditsPanelAnimationPlaying = true;
            yield return new WaitForSeconds(1f);
            isCreditsPanelAnimationPlaying = false;
            isCreditsPanelOpen = true;
        }
    }

    public void CloseCredits()
    {
        if(!isCreditsPanelAnimationPlaying)
            StartCoroutine(CloseCreditsPanel());
    }
    IEnumerator CloseCreditsPanel()
    {
        creditsPanelAnimator.Play("PanelCreditsBackwards");
        isCreditsPanelAnimationPlaying = true;
        yield return new WaitForSeconds(1f);
        creditsPanel.SetActive(false);
        isCreditsPanelAnimationPlaying = false;
        isCreditsPanelOpen = false;
    }

    public void OpenSettings()
    {
        StartCoroutine(OpenSettingsPanel());
    }
    IEnumerator OpenSettingsPanel()
    {
        if (!isSettingsPanelAnimationPlaying && !isCreditsPanelOpen)
        {
            settingsPanel.SetActive(true);
            isSettingsPanelAnimationPlaying = true;
            yield return new WaitForSeconds(1f);
            isSettingsPanelAnimationPlaying = false;
            isSettingsPanelOpen = true;
        }
    }

    public void CloseSettings()
    {
        if(!isSettingsPanelAnimationPlaying)
            StartCoroutine(CloseSettingsPanel());
    }
    IEnumerator CloseSettingsPanel()
    {
        settingsPanelAnimator.Play("PanelSettingsBackwards");
        isSettingsPanelAnimationPlaying = true;
        yield return new WaitForSeconds(1f);
        settingsPanel.SetActive(false);
        isSettingsPanelAnimationPlaying = false;
        isSettingsPanelOpen = false;
    }
    public void CloseGame()
    {
        Application.Quit();
    }
}
