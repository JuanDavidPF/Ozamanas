using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using Ozamanas.Board;

namespace Ozamanas.Machines
{
    public class WormPhysics : MachinePhysicsManager
    {
        private SandWormBoss worm;

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
    }
}
