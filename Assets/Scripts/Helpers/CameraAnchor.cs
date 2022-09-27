using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JuanPayan.Helpers
{
    public class CameraAnchor : MonoBehaviour
    {
        [Tooltip("Vector2 action for rotation movement")]
        [SerializeField] private InputActionReference _rotationInput;

        [Tooltip("Vector2 action for movement")]
        [SerializeField] private InputActionReference _movementInput;

        // Start is called before the first frame update

        [SerializeField] private float anchorSpeed;
        [SerializeField] private float rotationSpeed;
        private Transform _t;

        private void Awake()
        {
            _t = transform;

        }//Closes Awake method

        private void OnEnable()
        {

            _movementInput.action.Enable();
            _rotationInput.action.Enable();
        }//Closes OnEnable method




        private void Update()
        {
            Vector2 movementAxis = _movementInput.action.ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3(movementAxis.x, 0, movementAxis.y).normalized;


            Vector3 newPosition = _t.position;



            transform.Rotate(new Vector3(0, _rotationInput.action.ReadValue<Vector2>().x, 0) * rotationSpeed * Time.deltaTime, Space.World);


            _t.position += transform.forward * movementAxis.y * Time.deltaTime * anchorSpeed;
            _t.position += transform.right * movementAxis.x * Time.deltaTime * anchorSpeed;





        }//Closes UpdatePosition method
        private void OnDisable()
        {

            _movementInput.action.Disable();
            _rotationInput.action.Disable();
        }//Closes OnDisable method
    }//Closes CameraAnchor class
}//Closes Namespace declaration
