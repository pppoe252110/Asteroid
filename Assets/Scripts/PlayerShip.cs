using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]private float acceleration = 2f;
    [SerializeField]private float angularSpeed = 2f;
    [SerializeField]private float maxSpeed = 6f;
    [SerializeField]private float drag = 1f;
    
    [Header("Shooting")]
    [SerializeField]private float shotsPerSecond = 3;
    [SerializeField]private GameObject projectile;
    [SerializeField]private Transform projectileSpawnPoint;
    [SerializeField]private AudioClip projectileShootSound;

    [Header("Health")]
    [SerializeField]private int maxHp = 4;
    [SerializeField]private float shieldTime = 3f;

    [Header("Effects")]
    [SerializeField] private float engineParticlesEmission = 50;
    [SerializeField] private ParticleSystem engineParticles;
    [SerializeField] private AudioSource moveSource;

    [HideInInspector]
    public int currentHp;
    private float shootTimer;
    private float shieldTimer;
    private Vector2 velocity;
    private SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        currentHp = maxHp;
    }

    private void Start()
    {
        ScreenBorders.AddInBorderTarget(new InBorderTarget(transform, transform.localScale / 2));
        shieldTimer = shieldTime;
    }

    public void Hit(int damage = 1)
    {
        shieldTimer = shieldTime;
        currentHp -= damage;
        PlayerHealth.onTakeDamage.Invoke();
        if(currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        PlayerHealth.onPlayerDead.Invoke();
    }

    private void Update()
    {
        if (shieldTimer > 0)
        {
            shieldTimer -= Time.deltaTime;
            renderer.color = Color.Lerp(new Color(1f, 1f, 1f, 0.5f), new Color(1f, 1f, 1f, 0), Mathf.PingPong(shieldTimer, 0.5f));
        }
        else
        {
            if (renderer.color != Color.white)
            {
                renderer.color = Color.white;
            }
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetKey(KeyCode.W) ? 1 : 0);

        if (input.y > 0)
        {
            if (!moveSource.isPlaying)
            {
                moveSource.Play();
            }
            velocity = Vector2.ClampMagnitude(transform.up * acceleration * Time.deltaTime + (Vector3)velocity, maxSpeed);
        }
        else
        {
            moveSource.Stop();
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, drag * Time.deltaTime);
        }

        var emission = engineParticles.emission;
        emission.rateOverTimeMultiplier = input.y * engineParticlesEmission;

        transform.position += (Vector3)velocity * Time.deltaTime;

        if (Menu.mouseInput)
        {
            Vector3 diff = Camera.allCameras[0].ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rot - 90), angularSpeed * Time.deltaTime * Mathf.Rad2Deg);
        }
        else
        {
            transform.rotation *= Quaternion.Euler(0, 0, -input.x * Mathf.Rad2Deg * angularSpeed * Time.deltaTime);
        }

        shootTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && shootTimer >= 1f/shotsPerSecond)
        {
            shootTimer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        Projectile projectile = PoolManager.projectilePool.Unpool();

        projectile.isEnemy = false;

        projectile.ApplyColor();

        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.up = transform.up;

        AudioManager.PlayAudioClip(projectileShootSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(shieldTimer <=0 && collision.collider.TryGetComponent(out Asteroid asteroid))
        {
            asteroid.Destroy();
            Hit();
        }
    }
}