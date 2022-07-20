using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AsteroidStates asteroidStates;
    private int state;

    private void Start()
    {
        ScreenBorders.AddInBorderTarget(new InBorderTarget(transform, transform.localScale / 2));
    }

    private void Update()
    {
        transform.position += transform.up * asteroidStates.asteroidStates[state].Speed * Time.deltaTime;
    }

    public void SetInitState()
    {
        SetState(asteroidStates.GetInitAsteroidState());
    }

    public void SetState(int state)
    {
        this.state = state;
        RecalculateState();

    }

    private void RecalculateState()
    {
        transform.localScale = Vector3.one * asteroidStates.asteroidStates[state].Scale;
    }

    private void SplitAsteroid()
    {
        Asteroid splittedAsteroid = PoolManager.asteroidPool.Unpool();

        EntitySpawner.AsteroidsAlive++;

        splittedAsteroid.SetState(state - 1);
        splittedAsteroid.transform.position = transform.position;
        splittedAsteroid.transform.rotation = Quaternion.Euler(0, 0, EntitySpawner.Remap(Random.Range(0, 2), 0, 1, -1, 1) * asteroidStates.asteroidStates[state - 1].SplitAngle + transform.eulerAngles.z);
    }

    public void Destroy()
    {
        EntitySpawner.AsteroidsAlive--;
        EntitySpawner.ResetAsteroidTimer();
        PoolManager.asteroidPool.Pool(this);
    }

    public void Split()
    {
        AudioManager.PlayAudioClip(asteroidStates.asteroidStates[state].explosionSound);
        if (state > 0)
        {
            SplitAsteroid();
            state--;
            RecalculateState();
        }
        else
        {

            EntitySpawner.AsteroidsAlive--;
            EntitySpawner.ResetAsteroidTimer();
            PoolManager.asteroidPool.Pool(this);
        }
        Score.AddScore(asteroidStates.asteroidStates[state].Score);
    }
}
