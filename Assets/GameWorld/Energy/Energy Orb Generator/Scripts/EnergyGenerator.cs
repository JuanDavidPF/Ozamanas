using System.Collections;
using System.Collections.Generic;
using JuanPayan.CodeSnippets.HelperComponents;
using JuanPayan.Extenders;
using JuanPayan.References.Floats;
using JuanPayan.References.Integers;
using Ozamanas.Energy;
using Unity.Mathematics;
using UnityEngine;

public class EnergyGenerator : MonoBehaviour
{
    [SerializeField] private EnergyOrb energyOrb;
    [SerializeField] private float3 offsetPosition;


    [Space(15)]
    [Header("Generation config")]
    [SerializeField] private IntegerReference generationAmount;


    [SerializeField] private FloatReference generationOffset;
    [SerializeField] private FloatReference generationCooldown;


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
        while (true)
        {
            yield return cooldown;


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
