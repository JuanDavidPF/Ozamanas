using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuanPayan.Helpers
{

    public class SceneStackManager : MonoBehaviour
    {
        public static List<string> screenStack = new List<string>();
        private void Start()
        {

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                screenStack.Add(scene.name);
            }
            DontDestroyOnLoad(this.gameObject);
        }//Closes Start method

        private void OnDestroy()
        {
            screenStack.Clear();
        }

    }//closes SceneStackManager class
}//closes Namespace declaration
