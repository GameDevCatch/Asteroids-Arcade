using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catch.Utils;

public class ShipGun : MonoBehaviour
{

    [SerializeField]
    private GameObject BulletPrefab;
    [SerializeField]
    private Transform GunTrans;

    [Space]

    [SerializeField]
    private float DelayBetweenShots;
    [SerializeField]
    private int BulletPoolSize;

    private ShipInput _playerInput;
    private bool _isReloading;

    private void Awake() 
    { 
        _playerInput = GetComponent<ShipInput>();
        UnityObjectPooler.Instance.WarmPool(BulletPrefab, 10, 100);
    }

    private void OnEnable()
    {
        if (_playerInput != null)
            _playerInput.OnShootKeyDown += Shoot;
    }

    private void OnDisable()
    {
        if (_playerInput != null)
            _playerInput.OnShootKeyDown -= Shoot;
    }

    private void Shoot()
    {
        if (_isReloading) return;

        var bullet = UnityObjectPooler.Instance.Spawn(BulletPrefab, GunTrans.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z));
        Asteroids_GameManager.Instance.ParentEntity(bullet);
        bullet.GetComponent<Bullet>().Fire();
        StartCoroutine(Reload(DelayBetweenShots));
    }

    private IEnumerator Reload(float time)
    {
        _isReloading = true;
        yield return new WaitForSeconds(time);
        _isReloading = false;
    }
}