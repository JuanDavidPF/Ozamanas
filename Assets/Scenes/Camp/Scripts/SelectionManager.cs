using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
   [SerializeField] private string selectableTag = "Selectable";
    private Transform _selection;
    
    private void Update()
    {
        if (_selection != null)
        {
            _selection = null;
        }
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                Debug.Log("clcik");
                _selection = selection;
            }
        }
    }
}
