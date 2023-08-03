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

        public Vector2 Direction { get; private set; }

        public Road(Vector2 _start, Vector2 _end)
        {
            if (_start != _end)
            {
                RoadSegment middleSegment = null;

                float signedVerticalDelta = _end.y - _start.y;
                float signedHorizontalDelta = _end.x - _start.x;

                float verticalDelta = Mathf.Abs(signedVerticalDelta);
                float horizontalDelta = Mathf.Abs(signedHorizontalDelta);

                Vector2 _primaryDirection;
                Vector2 _secondaryDirection;

                // vertical road
                if (verticalDelta > horizontalDelta)
                {
                    // going up or down
                    _primaryDirection = (signedVerticalDelta > 0) ? Vector2.up : Vector2.down;
                    // going right or left
                    _secondaryDirection = (signedHorizontalDelta > 0) ? Vector2.right : Vector2.left;
                }
                // horizontal road
                else
                {
                    // going right or left
                    _primaryDirection = (signedHorizontalDelta > 0) ? Vector2.right : Vector2.left;
                    // going up or down
                    _secondaryDirection = (signedVerticalDelta > 0) ? Vector2.up : Vector2.down;
                }
                Direction = _primaryDirection;

                // strictly vertical or horizontal road
                if (verticalDelta <= float.Epsilon || horizontalDelta <= float.Epsilon)
                {
                    Start = new RoadSegment(_start, _end, _primaryDirection);

                    roadSegments = new List<RoadSegment>();
                    roadSegments.Add(Start);
                }
                else
                {
                    Vector2 wayPointA = _start + Mathf.Abs(verticalDelta - horizontalDelta) / 2f * _primaryDirection;
                    Vector2 wayPointB = _end - Mathf.Abs(verticalDelta - horizontalDelta) / 2f * _primaryDirection;

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
                }
            }
        }
    }
}
