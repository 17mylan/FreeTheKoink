using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInternet : MonoBehaviour
{
    public string internetURL;

    public void OpenInternetURL()
    {
        Application.OpenURL(internetURL);
    }
}
