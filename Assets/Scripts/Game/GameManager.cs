using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public AudioSource _win;
    public AudioSource _lose;

    public bool isEnd;
    public bool isVictory;
    public GameObject _textAmmoObject;
    public GameObject _panelGameOver;
    public GameObject _panelVictory;
    public PlayerController playerController;
    public int countEnemy;
    public int amountEnemy;
    public bool endAudio;
    
    void Awake () {
        if (gm == null)
            gm = this;
    }

    void Update()
    {
        if (playerController.getAmmo() == -1)
        {
            _textAmmoObject.GetComponent<Text>().text = "Ammo: Infinity $_$";
        }
        else
        {
            _textAmmoObject.GetComponent<Text>().text = "Ammo: " + playerController.getAmmo();
        }

        if (!endAudio)
        {
            if (isVictory && isEnd)
            {
                victory();
            }
            else if (!isVictory && isEnd)
            {
                gameOver();
            }
        }
    }

    public void victory()
    {
        _win.Play();
        _panelVictory.SetActive(true);
        endAudio = true;
    }

    public void gameOver()
    {
        _lose.Play();
        _panelGameOver.SetActive(true);
        endAudio = true;
    }
}
