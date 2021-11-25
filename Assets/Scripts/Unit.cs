using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    public int Price;
    public int Health;

    public NavMeshAgent _navMeshAgent;
    [SerializeField] GameObject _healthBarPrefab;

    private HealthBar _healthBar;
    private int _maxHealth;

    public override void Start()
    {
        base.Start();
        _maxHealth = Health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }
    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);
        _navMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if(Health <= 0)
        {
            // Убить юнита
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(FindObjectOfType<Management>())
            FindObjectOfType<Management>().Unselect(this);
        
        if (_healthBar)
        {
            Destroy(_healthBar.gameObject);
        }        
    }
}
