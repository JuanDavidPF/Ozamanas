using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ozamanas.GameScenes
{

[CreateAssetMenu(menuName = "ScriptableObjects/Settings", fileName = "GameSettings")]
public class GameSettings: ScriptableObject
{
   public int localeId;
   public int graphicsSettings;
   public bool fullScreen;
}
}
