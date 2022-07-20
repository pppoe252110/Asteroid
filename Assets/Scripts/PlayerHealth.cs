using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerShip player;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private Transform healthContainer;
    private List<GameObject> currentHealth;
    public static UnityEvent onTakeDamage;
    public static UnityEvent onPlayerDead;

    private void Awake()
    {
        currentHealth = new();
        onTakeDamage = new UnityEvent();
        onTakeDamage.AddListener(delegate { UpdateHealth(); });
        onPlayerDead = new UnityEvent();
    }
    private void Start()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        for (int i = 0; i < currentHealth.Count; i++)
        {
            Destroy(currentHealth[i]);
        }
        currentHealth.Clear();
        
        for (int i = 0; i < player.currentHp; i++)
        {
            GameObject health = Instantiate(healthPrefab, healthContainer);
            currentHealth.Add(health);
        }
    }
}
