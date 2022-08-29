using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JuanPayan.References
{
    [CreateAssetMenu(fileName = ("newStringVariable"), menuName = ("References/String/String variable"))]
    public class StringVariable : ScriptableObject
    {

        private List<StringListener> listeners = new List<StringListener>();

        [SerializeField] private string _value;
        public string value { get { return _value; } set { _value = value; OnChange(); } }


        public void OnChange()
        {
            foreach (StringListener listener in listeners)
            {
                if (!listener) { Unlisten(listener); continue; }
                listener.Behaviour();
            }
        }//closes Invoke method

        public void Listen(StringListener listener)
        {
            if (listeners.Contains(listener)) return;
            listeners.Add(listener);
        }//closes RegisterListener method

        public void Unlisten(StringListener listener)
        {
            listeners.Remove(listener);
        }//closes UnRegisterListener method



    }//closes FloatVariable class
}//Closes Namespace declaration