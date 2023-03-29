using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;

namespace Ozamanas.GameScenes
{
    public class StartGameSettings : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private List<RenderPipelineAsset> qualityLevels;
       
       public void LoadGameSettings()
       {
            if(!gameSettings) return;

            StartCoroutine( SetLocale(gameSettings.localeId));

            QualitySettings.SetQualityLevel(gameSettings.graphicsSettings);
            QualitySettings.renderPipeline = qualityLevels[gameSettings.graphicsSettings];

       }

        IEnumerator SetLocale(int localeId)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        }
    }
}
