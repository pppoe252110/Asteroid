using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private AsteroidStates asteroidStates;
    [SerializeField] private float asteroidSpawnDelay = 2;
    [SerializeField] private float minUfoSpawnDelay = 20;
    [SerializeField] private float maxUfoSpawnDelay = 40;
    [SerializeField] private UFO targetUfo;

    public static int AsteroidsAlive = 0;
    private int asteroidSpawnAmount = 2;

    private static float asteroidSpawnTimer = 0;

    private static UnityEvent ufoHitEvent;

    public static bool ufoAvailible = true;
    private static float ufoSpawnTimer = 0;
    private static float ufoSpawnDelay = 0;

    void Start()
    {
        ufoAvailible = true;
        AsteroidsAlive = 0;
        ufoSpawnTimer = 0;
        ufoHitEvent = new UnityEvent();
        AsteroidLogic();
        ufoSpawnDelay = Random.Range(minUfoSpawnDelay, maxUfoSpawnDelay);
        ufoHitEvent.AddListener(delegate { HideUfo(); });

        HideUfo();
    }

    public static void OnUfoHit()
    {
        ufoHitEvent.Invoke();
    }

    public static void ResetAsteroidTimer()
    {
        asteroidSpawnTimer = 0;
    }

    public void HideUfo()
    {
        ufoAvailible = true;
        targetUfo.gameObject.SetActive(false);
    }

    private void ShowUfo()
    {
        targetUfo.gameObject.SetActive(true);
        targetUfo.SetDirection(Remap(Random.Range(0, 2), 0, 1, -1, 1));
        targetUfo.transform.position = GetRandomSpawnPoint(0.8f,1,0);
    }

    private void Update()
    {
        asteroidSpawnTimer += Time.deltaTime;
        if (AsteroidsAlive <= 0 && asteroidSpawnTimer >= asteroidSpawnDelay)
        {
            asteroidSpawnTimer = 0;
            asteroidSpawnAmount++;
            AsteroidLogic();
        }
        if (ufoAvailible)
        {
            ufoSpawnTimer += Time.deltaTime;
            if (ufoSpawnTimer >= ufoSpawnDelay)
            {
                ufoSpawnDelay = Random.Range(minUfoSpawnDelay, maxUfoSpawnDelay);
                ufoSpawnTimer = 0;
                ufoAvailible = false;
                ShowUfo();
            }
        }
    }

    public void AsteroidLogic()
    {
        for (int i = 0; i < asteroidSpawnAmount; i++)
        {
            SpawnAsteroid();
        }
    }

    private Vector2 GetRandomSpawnPoint(float maxHeight = 1, float maxWeight = 1, int side = 1)
    {
        Vector2 targetPosition = Vector2.zero;
        float randomSide = Remap(Random.Range(0, 2), 0, 1, -1, 1);
        if (side == 2 || (side == 1 && Random.Range(0, 2) == 0))//top bottom
        {
            targetPosition = new Vector2(Random.Range(-ScreenBorders.screenBorder.x*maxWeight, ScreenBorders.screenBorder.x * maxWeight), randomSide * ScreenBorders.screenBorder.y);
        }
        else if(side == 0||side == 1)//left right
        {
            targetPosition = new Vector2(randomSide * ScreenBorders.screenBorder.x, Random.Range(-ScreenBorders.screenBorder.y* maxHeight, ScreenBorders.screenBorder.y * maxHeight));
        }
        return targetPosition;
    }

    public void SpawnAsteroid()
    {
        int initState = asteroidStates.GetInitAsteroidState();
        float scale = asteroidStates.asteroidStates[initState].Scale;

        Vector2 targetPosition = GetRandomSpawnPoint();
        targetPosition += new Vector2(Mathf.Sign(targetPosition.x), Mathf.Sign(targetPosition.y)) * scale;

        Asteroid asteroid = PoolManager.asteroidPool.Unpool();

        AsteroidsAlive++;

        asteroid.SetInitState();

        asteroid.transform.position = targetPosition;
        asteroid.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
        while(asteroid.transform.eulerAngles.z / 90==0)
        {
            asteroid.transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
        }
    }


    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
