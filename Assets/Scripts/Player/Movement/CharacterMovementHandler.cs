using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterControllerCustom))]
public class CharacterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerCustom _myCharacterController;

    NetworkMecanimAnimator _myCharacterAnimator;

    float _movementValue;

    private void Awake()
    {
        _myCharacterController = GetComponent<NetworkCharacterControllerCustom>();

        if (TryGetComponent(out LifeHandler myLifeHandler))
        {
            myLifeHandler.OnStateChange += SetControllerEnabled;
            myLifeHandler.OnRespawn += Respawn;
        }

        _myCharacterAnimator = GetComponent<NetworkMecanimAnimator>();
    }

    public override void FixedUpdateNetwork()
    {
        //if (Object.HasInputAuthority) return;

        if (GetInput(out NetworkInputData networkInputData))
        {
            //MOVIMIENTO

            Vector3 moveDirection = Vector3.forward * networkInputData.movementInput;

            _myCharacterController.Move(moveDirection);

            //SALTO

            if (networkInputData.isJumpPressed)
            {
                _myCharacterController.Jump();
            }

            //ANIMATOR

            _movementValue = Mathf.Abs(_myCharacterController.Velocity.x);


            //_movementValue = new Vector2(_myCharacterController.Velocity.x, _myCharacterController.Velocity.z).sqrMagnitude;

            //if (_movementValue > 1)
            //{
            //    _movementValue = 1;
            //}

            _myCharacterAnimator.Animator.SetFloat("MovementValue", _movementValue);
        }
    }

    void Respawn()
    {
        //Aquel que quiera agregar un Respawn con posiciones 'random' puede aplicar ese vector3 como parametro
        _myCharacterController.TeleportToPosition(transform.position);
    }

    void SetControllerEnabled(bool isEnabled)
    {
        _myCharacterController.Controller.enabled = isEnabled;
    }
}
