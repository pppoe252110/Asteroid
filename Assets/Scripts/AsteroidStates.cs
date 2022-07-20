using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidStates", menuName = "ScriptableObjects/AsteroidStates", order = 1)]
public class AsteroidStates : ScriptableObject
{
    public AsteroidState[] asteroidStates;

    public int GetInitAsteroidState()
    {
        return asteroidStates.Length - 1;
    }
}
[System.Serializable]
public class AsteroidState
{
    public float Scale = 1;
    public float Speed = 3;
    public int Score = 100;
    public float SplitAngle = 3;
    public AudioClip explosionSound;
}