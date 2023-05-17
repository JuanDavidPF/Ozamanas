using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace JuanPayan.Helpers
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraMiniMap : MonoBehaviour
    {
        
    
        [Tooltip("Vector2 action for movement")]
        [SerializeField] private InputActionReference _movementInput;

        [SerializeField] private InputActionReference _zoomInput;

        [SerializeField] private float anchorSpeed;
        [SerializeField] private float rotationSpeed;

        [SerializeField] private Vector2 yLimits;

        [SerializeField] private Vector2 xLimits;

        [SerializeField] private Vector2 zLimits;


        private Transform _t;

        private void Awake()
        {
            _t = transform;

        }//Closes Awake method

        private void OnEnable()
        {
            _movementInput.action.Enable();
            _zoomInput.action.Enable();
        }//Closes OnEnable method

        private void Update()
        {
            float zoom = _zoomInput.action.ReadValue<float>();
            Vector2 movementAxis = _movementInput.action.ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3(movementAxis.x, 0, movementAxis.y).normalized;
            Vector3 newPosition = _t.position;

            newPosition += transform.up * movementAxis.y * Time.deltaTime * anchorSpeed;
            newPosition += transform.right * movementAxis.x * Time.deltaTime * anchorSpeed;
            newPosition += transform.forward * zoom * Time.deltaTime * anchorSpeed;

            newPosition = new Vector3(Mathf.Clamp(newPosition.x,xLimits.x,xLimits.y),Mathf.Clamp(newPosition.y,yLimits.x,yLimits.y),Mathf.Clamp(newPosition.z,zLimits.x,zLimits.y));

            _t.position = newPosition;


        }//Closes UpdatePosition method
        private void OnDisable()
        {
            _movementInput.action.Disable();
            _zoomInput.action.Disable();

        }//Closes OnDisable method
    }
}
