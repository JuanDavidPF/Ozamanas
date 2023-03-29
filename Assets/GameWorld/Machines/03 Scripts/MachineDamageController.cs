using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.Machines
{
    public class MachineDamageController : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private List<GameObject> bodyParts;

        public void HideBodyParts()
        {
            foreach(GameObject part in bodyParts)
            {
                part.SetActive(false);
            }
        }

        public void ShowBodyParts()
        {
             foreach(GameObject part in bodyParts)
            {
                part.SetActive(true);
            }
        }

    }
}
