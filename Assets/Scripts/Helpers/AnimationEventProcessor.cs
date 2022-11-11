using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventProcessor : MonoBehaviour
{

    [Serializable]
    struct AnimationEventProcessorType
    {
        public string key;
        public UnityEvent processes;
    }

    [SerializeField] List<AnimationEventProcessorType> processes = new List<AnimationEventProcessorType>();

    Dictionary<string, UnityEvent> processesAtlas = new Dictionary<string, UnityEvent>();


    private void Awake()
    {

        foreach (var item in processes)
        {
            if (!processesAtlas.ContainsKey(item.key)) processesAtlas.Add(item.key, item.processes);
        }
    }//Closes Awake method

    public void ExecuteAnimationEventProcessor(string key)
    {
        UnityEvent process = null; processesAtlas.TryGetValue(key, out process);
        process?.Invoke();
    }//closes ExecuteAnimationEventProcessor method



}//Closes AnimationEventProcessor class
