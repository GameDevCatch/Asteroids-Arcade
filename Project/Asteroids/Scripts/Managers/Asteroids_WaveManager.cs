using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using Catch.Utils;

public class Asteroids_WaveManager : MonoBehaviour
{
    [Serializable]
    public struct Wave
    {
        public GameObject enemyPrefab;
        public int numOfEnemys;
    }

    [SerializeField]
    private Wave[] Waves;
    [SerializeField]
    private float SpawnRadius = 60;

    public int _nextWaveIndex = 0;
    private int _enemysAlive;

    private readonly List<IEnemy> _enemiesSpawned = new();

    public event Action<GameObject> OnEnemySpawned;

    public static Asteroids_WaveManager Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        Asteroids_UIManager.Instance.OnScreenChanged += (UIScreen screenChangedTo) =>
        {
            if (screenChangedTo == UIScreen.GAME_SCREEN)
                SetUp();
        };
    }

    private void SetUp()
    {
        _nextWaveIndex = 0;
        _enemysAlive = 0;
        NextWave();
    }

    private void NextWave()
    {
        var waveToSpawn = Waves[_nextWaveIndex];

        UnityObjectPooler.Instance.WarmPool(waveToSpawn.enemyPrefab, waveToSpawn.numOfEnemys + 5, 100);

        for (int i = 0; i < waveToSpawn.numOfEnemys; i++)
        {
            var enemySpwnd = UnityObjectPooler.Instance.Spawn(waveToSpawn.enemyPrefab, Vector3.zero, Quaternion.identity);
            var randPos = (Vector3)Random.insideUnitCircle.normalized * SpawnRadius;

            OnEnemySpawned?.Invoke(enemySpwnd);

            enemySpwnd.transform.localPosition = randPos;

            var IEnemy = enemySpwnd.GetComponent<IEnemy>();

            IEnemy.OnKilled += EnemyKilled;
            IEnemy.OnKilled += Asteroids_ScoreManager.Instance.OnScored;

            _enemiesSpawned.Add(IEnemy);
            _enemysAlive++;
        }

        if (_nextWaveIndex < Waves.Length - 1)
            _nextWaveIndex++;
    }

    private void EnemyKilled(IEnemy enemy, IEnemy[] EnemiesSpawnedAfter)
    {
        _enemysAlive--;
        _enemiesSpawned.Remove(enemy);

        if (EnemiesSpawnedAfter != null)
        {
            for (int i = 0; i < EnemiesSpawnedAfter.Length; i++)
            {
                var enemySpwnd = EnemiesSpawnedAfter[i];

                enemySpwnd.OnKilled += EnemyKilled;
                enemySpwnd.OnKilled += Asteroids_ScoreManager.Instance.OnScored;

                _enemiesSpawned.Add(enemySpwnd);
                OnEnemySpawned?.Invoke(enemySpwnd.Myself);

                _enemysAlive++;
            }
        }

        if (_enemysAlive <= 0)
            NextWave();
    }
}