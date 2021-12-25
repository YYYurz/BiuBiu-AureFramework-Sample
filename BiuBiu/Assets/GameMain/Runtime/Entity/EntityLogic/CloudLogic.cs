using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudLogic : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down * Time.unscaledDeltaTime, Space.Self);
    }
}
