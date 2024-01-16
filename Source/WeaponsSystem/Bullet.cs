using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PoolableObject    
{
    [SerializeField] private float _autoDestroyTime = 5f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _damage = 50;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private TrailConfigurationSO _trailConfigurationSO;

    protected TrailRenderer trailRenderer;
    protected Transform target;

    [SerializeField] private Renderer _renderer;

    private bool _isDisabling = false;

    protected const string DISABLE_METHOD_NAME = "Disable";
    protected const string DO_DISABLE_METHOD_NAME = "DoDisable";

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        if (_renderer != null)
        {
            _renderer.enabled = true;
        }

        _isDisabling = false;
        CancelInvoke(DISABLE_METHOD_NAME);
        ConfigureTrail();
        Invoke(DISABLE_METHOD_NAME, _autoDestroyTime);
    }

    private void ConfigureTrail()
    {
        if (trailRenderer != null && _trailConfigurationSO != null)
        {
            _trailConfigurationSO.SetupTrail(trailRenderer);
        }
    }


    public virtual void Spawn(Vector3 forward, int damage, Transform target)
    {
        this._damage = damage;
        this.target = target;
        _rigidBody.AddForce(forward * _moveSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.collider.GetComponent<IDamageable>();
            damageable.TakeDamage(_damage);
        }


        Disable();

    }

    private void Disable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        CancelInvoke(DO_DISABLE_METHOD_NAME);
        _rigidBody.velocity = Vector3.zero;
        if (_renderer != null)
        {
            _renderer.enabled = false;
        }

        if (trailRenderer != null && _trailConfigurationSO != null)
        {
            _isDisabling = true;
            Invoke(DO_DISABLE_METHOD_NAME, _trailConfigurationSO.Time);
        }
        else
        {
            DoDisable();
        }
    }

    private void DoDisable()
    {
        if (trailRenderer != null && _trailConfigurationSO != null)
        {
            trailRenderer.Clear();
        }

        gameObject.SetActive(false);
    }
}
