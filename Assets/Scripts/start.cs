using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Level0", LoadSceneMode.Single);
    }
}
