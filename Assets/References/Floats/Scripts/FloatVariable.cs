using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JuanPayan.References.Floats
{
    [CreateAssetMenu(fileName = ("newFloatVariable"), menuName = ("References/Float/Float variable"))]
    public class FloatVariable : ScriptableObject
    {

        private List<FloatListener> listeners = new List<FloatListener>();

        [SerializeField] private float _value;
        public float value { get { return _value; } set { _value = value; OnChange(); } }


        public void OnChange()
        {
            foreach (FloatListener listener in listeners)
            {
                if (!listener) { Unlisten(listener); continue; }
                listener.Behaviour();
            }
        }//closes Invoke method

        public void Listen(FloatListener listener)
        {
            if (listeners.Contains(listener)) return;
            listeners.Add(listener);
        }//closes RegisterListener method

        public void Unlisten(FloatListener listener)
        {
            listeners.Remove(listener);
        }//closes UnRegisterListener method



    }//closes FloatVariable class
}//Closes Namespace declaration