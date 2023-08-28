using UnityEngine;
using TMPro;
using BarNerdGames.Transport;

public class RouteUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text destinationText;

    public Route Route { get; private set; }

    public void SetText(Route _route, Location _current)
    {
        Route = _route;

        SetLevel(_route);
        SetDestination(_route, _current);
    }

    public void SetLevel(Route _route)
    {
        levelText.text = _route.CurrentRoadLevel.name;
    }

    public void SetDestination(Route _route, Location _current)
    {
        destinationText.text = "route to " + ((Route.start == _current) ? _route.end.name : _route.start.name);
    }
}
