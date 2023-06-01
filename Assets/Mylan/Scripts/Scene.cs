using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour
{
    public string sceneName;
    public void ChangeSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
