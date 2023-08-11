using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BarNerdGames.Transport;

public class VehicleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text outputText;
    [SerializeField] private TMP_Text costsText;

    public Vehicle Vehicle { get; private set; }
    public VehicleData VehicleData { get; private set; }

    public void SetText(Vehicle _vehicle, bool _label = false, bool _showOutput = true)
    {
        Vehicle = _vehicle;
        SetText(_vehicle.data, _label, _showOutput);
    }

    public void SetText(VehicleData _vehicle, bool _label = false, bool _showOutput = true, bool _showCosts = false)
    {
        VehicleData = _vehicle;

        SetName(_vehicle);
        SetCapacity(_vehicle, _label);
        SetSpeed(_vehicle, _label);
        SetOutput(_vehicle, _label, _showOutput);
        SetCosts(_vehicle, _label, _showCosts);
    }

    public void SetName(VehicleData _vehicle)
    {
        nameText.text = _vehicle.name;
    }

    public void SetCapacity(VehicleData _vehicle, bool _label = false)
    {
        capacityText.text = (_label ? "Load: " : "") + _vehicle.capacity.ToString();
    }

    public void SetSpeed(VehicleData _vehicle, bool _label = false)
    {
        speedText.text = (_label ? "Spd: " : "") + _vehicle.travelingSpeed.ToString();
    }

    public void SetOutput(VehicleData _vehicle, bool _label = false, bool _showOutput = true)
    {
        if (_showOutput)
        {
            outputText.text = (_label ? "Output: " : "") + (_vehicle.capacity * _vehicle.travelingSpeed).ToString("0.##");
            outputText.gameObject.SetActive(_showOutput);
        }
    }

    public void SetCosts(VehicleData _vehicle, bool _label = false, bool _showCosts = false)
    {
        string _costs = "";

        if (_showCosts)
        {
            foreach (var _resource in _vehicle.resourceCost.Keys)
            {
                _costs += _vehicle.resourceCost[_resource].ToString() + " " + _resource.name + ", ";
            }
            _costs = _costs.Substring(0, _costs.Length - 2);
        }

        if (costsText != null)
        {
            costsText.text = (_showCosts) ? (_label ? "Costs: " : "") + _costs : "";
            costsText.gameObject.SetActive(_showCosts);
        }
    }

    public void SetEnabled(bool _enabled = true)
    {
        Toggle _t = GetComponent<Toggle>();
        _t.interactable = _enabled;

        GetComponent<HoverAndSelectedHighlighter>().SetEnabled(_enabled);
    }
}
