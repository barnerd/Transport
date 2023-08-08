using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace BarNerdGames.Transport
{
    public class ResourceContainer : MonoBehaviour
    {
        [SerializedDictionary("ResourceType", "Resources")] private SerializedDictionary<Resource, List<Resource>> resources;
        [SerializeField] private GameObject resourceRendererPrefab;

        [SerializeField] private Vector2 Spacing;
        [SerializeField] private Vector2 ResourceSize;
        [SerializeField] private Vector2Int GridSize;

        void Awake()
        {
            if (resources == null) resources = new SerializedDictionary<Resource, List<Resource>>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // TODO: add this back in when loading from a save file
            // RefreshGraphics();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RefreshGraphics(SerializedDictionary<Resource, float> _resources)
        {
            // Clear all
            ClearAll();

            // Display all
            int x = 0, y;
            foreach (var _resouce in _resources.Keys)
            {
                y = 0;
                for (int i = 0; i < Mathf.RoundToInt(_resources[_resouce]); i++)
                {
                    // Create Prefab
                    GameObject _new = Instantiate(resourceRendererPrefab, transform);

                    // move to x, y
                    _new.transform.localPosition += new Vector3(-x * (ResourceSize.x + Spacing.x), y * (ResourceSize.y + Spacing.y), 0);

                    _new.GetComponent<ResourceRenderer>().UpdateGraphics(_resouce);

                    // move to next spot in the grid
                    y++;
                }
                x++;
            }
        }

        public void RefreshGraphics(List<Resource> _resources)
        {
            // Clear all
            ClearAll();

            // Display all
            int x = 0, y = 0;
            foreach (var _resouce in _resources)
            {
                // Create Prefab
                GameObject _new = Instantiate(resourceRendererPrefab, transform);

                // move to x, y
                _new.transform.localPosition += new Vector3(x * (ResourceSize.x + Spacing.x), y * (ResourceSize.y + Spacing.y), 0);

                _new.GetComponent<ResourceRenderer>().UpdateGraphics(_resouce);

                // move to next spot in the grid
                y++;

                if (y >= GridSize.y)
                {
                    y = 0;
                    x++;
                }
            }
        }

        private void ClearAll()
        {
            ResourceRenderer[] _children = GetComponentsInChildren<ResourceRenderer>();
            for (int i = _children.Length - 1; i >= 0; i--)
            {
                Destroy(_children[i].gameObject);
            }
        }
    }
}
