using System.Collections;
using System.Collections.Generic;
using JuanPayan.CodeSnippets.HelperComponents;
using UnityEngine;
using UnityEngine.Events;

namespace JuanPayan.References.Floats
{
    public class FloatListener : MonobehaviourEvents
    {

        [TextArea(1, 5)]
        [SerializeField] private string eventDescription;

        [Space(20)]
        [SerializeField] private FloatVariable floatReference;
        [SerializeField] private UnityEvent<float> Response;


        protected override void OnEnable()
        {
            base.OnEnable();
            floatReference.Listen(this);
        }//closes OnEnable method

        protected override void OnDisable()
        {
            base.OnDisable();
            if (!floatReference) return;
            floatReference.Unlisten(this);
        }//closes OnDisable method


        [ContextMenu("Raise Event")]
        public override void Behaviour()
        {
            if (!floatReference) return;
            Response.Invoke(floatReference.value);
        }//Closes Behaviour method

    }//closes FloatListener Class
}//Closes Namespace declaration
