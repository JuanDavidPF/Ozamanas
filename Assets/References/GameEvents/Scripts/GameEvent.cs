using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JuanPayan.References
{

    [CreateAssetMenu(menuName = "References/Events/GameEvent", fileName = "new GameEvent")]
    public class GameEvent : ScriptableObject
    {

        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Invoke()
        {
            foreach (var listener in new List<GameEventListener>(listeners))
            {
                if (!listener) listeners.Remove(listener);
                listener.Behaviour();
            }
        }//Close Invoke method

        public void AddListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener)) listeners.Add(listener);
        }//Closes AddListener method

        public void RemoveListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }//Closes AddListener method

    }//Closes GameEvent class
}//Closes Namespace declaration