using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsInteractable { get;}
    void OnInteractStart(Transform parent, Transform destination);
    void OnInteractEnd(Transform forceDirection);
}
