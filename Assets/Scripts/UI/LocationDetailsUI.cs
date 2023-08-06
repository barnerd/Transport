using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BarNerdGames.Transport;

public class LocationDetailsUI : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel;
    [SerializeField] private TMP_Text locationName;

    private Location currentLocation;
    private Location routeStart;

    [Header("'None' Text")]
    [SerializeField] private GameObject routesNone;
    [SerializeField] private GameObject vehiclesNone;

    [Space(10)]
    [SerializeField] private GameObject vehiclesHeader;
    [SerializeField] private ToggleGroup routesToggleGroup;
    [SerializeField] private ToggleGroup vehiclesToggleGroup;

    [Header("Parents")]
    [SerializeField] private Transform routesParent;
    [SerializeField] private Transform vehiclesParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject routeUIPrefab;
    [SerializeField] private GameObject vehicleUIPrefab;

    public void ShowUIPanel(bool _show = true)
    {
        uiPanel.gameObject.SetActive(_show);
    }

    public void UpdateLocationDetails(Location _loc)
    {
        currentLocation = _loc;
        locationName.text = _loc.name;

        // Update Routes
        UpdateRoutesDetails();

        // Update Vehicles
        UpdateVehiclesDetails();

        // TODO: hide buttons if not a Town
    }

    private void UpdateRoutesDetails()
    {
        // Delete old routes
        RouteUI[] _oldRoutes = routesParent.GetComponentsInChildren<RouteUI>();
        foreach (var _old in _oldRoutes)
        {
            Destroy(_old.gameObject);
        }

        // if no routes, show "none...", else hide "none..."
        routesNone.SetActive(currentLocation.routes.Count == 0);

        // Create routesUI
        foreach (var _route in currentLocation.routes)
        {
            GameObject _newRoute = Instantiate(routeUIPrefab, routesParent);
            _newRoute.GetComponent<RouteUI>().SetText(_route);


            // Set Toggle group
            _newRoute.GetComponent<Toggle>().group = routesToggleGroup;
        }
    }

    private void UpdateVehiclesDetails()
    {
        // Delete old vehicles
        VehicleUI[] _oldVehicles = vehiclesParent.GetComponentsInChildren<VehicleUI>();
        foreach (var _old in _oldVehicles)
        {
            Destroy(_old.gameObject);
        }

        // if no vehicles, show "none...", else hide "none..."
        vehiclesNone.SetActive(currentLocation.vehicles.Count == 0);
        vehiclesHeader.SetActive(currentLocation.vehicles.Count > 0);

        // Create vehiclesUI
        foreach (var _vehicle in currentLocation.vehicles)
        {
            GameObject _newVehicle = Instantiate(vehicleUIPrefab, vehiclesParent);
            _newVehicle.GetComponent<VehicleUI>().SetText(_vehicle);

            // Set Toggle group
            _newVehicle.GetComponent<Toggle>().group = vehiclesToggleGroup;
        }
    }

    public void StartRoute()
    {
        // TODO: Set static Loc.mode to routeBuilding
        Location.RouteBuilding = true;
        routeStart = currentLocation;

        // TODO: Change "Start Route" button to "Cancel Route" button

        // TODO: Have location check for mode
        // TODO: if mode == routeBuilding, then return loc if != currentLoc and build route
    }

    public void FinishRoute(Location _end)
    {
        if (currentLocation != _end)
        {
            currentLocation.BuildRouteTo(_end);
            UpdateRoutesDetails();
        }
    }

    public void CancelRoute()
    {
        Location.RouteBuilding = false;
        routeStart = null;
    }

    public void BuildVehicle(VehicleData _vehicle)
    {
        currentLocation.BuildVehicle(_vehicle);
        UpdateVehiclesDetails();
    }

    public void AssignVehicleToRoute()
    {
        // TODO: is GetFirstActiveToggle correct here?

        // Get Route
        Route _route = routesToggleGroup.GetFirstActiveToggle().GetComponent<RouteUI>().Route;

        // Get Vehicle
        Vehicle _vehicle = vehiclesToggleGroup.GetFirstActiveToggle().GetComponent<VehicleUI>().Vehicle;

        // Assign Vehicle to Route
        _vehicle.SetRoute(_route);
        _route.AddVehicle(_vehicle);
    }
}
