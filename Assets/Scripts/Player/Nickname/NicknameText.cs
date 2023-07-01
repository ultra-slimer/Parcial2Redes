using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    Transform _owner;

    const float _headOffset = 2.5f;

    Text _myText;

    public NicknameText SetOwner(NetworkPlayer owner)
    {
        _myText = GetComponent<Text>();
        _owner = owner.transform;
        return this;
    }

    public void UpdateText(string str)
    {
        _myText.text = str;
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * _headOffset;
    }
}
