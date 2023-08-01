using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    public class RoadSegment
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }

        public Vector2 Direction { get; private set; }
        public Vector2 Midpoint { get; private set; }
        public float Length { get; private set; }

        public RoadSegment Previous;
        public RoadSegment Next;

        public RoadSegment(Vector2 _start, Vector2 _end, Vector2 _dir, RoadSegment _prev = null, RoadSegment _next = null)
        {
            Start = _start;
            End = _end;

            Direction = _dir;
            Midpoint = (_start + _end) / 2f;
            Length = Vector2.Distance(_start, _end);

            Previous = _prev;
            Next = _next;
        }
    }
}
