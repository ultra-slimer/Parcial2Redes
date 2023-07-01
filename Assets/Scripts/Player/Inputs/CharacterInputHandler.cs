using UnityEngine;

//Clase que va a tomar los inputs del jugador siempre y cuando tenga autoridad para hacerlo
public class CharacterInputHandler : MonoBehaviour
{
    float _horizontalMovement;
    bool _isJumpPressed;
    bool _isFirePressed;
    bool _isCrouchPressed;


    NetworkInputData _inputData;

    void Start()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        //Tomo todos los Inputs

        _horizontalMovement = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            _isJumpPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isCrouchPressed = true;
        }

        
    }

    public NetworkInputData GetNetworkInputs()
    {
        _inputData.movementInput = _horizontalMovement;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isCrouchPressed = _isCrouchPressed;
        _isCrouchPressed = false;

        return _inputData;
    }
}
