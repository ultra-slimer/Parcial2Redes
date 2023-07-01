using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LifeHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnLifeChanged))]
    byte _currentLife { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    bool _isDead { get; set; }

    public bool IsDead => _isDead;

    const byte FULL_LIFE = 100;

    [SerializeField] GameObject _playerVisual;

    int _currentDeads;

    public event System.Action<bool> OnStateChange = delegate { };
    public event System.Action OnRespawn = delegate { };

    void Start()
    {
        _currentLife = FULL_LIFE;

        _isDead = false;
    }

    public void TakeDamage(byte dmg)
    {
        if (_isDead) return;

        if (dmg > _currentLife) dmg = _currentLife;

        _currentLife -= dmg;

        if (_currentLife == 0)
        {
            if (_currentDeads >= 1)
            {
                //Si no tenemos autoridad de input (Evitamos desconectar al Host)
                if (!Object.HasInputAuthority)
                    //Desconectamos del servidor al jugador
                    Runner.Disconnect(Object.InputAuthority);

                //Desconectamos de la red al objeto del jugador
                Runner.Despawn(Object);
            }
            else
            {
                StartCoroutine(Server_RespawnCO());

                _isDead = true;

                _currentDeads++;
            }
        }
    }

    static void OnLifeChanged(Changed<LifeHandler> changed)
    {

    }

    IEnumerator Server_RespawnCO()
    {
        yield return new WaitForSeconds(2f);

        Respawn();
    }

    void Respawn()
    {
        OnRespawn();
        _currentLife = FULL_LIFE;
        _isDead = false;
    }

    static void OnStateChanged(Changed<LifeHandler> changed)
    {
        bool isDeadCurrent = changed.Behaviour.IsDead;

        changed.LoadOld();

        bool isDeadBefore = changed.Behaviour.IsDead;

        if (isDeadCurrent)
        {
            changed.Behaviour.Death();
        }
        else if (!isDeadCurrent && isDeadBefore)
        {
            changed.Behaviour.Revive();
        }
    }

    void Death()
    {
        _playerVisual.SetActive(false);
        //GetComponent<HitboxRoot>().HitboxRootActive = false;
        OnStateChange(false);
    }

    void Revive()
    {
        _playerVisual.SetActive(true);
        //GetComponent<HitboxRoot>().HitboxRootActive = true;
        OnStateChange(true);
    }

    

    
}
