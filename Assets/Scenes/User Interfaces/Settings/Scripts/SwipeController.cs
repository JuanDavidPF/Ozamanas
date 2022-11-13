using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Rendering;

public class SwipeController : MonoBehaviour
{
    [SerializeField] private string tableName = "Settings";
    [SerializeField] private List<string> keyName;
    [SerializeField] private LocalizeStringEvent localizedStringEvent;
    [SerializeField] private int graphicsSettings;
    [SerializeField] private List<RenderPipelineAsset> qualityLevels;
    

    public void OnLeftButtonPress()
    {
        if(graphicsSettings == 0) return;
        graphicsSettings--;
        graphicsSettings = Mathf.Clamp(graphicsSettings,0,keyName.Count-1);
        localizedStringEvent.StringReference.SetReference(tableName, keyName[graphicsSettings]);
        QualitySettings.SetQualityLevel(graphicsSettings);
        QualitySettings.renderPipeline = qualityLevels[graphicsSettings];
    }

    public void OnRightButtonPress()
    {
        if (graphicsSettings == qualityLevels.Count-1) return;
        graphicsSettings++;
        graphicsSettings = Mathf.Clamp(graphicsSettings,0,keyName.Count-1);
        localizedStringEvent.StringReference.SetReference(tableName, keyName[graphicsSettings]);
        QualitySettings.SetQualityLevel(graphicsSettings);
        QualitySettings.renderPipeline = qualityLevels[graphicsSettings];
    }

    public void SetKeyName(int i)
    {
        graphicsSettings=i;
        graphicsSettings = Mathf.Clamp(graphicsSettings,0,keyName.Count-1);
        localizedStringEvent.StringReference.SetReference(tableName, keyName[graphicsSettings]);
        QualitySettings.SetQualityLevel(graphicsSettings);
        QualitySettings.renderPipeline = qualityLevels[graphicsSettings];
    }

    public int GetKeyName()
    {
        return graphicsSettings;
    }
}
