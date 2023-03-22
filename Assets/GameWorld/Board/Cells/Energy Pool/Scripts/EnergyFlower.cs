using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Board;
using DG.Tweening;

namespace Ozamanas.Energy
{
    public class EnergyFlower : MonoBehaviour
    {

        private Animator animator;
        [SerializeField] private GameObject onDestroyVFX;


        void Awake()
        {
            animator=GetComponent<Animator>();
        }
        public void ShowOrHideFlower(bool show)
        {
          if(show) animator.SetTrigger("Show");
          else animator.SetTrigger("Hide");
        }

        void OnDestroy()
        {
            if(!onDestroyVFX) return;

            GameObject vfx = Instantiate(onDestroyVFX,transform.position,transform.rotation);
            vfx.transform.position += new Vector3(0,0.5f,0) ;
        }
    }

}
