using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade : MonoBehaviour, IInteractable
{

    [SerializeField]
    private GameObject GamePrefab;
    [SerializeField]
    private Transform GameSpawnPoint;
    [SerializeField]
    private Transform PlayerPositionOnPlay;
    [SerializeField]
    private Quaternion PlayerLookRotationOnPlay;

    [Space]

    [SerializeField]
    private KeyCode ExitKey;

    public bool IsPlayerPlaying { get { return _isPlayerPlaying; } }
    private bool _isPlayerPlaying;

    private GameObject _player;
    private FPSController _fpsController;
    private Interact _interact;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _fpsController = _player.GetComponent<FPSController>();
        _interact = _player.GetComponent<Interact>();

        if (GamePrefab != null)
        {
            var g = Instantiate(GamePrefab, GameSpawnPoint.position, Quaternion.identity);
            g.transform.parent = transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(ExitKey))
            Exit();
    }

    public void OnInteract()
    {
        Play();
    }

    public void Play()
    {
        _player.transform.position = PlayerPositionOnPlay.position;
        _player.transform.rotation = Quaternion.identity;

        _interact.CanInteract(false);
        _fpsController.SetLookPosition(PlayerLookRotationOnPlay);
        _fpsController.Disable();

        _isPlayerPlaying = true;
    }

    public void Exit()
    {
        _interact.CanInteract(true);
        _fpsController.Enable();

        _isPlayerPlaying = false;
    }
}