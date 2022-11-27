using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Machines
{
    public class HumanMachineBulldozerBoss : HumanMachine
    {
        public void Attack()
        {

        }

        public UnityEvent OnAttack;

        void OnTriggerEnter()
        {
            Debug.Log("sdasdas");
        }
    }
}
