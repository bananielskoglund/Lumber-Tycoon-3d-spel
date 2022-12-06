using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TrickShotAssets.CustomInputManager
{
    public class KeyDetector : MonoBehaviour
    {
        private InputManagerStorage inputManagerStorage;
        private KeyCode currentKey;
        public string inputName;
        private bool detectKey;

        [Header("UI Elements")]
        public TMPro.TextMeshPro inputNameText;

        public TMPro.TextMeshPro keyCodeText;

        private void Start()
        {
            inputManagerStorage = InputManagerStorage.Instance;
            inputNameText.text = inputName;
            AssignKeyToInput(inputName, inputManagerStorage.GetKeycodeByName(inputName));
        }

        private void Update()
        {
            if (detectKey)
            {
                foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(kcode))
                    {
                        if (!inputManagerStorage.allowDuplicateKeys && inputManagerStorage.isKeyAlreadyMapped(kcode))
                        {
                            //show error message due to already bound key, if it is disabled
                            Debug.LogError("Key " + kcode.ToString() + " already in use!");
                        }
                        else if ((!inputManagerStorage.allowDuplicateKeys && !inputManagerStorage.isKeyAlreadyMapped(kcode)) || inputManagerStorage.allowDuplicateKeys)
                        {
                            //bind key to new one
                            print(kcode);
                            print(inputName);
                            currentKey = kcode;
                            AssignKeyToInput(inputName, kcode);
                            inputManagerStorage.SaveToXML();
                        }
                        detectKey = false;
                    }
                }
            }
        }

        public void UpdateKeyDisplay()
        {
            currentKey = inputManagerStorage.keyMappings[inputManagerStorage.GetIndexByName(inputName)].key;
            keyCodeText.text = currentKey.ToString();
        }

        public void AssignKeyToInput(string inputName, KeyCode newKey)
        {
            print(newKey);
            inputManagerStorage.keyMappings[inputManagerStorage.GetIndexByName(inputName)].key = newKey;
            keyCodeText.text = newKey.ToString();
        }

        public void ListenForButton()
        {
            detectKey = true;
        }
    }
}