using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Catch.Utils;

public interface IEnemy
{
    public int PointsGiven { get; }
    public GameObject Myself { get; }

    public void Kill();
    public event Action<IEnemy, IEnemy[]> OnKilled;
}

public class Asteroid : MonoBehaviour, IEnemy
{

    [SerializeField]
    private GameObject SplitInto;
    [SerializeField]
    private int numOfSplits;

    [Space]

    [SerializeField]
    private float Speed;
    [SerializeField]
    private bool CanSplit;
    [SerializeField]
    private int PointsForKill;

    public event Action<IEnemy, IEnemy[]> OnKilled;

    public int PointsGiven { get { return PointsForKill; } }
    public GameObject Myself { get { return gameObject; } }

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        if (numOfSplits > 0)
            UnityObjectPooler.Instance.WarmPool(SplitInto, 100, 100);
    }

    private void OnEnable()
    {
        MoveInRandomDir();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
            Kill();
    }

    private void MoveInRandomDir()
    {
        var randDir = UnityEngine.Random.insideUnitCircle.normalized;
        _rb.AddForce(randDir * Speed);
    }

    public void Kill()
    {
        if (CanSplit)
            Split();
        else
            OnKilled?.Invoke(this, null);

        UnityObjectPooler.Instance.Release(gameObject);
    }

    private void Split()
    {
        List<IEnemy> splitAsteroids = new List<IEnemy>();

        for (int i = 0; i < numOfSplits; i++)
        {
           var astr = UnityObjectPooler.Instance.Spawn(SplitInto, transform.position, Quaternion.identity);
           splitAsteroids.Add(astr.GetComponent<IEnemy>());
        }

        OnKilled?.Invoke(this, splitAsteroids.ToArray());
    }
}