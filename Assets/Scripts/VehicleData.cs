using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

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

        [Header("Costs")]
        // cost per unit
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, int> resourceCost;

    }
}
