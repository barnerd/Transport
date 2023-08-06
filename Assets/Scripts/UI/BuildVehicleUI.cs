using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BarNerdGames.Transport;

public class BuildVehicleUI : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel;

    public List<VehicleData> availableVehicles;

    [SerializeField] private Transform vehiclesParent;
    [SerializeField] private GameObject vehicleUIPrefab;

    [SerializeField] private ToggleGroup toggleGroup;

    [SerializeField] private LocationDetailsUI locationDetailsUI;

    void Awake()
    {
        if (availableVehicles == null) availableVehicles = new List<VehicleData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var _vehicle in availableVehicles)
        {
            AddVehicle(_vehicle);
        }
    }

    public void ShowUIPanel(bool _show = true)
    {
        uiPanel.gameObject.SetActive(_show);
    }

    public void AddVehicle(VehicleData _new)
    {
        if (!availableVehicles.Contains(_new))
        {
            availableVehicles.Add(_new);
        }

        GameObject _newVehicle = Instantiate(vehicleUIPrefab, vehiclesParent);
        _newVehicle.GetComponent<VehicleUI>().SetText(_new, true, false);

        // Set Toggle group
        _newVehicle.GetComponent<Toggle>().group = toggleGroup;
    }

    public void BuildVehicle()
    {
        // TODO: is GetFirstActiveToggle correct here?
        VehicleData _vehicle = toggleGroup.GetFirstActiveToggle().GetComponent<VehicleUI>().VehicleData;
        locationDetailsUI.BuildVehicle(_vehicle);

        Close();
    }

    public void Close()
    {
        toggleGroup.SetAllTogglesOff();
        ShowUIPanel(false);
    }
}
