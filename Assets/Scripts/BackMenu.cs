using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
    public void back()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
