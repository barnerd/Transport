using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace BarNerdGames.Transport
{
    public class Location : MonoBehaviour
    {
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, int> desiredResources;
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, float> storedResources;

        [SerializeField] protected int capacity;

        [SerializeField] private LocationDetailsUI detailsUI;

        [SerializeField] private GameObject routePrefab;

        public static bool RouteBuilding;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            if (Location.RouteBuilding)
            {
                detailsUI.FinishRoute(this);
            }
            else
            {
                detailsUI.UpdateLocationDetails(this);
                detailsUI.ShowLocationDetails(true);
            }
        }

        public void BuildRouteTo(Location _end)
        {
            // TODO: check for duplicate or overlapping route

            GameObject routeGO = Instantiate(routePrefab);
            Route route = routeGO.GetComponent<Route>();

            route.SetEndPoints(this, _end);

            // TODO: check for resources for route cost
            var resourceCost = route.CurrentRoadLevel.resourceCost;
            int cost = Mathf.RoundToInt(route.Length);

            // TODO: add to this Locations set of routes

            RouteBuilding = false;
        }

        public bool UnloadResource(Resource _resource)
        {
            if (!desiredResources.ContainsKey(_resource) || desiredResources[_resource] == 0)
            {
                return false;
            }

            if (storedResources.ContainsKey(_resource))
            {
                if (storedResources[_resource] >= capacity)
                {
                    return false;
                }
                else
                {
                    if (desiredResources[_resource] > 0) desiredResources[_resource]--;
                    storedResources[_resource]++;
                }
            }
            else
            {
                storedResources.Add(_resource, 1);
            }

            return true;
        }

        public Resource LoadResource(List<Resource> _desiredResources)
        {
            if (_desiredResources.Count < storedResources.Count)
            {
                foreach (var _resource in _desiredResources)
                {
                    if (storedResources.ContainsKey(_resource))
                    {
                        if (storedResources[_resource] > 0)
                        {
                            storedResources[_resource]--;
                            return _resource;
                        }
                    }
                }
            }
            else
            {
                foreach (var _resource in storedResources.Keys)
                {
                    if (_desiredResources.Contains(_resource))
                    {
                        if (storedResources[_resource] > 0)
                        {
                            storedResources[_resource]--;
                            return _resource;
                        }
                    }
                }
            }

            return null;
        }
    }
}
