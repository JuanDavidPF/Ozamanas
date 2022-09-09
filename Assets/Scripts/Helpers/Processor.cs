using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JuanPayan.Helpers
{
    public class Processor : MonobehaviourEvents
    {

        public UnityEvent processes;

        [ContextMenu("Execute Processor")]
        public override void Behaviour()
        {

            processes?.Invoke();

        }//Closes Behaviour Implementations
    }//Close Processor class
}//Closes Namespace declaration