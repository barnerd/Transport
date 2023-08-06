using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace BarNerdGames.Transport
{
    public class Location : MonoBehaviour
    {
        [Header("Resources")]
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, int> desiredResources;
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, float> storedResources;
        public List<Resource> producingResources;

        [SerializeField] protected int capacity;

        [Header("Routes")]
        public Transform routesParent;
        public List<Route> routes;

        [Header("Vehicles")]
        public Transform vehiclesParent;
        public List<Vehicle> vehicles;

        [Header("UI")]
        [SerializeField] private LocationDetailsUI detailsUI;

        [Header("Prefabs")]
        [SerializeField] private GameObject routePrefab;
        [SerializeField] private GameObject vehiclePrefab;

        public static bool RouteBuilding;

        void Awake()
        {
            if (desiredResources == null) desiredResources = new SerializedDictionary<Resource, int>();
            if (storedResources == null) storedResources = new SerializedDictionary<Resource, float>();
            if (producingResources == null) producingResources = new List<Resource>();

            if (routes == null) routes = new List<Route>();
            if (vehicles == null) vehicles = new List<Vehicle>();
        }

        // Update is called once per frame
        void Update()
        {
            if (producingResources != null && producingResources.Count > 0)
            {
                ProduceResources();
            }
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
                detailsUI.ShowUIPanel(true);
            }
        }

        private void ProduceResources()
        {
            foreach (var _resource in producingResources)
            {
                storedResources[_resource] += _resource.generationRate * Time.deltaTime;

                if (storedResources[_resource] > capacity)
                {
                    storedResources[_resource] = capacity;
                }
            }
        }

        public void BuildRouteTo(Location _end)
        {
            // TODO: check for duplicate or overlapping route

            GameObject _routeGO = Instantiate(routePrefab, routesParent);
            Route _route = _routeGO.GetComponent<Route>();

            _route.SetEndPoints(this, _end);

            // TODO: check for resources for route cost
            var _resourceCost = _route.CurrentRoadLevel.resourceCost;
            float _numUnits = _route.Length;
            bool _haveCosts = CheckCosts(_resourceCost, _numUnits);

            // add to this Location's set of routes
            routes.Add(_route);

            RouteBuilding = false;
        }

        public void BuildVehicle(VehicleData _vehicle)
        {
            // Check Costs
            if (CheckCosts(_vehicle.resourceCost))
            {
                // Spend Costs
                SpendCosts(_vehicle.resourceCost);

                // Build Vehicle
                GameObject _vehicleGO = Instantiate(vehiclePrefab, vehiclesParent);
                Vehicle _newVehicle = _vehicleGO.GetComponent<Vehicle>();

                _newVehicle.data = _vehicle;
                _newVehicle.homeTown = this;

                // Add to list of Vehicles
                vehicles.Add(_newVehicle);

                // TODO: Hide vehicle
            }
        }

        public bool CheckCosts(Dictionary<Resource, int> _costsPerUnit, float _numUnits = 1f)
        {
            foreach (var _resource in _costsPerUnit.Keys.ToList())
            {
                if (storedResources[_resource] < Mathf.RoundToInt(_costsPerUnit[_resource] * _numUnits))
                {
                    return false;
                }
            }

            return true;
        }

        public void SpendCosts(Dictionary<Resource, int> _costsPerUnit, float _numUnits = 1f)
        {
            foreach (var _resource in _costsPerUnit.Keys.ToList())
            {
                storedResources[_resource] -= Mathf.RoundToInt(_costsPerUnit[_resource] * _numUnits);
            }
        }

        public bool UnloadResource(Resource _resource)
        {
            // I don't want this resource, or I don't want anymore of it
            if (!desiredResources.ContainsKey(_resource) || desiredResources[_resource] == 0)
            {
                return false;
            }

            if (storedResources.ContainsKey(_resource))
            {
                // I can't hold anymore of this resource
                if (storedResources[_resource] >= capacity)
                {
                    return false;
                }
                else
                {
                    // if I want a finite amount, reduce desire. -1 is infinite desire
                    if (desiredResources[_resource] > 0) desiredResources[_resource]--;
                    storedResources[_resource]++;
                }
            }
            // first time storing this resource
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
