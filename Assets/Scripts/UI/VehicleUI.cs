using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BarNerdGames.Transport;

public class VehicleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text outputText;

    [SerializeField] public bool Selected { get; private set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSelected(bool _selected = true)
    {
        if (Selected != _selected)
        {
            Selected = _selected;
            // TODO: change graphics to highlighted
            // TODO: Let BuildVehiclesUI know I'm selected
        }
    }

    public void SetText(VehicleData _vehicle, bool _showOutput = true)
    {
        SetName(_vehicle);
        SetCapacity(_vehicle);
        SetSpeed(_vehicle);
        SetOutput(_vehicle, _showOutput);
    }

    public void SetName(VehicleData _vehicle)
    {
        nameText.text = _vehicle.name;
    }

    public void SetCapacity(VehicleData _vehicle)
    {
        capacityText.text = _vehicle.capacity.ToString();
    }

    public void SetSpeed(VehicleData _vehicle)
    {
        speedText.text = _vehicle.travelingSpeed.ToString();
    }

    public void SetOutput(VehicleData _vehicle, bool _showOutput = true)
    {
        outputText.text = (_vehicle.capacity * _vehicle.travelingSpeed).ToString("0.##");
        outputText.gameObject.SetActive(_showOutput);
    }
}
