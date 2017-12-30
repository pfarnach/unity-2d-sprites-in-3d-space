using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSettings : MonoBehaviour {

	void Awake()
    {
        // Sort sprites by where they are on the z-axis rather than by camera perspective
        Camera cam = Camera.main;
        cam.transparencySortMode = TransparencySortMode.CustomAxis;
        cam.transparencySortAxis = new Vector3(0f, 0f, 1f);
    }

}
