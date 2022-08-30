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


        [Space(15)]
        [Header("Generation config")]
        [SerializeField] private IntegerReference generationAmount;


        [SerializeField] private FloatReference generationOffset;
        [SerializeField] private FloatReference generationCooldown;


        [Space(15)]
        [Header("Lifetime config")]
        public LifetimeConfig lifetime;
        public IntegerReference maxCycles;
        [HideInInspector] public int currentCycle;



        [Space(15)]
        [Header("Generation Events")]
        public UnityEvent OnEnergyGenerated;
        public UnityEvent OnEnergyDepleated;


        private YieldInstruction offset;
        private YieldInstruction cooldown;

        private IEnumerator generationCoroutine;

        private void Awake()
        {

            generationCoroutine = HandleGeneration();


            offset = new WaitForSeconds(generationOffset.value);
            cooldown = new WaitForSeconds(generationCooldown.value);
        }//Closes Awake method



        private IEnumerator HandleGeneration()
        {
            while (lifetime == LifetimeConfig.Unlimited ||
             (lifetime == LifetimeConfig.Limited && currentCycle < maxCycles.value))
            {
                yield return cooldown;

                currentCycle++;
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

            StartCoroutine(generationCoroutine);
        }//Closes ResumeGeneration method

        public void StopGeneration()
        {
            StopCoroutine(generationCoroutine);
        }//Closes ResumeGeneration method

        public void Generate()
        {
            if (!energyOrb) return;
            Instantiate(energyOrb, offsetPosition + transform.position.ToFloat3(), Quaternion.identity);
        }//Closes Generate method

    }//Closes EnergyGenerator
}//Closes namespace declaration
