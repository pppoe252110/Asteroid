using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 6;
    [SerializeField] private Color playerColor = Color.green;
    [SerializeField] private Color enemyColor = Color.red;

    [HideInInspector]
    public bool isEnemy;
    private SpriteRenderer renderer;

    private void OnEnable()
    {
        StartCoroutine(PoolDelayed());
    }

    IEnumerator PoolDelayed(float delay = 3)
    {
        yield return new WaitForSeconds(delay);
        Pool();
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void ApplyColor()
    {
        if(!renderer)
            renderer = GetComponent<SpriteRenderer>();
        if (isEnemy)
        {
            renderer.color = enemyColor;
        }
        else
        {
            renderer.color = playerColor;
        }
    }

    private void Pool()
    {
        StopCoroutine(PoolDelayed());
        PoolManager.projectilePool.Pool(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isEnemy)
        {
            if (collision.collider.TryGetComponent(out PlayerShip player))
            {
                player.Hit();
                Pool();
            }
        }
        else
        {
            if(collision.collider.TryGetComponent(out Asteroid asteroid))
            {
                asteroid.Split();
                Pool();
            }
        
            if(collision.collider.TryGetComponent(out UFO ufo))
            {
                ufo.Hit();
                Pool();
            }
        }
    }
}
