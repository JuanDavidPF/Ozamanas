    #if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace JuanPayan.References
{

    [CustomEditor(typeof(GameEvent))]
    public class GameEventDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GameEvent gameEvent = (GameEvent)target;

            if (GUILayout.Button("Invoke Event", GUILayout.Height(40))) gameEvent.Invoke();


        }//Closes OnInspectorGUI method

    }
    
    //Closes GameEvent class
}//Closes Namespace declaration
#endif