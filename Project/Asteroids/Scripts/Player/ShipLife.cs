using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShipLife : MonoBehaviour
{

    [SerializeField]
    private float InvincibilityDuration;
    [SerializeField]
    private float RespawnFlickerSpeed;

    public event Action OnKilled;

    private Image _img;
    private bool _Invincible = false;
    private float _flickerTimer;

    private void Awake() 
    {
        _img = GetComponent<Image>();
        StartCoroutine(nameof(Invincibility));
    } 

    private void Update()
    {
        if (_Invincible)
        {
            _flickerTimer += Time.deltaTime * RespawnFlickerSpeed;
            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, Mathf.PingPong(_flickerTimer, 1f));
        } else
        {
            _flickerTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") && !_Invincible)
            Kill();
    }

    IEnumerator Invincibility()
    {
        _Invincible = true;
        yield return new WaitForSeconds(InvincibilityDuration);
        _Invincible = false;
    }

    public void Kill()
    {
        OnKilled?.Invoke();
        Destroy(gameObject);
    }
}