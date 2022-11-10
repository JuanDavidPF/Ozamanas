using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputKeyProcessor : MonoBehaviour
{


    [SerializeField] List<InputActionReference> inputs;

    [SerializeField] UnityEvent tasks;

    private void OnEnable()
    {

        foreach (var input in inputs)
        {

            input.action.Enable();
            input.action.started += OnKeyPressed;
        }
    }//Closes OnEnable method

    private void OnDisable()
    {
        foreach (var input in inputs)
        {
            input.action.Dispose();
            input.action.started -= OnKeyPressed;
        }
    }//Closes OnDisable method

    public void OnKeyPressed(InputAction.CallbackContext context)
    {
        if (context.performed) tasks.Invoke();

    }//Closes OnKeyPressed method

}//Closes InputKeyProcessor method
