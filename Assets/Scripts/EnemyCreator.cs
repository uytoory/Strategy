using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] Transform _spawn;
    [SerializeField] float _randomCreationPeriod = 9;
    [SerializeField] float _timerToRespawn = 10;
    [SerializeField] GameObject _enemyPrefab;

    private float _timer;
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _timerToRespawn)
        {
            _timer = Random.Range(0,_randomCreationPeriod);
            Instantiate(_enemyPrefab, _spawn.position, _spawn.rotation);
        }
    }
}
