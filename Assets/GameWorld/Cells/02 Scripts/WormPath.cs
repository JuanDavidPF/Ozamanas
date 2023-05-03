using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Machines;
using Ozamanas.Extenders;

namespace Ozamanas.Board
{
    public class WormPath : MonoBehaviour
    {
        [SerializeField] private bool isOnTop;
        [SerializeField] private WormPath otherCollider;

        private bool wormCrossed = false;

        public bool WormCrossed { get => wormCrossed; set => wormCrossed = value; }

        private void OnTriggerEnter(Collider other)
        {
            if(WormCrossed) return;

            if (other.tag != "Machine") return;

            if(other.TryGetComponentInParent<SandWormBoss>(out SandWormBoss worm))
            {
                if(!isOnTop) worm.PlayGoingUPAnim();
                WormCrossed = true;
                otherCollider.WormCrossed = true;
            }
        }
    }
}
