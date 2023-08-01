using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Resource/Resource")]
    public class Resource : ScriptableObject
    {
        new public string name;

        public Sprite image;
    }
}