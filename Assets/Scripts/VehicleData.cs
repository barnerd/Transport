using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    [CreateAssetMenu(fileName = "New Vehicle", menuName = "Transport/Vehicle")]
    public class VehicleData : ScriptableObject
    {
        new public string name;

        public int capacity;

        [Header("Speed")]
        public float travelingSpeed;
        public float loadingSpeed;
        public float unloadingSpeed;
    }
}
