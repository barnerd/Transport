using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    public class Route : MonoBehaviour
    {
        public enum TravelingDirection
        {
            Forwards = 1,
            Backwards = -1
        }

        public Location start;
        public Location end;

        public float Length { get { return Road.Length; } }

        public Road Road { get; private set; }

        [SerializeField] public RoadLevelData BaseLevel;
        [SerializeField] public RoadLevelData CurrentRoadLevel { get; private set; }

        private List<GameObject> roadSegments;

        [SerializeField] private GameObject roadTile;

        private List<Vehicle> vehicles;

        private void Awake()
        {
            roadSegments = new List<GameObject>();
            vehicles = new List<Vehicle>();

            CurrentRoadLevel = BaseLevel;
        }

        /// <summary>
        /// Level this route up to the next level
        /// </summary>
        public void LevelUp()
        {
            if(CurrentRoadLevel.nextLevel != null)
            {
                CurrentRoadLevel = CurrentRoadLevel.nextLevel;
            }
        }

        /// <summary>
        /// Set the 2 end points of this route, and build a road inbetween
        /// </summary>
        /// <param name="_start">The start Location</param>
        /// <param name="_end">The end Location</param>
        public void SetEndPoints(Location _start, Location _end)
        {
            start = _start;
            end = _end;

            Road = new Road(start.transform.position, end.transform.position);
            InitRoadSegments();
        }

        /// <summary>
        /// Create the graphics for the road segements
        /// </summary>
        private void InitRoadSegments()
        {
            RoadSegment _segment = Road.Start;

            while (_segment != null)
            {
                GameObject roadSegmentGFX = Instantiate(roadTile, _segment.Midpoint, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, _segment.Direction)), this.transform);
                roadSegmentGFX.transform.localScale = new Vector3(_segment.Length, roadSegmentGFX.transform.localScale.y, roadSegmentGFX.transform.localScale.z);

                roadSegments.Add(roadSegmentGFX);

                _segment = _segment.Next;
            }
        }

        /// <summary>
        /// Add a vehicle to this route
        /// </summary>
        /// <param name="_vehicle">which vehicle to add</param>
        public void AddVehicle(Vehicle _vehicle)
        {
            if (!vehicles.Contains(_vehicle))
            {
                vehicles.Add(_vehicle);
                _vehicle.SetRoute(this);
            }
        }

        /// <summary>
        /// Remove a vehicle to this route
        /// </summary>
        /// <param name="_vehicle">which vehicle to remove</param>
        public void RemoveVehicle(Vehicle _vehicle)
        {
            if (vehicles.Contains(_vehicle))
            {
                vehicles.Remove(_vehicle);
                _vehicle.SetRoute(null);
            }
        }

        /*public Vector2 GetDirection(Vector2 _position, TravelingDirection _travelingDirection)
        {
            RoadSegment _segment = (_travelingDirection == TravelingDirection.Forwards) ? Road.Start : Road.End;

            while (_segment != null)
            {
                // FROM: https://lucidar.me/en/mathematics/check-if-a-point-belongs-on-a-line-segment/

                // check colinear
                Vector2 AB = new Vector2(_segment.End.x - _segment.Start.x, _segment.End.y - _segment.Start.y);
                Vector2 AC = new Vector2(_position.x - _segment.Start.x, _position.y - _segment.Start.y);

                float angle = Vector2.Angle(AB, AC);
                if (angle <= float.Epsilon || angle - 180f <= float.Epsilon)
                {
                    float KAC = Vector2.Dot(AB, AC);
                    float KAB = Vector2.Dot(AB, AB);

                    // _position is at the starting point, traveling forwards
                    if (KAC == 0)
                    {
                        if (_travelingDirection == TravelingDirection.Forwards)
                        {
                            return _segment.Direction;
                        }
                        // road endpoint
                        else if (_segment.Previous == null)
                        {
                            // you've arrived at the end
                        }
                    }
                    // _position is at the starting point, traveling backwards
                    if (KAC == KAB)
                    {
                        if (_travelingDirection == TravelingDirection.Backwards)
                        {
                            return _segment.Direction * -1f;
                        }
                        // road endpoint
                        else if (_segment.Next == null)
                        {
                            // you've arrived at the end
                        }
                    }

                    // _position is on this segment
                    if (KAB > KAC && KAC > 0)
                    {
                        return (_travelingDirection == TravelingDirection.Forwards) ? _segment.Direction : _segment.Direction * -1f;
                    }
                }

                _segment = (_travelingDirection == TravelingDirection.Forwards) ? _segment.Next : _segment.Previous;
            }

            return Vector2.zero;
        }*/
    }
}
