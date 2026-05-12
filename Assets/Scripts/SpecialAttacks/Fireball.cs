using Enemies;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float MagikaCost => magikaCost;
    
    [Header("Fireball Stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float magikaCost = 10f;
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
        transform.Translate(speed * Time.deltaTime * Vector3.right * direction);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            if (collider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(damage);
                animator.SetTrigger(OnHitAnimParam);
            }
        }
    }
}