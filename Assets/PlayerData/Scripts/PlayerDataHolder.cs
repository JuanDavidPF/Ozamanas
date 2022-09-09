using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.SaveSystem
{
    [CreateAssetMenu(fileName = "New PlayerData holder", menuName = "PlayerData/Holder")]

    public class PlayerDataHolder : ScriptableObject
    {
        public PlayerData emptySaveState;
        [SerializeField] private PlayerData _data;
        public PlayerData data
        {
            get { return _data; }
            set
            {
                if (value == _data) return;
                _data = value;
                OnPlayerDataChanged?.Invoke(value);
            }
        }

        [HideInInspector] public UnityEvent<PlayerData> OnPlayerDataChanged;

        public void ClearData()
        {
            if (!_data) return;
            data.ClearData(emptySaveState);

        }//Closes ClearData method

    }//Closes PlayerDataHolder class
}//closes namespace declaration