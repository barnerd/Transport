using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using AYellowpaper.SerializedCollections;

namespace BarNerdGames.Transport
{
    /// <summary>
    /// A Location on the Map for producing/storing resources. i.e. town, building
    /// </summary>
    public class Location : MonoBehaviour
    {
        [Header("Resources")]
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, int> desiredResources;
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, float> storedResources;
        public List<Resource> producingResources;
        [SerializeField] private ResourceContainer resourceContainer;

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
        [SerializeField] private GameObject resourcePrefab;

        public static bool RouteBuilding;

        void Awake()
        {
            if (desiredResources == null) desiredResources = new SerializedDictionary<Resource, int>();
            if (storedResources == null) storedResources = new SerializedDictionary<Resource, float>();
            if (producingResources == null) producingResources = new List<Resource>();

            if (routes == null) routes = new List<Route>();
            if (vehicles == null) vehicles = new List<Vehicle>();
        }

        // Start is called before the first frame update
        void Start()
        {
            resourceContainer.RefreshGraphics(storedResources);
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
            if (!EventSystem.current.IsPointerOverGameObject())
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
        }

        /// <summary>
        /// Produces resources
        /// </summary>
        private void ProduceResources()
        {
            foreach (var _resource in producingResources)
            {
                if (!storedResources.ContainsKey(_resource))
                {
                    storedResources.Add(_resource, 0f);
                }

                //if (Mathf.Abs(storedResources[_resource] - capacity) < float.Epsilon)
                {
                    storedResources[_resource] += _resource.generationRate * Time.deltaTime;

                    if (storedResources[_resource] > capacity)
                    {
                        storedResources[_resource] = capacity;
                    }

                    resourceContainer.RefreshGraphics(storedResources);
                }
            }
        }

        /// <summary>
        /// Build a route from this Location to the provided Location
        /// </summary>
        /// <param name="_end">The end of the route built</param>
        public void BuildRouteTo(Location _end)
        {
            // TODO: check for duplicate or overlapping route

            GameObject _routeGO = Instantiate(routePrefab, routesParent);
            Route _route = _routeGO.GetComponent<Route>();

            _route.SetEndPoints(this, _end);

            var _resourceCost = _route.CurrentRoadLevel.resourceCost;
            float _numUnits = _route.Length;
            bool _haveCosts = CheckCosts(_resourceCost, _numUnits);
            // TODO: spend resources for route cost

            // add to this Location's set of routes
            routes.Add(_route);

            RouteBuilding = false;
        }

        /// <summary>
        /// Build a vehicle with the data provided and assign it to this Lcation
        /// </summary>
        /// <param name="_vehicle">The VehicleData to build this Vehicle from</param>
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

        /// <summary>
        /// Checks to see if this Location can afford the cost, in resources per unit.
        /// </summary>
        /// <param name="_costsPerUnit">How much each unit costs</param>
        /// <param name="_numUnits">How many units to check</param>
        /// <returns>True, if this Location can afford the costs; otherwise, false</returns>
        public bool CheckCosts(Dictionary<Resource, int> _costsPerUnit, float _numUnits = 1f)
        {
            foreach (var _resource in _costsPerUnit.Keys.ToList())
            {
                if (!storedResources.ContainsKey(_resource) || storedResources[_resource] < Mathf.RoundToInt(_costsPerUnit[_resource] * _numUnits))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Deduct the resource costs from storage
        /// </summary>
        /// <param name="_costsPerUnit">How much each unit costs</param>
        /// <param name="_numUnits">How many units being deducted</param>
        public void SpendCosts(Dictionary<Resource, int> _costsPerUnit, float _numUnits = 1f)
        {
            foreach (var _resource in _costsPerUnit.Keys.ToList())
            {
                int _count = Mathf.RoundToInt(_costsPerUnit[_resource] * _numUnits);
                storedResources[_resource] -= _count;

                resourceContainer.RefreshGraphics(storedResources);
            }
        }

        /// <summary>
        /// Add a resource to storage
        /// </summary>
        /// <param name="_resource">which resource to add</param>
        /// <returns>true, if the resource is stored; otherwise, false</returns>
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

                    resourceContainer.RefreshGraphics(storedResources);
                }
            }
            // first time storing this resource
            else
            {
                storedResources.Add(_resource, 1);

                resourceContainer.RefreshGraphics(storedResources);
            }

            return true;
        }

        /// <summary>
        /// Subtract a resource from storage
        /// </summary>
        /// <param name="_desiredResources">which resource to subtract</param>
        /// <returns>true, if the resource is subtracted; otherwise, false</returns>
        public Resource LoadResource(List<Resource> _desiredResources)
        {
            foreach (Resource _resource in _desiredResources)
            {
                if (storedResources.ContainsKey(_resource))
                {
                    if (storedResources[_resource] > 0)
                    {
                        storedResources[_resource]--;

                        resourceContainer.RefreshGraphics(storedResources);

                        return _resource;
                    }
                }
            }

            return null;
        }
    }
}
