using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.References.Strings
{
    [Serializable]
    public class StringReference
    {
        public bool useConstant = true;
        public string constantValue;
        public StringVariable variable;
        private List<StringListener> listeners = new List<StringListener>();

        public string value
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

            foreach (StringListener listener in listeners)
            {
                if (!listener) { Unlisten(listener); continue; }
                listener.Behaviour();
            }

        }//closes OnChange method

        public void Listen(StringListener listener)
        {
            variable.Listen(listener);
            if (!listeners.Contains(listener)) listeners.Add(listener);

        }//closes Listen method

        public void Unlisten(StringListener listener)
        {
            variable.Unlisten(listener);
            listeners.Remove(listener);
        }//closes Unlisten method

    }//closes FloatReference class
}//Closes Namespace declaration