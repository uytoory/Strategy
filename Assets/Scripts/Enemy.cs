using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour
{

    public EnemyState CurrentEnemyState;
    public int Health = 5;
    [SerializeField] Building _targetBuilding;
    [SerializeField] Unit _targetUnit;
    [SerializeField] float _distanceToFollow = 7f;
    [SerializeField] float _distanceToAttack = 1f;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] float _attackPeriod = 1f;

    private float _timer;
    private int _maxHealth;

    [SerializeField] GameObject _healthBarPrefab;
    private HealthBar _healthBar;


    void Start()
    {
        SetState(EnemyState.WalkToBuilding);

        _maxHealth = Health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEnemyState == EnemyState.Idle)
        {
            FindClosestBuilding();
            if (_targetBuilding)
            {
                SetState(EnemyState.WalkToBuilding);
            }
            FindClosestUnit();
        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestUnit();
            if (_targetBuilding == null)
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (_targetUnit)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > _distanceToFollow)
                {
                    SetState(EnemyState.WalkToBuilding);
                }
                if (distance < _distanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            if (_targetUnit)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > _distanceToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > _attackPeriod)
                {
                    _timer = 0f;
                    //Нанести урон юниту
                    _targetUnit.TakeDamage(1);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle)
        {

        }
        else if (CurrentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestBuilding();
            if (_targetBuilding)
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }

        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {

        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            _timer = 0f;
        }
    }

    public void FindClosestBuilding()
    {
        Building[] AllBuildings = FindObjectsOfType<Building>();

        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;
        for (int i = 0; i < AllBuildings.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, AllBuildings[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = AllBuildings[i];
            }
        }
        _targetBuilding = closestBuilding;
    }

    public void FindClosestUnit()
    {
        Unit[] AllUnits = FindObjectsOfType<Unit>();

        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;
        for (int i = 0; i < AllUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, AllUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestUnit = AllUnits[i];
            }
        }
        if (minDistance < _distanceToFollow)
        {
            _targetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
    }

    public void TakeDamage(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_healthBar)
        {
            Destroy(_healthBar.gameObject);
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);

    }
#endif
}


