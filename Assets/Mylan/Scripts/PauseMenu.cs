using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public float waitTransitionToFinishTimer = 2f;
    public bool canPauseMenu = false, gameIsPaused = false;
    public GameObject pauseMenuUI, fadeToBlack;
    public void Start()
    {
        StartCoroutine(WaitTransitionScene());
    }
    IEnumerator WaitTransitionScene()
    {
        yield return new WaitForSeconds(waitTransitionToFinishTimer);
        canPauseMenu =  true;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && canPauseMenu)
        {
            if(gameIsPaused)
                Resume();
            else
                 Pause();
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    public void ChangeScene()
    {
        StartCoroutine(ChangeSceneWaiter());
    }
    IEnumerator ChangeSceneWaiter()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        fadeToBlack.SetActive(true);
        PlayerPrefs.SetString("PreviousScene", "Playground");
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }
}
