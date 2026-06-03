using UnityEngine;

namespace Misc
{
    public class ParallaxLayer : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float parallaxSpeed = 0.1f;
        [SerializeField] private bool infiniteScrolling = false;

        private Camera cam;
        private float startPosX;
        private float tileWidth;      // total width of all child sprites combined
        private Transform[] sprites;  // direct children (the repeating tiles)

        private void Start()
        {
            cam = Camera.main;
            startPosX = transform.position.x;

            if (infiniteScrolling)
                InitTiling();
        }

        private void InitTiling()
        {
            sprites = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                sprites[i] = transform.GetChild(i);

            // measure one sprite's width via its SpriteRenderer
            if (sprites.Length > 0)
            {
                var sr = sprites[0].GetComponent<SpriteRenderer>();
                if (sr != null)
                    tileWidth = sr.bounds.size.x * sprites.Length;
                else
                    Debug.LogWarning($"ParallaxLayer: child '{sprites[0].name}' has no SpriteRenderer.");
            }
        }

        private void LateUpdate()
        {
            float offset = cam.transform.position.x * parallaxSpeed;
            transform.position = new Vector3(startPosX + offset, transform.position.y, transform.position.z);

            if (infiniteScrolling && tileWidth > 0)
                WrapSprites();
        }

        private void WrapSprites()
        {
            float halfTile = tileWidth / 2f;
            float camX = cam.transform.position.x;

            foreach (var sprite in sprites)
            {
                float dist = sprite.position.x - camX;

                if (dist < -halfTile)
                    sprite.position += new Vector3(tileWidth, 0f, 0f);
                else if (dist > halfTile)
                    sprite.position -= new Vector3(tileWidth, 0f, 0f);
            }
        }
    }
}

