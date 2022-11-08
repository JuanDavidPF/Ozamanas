using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuanPayan.Helpers
{
    public class SceneUnloader : MonobehaviourEvents
    {
        [SerializeField] private string sceneToUnload;
    

        public override void Behaviour()
        {
            if (SceneManager.GetSceneByName(sceneToUnload).isLoaded) SceneManager.UnloadSceneAsync(sceneToUnload);

         
        }//Closes Behaviour method



    }//Closes SceneSwitcher class
}//Closes namespace declaration