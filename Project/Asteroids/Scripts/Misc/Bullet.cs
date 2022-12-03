using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catch.Utils;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private float destroyDelay;

    private Rigidbody2D _rb;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
            DestroySelf();
    }

    public void Fire()
    {
        _rb.AddForce(transform.up * speed);
        Invoke(nameof(DestroySelf), destroyDelay);
    }

    private void DestroySelf()
    {
        UnityObjectPooler.Instance.Release(gameObject);
        CancelInvoke(nameof(DestroySelf));
        _rb.velocity = Vector2.zero;
    }
}