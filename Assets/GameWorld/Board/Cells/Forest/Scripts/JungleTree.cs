using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Ozamanas.Tags;
using DG.Tweening;

namespace Ozamanas.Board
{
    [RequireComponent(typeof(MeshRenderer))]

    [RequireComponent(typeof(Collider))]
    public class JungleTree : MonoBehaviour
    {
        [SerializeField] private float lifetime = 5f;

        [SerializeField] private float growingTime = 2f;
        [SerializeField] private GameObject fragmentedModel;

        private bool alreadyTriggered;

        private Collider cd;

        private MeshRenderer rd;

        [SerializeField] public UnityEvent OnDestruction;


        private void Awake()
        {

            cd = GetComponent<Collider>();
            rd = GetComponent<MeshRenderer>();
        }//Closes Awake method
        private void OnCollisionEnter(Collision other)
        {


            if (other.transform.tag != "Machine") return;

            DestroyTree();

        }//Closes OnColissionEnter method



        public void DestroyTree()
        {
            if (alreadyTriggered) return;
            alreadyTriggered = true;
            OnDestruction?.Invoke();
            gameObject.SetActive(false);
            GameObject dummy = Instantiate(fragmentedModel, transform.position, transform.rotation);
            if (dummy) Destroy(dummy, lifetime);

        }


    }
}
