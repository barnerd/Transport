using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace BarNerdGames.Transport
{
    [CreateAssetMenu(fileName = "New Road Level", menuName = "Transport/Road Level")]
    public class RoadLevelData : ScriptableObject
    {
        // cost per unit
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, int> resourceCost;

        // TODO: Add sprite for displaying road level

        public RoadLevelData nextLevel;
    }
}
