using System.Collections.Generic;
using DG.Tweening;
using JuanPayan.References;
using Ozamanas.Levels;
using Ozamanas.World;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Ozamanas.UI.WavesCounter
{

    [SelectionBase]
    [RequireComponent(typeof(Image))]
    public class WavesCounter : MonoBehaviour
    {
        Tween progressTween;



        [SerializeField] private TextMeshProUGUI waveTMP;
        [SerializeField] private Image progressBar;
        [SerializeField] private Gradient waveProgressColor;

        private void Start()
        {
            UpdateProgressBar();
            UpdateWaveCounter();
        }//Closes Awake method

        private void Update()
        {
            UpdateProgressBar();
        }//Closes Update method

        private void UpdateProgressBar()
        {

            if (!progressBar) return;
            progressBar.fillAmount = WavesManager.waveProgress;
            progressBar.color = waveProgressColor.Evaluate(WavesManager.waveProgress);
        }//Closes HandleProgressBarColor method

        public void UpdateWaveCounter()
        {
            if (!waveTMP) return;
            waveTMP.text = WavesManager.currentWave + "/" + WavesManager.wavesAmount;

        }//Closes UpdateWaveCounter method



    }//Closes WavesManager class
}//Closes namespace declaration