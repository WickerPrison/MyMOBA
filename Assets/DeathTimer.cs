using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    [SerializeField] float timeToDeath;

    private void Update()
    {
        timeToDeath = Time.deltaTime;
        if(timeToDeath <= 0)
        {
            Destroy(gameObject);
        }
    }

}
