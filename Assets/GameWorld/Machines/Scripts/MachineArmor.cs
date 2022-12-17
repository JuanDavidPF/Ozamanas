using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


namespace Ozamanas.Machines
{

    [RequireComponent(typeof(HumanMachine))]
    public class MachineArmor : MonoBehaviour
    {
        private Transform _t;

       private int m_armorPoints = 1;

        private HumanMachine machine; 
        public int armorPoints
        {
            get { return m_armorPoints; }
            set
            {

                m_armorPoints = value;
                OnArmorChanged?.Invoke(value);
            }
        }
         private int maxArmorPoints = 1;
         private bool doubleDamage = false;
        private bool invulnerable = false;

        private GameObject replacement;

        private bool broken = false;

        private float lifeSpan = 2f;
        private float explosionPower = 2f;

        [Space(15)]
        [Title("Events")]
        [SerializeField] public UnityEvent OnMachineDamaged;
        [SerializeField] public UnityEvent OnMachineGainArmor;
        [SerializeField] public UnityEvent<int> OnArmorChanged;
        [SerializeField] public UnityEvent OnMachineDisarm;

        [SerializeField] public UnityEvent OnMachineDestroyed;

        void Awake()
        {
            machine = GetComponent<HumanMachine>();
            _t = transform;

        }
        private void Start()
        {
            if(!machine || !machine.Machine_token) return;

            armorPoints = machine.Machine_token.armorPoints;
            maxArmorPoints = machine.Machine_token.maxArmorPoints;
            lifeSpan = machine.Machine_token.lifeSpan;
            explosionPower = machine.Machine_token.explosionPower;
            replacement = machine.Machine_token.destroyedMachine;
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
            armorPoints = Mathf.Clamp(armorPoints, 1, maxArmorPoints);
            OnMachineGainArmor?.Invoke();
        }

        public void DisarmMachine()
        {
            armorPoints--;
            armorPoints = Mathf.Clamp(armorPoints, 1, maxArmorPoints);
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
            if (invulnerable) return;

            int currentDamage = damageAmount;

            if (doubleDamage) currentDamage = currentDamage * 2;

            armorPoints = armorPoints - currentDamage;
            armorPoints = Mathf.Clamp(armorPoints, 0, maxArmorPoints);

            if (armorPoints > 0) OnMachineDamaged?.Invoke();
            else Destroy();

        }

        public void Destroy()
        {
            if (broken) return;

            if (replacement)
            {
                broken = true;
                GameObject temp = Instantiate(replacement, transform.position, transform.rotation);
                temp.SetActive(true);
                temp.GetComponent<MachineOnDestroy>().DestructableSetup(lifeSpan);
                Rigidbody[] rbs = temp.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in rbs)
                {
                    rb.AddExplosionForce(explosionPower, temp.transform.position, 10f, 3F);
                }
            }
            machine.Machine_status = Tags.MachineState.Destroyed;
            OnMachineDestroyed?.Invoke();
            gameObject.SetActive(false);
            Destroy(gameObject);

        }




        private void Update()
        {
            //Fall to void
            if (_t.position.y <= -1) Destroy();
        }

    }
}
