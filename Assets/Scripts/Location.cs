using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace BarNerdGames.Transport
{
    public class Location : MonoBehaviour
    {
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, int> desiredResources;
        [SerializedDictionary("Resource", "Amount")] public SerializedDictionary<Resource, int> producedResources;

        [SerializeField] private LocationDetailsUI detailsUI;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            Debug.Log("Sprite Clicked: " + name);
            detailsUI.UpdateLocationDetails(name);
            detailsUI.ShowLocationDetails(true);
        }
    }
}