using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerOVRCameraRig : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
