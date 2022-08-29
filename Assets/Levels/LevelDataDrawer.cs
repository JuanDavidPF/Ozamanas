using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Ozamanas.Levels
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LevelData levelData = (LevelData)target;

            GUILayout.Space(20);
            if (GUILayout.Button("Select Level", GUILayout.Height(30)))
            {
                levelData.SelectLevel();

            }
        }
    }//Closes GameEvent class
}//Closes Namespace declaration