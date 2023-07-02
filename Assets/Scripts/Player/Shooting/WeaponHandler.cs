using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _firingTransform;
    [SerializeField] ParticleSystem _shootParticle;
    private NetworkPlayer local;

    [Networked(OnChanged = nameof(OnFiringChange))]
    bool IsFiring { get; set; }

    float _lastFireTime;

    LifeHandler _lifeHandler;

    private void Awake()
    {
        _lifeHandler = GetComponent<LifeHandler>();
        local = GetComponent<NetworkPlayer>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_lifeHandler.IsDead || !local.HasInputAuthority) return;

        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFirePressed)
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        if (Time.time - _lastFireTime < 0.15f) return;

        _lastFireTime = Time.time;

        StartCoroutine(COR_Fire());

        Runner.Spawn(_bulletPrefab, _firingTransform.position, transform.rotation);

        //No -> Physics.Raycast
        /*
        Runner.LagCompensation.Raycast(origin: _firingTransform.position, 
                                       direction: _firingTransform.forward, 
                                       length: 100, 
                                       player: Object.InputAuthority,
                                       hit: out var hitInfo);

        if (hitInfo.Hitbox != null)
        {
            if (Object.HasStateAuthority)
            {
                hitInfo.Hitbox.transform.root.GetComponent<LifeHandler>()?.TakeDamage(25);
            }
        }
        */
    }

    IEnumerator COR_Fire()
    {
        IsFiring = true;

        yield return new WaitForSeconds(0.15f);

        IsFiring = false;
    }

    static void OnFiringChange(Changed<WeaponHandler> changed)
    {
        bool isFiringCurrent = changed.Behaviour.IsFiring;

        changed.LoadOld();

        bool isFiringOld = changed.Behaviour.IsFiring;

        if (isFiringCurrent && !isFiringOld)
        {
            changed.Behaviour.FireRemote();
        }
    }

    void FireRemote()
    {
        _shootParticle.Play();
    }
}
