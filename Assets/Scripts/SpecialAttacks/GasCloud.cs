using UnityEngine;
using System.Collections;
using Player;

namespace SpecialAttacks
{
    public class GasCloud : MonoBehaviour
    {
        [Header("Collider Expansion")]
        [SerializeField] private float maxRadius = 0.75f;
        [SerializeField] private float expandDuration = 0.75f;
        [Header("Damage")]
        //[SerializeField] private float damagePerSecond = 10f;

        private CircleCollider2D gasCloudCollider;
        private int facingDir;

        private void Awake()
        {
            gasCloudCollider = GetComponentInParent<CircleCollider2D>();
        }

        private void Start()
        {
            StartCoroutine(ExpandCloud());
        }

        public void Init(int dir) => facingDir = dir;

        private IEnumerator ExpandCloud()
        {
            float t = 0f;
            gasCloudCollider.radius = 0f;
            gasCloudCollider.offset = new Vector2(0f, gasCloudCollider.offset.y);

            while (t < expandDuration)
            {
                t += Time.deltaTime;
                float progress = t / expandDuration;

                gasCloudCollider.radius = Mathf.Lerp(0f, maxRadius, progress);
                gasCloudCollider.offset = new Vector2(
                    Mathf.Lerp(0.1f, 0.5f, progress) * facingDir,
                    gasCloudCollider.offset.y
                );

                yield return null;
            }

            gasCloudCollider.radius = maxRadius;
            gasCloudCollider.offset = new Vector2(0.5f * facingDir, gasCloudCollider.offset.y);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            print("Player takes dot from gas cloud.");
        }
    }
}