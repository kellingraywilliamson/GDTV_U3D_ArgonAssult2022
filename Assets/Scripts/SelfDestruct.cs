using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 3f;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
