using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Board
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]



    public class DestructableElement : MonoBehaviour
    {
        private Rigidbody rb;
        private Collider cd;

        [SerializeField] public UnityEvent OnDestruction;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            cd = GetComponent<Collider>();
        }//Closes Awake method
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag != "Machine") return;

            transform.parent = null;
            OnDestruction?.Invoke();
            enabled = false;
        }//Closes OnColissionEnter method



    }//Closes DestructableElement class
}//Closes Namespace declaration
