using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Levels;

namespace Ozamanas.World
{
    public class ClimateManager : MonoBehaviour
    {
        [SerializeField] private LevelReference levelSelected;

        void Start()
        {

           SetupSkyBox();
        }

        private void SetupSkyBox()
        {
           
            if(!levelSelected.level.skyBox) return;

            RenderSettings.skybox =levelSelected.level.skyBox;

            
        }

        
    }
}
