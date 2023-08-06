using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarNerdGames.Transport;

public class BuildVehicleUI : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel;

    public List<VehicleData> availableVehicles;

    [SerializeField] private Transform vehiclesParent;
    [SerializeField] private GameObject vehicleUIPrefab;

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
    }

    public void Cancel()
    {
        // TODO: remove selected
        ShowUIPanel(false);
    }
}
