using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrickShotAssets.CustomInputManager
{
    [Serializable]
    public class AxisMapping
    {
        public string inputName;
        public KeyCode key;
        public KeyCode defaultKey;
    }
}