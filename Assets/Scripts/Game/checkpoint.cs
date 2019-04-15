using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour {

    public GameObject nextCheckpoint;

	public string room;
		
	public void Start()
	{
		room = getNameRoom();
	}
	
	public string getNameRoom()
	{
		return gameObject.transform.parent.gameObject.name;
	}
}