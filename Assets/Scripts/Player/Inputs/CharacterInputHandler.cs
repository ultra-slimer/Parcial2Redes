using UnityEngine;
using Fusion;

//Clase que va a tomar los inputs del jugador siempre y cuando tenga autoridad para hacerlo
public class CharacterInputHandler : MonoBehaviour
{
    float _horizontalMovement;
    bool _isJumpPressed;
    bool _isFirePressed;
    bool _isCrouchPressed;
    [Networked(OnChanged = nameof(ToggleInputs))]
    public bool _canPlay { get; set; }
    private delegate void controls();
    private controls Controls = delegate { };


    NetworkInputData _inputData;

    void Start()
    {
        _inputData = new NetworkInputData();
        _canPlay = false;
    }

    void Update()
    {
        Controls(); 
        _horizontalMovement = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            _isJumpPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isCrouchPressed = true;
        }
    }

    public void ToggleInputs()
    {
        /*print("Toggle controllers");
        if (_canPlay)
        {
            Controls = delegate {

                _horizontalMovement = Input.GetAxis("Horizontal");

                if (Input.GetKeyDown(KeyCode.W))
                {
                    _isJumpPressed = true;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    _isFirePressed = true;
                }

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    _isCrouchPressed = true;
                }
            };
        }
        else
        {
            Controls = delegate { };
        }*/
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
