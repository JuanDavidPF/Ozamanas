using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
using Stem;

namespace Ozamanas.GameScenes
{

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private SwipeController graphicsSettings;
    [SerializeField] private FullScreenController fullScreenController;
     [SerializeField] private MusicBank musicBank;
      [SerializeField] private SoundBank soundBank;

    private bool active = false;

    void Awake()
    {
        if(!gameSettings) return;
         StartCoroutine( SetLocale(gameSettings.localeId));

         if(!languageDropdown) return;
        languageDropdown.value =    gameSettings.localeId;

         if(!graphicsSettings) return;
        graphicsSettings.SetKeyName(gameSettings.graphicsSettings);

         if(!fullScreenController) return;
        fullScreenController.ActivateFullScreen(gameSettings.fullScreen);

    }

    public void ChangeLocale( int localeId)
    {
        if (active) return;
        StartCoroutine( SetLocale(localeId));

    }
   IEnumerator SetLocale(int localeId)
   {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        active = false;
   }

   public void SaveSettings()
   {
        if(!languageDropdown) return;
        gameSettings.localeId = languageDropdown.value;

        if(!graphicsSettings) return;
        gameSettings.graphicsSettings = graphicsSettings.GetKeyName();

        if(!fullScreenController) return;
        gameSettings.fullScreen = fullScreenController.FullScreen;
   }

   public void ChangeMusicVolumen(float volumen)
   {
        if(!musicBank) return;
        musicBank.Players[0].Volume = volumen;
   }

   public void ChangeEffectsVolumen(float volumen)
   {    
        if(!soundBank) return;
        soundBank.Buses[0].Volume = volumen;
   }
}
}
