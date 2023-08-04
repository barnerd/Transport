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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(VehicleData _vehicle)
    {
        SetName(_vehicle);
        SetCapacity(_vehicle);
        SetSpeed(_vehicle);
        SetOutput(_vehicle);
    }

    public void SetName(VehicleData _vehicle)
    {
        nameText.text = _vehicle.name;
    }

    public void SetCapacity(VehicleData _vehicle)
    {
        capacityText.text = _vehicle.capacity.ToString();
        SetOutput(_vehicle);
    }

    public void SetSpeed(VehicleData _vehicle)
    {
        speedText.text = _vehicle.travelingSpeed.ToString();
        SetOutput(_vehicle);
    }

    public void SetOutput(VehicleData _vehicle)
    {
        outputText.text = (_vehicle.capacity * _vehicle.travelingSpeed).ToString("0.##");
    }
}
