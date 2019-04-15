using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;
    
    public void letsGo()
    {
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
    }
}
