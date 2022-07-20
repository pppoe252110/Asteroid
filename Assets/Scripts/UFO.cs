using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private int score = 200;
    [SerializeField] private float minShootRate = 2;
    [SerializeField] private float maxShootRate = 4;
    [SerializeField] private PlayerShip player;
    [SerializeField] private float projectileSpeed = 3;
    [SerializeField] private AudioClip projectileShootSound;

    private float shootTimer;
    private float shootDelay;
    private float direction = 1;

    private void Start()
    {
        ScreenBorders.AddInBorderTarget(new InBorderTarget(transform, transform.localScale / 2));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out Asteroid asteroid))
        {
            EntitySpawner.ufoAvailible = true;
            EntitySpawner.OnUfoHit();
        }
    }

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    public void Hit()
    {
        Score.AddScore(score);

        EntitySpawner.OnUfoHit();
    }

    public void Shoot()
    {
        Vector2 dir = player.transform.position - transform.position;
        dir.Normalize();

        Projectile projectile = PoolManager.projectilePool.Unpool();

        projectile.isEnemy = true;

        projectile.ApplyColor();

        projectile.transform.position = transform.position;
        projectile.transform.up = dir;

        AudioManager.PlayAudioClip(projectileShootSound);
    }

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime * direction;

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootDelay)
        {
            shootTimer = 0;
            shootDelay = Random.Range(minShootRate, maxShootRate);
            Shoot();
        }
    }
}
