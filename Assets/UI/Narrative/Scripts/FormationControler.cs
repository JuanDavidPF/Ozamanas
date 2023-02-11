using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozamanas.UI
{
    public class FormationControler: MonoBehaviour
    {

        [SerializeField] private GameObject UIObject;
        [SerializeField] private int UIObjectsQty;
        [SerializeField] private float circleRadio;

        [SerializeField] private RectTransform origin;


        
        // Start is called before the first frame update
        void Start()
        {
            CircleFormation();
        }

      
        private void CircleFormation()
        {
           
            Vector3 targetPosition = Vector3.zero;

            for( int i=0;i< UIObjectsQty ; i++)
            {
                GameObject instance = Instantiate(UIObject,gameObject.transform);

                float angle = i * (2 * Mathf.PI / UIObjectsQty ) ;

               //float angle = i * (180 / UIObjectsQty) ;

                float x = Mathf.Cos(angle) * circleRadio;

                float y = Mathf.Sin(angle) * circleRadio;

                targetPosition = new Vector3(targetPosition.x + x,targetPosition.y+y,targetPosition.z);

                instance.GetComponent<RectTransform>().anchoredPosition = targetPosition;

                //instance.transform.localPosition = targetPosition;

            }


        }
    }
}
