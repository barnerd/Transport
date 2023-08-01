using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    [CreateAssetMenu(fileName = "New Vehicle", menuName = "Vehicle/Vehicle")]
    public class VehicleData : ScriptableObject
    {
        new public string name;

        public float capacity;
        public float travelingSpeed;
    }
}