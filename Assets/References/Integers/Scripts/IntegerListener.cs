using System.Collections;
using System.Collections.Generic;
using JuanPayan.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace JuanPayan.References

{
    public class IntegerListener : MonobehaviourEvents
    {

        [TextArea(1, 5)]
        [SerializeField] private string eventDescription;

        [Space(20)]
        [SerializeField] private IntegerVariable intReference;
        [SerializeField] private UnityEvent<int> Response;


        protected override void OnEnable()
        {
            base.OnEnable();
            intReference.Listen(this);
        }//closes OnEnable method

        protected override void OnDisable()
        {
            base.OnDisable();
            if (!intReference) return;
            intReference.Unlisten(this);
        }//closes OnDisable method


        [ContextMenu("Raise Event")]
        public override void Behaviour()
        {
            if (!intReference) return;
            Response.Invoke(intReference.value);
        }//Closes Behaviour method

    }//closes IntegerListener classs
}//Closes Namespace declaration