using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.References.Integers
{
    [Serializable]
    public class IntegerReference
    {
        public bool useConstant = true;
        public int constantValue;
        public IntegerVariable variable;
        private List<IntegerListener> listeners = new List<IntegerListener>();

        public int value
        {
            get { return useConstant ? constantValue : variable.value; }
            set
            {
                if (useConstant)
                {
                    constantValue = value;
                    OnChange();
                }
                else if (variable) variable.value = value;

            }
        }

        public void OnChange()
        {
            if (!useConstant) return;

            foreach (IntegerListener listener in listeners)
            {
                if (!listener) { Unlisten(listener); continue; }
                listener.Behaviour();
            }

        }//closes OnChange method

        public void Listen(IntegerListener listener)
        {
            variable.Listen(listener);
            if (!listeners.Contains(listener)) listeners.Add(listener);

        }//closes Listen method

        public void Unlisten(IntegerListener listener)
        {
            variable.Unlisten(listener);
            listeners.Remove(listener);
        }//closes Unlisten method

    }//closes IntegerReference Class
}//Closes Namespace delaration
