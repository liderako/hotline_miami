using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPathCheckPoint : MonoBehaviour
{
    
    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "Arrow.png", false);
        if (gameObject.GetComponent<checkpoint>().nextCheckpoint)
        {
            Transform target = gameObject.GetComponent<checkpoint>().nextCheckpoint.transform;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
