using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    public class Vehicle : MonoBehaviour
    {
        public VehicleData data;
        [SerializeField] private float LocationThreshold;

        public Location homeTown; // should not be null
        public Route route; // can be null
        private Route.TravelingDirection travelingDirection;
        private Vector2 direction;
        private RoadSegment currentSegment;
        private float segmentPercent;
        private Location currentLocation; // can be null
        public State currentState;

        private float timer;

        public enum State
        {
            Parked,
            Loading,
            Unloading,
            Moving
        }

        private List<Resource> resources;
        [SerializeField] private ResourceContainer resourceContainer;

        void Awake()
        {
            resources = new List<Resource>();

            currentState = State.Parked;
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

        /// <summary>
        /// Set which route this vehicle is on
        /// </summary>
        /// <param name="_route">the route</param>
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
                currentSegment = _route.Road.Start;
                SetDirection();
                currentState = State.Moving;
            }
            else
            {
                // TODO: Return to home town?
                currentState = State.Parked;
            }
        }

        /// <summary>
        /// Unload a resource to the current Location
        /// </summary>
        private void UnloadResource()
        {
            // TODO: Figure out how to not wait for the timer if there's nothing to unload
            if (Time.time > timer)
            {
                Debug.Log("Unloading resources");

                // Deliver Resources if possible
                foreach (Resource resource in resources)
                {
                    if (currentLocation.UnloadResource(resource))
                    {
                        resources.Remove(resource);

                        resourceContainer.RefreshGraphics(resources);

                        timer = Time.time + data.unloadingSpeed;
                        return;
                    }
                }

                currentState = State.Loading;
                timer = Time.time + data.loadingSpeed;
            }
        }

        /// <summary>
        /// Load a resource to the current Location
        /// </summary>
        private void LoadResource()
        {
            // TODO: Figure out how to not wait for the timer if there's nothing to load
            if (Time.time > timer)
            {
                Debug.Log("Loading resources");

                // Collect Resources if possible
                if (resources.Count < data.capacity)
                {
                    Location target = (currentLocation == route.start) ? route.end : route.start;
                    Resource collectedResource = currentLocation.LoadResource(new List<Resource>(target.desiredResources.Keys));
                    if (collectedResource != null)
                    {
                        resources.Add(collectedResource);

                        resourceContainer.RefreshGraphics(resources);

                        timer = Time.time + data.loadingSpeed;
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
        }

        /// <summary>
        /// Move this vehicle along the route that it's on
        /// </summary>
        private void Move()
        {
            // TODO: this is a hack. SetRoute should be called by now
            if (currentSegment == null && route != null)
            {
                Debug.LogError("SetRoute being called because it's not set before Move()");
                SetRoute(route);
            }

            // check if at segment end
            if (segmentPercent > 1f)
            {
                // go to next segment
                RoadSegment nextSegment = (travelingDirection == Route.TravelingDirection.Forwards) ? currentSegment.Next : currentSegment.Previous; ;

                // finished traversing the route
                if (nextSegment == null)
                {
                    currentLocation = (travelingDirection == Route.TravelingDirection.Forwards) ? route.end : route.start;

                    // Turn around
                    travelingDirection = (Route.TravelingDirection)((int)travelingDirection * -1);
                    SetDirection();

                    currentState = State.Unloading;
                    timer = Time.time + data.unloadingSpeed;
                    return;
                }
                else
                {
                    currentSegment = nextSegment;
                    SetDirection();
                }
            }
            else
            {
                Vector2 segmentStart = (travelingDirection == Route.TravelingDirection.Forwards) ? currentSegment.Start : currentSegment.End;

                // move in that direction f(vehicle.movingSpeed, road.movingSpeed)
                segmentPercent += data.travelingSpeed * Time.deltaTime / currentSegment.Length;
                transform.position = segmentStart + segmentPercent * direction.normalized * currentSegment.Length;
                //transform.localPosition += data.travelingSpeed * Time.deltaTime * (Vector3)direction.normalized;
            }
        }

        /// <summary>
        /// Helper function for changing the direction and updating the graphics.
        /// </summary>
        private void SetDirection()
        {
            segmentPercent = 0f;
            direction = currentSegment.Direction * (int)travelingDirection;
            transform.localRotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, direction));
        }
    }
}
