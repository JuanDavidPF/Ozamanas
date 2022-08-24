using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JuanPayan.CodeSnippets.HelperComponents.Processors
{
    public class Processor : MonobehaviourEvents
    {

        public UnityEvent processes;

        public override void Behaviour()
        {
            processes?.Invoke();

        }//Closes Behaviour Implementations
    }//Close Processor class
}//Closes Namespace declaration