using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (GameManager.gm.playerController.isWeapon && GameManager.gm.playerController.getAmmo() != -1)
            {
                GameManager.gm.playerController.setAmmo(25);
                Destroy(gameObject);
            }
        }
    }
}
