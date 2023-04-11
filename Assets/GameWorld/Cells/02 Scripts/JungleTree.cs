using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Forest
{
    public class JungleTree : MonoBehaviour
    {

        [SerializeField] private float lifeTime = 2f;
        [SerializeField] private GameObject vfxOnDestroy;
        private Rigidbody rb;

        private bool destroyed = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
    

        void OnCollisionEnter(Collision collision)
        {

            if(collision.gameObject.tag != "Machine") return;

            if(destroyed) return;

            destroyed = true;

            if(vfxOnDestroy) Instantiate(vfxOnDestroy,transform);

            Destroy(gameObject,lifeTime);

            rb.constraints = RigidbodyConstraints.None;
      
        }
       
    }
}
