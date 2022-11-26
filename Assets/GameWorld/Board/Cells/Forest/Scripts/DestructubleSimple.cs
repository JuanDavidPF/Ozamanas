using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.Forest
{
public class DestructubleSimple : MonoBehaviour
{

     private void OnTriggerEnter(Collider other)
    {
        if ( other.transform.tag != "Machine") return;
        Destroy(gameObject);
    }
}
}
