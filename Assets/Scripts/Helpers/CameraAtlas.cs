using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuanPayan.Helpers
{
    [RequireComponent(typeof(Camera))]
    public class CameraAtlas : MonoBehaviour
    {
        [SerializeField] private string key;
        private static Dictionary<string, Camera> references = new Dictionary<string, Camera>();

        private void Awake()
        {
            if (!references.TryAdd(key, GetComponent<Camera>()))
            {
                Debug.LogWarning("Camera with key: " + key + "could not bet added to the atlas");
            }

        }//Closes Awake method


        private void OnDestroy()
        {
            if (references.ContainsKey(key) && references[key].gameObject == gameObject)
            {
                references.Remove(key);
            }
        }//Closes OnDestroy method

        public static Camera Get(string cameraKey)
        {
            Camera reference = null;
            references.TryGetValue(cameraKey, out reference);
            return reference;
        }//Closes Get method


    }//Closes CameraAtlas class
}//closes namespace declaration
