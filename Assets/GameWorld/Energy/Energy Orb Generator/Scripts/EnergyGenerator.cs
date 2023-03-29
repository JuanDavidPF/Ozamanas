using System.Collections;
using System.Collections.Generic;
using JuanPayan.References;
using Ozamanas.Energy;
using Ozamanas.Extenders;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Energy
{
    public class EnergyGenerator : MonoBehaviour
    {
        public enum LifetimeConfig
        {
            Unlimited,
            Limited,
        }

        [SerializeField] private EnergyOrb energyOrb;
        [SerializeField] private float3 offsetPosition;

        private Canvas gameplayCanvas;

        private  EnergyAbsorber absorber;

        private Camera cam;


        [Space(15)]
        [Header("Generation config")]
        [SerializeField] private IntegerReference generationAmount;


        [SerializeField] private FloatReference generationOffset;
        [SerializeField] private FloatReference generationCooldown;


        [Space(15)]
        [Header("Lifetime config")]
        public LifetimeConfig lifetime;
        public IntegerReference fullLevel;

        [SerializeField] private int m_currentLevel;
        public int currentLevel
        {
            get { return m_currentLevel; }
            set
            {
                m_currentLevel = value > 0 ? m_currentLevel = value : 0;

                OnEnergyLevelChanged?.Invoke(value);
                if (m_currentLevel == 0) OnEnergyDepleated?.Invoke();

            }
        }



        [Space(15)]
        [Header("Generation Events")]
        public UnityEvent<int> OnEnergyLevelChanged;
        public UnityEvent OnEnergyOrbGenerated;
        public UnityEvent OnEnergyDepleated;


        private YieldInstruction offset;
        private YieldInstruction cooldown;

        private Coroutine generationCoroutine;

        private void Awake()
        {
            offset = new WaitForSeconds(generationOffset.value);
            cooldown = new WaitForSeconds(generationCooldown.value);
            currentLevel = fullLevel.value;
            gameplayCanvas = FindObjectOfType<Canvas>();
            cam = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        }//Closes Awake method



        private IEnumerator HandleGeneration()
        {

            while (lifetime == LifetimeConfig.Unlimited ||
             (lifetime == LifetimeConfig.Limited && currentLevel > 0))
            {
                yield return cooldown;

                currentLevel--;
                for (int i = 0; i < generationAmount.value; i++)
                {
                    Generate();
                    yield return offset;
                }

            }
        }//Closes GenerateEnergy coroutine

        public void ResumeGeneration()
        {
            if (!energyOrb) return;
            if (generationCoroutine != null) StopGeneration();
            generationCoroutine = StartCoroutine(HandleGeneration());
        }//Closes ResumeGeneration method


        public void RestartGeneration()
        {
            if (!energyOrb) return;
            currentLevel = fullLevel.value;

            ResumeGeneration();
        }//Closes ResumeGeneration method

        public void StopGeneration()
        {
            if (generationCoroutine != null) StopCoroutine(generationCoroutine);
        }//Closes ResumeGeneration method

        public void Generate()
        {
            if (!energyOrb) return;

            if(!gameplayCanvas) return;

            if(!absorber) SetUpAbsorver();

            if(!absorber) return;

            EnergyOrb orb = Instantiate(energyOrb,gameplayCanvas.transform).GetComponent<EnergyOrb>();

            RectTransform CanvasRect=gameplayCanvas.GetComponent<RectTransform>();

            Vector2 ViewportPosition=cam.WorldToViewportPoint(transform.position);

             Vector2 WorldObject_ScreenPosition=new Vector2(
                ((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
                ((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));

            orb.GetComponent<RectTransform>().anchoredPosition=WorldObject_ScreenPosition;

            orb.Absorber = absorber;

            orb.leftTopPosition = new Vector2(0, cam.pixelHeight);

            orb.GoToEnergyAbsorber();

            OnEnergyOrbGenerated?.Invoke();
        }//Closes Generate method

        private void SetUpAbsorver()
        {
            absorber = FindObjectOfType<EnergyAbsorber>();
        }

    }//Closes EnergyGenerator
}//Closes namespace declaration
