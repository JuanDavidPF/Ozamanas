using System.Collections.Generic;
using DG.Tweening;
using JuanPayan.References;
using Ozamanas.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Ozamanas.UI.WavesManager
{

    [SelectionBase]
    [RequireComponent(typeof(Image))]
    public class WavesManager : MonoBehaviour
    {
        Tween progressTween;

        private static int m_wave;
        public static int wave { get { return m_wave; } }

        private int m_currentWave;
        private int currentWave
        {
            get { return m_currentWave; }
            set
            {
                m_currentWave = value; m_wave = value;
                UpdateWaveCounter();
            }
        }

        private float m_waveProgress;
        private float waveProgress
        {
            get { return m_waveProgress; }
            set
            {
                m_waveProgress = value;
                UpdateProgressBar();

            }
        }

        [SerializeField] private LevelReference levelSelected;
        [SerializeField] private IntegerReference initialDelay;


        [Space(15)]
        [Header("On New Wave event")]
        [SerializeField] private UnityEvent<int> OnNewWave;

        [Space(15)]
        [Header("UI elements")]

        [SerializeField] private TextMeshProUGUI waveTMP;
        [SerializeField] private Image progressBar;
        [SerializeField] private Gradient waveProgressColor;

        private void Awake()
        {
            currentWave = 0;
            waveProgress = 0;
        }//Closes Awake method;

        public void StartWaves()
        {
            if (!WavesRemaining()) return;

            progressTween = DOTween.To(
                   () => waveProgress, x => waveProgress = x, 1f, initialDelay.value)
                   .SetEase(Ease.Linear)
                   .OnComplete(StartNextWave);

        }//Closes StartWaves method;

        private void StartNextWave()
        {

            currentWave++;
            OnNewWave?.Invoke(currentWave);

            if (!WavesRemaining()) return;
            progressTween = DOTween.To(
                   () => waveProgress, x => waveProgress = x, 0f, .3f)
                   .OnComplete(HandleWaveCooldown);
        }//Closes NewWave method

        private void HandleWaveCooldown()
        {
            if (!WavesRemaining()) return;

            progressTween = DOTween.To(
                   () => waveProgress, x => waveProgress = x, 1f, levelSelected.level.wavesCooldown.value)
                   .SetEase(Ease.Linear)
                   .OnComplete(StartNextWave);
        }//Closes HandleWaveDuration method

        private void UpdateProgressBar()
        {

            if (!progressBar) return;
            progressBar.fillAmount = waveProgress;
            progressBar.color = waveProgressColor.Evaluate(waveProgress);
        }//Closes HandleProgressBarColor method

        private void UpdateWaveCounter()
        {
            if (!waveTMP || !levelSelected.level) return;
            waveTMP.text = currentWave + "/" + levelSelected.level.wavesAmount.value;

        }//Closes UpdateWaveCounter method


        private bool WavesRemaining()
        {
            if (!levelSelected || !levelSelected.level) return false;
            if (currentWave >= levelSelected.level.wavesAmount.value) return false;
            return true;
        }//Closes WavesRemainig method
    }//Closes WavesManager class
}//Closes namespace declaration