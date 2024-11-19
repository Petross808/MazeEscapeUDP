using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    [SerializeField] private GameEvent _interactEvent; 

    public void OnInteract()
    {
        _interactEvent.Raise(this);
    }
}
