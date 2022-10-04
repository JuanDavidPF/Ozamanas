using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Ozamanas.Machines
{

    [RequireComponent(typeof(MachineAttributes))]
    public class MachineArmor : MonoBehaviour
    {
        private Transform _t;
        [Header("Armor")]
        [Space(15)]
        [SerializeField] private int m_armorPoints = 1;

        public int armorPoints
        {
            get { return m_armorPoints; }
            set
            {

                m_armorPoints = value;
                OnArmorChanged?.Invoke(value);
            }
        }
        [SerializeField] private int maxArmorPoints = 1;
        [SerializeField] private bool doubleDamage = false;
        [SerializeField] private bool invulnerable = false;
        private MachineAttributes machineAttributes;

        [SerializeField] private GameObject replacement;

        private bool broken = false;

        [SerializeField] private float lifeSpan = 2f;
        [Range(100, 1000)]
        [SerializeField] private float explosionPower = 2f;

        [Space(15)]

        [SerializeField] public UnityEvent OnMachineDamaged;
        [SerializeField] public UnityEvent OnMachineGainArmor;
        [SerializeField] public UnityEvent<int> OnArmorChanged;
        [SerializeField] public UnityEvent OnMachineDisarm;



        void Awake()
        {
            _t = transform;
            machineAttributes = GetComponent<MachineAttributes>();

        }
        private void Start()
        {
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
