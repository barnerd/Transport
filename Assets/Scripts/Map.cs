using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    [Header("UI")]
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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            detailsUI.CancelRoute();
            detailsUI.ShowUIPanel(false);
        }
    }
}
