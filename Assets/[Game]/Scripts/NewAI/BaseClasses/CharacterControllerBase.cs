using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterControllerBase : MonoBehaviour, ICharacterController
{

    private Character character;
    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public float GetCurrentSpeed()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}
