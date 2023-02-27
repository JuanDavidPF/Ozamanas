using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Helpers
{
    public class ApplicationPause : MonoBehaviour
    {
        
        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.pKey.wasPressedThisFrame )
            {
                 Debug.Break();
            }
        }
    }
}
