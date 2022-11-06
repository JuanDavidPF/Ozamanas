using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;

namespace Ozamanas.Forces
{

    [RequireComponent(typeof(Animator))]
    public class MultipleOffensiveBird : AncientForce
    {


        [Space(15)]
        [Header("Visuals Setup")]

        [SerializeField] private Transform attackIndicator;
        [SerializeField] private Transform birdPosition;
        [SerializeField] private List<Transform> thunderPositions;
        [SerializeField] private GameObject thunder;

        List<Machines.MachineArmor> machinesAffected = new List<Machines.MachineArmor>();
        private Tween birdTween;

        private Animator animator;
        [SerializeField] private float waitingTime = 0.2f;

        [Space(15)]
        [Header("Attack Setup")]
        [Range(1, 7)]
        [SerializeField] private int totalThunders;
        [SerializeField] private int damageAmount;
        [SerializeField] private Vector3 elevationForce;
        [SerializeField] private Vector3 torqueForce;
        [SerializeField] private float posY = 0f;

        protected override void Awake()
        {
            base.Awake();
            animator = gameObject.GetComponent<Animator>();
            thunderPositions.Shuffle(7);

        }//Closes Awake method



        public override void FirstPlacement()
        {

            base.FirstPlacement();

            animator.SetTrigger("Release");

            birdPosition.position = new Vector3(birdPosition.position.x,posY,birdPosition.position.z);

            ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));


            StartCoroutine(InstantiateThunder());

        }//Closes FirstPlacement method

        IEnumerator InstantiateThunder()
        {
            for (int i = 0; i < totalThunders; i++)
            {
                yield return new WaitForSeconds(waitingTime);
                Instantiate(thunder, thunderPositions[i]);
            }

            Destroy(gameObject);
        }

        private void Update()
        {
            if (isPlaced || !attackIndicator) return;


            Ray AOERay = new Ray(transform.position, -transform.up);


            foreach (var hit in Physics.RaycastAll(AOERay))
            {
                if (hit.transform.tag != "Cell") return;

                attackIndicator.position = new Vector3(hit.point.x, posY, hit.point.z);
                break;
            }

        }//Closes Update Method

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (birdTween != null) birdTween.Kill();
        }//Closes OnDestroy method


        private void OnCollisionEnter(Collision other)
        {

            if (!isPlaced || other.transform.tag != "Machine") return;

            if (other.transform.TryGetComponent(out Machines.MachineArmor machine))
            {
                AttackMachine(machine);
            }

        }//Closes OnCollisionEnter method
        private void ActivateTraits(Cell origin)
        {
            if (!origin) return;

            foreach (var cell in origin.GetCellsOnRange(traitRange))
            {
                if (!cell) continue;

                foreach (var trait in traits)
                {
                    if (!trait) continue;
                    cell.AddTraitToMachine(trait);
                }
            }


        }//Closes ActivateTraits method

        private void AttackMachine(Machines.MachineArmor machine)
        {
            if (!machine || machine.tag != "Machine") return;

            machine.TakeDamage(damageAmount);
            machinesAffected.Remove(machine);



            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.SetPhysical();
                if (physics.rb)
                {

                    physics.rb.AddForce(machine.transform.up + elevationForce, ForceMode.Impulse);
                    physics.rb.AddTorque(torqueForce, ForceMode.Impulse);
                }
            }

        }//Closes AttemptMachineDamage method







    }
}
