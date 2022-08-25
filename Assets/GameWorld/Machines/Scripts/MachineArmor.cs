using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Machines
{

public class MachineArmor : MonoBehaviour
{
    [Header("Armor")]
        [Space(15)]
        [SerializeField] private int _armorPoints = 1;
        public int armorPoints
        {
            get { return _armorPoints; }
            set
            {
                _armorPoints = value;
            }
        }
        [Space(15)]

        [SerializeField] public UnityEvent OnMachineDestroyedEvent;
        [SerializeField] public UnityEvent OnMachineDamaged;
        [SerializeField] public UnityEvent OnMachineGainArmor;
        public event Action<int> OnDamageTaken;

        void Awake()
        {
            
        }
        
        void OnEnable()
        {
            
        }//Closes OnEnable method
        
            public void GainArmor(int armor)
        {
            armorPoints += armor;

            OnMachineGainArmor?.Invoke();
        }


        public void TakeDamage(int damageAmount)
        {

            armorPoints = armorPoints - damageAmount < 0 ? 0 : armorPoints - damageAmount;
            OnDamageTaken?.Invoke(damageAmount);

            if (armorPoints > 0) OnMachineDamaged?.Invoke();
            else
            {
                Destroy();
            }
        }

         public void Destroy()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            OnMachineDestroyedEvent?.Invoke();
        }
}
}
