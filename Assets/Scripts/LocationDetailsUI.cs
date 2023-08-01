using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationDetailsUI : MonoBehaviour
{
    [SerializeField] private RectTransform uiPanel;
    [SerializeField] private TMP_Text locationName;

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

    public void UpdateLocationDetails(string _name)
    {
        locationName.text = _name;
    }
}
