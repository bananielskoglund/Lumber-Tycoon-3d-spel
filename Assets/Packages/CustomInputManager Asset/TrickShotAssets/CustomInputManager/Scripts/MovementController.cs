using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickShotAssets.CustomInputManager
{
    public class MovementController : MonoBehaviour
    {
        public GameObject Player;
        private InputManagerStorage inputManagerStorage;

        // Start is called before the first frame update
        private void Start()
        {
            inputManagerStorage = InputManagerStorage.Instance;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(inputManagerStorage.keyMappings[inputManagerStorage.GetIndexByName("Forward")].key))
            {
                Player.transform.Translate(Vector3.forward * Time.deltaTime);
            }

            if (Input.GetKey(inputManagerStorage.keyMappings[inputManagerStorage.GetIndexByName("Backward")].key))
            {
                Player.transform.Translate(Vector3.back * Time.deltaTime);
            }

            if (Input.GetKey(inputManagerStorage.keyMappings[inputManagerStorage.GetIndexByName("Left")].key))
            {
                Player.transform.Translate(Vector3.left * Time.deltaTime);
            }

            if (Input.GetKey(inputManagerStorage.keyMappings[inputManagerStorage.GetIndexByName("Right")].key))
            {
                Player.transform.Translate(Vector3.right * Time.deltaTime);
            }
        }
    }
}