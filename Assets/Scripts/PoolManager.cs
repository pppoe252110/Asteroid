using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Asteroid asteroidPrefab;

    public static ObjectPooler<Projectile> projectilePool;
    public static ObjectPooler<Asteroid> asteroidPool;

    private void Awake()
    {
        projectilePool = new ObjectPooler<Projectile>(projectilePrefab);
        asteroidPool = new ObjectPooler<Asteroid>(asteroidPrefab);
    }

}
