using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.References
{
    [Serializable]
    public class FloatReference
    {
        public bool useConstant = true;
        public float constantValue;
        public FloatVariable variable;
        private List<FloatListener> listeners = new List<FloatListener>();

        public float value
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

            foreach (FloatListener listener in listeners)
            {
                if (!listener) { Unlisten(listener); continue; }
                listener.Behaviour();
            }

        }//closes OnChange method

        public void Listen(FloatListener listener)
        {
            variable.Listen(listener);
            if (!listeners.Contains(listener)) listeners.Add(listener);

        }//closes Listen method

        public void Unlisten(FloatListener listener)
        {
            variable.Unlisten(listener);
            listeners.Remove(listener);
        }//closes Unlisten method

    }//closes FloatReference class
}//Closes Namespace declaration