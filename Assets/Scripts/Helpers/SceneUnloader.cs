using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ozamanas.Tags;

namespace JuanPayan.Helpers
{
    public class SceneUnloader : MonobehaviourEvents
    {
        [SerializeField] private Scenes sceneToUnload;

        public Scenes SceneToUnload { get => sceneToUnload; set => sceneToUnload = value; }

        public override void Behaviour()
        {
            Debug.Log("Attempting to unload scene: " + SceneToUnload);
            
            if (!SceneManager.GetSceneByName(SceneToUnload.ToString()).isLoaded) return;

            SceneStackManager.screenStack.Remove(SceneToUnload.ToString());
            SceneManager.UnloadSceneAsync(SceneToUnload.ToString());

        }//Closes Behaviour method



    }//Closes SceneSwitcher class
}//Closes namespace declaration