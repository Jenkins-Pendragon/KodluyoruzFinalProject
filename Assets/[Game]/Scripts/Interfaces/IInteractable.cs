using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsInteractable { get; set; }
    void OnInteractStart(Transform parent, Transform destination);
    void OnInteractEnd(Transform forceDirection);
}
