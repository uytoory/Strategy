using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}

public class Knight : Unit
{

    public UnitState CurrentUnitState;

    [SerializeField] Vector3 _targetpoint;
    [SerializeField] Enemy _targetEnemy;
    [SerializeField] float _distanceToFollow = 7f;
    [SerializeField] float _distanceToAttack = 1f;

    [SerializeField] float _attackPeriod = 1f;

    private float _timer;


    public override void Start()
    {
        base.Start();
        SetState(UnitState.WalkToPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentUnitState == UnitState.Idle)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            if (_targetEnemy)
            {
                _navMeshAgent.SetDestination(_targetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                if (distance > _distanceToFollow)
                {
                    SetState(UnitState.WalkToPoint);
                }
                if (distance < _distanceToAttack)
                {
                    SetState(UnitState.Attack);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            if (_targetEnemy)
            {
                _navMeshAgent.SetDestination(_targetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                if (distance > _distanceToAttack)
                {
                    SetState(UnitState.WalkToEnemy);
                }
                _timer += Time.deltaTime;
                if (_timer > _attackPeriod)
                {
                    _timer = 0f;
                    //Нанести урон юниту
                    _targetEnemy.TakeDamage(1);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }
        }
    }

    public void SetState(UnitState unitState)
    {
        CurrentUnitState = unitState;
        if (CurrentUnitState == UnitState.Idle)
        {

        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {

        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {

        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            _timer = 0f;
        }
    }



    public void FindClosestEnemy()
    {
        Enemy[] AllEnemies = FindObjectsOfType<Enemy>();

        float minDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        for (int i = 0; i < AllEnemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, AllEnemies[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = AllEnemies[i];
            }
        }
        if (minDistance < _distanceToFollow)
        {
            _targetEnemy = closestEnemy;
            SetState(UnitState.WalkToEnemy);
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
