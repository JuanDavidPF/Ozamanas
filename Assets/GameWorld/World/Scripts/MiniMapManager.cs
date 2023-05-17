using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Ozamanas.Machines;
using Ozamanas.Extenders;

namespace Ozamanas.World
{
    public class MiniMapManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference miniMapAction;

        [SerializeField] private CinemachineVirtualCamera rtsCamera;

         [SerializeField] private CinemachineVirtualCamera miniMapCamera;


         private bool miniMapActive = false;

        public bool MiniMapActive { get => miniMapActive; set => miniMapActive = value; }

        private void OnEnable()
        {
            miniMapAction.action.Enable();
            miniMapAction.action.performed += _ => SwitchState();

        }

        private void OnDisable()
        {
            miniMapAction.action.Disable();
            miniMapAction.action.performed -= _ => SwitchState();
        }

        private void SwitchState()
        {
            if(!rtsCamera || !miniMapCamera) return;

            if(MiniMapActive) SwitchToGameplay();
            else SwitchToMinimap();
            
            MiniMapActive = !MiniMapActive;
        }

        private void SwitchToMinimap()
        {
            Vector3 pos =  new Vector3(rtsCamera.transform.position.x,miniMapCamera.transform.position.y,rtsCamera.transform.position.z);
            miniMapCamera.transform.position = pos;
            rtsCamera.Priority = 0;
            miniMapCamera.Priority = 1;
            
          
           
        }

        private void SwitchToGameplay()
        {
            rtsCamera.Priority = 1;
            miniMapCamera.Priority = 0;

         
        }

   
    }
}
