using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject _door;
	public BoxCollider2D _box;
    

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
		{	_door.GetComponent<SpriteRenderer>().enabled = false;
			_box.enabled = false;
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
		{
			_door.GetComponent<SpriteRenderer>().enabled = true;
			_box.enabled = true;
		}	
	}
}