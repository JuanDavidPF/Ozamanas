using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using Ozamanas.Board;
using Ozamanas.Forces;
using DG.Tweening;
using Ozamanas.Tags;

namespace Ozamanas.Machines
{
    public class WormPhysics : MachinePhysicsManager
    {
        private SandWormBoss worm;

        [SerializeField] private Transform head;
        [SerializeField] private float shakeTime = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            worm = GetComponent<SandWormBoss>();
        }
        public override void ResetMachineNavAndAI()
        {
          //worm.IsCrawling = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

             if (other.tag != "Cell") return;

            if (other.transform.TryGetComponentInParent(out Cell cell))
            {
                worm.AddCurrentCell(cell);
                
            }
        }

        public override void AddForceToMachine(PhysicsForce force, Vector3 forceOrigin)
        {
            if(!force) return;

            if(forceOrigin.Equals(null)) return;

            SetKinematic();

            machineTween = head.DOShakePosition(shakeTime,0.3f,10,45,false,true);

            machineTween.OnComplete(() =>
            {
                SetIntelligent();
            });
        }

        public override void SetIntelligent()
        {
            state = PhysicMode.Intelligent;
            worm.WormMovement.StartWorm();
            OnActivateIntelligent?.Invoke();
        }

        public override void SetKinematic()
        {
            state = PhysicMode.Kinematic;
            worm.WormMovement.StopWorm();
            OnActivateKinematic?.Invoke();
        }

      
    }
}
