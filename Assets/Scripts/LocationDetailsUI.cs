using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BarNerdGames.Transport;

public class LocationDetailsUI : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel;
    [SerializeField] private TMP_Text locationName;

    private Location currentLocation;
    private Location routeStart;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowLocationDetails(bool _show = true)
    {
        uiPanel.gameObject.SetActive(_show);
    }

    public void UpdateLocationDetails(Location _loc)
    {
        currentLocation = _loc;
        locationName.text = _loc.name;

        // TODO: hide buttons if not a Town
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
        }
    }

    public void CancelRoute()
    {
        Location.RouteBuilding = false;
        routeStart = null;
    }

    public void CreateVehicle()
    {
        // TODO: Have list of available vehicles, where?
        // TODO: Call that list with desired Vehicle and currentLoc
    }
}
