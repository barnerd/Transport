using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BarNerdGames.Transport
{
    public class Route : MonoBehaviour
    {
        public Location start;
        public Location end;

        private Road road;

        private List<GameObject> roadSegments;

        [SerializeField] private GameObject roadTile;

        // TODO: have list of vehicles
        // TODO: AddVehicle()
        // TODO: RemoveVehicle()
        // TODO: Arrive()

        private void Awake()
        {
            roadSegments = new List<GameObject>();
        }

        // Start is called before the first frame update
        void Start()
        {
            road = new Road(start.transform.position, end.transform.position);
            InitRoadSegments();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitRoadSegments()
        {
            RoadSegment _segment = road.Start;

            while (_segment != null)
            {
                GameObject roadSegmentGFX = Instantiate(roadTile, _segment.Midpoint, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, _segment.Direction)), this.transform);
                roadSegmentGFX.transform.localScale = new Vector3(_segment.Length, roadSegmentGFX.transform.localScale.y, roadSegmentGFX.transform.localScale.z);

                roadSegments.Add(roadSegmentGFX);

                _segment = _segment.Next;
            }
        }
    }
}
