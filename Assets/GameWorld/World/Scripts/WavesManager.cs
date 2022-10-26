using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JuanPayan.References;
using Ozamanas.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.World
{
    public class WavesManager : MonoBehaviour
    {
        Tween progressTween;

        public static int wavesAmount;
        public static float wavesDuration;
        private static int m_currentWave;
        public static int currentWave
        {
            get { return m_currentWave; }
            set
            {
                m_currentWave = value;

            }
        }

        private static float m_waveProgress;
        public static float waveProgress
        {
            get { return m_waveProgress; }
            set
            {
                m_waveProgress = value;


            }
        }

        [SerializeField] private LevelReference levelSelected;
        [SerializeField] private IntegerReference initialDelay;


        [Space(15)]
        [Header("On New Wave event")]
        [SerializeField] private UnityEvent<int> OnNewWave;


        private void Awake()
        {
            currentWave = 0;
            waveProgress = 0;

            if (!levelSelected || !levelSelected.level) return;

            wavesAmount = levelSelected.level.wavesAmount.value;
            wavesDuration = levelSelected.level.wavesCooldown.value;

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
                   () => waveProgress, x => waveProgress = x, 1f, wavesDuration)
                   .SetEase(Ease.Linear)
                   .OnComplete(StartNextWave);
        }//Closes HandleWaveDuration method



        private bool WavesRemaining()
        {
            if (!levelSelected || !levelSelected.level) return false;
            if (currentWave >= levelSelected.level.wavesAmount.value) return false;
            return true;
        }//Closes WavesRemainig method

    }//Closes WavesManager class
}//Closes Namespace declaration
