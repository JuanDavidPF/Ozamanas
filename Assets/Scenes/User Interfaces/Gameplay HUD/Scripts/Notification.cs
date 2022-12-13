using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stem;
using UnityEngine.Localization;

namespace Ozamanas
{
[CreateAssetMenu(menuName = "ScriptableObjects/Notifications/Notification", fileName = "new Notification")]
public class Notification : ScriptableObject
{
    [SoundID, SerializeField]
    internal Stem.ID sound = Stem.ID.None;
    public LocalizedString notification = new LocalizedString();
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    public float lifeTime = 3f;
    public Color textColor = Color.white;

}
}
