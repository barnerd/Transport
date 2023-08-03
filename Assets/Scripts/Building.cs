using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace BarNerdGames.Transport
{
    public class Building : Location
    {
        public bool producing;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (producing)
            {
                ProduceResources();
            }
        }

        private void ProduceResources()
        {
            foreach (var _resource in storedResources.Keys.ToList())
            {
                storedResources[_resource] += _resource.generationRate * Time.deltaTime;

                if (storedResources[_resource] > capacity)
                {
                    storedResources[_resource] = capacity;
                }
            }
        }
    }
}
