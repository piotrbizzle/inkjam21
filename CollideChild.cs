using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideChild : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider)
     {
         transform.parent.GetComponent<TopScript>().CollisionDetected(collider);
     }
 }
