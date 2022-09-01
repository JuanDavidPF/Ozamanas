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
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private Transform fragmentedModel;
        [SerializeField] private ParticleSystem destructionVFX;

        private bool alreadyTriggered;
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
            if (alreadyTriggered || other.transform.tag != "Machine") return;
            alreadyTriggered = true;

            transform.parent = null;
            transform.DetachChildren();


            OnDestruction?.Invoke();

            if (fragmentedModel) fragmentedModel.gameObject.SetActive(true);
            if (destructionVFX) destructionVFX.Play();

            StartCoroutine(HandleVanishing());
        }//Closes OnColissionEnter method

        private IEnumerator HandleVanishing()
        {
            yield return new WaitForSeconds(lifetime);

            if (destructionVFX) Destroy(destructionVFX);
            if (fragmentedModel) Destroy(fragmentedModel.gameObject);
            Destroy(gameObject);

        }//Closes HandleVanishing corotuine

    }//Closes DestructableElement class
}//Closes Namespace declaration
