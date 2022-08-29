using System.Collections;
using System.Collections.Generic;
using JuanPayan.CodeSnippets.HelperComponents;
using UnityEngine;
using UnityEngine.Events;

namespace JuanPayan.References.Strings
{
    public class StringListener : MonobehaviourEvents
    {

        [TextArea(1, 5)]
        [SerializeField] private string eventDescription;

        [Space(20)]
        [SerializeField] private StringVariable stringReference;
        [SerializeField] private UnityEvent<string> Response;


        protected override void OnEnable()
        {
            base.OnEnable();
            stringReference.Listen(this);
        }//closes OnEnable method

        protected override void OnDisable()
        {
            base.OnDisable();
            if (!stringReference) return;
            stringReference.Unlisten(this);
        }//closes OnDisable method


        [ContextMenu("Raise Event")]
        public override void Behaviour()
        {
            if (!stringReference) return;
            Response.Invoke(stringReference.value);
        }//Closes Behaviour method

    }//closes FloatListener Class
}//Closes Namespace declaration
