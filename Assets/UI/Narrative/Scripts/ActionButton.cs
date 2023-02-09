using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Tags;

namespace Ozamanas.UI
{
    public class ActionButton : MonoBehaviour
    {
                [SerializeField] private ActionButtonType actionButton;

                void Start()
                {
                    gameObject.SetActive(false);
                }

                public void CheckActionButtonType(ActionButtonTypes type)
                {
                   switch(actionButton)
                   {
                    case ActionButtonType.Continue:
                    
                    if(type.HasFlag(ActionButtonTypes.Continue)) gameObject.SetActive(true);
                    break;
                    case ActionButtonType.Exit:
                    Debug.Log("Continue");
                    if(type.HasFlag(ActionButtonTypes.Exit)) gameObject.SetActive(true);
                    break;
                    case ActionButtonType.Play:
                    if(type.HasFlag(ActionButtonTypes.Play)) gameObject.SetActive(true);
                    break;
                    case ActionButtonType.Restart:
                    if(type.HasFlag(ActionButtonTypes.Restart)) gameObject.SetActive(true);
                    break;
                    case ActionButtonType.ToCamp:
                    if(type.HasFlag(ActionButtonTypes.ToCamp)) gameObject.SetActive(true);
                    break;
                    case ActionButtonType.ToCodex:
                    if(type.HasFlag(ActionButtonTypes.ToCodex)) gameObject.SetActive(true);
                    break;
                    case ActionButtonType.ToFire:
                    if(type.HasFlag(ActionButtonTypes.ToFire)) gameObject.SetActive(true);
                    break;
                    case ActionButtonType.ToSky:
                    if(type.HasFlag(ActionButtonTypes.ToSky)) gameObject.SetActive(true);
                    break;
                    
                   }
                   
                }
    }
}
