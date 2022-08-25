using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace JuanPayan.References.GameEvents
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GameEvent gameEvent = (GameEvent)target;

            if (GUILayout.Button("Invoke Event", GUILayout.Height(40))) gameEvent.Invoke();


        }
    }//Closes GameEvent class
}//Closes Namespace declaration