using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ozamanas.Forces;


namespace Ozamanas.GameScenes
{

public class Constellation : MonoBehaviour
{
    [SerializeField] private List<GameObject> stars;
    [SerializeField] private Image _constellationImage;


    public Image constellationImage { get => _constellationImage; }

    private ForceData _data;
    public void SetData(ForceData data)
    {
        if (!data) return;
        _data = data;
        UpdateProgress();
    }//Closes SetData method
    public void UpdateProgress()
    {
        /*if (!_data) return;
        for (int i = 0; i < stars.Count; i++)
        {
            ConstellationStar star = stars[i];
            if (!star) break;
            star.Toogle(i < _data.unlockProgress);

        }

        _data.isUnlocked = _data.unlockProgress >= stars.Count;*/

    }//Closes SetProgress


}//Closes Constellation class
}