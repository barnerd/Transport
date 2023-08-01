using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    public class Road
    {
        public RoadSegment Start { get; private set; }
        public RoadSegment End { get; private set; }

        private List<RoadSegment> roadSegments;

        public Road(Vector2 _start, Vector2 _end)
        {
            // TODO: Check for _start = _end
            // TODO: Also check for no waypoints. i.e. strictly vertical, or horiztonal

            RoadSegment middleSegment;

            float verticalDelta = _end.y - _start.y;
            float horizontalDelta = _end.x - _start.x;

            Debug.Log("vertical delta: " + verticalDelta);
            Debug.Log("horizontal delta: " + horizontalDelta);

            Vector2 _primaryDirection;
            Vector2 _secondaryDirection;

            // vertical road
            if (Mathf.Abs(verticalDelta) > Mathf.Abs(horizontalDelta))
            {
                // going up or down
                _primaryDirection = (verticalDelta > 0) ? Vector2.up : Vector2.down;
                // going right or left
                _secondaryDirection = (horizontalDelta > 0) ? Vector2.right : Vector2.left;
            }
            // horizontal road
            else
            {
                // going right or left
                _primaryDirection = (horizontalDelta > 0) ? Vector2.right : Vector2.left;
                // going up or down
                _secondaryDirection = (verticalDelta > 0) ? Vector2.up : Vector2.down;
            }

            Debug.Log("Primary Direction: " + _primaryDirection);
            Debug.Log("Secondary Direction: " + _secondaryDirection);
            Debug.Log("Tertiary Direction: " + (_primaryDirection + _secondaryDirection));
            Vector2 wayPointA = _start + (Mathf.Abs(verticalDelta) - Mathf.Abs(horizontalDelta)) / 2f * _primaryDirection;
            Vector2 wayPointB = _end - (Mathf.Abs(verticalDelta) - Mathf.Abs(horizontalDelta)) / 2f * _primaryDirection;
            Debug.Log("WaypointA: " + wayPointA);
            Debug.Log("WaypointB: " + wayPointB);


            Start = new RoadSegment(_start, wayPointA, _primaryDirection);
            middleSegment = new RoadSegment(wayPointA, wayPointB, _primaryDirection + _secondaryDirection);
            End = new RoadSegment(wayPointB, _end, _primaryDirection);

            Start.Next = middleSegment;
            middleSegment.Previous = Start;
            middleSegment.Next = End;
            End.Previous = middleSegment;

            roadSegments = new List<RoadSegment>();
            roadSegments.Add(Start);
            roadSegments.Add(middleSegment);
            roadSegments.Add(End);

            Debug.Log("Start.Start: " + Start.Start);
            Debug.Log("Start.End: " + Start.End);
            Debug.Log("Waypoint.Start: " + middleSegment.Start);
            Debug.Log("Waypoint.End: " + middleSegment.End);
            Debug.Log("End.Start: " + End.Start);
            Debug.Log("End.End: " + End.End);
        }
    }
}