using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom : MonoBehaviour
{
    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "barril.png", false);
    }
}
