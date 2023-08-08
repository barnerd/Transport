using UnityEngine;

namespace BarNerdGames.Transport
{
    public class ResourceRenderer : MonoBehaviour
    {
        public Resource Resource { get; private set; }
        [SerializeField] private SpriteRenderer spriteRenderer;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void UpdateGraphics(Resource _resource)
        {
            Resource = _resource;

            //spriteRenderer.sprite = _resource.image;
            spriteRenderer.color = _resource.color;
        }
    }
}
