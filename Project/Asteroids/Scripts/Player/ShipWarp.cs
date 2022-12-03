using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWarp : MonoBehaviour
{

    [SerializeField]
    private float CooldownDelay;
    [Range(0, 1)]
    [SerializeField]
    private float ChanceToExplode;

    private ShipInput _playerInput;
    private ShipLife _playerLife;

    private bool _warpReady = true;

    private RectTransform _screen;
    private RectTransform _rect;

    private void Awake()
    {
        _screen = gameObject.FindParentCanvas().GetComponent<RectTransform>();
        _rect = GetComponent<RectTransform>();
        _playerInput = GetComponent<ShipInput>();
        _playerLife = GetComponent<ShipLife>();
    }

    private void OnEnable()
    {
        if (_playerInput != null)
            _playerInput.OnWarpKeyDown += Warp;
    }

    private void OnDisable()
    {
        if (_playerInput != null)
            _playerInput.OnWarpKeyDown -= Warp;
    }

    private void Warp()
    {
        if (!_warpReady) return;

        if (Random.value > ChanceToExplode)
        {
            var randPos = new Vector2(Random.Range(-(_screen.rect.width / 2), (_screen.rect.width / 2)),
                                      Random.Range(-(_screen.rect.height / 2), (_screen.rect.height / 2)));
            transform.localPosition = new Vector2(randPos.x, randPos.y);
            _warpReady = false;

            StartCoroutine(Cooldown(CooldownDelay));
        }
        else
            SelfDestruct();
    }

    private IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        _warpReady = true;
    }

    private void SelfDestruct()
    {
        _playerLife.Kill();
    }
}
