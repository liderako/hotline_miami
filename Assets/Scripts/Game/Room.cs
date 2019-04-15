using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> exit;
    
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<PlayerController>().room = gameObject.name;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            if (other is BoxCollider2D)
                other.gameObject.GetComponent<EnemyController>().room = gameObject.name;
        }
    }

    public GameObject findPath(string anotherRom)
    {
        foreach (GameObject ex in exit)
        {
            if (ex.GetComponent<checkpoint>().nextCheckpoint.GetComponent<checkpoint>().room == anotherRom)
            {
                return (ex);
            }
        }
        return null;
    }
}
