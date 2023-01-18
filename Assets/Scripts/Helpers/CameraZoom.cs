using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Ozamanas.Extenders;
using DG.Tweening;

namespace JuanPayan.Helpers
{

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraZoom : MonoBehaviour
    {

        [Tooltip("Vector2 action for rotation movement")]
        [SerializeField] private InputActionReference _scrollInput;

        private CinemachineVirtualCamera _vcamera;
        private CinemachineOrbitalTransposer _orbit;
        [SerializeField] private float zoom = 50f;
        [SerializeField] private float zoomSteps = 5f;
        [SerializeField] private Vector2 yLimits;
        [SerializeField] private Vector2 zLimits;
        [SerializeField] private CursorLockMode lockMode;
        private void Awake()
        {
            _vcamera = GetComponent<CinemachineVirtualCamera>();
            _orbit = _vcamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            UpdateCameraZoom(zoom);
            Cursor.lockState = lockMode;
        }//Closes Awake method

        private void OnEnable()
        {
            _scrollInput.action.Enable();
            _scrollInput.action.started += UpdateScroll;
        }//Closes OnEnable method


        private void UpdateScroll(InputAction.CallbackContext context)
        {

            zoom = Mathf.Clamp(zoom + context.action.ReadValue<float>() * zoomSteps, 0, 100);

            UpdateCameraZoom(zoom);
        }//Closes UpdateScroll method 

        private void OnDisable()
        {
            _scrollInput.action.Disable();
            _scrollInput.action.started -= UpdateScroll;
        }//Closes OnDisable method

        
        public void UpdateCameraZoom(float value)
        {


            DOTween.To(() => _orbit.m_FollowOffset.y, y => _orbit.m_FollowOffset.y = y, value.Map(0, 100, yLimits.x, yLimits.y), .3f);
            DOTween.To(() => _orbit.m_FollowOffset.z, z => _orbit.m_FollowOffset.z = z, value.Map(0, 100, zLimits.x, zLimits.y), .3f);



        }//Closes UpdateCameraZoom method

    }//closes Camerazoom class
}//Closes namespace declaration
