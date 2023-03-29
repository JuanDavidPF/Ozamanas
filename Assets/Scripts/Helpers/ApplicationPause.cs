using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JuanPayan.Helpers
{
    public class ApplicationPause : MonoBehaviour
    {
        public UnityEvent OnWinningLevel;
        public UnityEvent OnLoseLevel;


        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.pKey.wasPressedThisFrame )
            {
                 Debug.Break();
            }

            if (UnityEngine.InputSystem.Keyboard.current.lKey.wasPressedThisFrame )
            {
                 OnLoseLevel?.Invoke();
            }

            if (UnityEngine.InputSystem.Keyboard.current.oKey.wasPressedThisFrame )
            {
                 OnWinningLevel?.Invoke();
            }
        }
    }
}