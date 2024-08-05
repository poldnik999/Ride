using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunHolder : MonoBehaviour
{
    public Button pressedEquipButton;
    private void Start()
    {
        
    }
    public static class DataHolder
    {
        private static GameObject prefabName;

        public static GameObject Prefab
        {
            get
            {
                Debug.Log(prefabName);
                return prefabName;
                
            }
            set
            {
                prefabName = value;
                Debug.Log(prefabName);
            }
        }
    }
}
