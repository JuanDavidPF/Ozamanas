using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozamanas.Extenders;
using DG.Tweening;
using Ozamanas.Board;
using Ozamanas.Forest;

namespace Ozamanas.Forces
{
    public class AddForceMammal : AncientForce
    {
        private enum MammalForceMode
        {
            Push,
            Pull
        }

        private Transform AOETransform;
        [SerializeField] private MeshRenderer AOERenderer;
        [SerializeField] private MammalForceMode mode;
        private List<Machines.MachineArmor> machinesAffected = new List<Machines.MachineArmor>();

        private List<JungleTree> treesAffected = new List<JungleTree>();

        [SerializeField] private Vector3 elevationForce;
        [SerializeField] private Vector3 torqueForce;

        [SerializeField] private float mammalSpeed = 2f;
        [Range(2000, 10000)]
        [SerializeField] private int mammalForce = 1000;
        private Tween mammalTween;
        private Animator animator;

        [SerializeField] private GameObject pullArrows;
        [SerializeField] private GameObject pushArrows;
        protected override void Awake()
        {
            base.Awake();
            if (AOERenderer) AOETransform = AOERenderer.transform;

            if (mode == MammalForceMode.Push) pushArrows.SetActive(true);
            else pullArrows.SetActive(true);
            animator = GetComponent<Animator>();
        }//Closes Awake method

        public override void FirstPlacement()
        {

            base.FirstPlacement();

            AOERenderer.gameObject.SetActive(false);
            mammalTween = transform.DOMoveY(0, mammalSpeed, false).SetSpeedBased();


            mammalTween.OnComplete(() =>
            {
                ActivateTraits(Board.Board.GetCellByPosition(transform.position.ToFloat3().UnityToGrid()));
                animator.SetTrigger("OnRelease");
                
                
                 foreach (JungleTree tree in treesAffected.ToArray())
                {
                    if (tree) AddForceToTrees(tree);
                }

                foreach (var machine in machinesAffected.ToArray())
                {
                    if (!machine) continue;
                    AddForceToMachine(machine);                    
                }

               
            });

           

        }//Closes FirstPlacement method

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


        private void AddForceToTrees(JungleTree tree)
        {
             if (!tree || tree.tag != "Tree") return;

            
            GameObject destroyedTree = tree.DestroyTree();

            if(!destroyedTree) return;

      


            Vector3 force = tree.transform.position - gameObject.transform.position;
            force = force.normalized * mammalForce;
            if (mode == MammalForceMode.Pull) force = force * -1;
            
            Rigidbody[] rigidbodies = destroyedTree.GetComponentsInChildren<Rigidbody>();

                

            for(int i =0;i<rigidbodies.Length;i++)
            {
                rigidbodies[i].AddForce(force + elevationForce, ForceMode.Impulse);
                rigidbodies[i].AddTorque(torqueForce, ForceMode.Impulse);
            }

             treesAffected.Remove(tree);


        }
        private void AddForceToMachine(Machines.MachineArmor machine)
        {
           
            if (!machine || machine.tag != "Machine") return;

            machinesAffected.Remove(machine);

            if (machine.TryGetComponent(out Machines.MachinePhysicsManager physics))
            {
                physics.SetPhysical();
                if (physics.rb)
                {
                    Vector3 force = machine.transform.position - gameObject.transform.position;
                    force = force.normalized * mammalForce;
                    if (mode == MammalForceMode.Pull) force = force * -1;
                    physics.rb.AddForce(force + elevationForce, ForceMode.Impulse);
                    physics.rb.AddTorque(torqueForce, ForceMode.Impulse);
                }
            }

        }//Closes AttemptMachineDamage method

        private void OnTriggerEnter(Collider other)
        {

            if (isPlaced) return;
            
            if(other.tag =="Tree" && other.TryGetComponentInParent(out JungleTree tree))
            {
                if (!treesAffected.Contains(tree)) treesAffected.Add(tree);
            }

            if (other.tag == "Machine" && other.TryGetComponentInParent(out Machines.MachineArmor armor))
            {
                if (!machinesAffected.Contains(armor)) machinesAffected.Add(armor);

            }
            else return;

            UpdateAOEColor();
        }//Closes OnTriggerEnter method


        private void OnTriggerExit(Collider other)
        {
            if (isPlaced) return;

             if(other.tag =="Tree" && other.TryGetComponentInParent(out JungleTree tree))
            {
                 treesAffected.Remove(tree);
            }
            
            if (other.tag == "Machine" && other.TryGetComponentInParent(out Machines.MachineArmor armor))
            {
                machinesAffected.Remove(armor);

            }
            else return;

            UpdateAOEColor();
        }
        private void UpdateAOEColor()
        {
            if (!AOERenderer) return;
            if (machinesAffected.Count == 0) AOERenderer.material.color = new Vector4(1,1,1,0.2f);
            else AOERenderer.material.color  = new Vector4(0,1,0,0.2f);
        }//Closes UpdateAOEColor method

        private void Update()
        {
            if (isPlaced || !AOETransform) return;
            Ray AOERay = new Ray(transform.position, -transform.up);


            foreach (var hit in Physics.RaycastAll(AOERay))
            {
                if (hit.transform.tag != "Cell") return;
                AOETransform.position = hit.point;
                break;
            }

             AOETransform.position = new Vector3(AOETransform.position.x,0,AOETransform.position.z);

        }//Closes Update Method

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (mammalTween != null) mammalTween.Kill();
        }//Closes OnDestroy method


    }



}
