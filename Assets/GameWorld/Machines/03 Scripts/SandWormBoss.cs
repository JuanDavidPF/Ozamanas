using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Ozamanas.Machines
{
    public class SandWormBoss : HumanMachine
    {   
        [SerializeField] private float ySpeed = 2f;
        [SerializeField] private float yPosUP = 1.5f;

        [SerializeField] private float yPosDown = 1.5f;

        [SerializeField] private float rotStart = 1.5f;

        [SerializeField] private Vector3 rot = new Vector3(180,0,0);

        [SerializeField] private float rotSpeed = 1.5f;

        [SerializeField] private Transform visuals;
        private  Sequence mySequence;

        protected override void Start()
        {
            base.Start();
            transform.parent = null;
           // Crawling();
        }
        private void Crawling()
        {
            float temp = ySpeed * rotStart;
            mySequence = DOTween.Sequence();
            mySequence.Append(visuals.DOMoveY(yPosUP,ySpeed,false));
            mySequence.Insert(0.8f,visuals.DORotate(rot,rotSpeed,RotateMode.Fast).From(new Vector3(0,0,0)));
            mySequence.Append(visuals.DOMoveY(-yPosDown,ySpeed,false));
            mySequence.Insert(2.8f,visuals.DORotate(new Vector3(0,0,0),rotSpeed,RotateMode.Fast).From(rot));
            mySequence.SetLoops(-1,LoopType.Restart);
        } 

        public void WormCanMove(bool canMove)
        {
            if(canMove) Crawling();
            else mySequence.Kill();
        }

        protected override void OnDestroy()
        {
            mySequence.Kill();
            base.OnDestroy();
        }
        
    }
}
