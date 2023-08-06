using UnityEngine;
using TMPro;
using BarNerdGames.Transport;

public class VehicleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text outputText;

    public Vehicle Vehicle { get; private set; }
    public VehicleData VehicleData { get; private set; }

    public void SetText(Vehicle _vehicle, bool _label = false, bool _showOutput = true)
    {
        Vehicle = _vehicle;
        SetText(_vehicle.data, _label, _showOutput);
    }

    public void SetText(VehicleData _vehicle, bool _label = false, bool _showOutput = true)
    {
        VehicleData = _vehicle;

        SetName(_vehicle);
        SetCapacity(_vehicle, _label);
        SetSpeed(_vehicle, _label);
        SetOutput(_vehicle, _label, _showOutput);
    }

    public void SetName(VehicleData _vehicle)
    {
        nameText.text = _vehicle.name;
    }

    public void SetCapacity(VehicleData _vehicle, bool _label = false)
    {
        capacityText.text = ((_label) ? "Load: " : "") + _vehicle.capacity.ToString();
    }

    public void SetSpeed(VehicleData _vehicle, bool _label = false)
    {
        speedText.text = ((_label) ? "Spd: " : "") + _vehicle.travelingSpeed.ToString();
    }

    public void SetOutput(VehicleData _vehicle, bool _label = false, bool _showOutput = true)
    {
        outputText.text = ((_label) ? "Output: " : "") + (_vehicle.capacity * _vehicle.travelingSpeed).ToString("0.##");
        outputText.gameObject.SetActive(_showOutput);
    }
}
