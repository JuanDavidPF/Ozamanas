using UnityEngine;
using UnityEngine.Events;

namespace Ozamanas.Machines
{

    [RequireComponent(typeof(MachineAttributes))]
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
        
         [SerializeField] private int maxArmorPoints = 1;
         [SerializeField] private bool doubleDamage = false;
         [SerializeField] private bool invulnerable = false;
         private MachineAttributes machineAttributes;
        
        [Space(15)]

        [SerializeField] public UnityEvent OnMachineDestroyedEvent;
        [SerializeField] public UnityEvent OnMachineDamaged;
        [SerializeField] public UnityEvent OnMachineGainArmor;
        [SerializeField] public UnityEvent OnMachineDisarm;



        void Awake()
        {
            machineAttributes = GetComponent<MachineAttributes>();
            armorPoints = machineAttributes.GetMachineArmorPoints();
            maxArmorPoints = machineAttributes.maxArmorPoints;
        }
        
        void OnEnable()
        {
            
        }//Closes OnEnable method
        
        public void RestoreOriginalValues()
        {
            doubleDamage = false;
            invulnerable = false;
        }

        public void RepairMachine()
        {
            armorPoints++;
            armorPoints = Mathf.Clamp(armorPoints,1,maxArmorPoints);   
            OnMachineGainArmor?.Invoke();
        }

        public void DisarmMachine()
        {
            armorPoints--;
            armorPoints = Mathf.Clamp(armorPoints,1,maxArmorPoints);   
            OnMachineDisarm?.Invoke();
        }

        public void SetInvulnerable()
        {
            invulnerable = true;
        }
        public void SetDoubleDamageOn()
        {
            doubleDamage = true;
        }
        public void TakeDamage(int damageAmount)
        {
            if(invulnerable) return;

            int currentDamage = damageAmount;

             if (doubleDamage) currentDamage = currentDamage*2;

            armorPoints = armorPoints - currentDamage;
            armorPoints = Mathf.Clamp(armorPoints,0,maxArmorPoints);   

            if (armorPoints > 0) OnMachineDamaged?.Invoke();
            else Destroy();

        }

        public void Destroy()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            OnMachineDestroyedEvent?.Invoke();
        }
}
}
