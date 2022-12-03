using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Catch.Utils;

public class Asteroids_GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject SpawnEntitiesInto;
    [SerializeField]
    private GameObject Ship;

    [Space]

    [SerializeField]
    private TextMeshProUGUI LivesText;
    [SerializeField]
    private int Lives;

    public GameObject PlayerShip { get { return _playerShip; } }

    private int _livesLeft;
    private GameObject _playerShip;

    public event Action OnGameOver;
    public static Asteroids_GameManager Instance;

    private void Awake() => Instance = this;

    private void Start()
    {
        Asteroids_UIManager.Instance.OnScreenChanged += (UIScreen screenChangedTo) => 
        {
            if (screenChangedTo == UIScreen.GAME_SCREEN) 
                Setup();
        };

        Asteroids_WaveManager.Instance.OnEnemySpawned += ParentEntity;
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
            GameOver();
        #endif
    }

    public void ParentEntity(GameObject entity)
    {
        var s = entity.transform.localScale;
        entity.transform.parent = SpawnEntitiesInto.transform;

        entity.transform.localScale = s;
    }

    private void SpawnPlayer()
    {
        var playerSpwnd = Instantiate(Ship, SpawnEntitiesInto.transform);
        _playerShip = playerSpwnd;
        playerSpwnd.GetComponent<ShipLife>().OnKilled += RemoveLife;
    }

    public void RemoveLife()
    {
        _livesLeft--;
        LivesText.SetText($"Lives: {_livesLeft.ToString("0")} ");

        if (_livesLeft > 0)
            SpawnPlayer();
        else
            GameOver();
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
        Clear();
    }

    private void Setup()
    {
        _livesLeft = Lives;
        LivesText.SetText("Lives: " + _livesLeft.ToString("0"));

        if (_livesLeft > 0)
            SpawnPlayer();
        else
            GameOver();
    }

    private void Clear()
    {
        UnityObjectPooler.Instance.ClearAllPools();

        if (SpawnEntitiesInto.transform.childCount > 0)
        {
            foreach (Transform child in SpawnEntitiesInto.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}