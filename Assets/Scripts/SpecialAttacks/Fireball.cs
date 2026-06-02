using Enemies;
using UnityEngine;

namespace SpecialAttacks
{
    public class Fireball : MonoBehaviour
    {
        public float ManaCost => manaCost;
    
        [Header("Fireball Stats")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float damage = 20f;
        [SerializeField] private float manaCost = 10f;
        [SerializeField] private float lifetime = 5f;

        private float direction;
        private GameObject player;
        private Animator animator;
        private static readonly int OnHitAnimParam = Animator.StringToHash("HitOnTarget");

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start()
        {
            if (player == null)
            {
                Destroy(gameObject);
                return;
            }

            // Derive facing direction from player scale, then apply only to fireball's own X
            direction = Mathf.Sign(player.transform.localScale.x);
            transform.localScale = new Vector3(
                direction * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );

            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime * direction));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;
            if (!other.TryGetComponent<Enemy>(out var enemy)) return;
            
            enemy.TakeDamage(damage);
            animator.SetTrigger(OnHitAnimParam);
        }
    }
}