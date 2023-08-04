using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Transport/Resource")]
    public class Resource : ScriptableObject
    {
        new public string name;

        public float generationRate;

        public Sprite image;
        public Color color;
    }
}
