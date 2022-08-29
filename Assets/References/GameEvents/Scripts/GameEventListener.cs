using System.Collections;
using System.Collections.Generic;
using JuanPayan.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace JuanPayan.References
{
    public class GameEventListener : MonobehaviourEvents
    {
        public enum InvokeMode
        {
            Unlimited,
            Limited
        }


        [Space(15)]
        public GameEvent[] events = new GameEvent[0];
        public InvokeMode mode;
        public int maxInvocations;
        private int m_invocations;

        public UnityEvent callback;

        protected override void Awake()
        {
            base.Awake();
            StartListening();
        }//Closes Awake method

        public override void Behaviour()
        {

            callback?.Invoke();

            m_invocations++;
            if (mode == InvokeMode.Limited && m_invocations >= maxInvocations) StopListening();


        }//Closes Behaviour method

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopListening();
        }//Closes OnDestroy method


        public void StartListening()
        {
            foreach (var gameEvent in events)
            {
                if (!gameEvent) continue;
                gameEvent.AddListener(this);
            }
        }//Closes StartListening method

        public void StopListening()
        {
            foreach (var gameEvent in events)
            {
                if (!gameEvent) continue;
                gameEvent.RemoveListener(this);
            }
        }//Closes StopListenig method


    }//Closes GameEventListener class

}//Closes Namespace declaration