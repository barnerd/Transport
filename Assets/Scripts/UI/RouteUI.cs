using UnityEngine;
using TMPro;
using BarNerdGames.Transport;

public class RouteUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text destinationText;

    public Route Route { get; private set; }

    public void SetText(Route _route)
    {
        Route = _route;

        SetLevel(_route);
        SetDestination(_route);
    }

    public void SetLevel(Route _route)
    {
        levelText.text = _route.CurrentRoadLevel.name;
    }

    public void SetDestination(Route _route)
    {
        destinationText.text = "route to " + _route.end.name;
    }
}
