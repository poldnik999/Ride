using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public static class DataHolder
    {
        private static GameObject prefabName;
        private static string userName = "1";

        public static GameObject Prefab
        {
            get
            {
                return prefabName;
            }
            set
            {
                prefabName = value;
            }
        }
        public static string Name
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }
    }
}
