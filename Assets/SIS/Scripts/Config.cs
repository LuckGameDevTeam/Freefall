using UnityEngine;
using System.Collections;

namespace SIS
{
    [System.Serializable]
    public class Config : ScriptableObject
    {
        [HideInInspector]
        public bool autoOpen = true;
    }
}
