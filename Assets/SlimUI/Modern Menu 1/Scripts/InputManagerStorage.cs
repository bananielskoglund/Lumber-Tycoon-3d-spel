using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace TrickShotAssets.CustomInputManager
{
    public class InputManagerStorage : MonoBehaviour
    {
        #region Variables

        //Serializer

        private XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<KeyMapping>), new XmlRootAttribute("KeyMappings"));

        //Singleton Instance

        public static InputManagerStorage Instance;

        //StorageList for all Keys and their Inputs

        [SerializeField]
        [XmlElement(ElementName = "KeyMappings")]
        public List<KeyMapping> keyMappings = new List<KeyMapping>();

        //Defaults

        public string path = "config";
        public string configFileName = "inputs.cfg";
        public bool allowDuplicateKeys = false;

        #endregion Variables

        #region Methods

        //create Singleton of this class
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            //Create Directory if not existing and try to load Keymappings from File. If this fails, create a File with the defaults defined in the Inspector
            Directory.CreateDirectory(path);
            try
            {
                LoadFromXML();
            }
            catch (System.Exception)
            {
                SaveToXML();
            }
        }

        //Get Position/Index of Keybind in the List by using the Iput Name
        public int GetIndexByName(string inputName)
        {
            for (int i = 0; i < keyMappings.Count; i++)
            {
                if (keyMappings[i].inputName == inputName)
                {
                    return i;
                }
            }
            return -1;
        }

        //Get Keycode by Input Name
        public KeyCode GetKeycodeByName(string inputName)
        {
            return keyMappings[GetIndexByName(inputName)].key;
        }

        //Checks if a Key is already bound to another action
        public bool isKeyAlreadyMapped(KeyCode key)
        {
            for (int i = 0; i < keyMappings.Count; i++)
            {
                if (keyMappings[i].key == key)
                {
                    return true;
                }
            }
            return false;
        }

        //Restores all Keymappings to the default ones from the config file
        public void ResetAllKeyMappings()
        {
            for (int i = 0; i < keyMappings.Count; i++)
            {
                keyMappings[i].key = keyMappings[i].defaultKey;
            }
            UpdateAllKeyDisplays();
        }

        //Serialization
        //Save KeyMappings to XML File at path from variable
        public void SaveToXML()
        {
            Directory.CreateDirectory(path);
            FileStream file = System.IO.File.Create(path + "/" + configFileName);
            xmlSerializer.Serialize(file, keyMappings);
            file.Close();
        }

        //Load KeyMappings from XML File at path from variable
        public void LoadFromXML()
        {
            keyMappings = (List<KeyMapping>)xmlSerializer.Deserialize(new StreamReader(path + "/" + configFileName));
            UpdateAllKeyDisplays();
        }

        public void UpdateAllKeyDisplays()
        {
            KeyDetector[] keyDetectors = GameObject.FindObjectsOfType<KeyDetector>();
            foreach (KeyDetector keyDetector in keyDetectors)
            {
                keyDetector.UpdateKeyDisplay();
            }
        }

        #endregion Methods
    }
}