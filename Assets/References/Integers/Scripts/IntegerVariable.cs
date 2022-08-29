using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JuanPayan.References
{
    [CreateAssetMenu(fileName = ("newIntegerVariable"), menuName = ("References/Integer/Integer Variable"))]
    public class IntegerVariable : ScriptableObject
    {

        private List<IntegerListener> listeners = new List<IntegerListener>();

        [SerializeField] private int _value;
        public int value { get { return _value; } set { _value = value; OnChange(); } }


        public void OnChange()
        {
            foreach (IntegerListener listener in listeners)
            {
                if (!listener) { Unlisten(listener); continue; }
                listener.Behaviour();
            }
        }//closes OnChange() method

        public void Listen(IntegerListener listener)
        {
            if (listeners.Contains(listener)) return;
            listeners.Add(listener);
        }//closes Listen method

        public void Unlisten(IntegerListener listener)
        {
            listeners.Remove(listener);
        }//closes Unlisten method


    }//Closes IntegerVariable Class
}//Closes Namespace declaration
