using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    public class Vehicle : MonoBehaviour
    {
        public VehicleData data;
        [SerializeField] public float LocationThreshold;

        public Town homeTown; // should not be null
        public Route route; // can be null
        private Route.TravelingDirection travelingDirection;
        private Vector2 direction;
        private RoadSegment currentSegement;
        private Location currentLocation; // can be null
        public State currentState;

        public enum State
        {
            Parked,
            Loading,
            Unloading,
            Moving
        }

        private List<Resource> resources;

        void Awake()
        {
            resources = new List<Resource>();

            currentState = State.Parked;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (route != null)
            {
                switch (currentState)
                {
                    case State.Moving:
                        Move();
                        break;
                    case State.Loading:
                        LoadResource();
                        break;
                    case State.Unloading:
                        UnloadResource();
                        break;
                }
            }
        }

        public void SetRoute(Route _route)
        {
            // if route != null, then clear previous info
            if (route != null)
            {
                // drop resources?
                // wait until you get back to town?
            }

            // TODO: Set position

            route = _route;
            travelingDirection = Route.TravelingDirection.Forwards;
            if (_route != null)
            {
                currentSegement = _route.Road.Start;
                SetDirection();
                currentState = State.Moving;
            }
            else
            {
                // TODO: Return to home town?
                currentState = State.Parked;
            }
        }

        private void UnloadResource()
        {
            Debug.Log("Unloading resources");
            // TODO: add timer so it's 1 per timer

            // Deliver Resources if possible
            foreach (Resource resource in resources)
            {
                if (currentLocation.UnloadResource(resource))
                {
                    resources.Remove(resource);
                    return;
                }
            }

            currentState = State.Loading;
        }

        private void LoadResource()
        {
            Debug.Log("Loading resources");
            // TODO: add timer so it's 1 per timer

            // Collect Resources if possible
            if (resources.Count < data.capacity)
            {
                Location target = (currentLocation == route.start) ? route.end : route.start;
                Resource collectedResource = currentLocation.LoadResource(new List<Resource>(target.desiredResources.Keys));
                if (collectedResource != null)
                {
                    resources.Add(collectedResource);
                }
                else
                {
                    currentState = State.Moving;
                }
            }
            else
            {
                currentState = State.Moving;
            }
        }

        private void Move()
        {
            // TODO: this is a hack. SetRoute should be called by now
            if (currentSegement == null && route != null)
            {
                SetRoute(route);
            }

            // check if at segment end
            Vector2 segmentEnd = (travelingDirection == Route.TravelingDirection.Forwards) ? currentSegement.End : currentSegement.Start;
            if (Vector2.Distance(transform.position, segmentEnd) < LocationThreshold)
            {
                // go to next segment
                RoadSegment nextSegement = (travelingDirection == Route.TravelingDirection.Forwards) ? currentSegement.Next : currentSegement.Previous; ;

                // finished traversing the route
                if (nextSegement == null)
                {
                    currentLocation = (travelingDirection == Route.TravelingDirection.Forwards) ? route.end : route.start;

                    // Turn around
                    travelingDirection = (Route.TravelingDirection)((int)travelingDirection * -1);
                    SetDirection();

                    currentState = State.Unloading;
                    return;
                }
                else
                {
                    currentSegement = nextSegement;
                    SetDirection();
                }
            }

            // move in that direction f(vehicle.movingSpeed, road.movingSpeed)
            transform.position += data.travelingSpeed * Time.deltaTime * (Vector3)direction;
        }

        private void SetDirection()
        {
            direction = currentSegement.Direction * (int)travelingDirection;
            transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, direction));
        }
    }
}
