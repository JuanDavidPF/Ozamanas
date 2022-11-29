 #if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ozamanas.SaveSystem
{
    [CustomEditor(typeof(PlayerData))]
    public class PlayerDataDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PlayerData playerData = (PlayerData)target;

            GUILayout.Space(20);
            if (GUILayout.Button("Select Player Save", GUILayout.Height(40))) playerData.SelectData();
            if (GUILayout.Button("Reset Save", GUILayout.Height(40))) playerData.ClearData();


        }//Closes OnInspectorGUI method

    }//Closes PlayerDataDrawer
}//Closes namespace declaration
#endif
