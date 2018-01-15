using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

    public event System.Action<float> rotateCamera;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 initialTargetOffset;

    [SerializeField]
    private float cameraLerpSpeed = 1f;

    [SerializeField]
    private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Vector3 currentTargetOffset;
    private Camera cam;
    private bool isRotating;



    void Start ()
    {
        cam = Camera.main;
        currentTargetOffset = initialTargetOffset;
    }
	
	void LateUpdate () 
    {
        if (!isRotating)
        {
            Vector3 targetPos = target.position + currentTargetOffset;

            // Camera
            if ((targetPos - transform.position).magnitude > .05)
                StartCoroutine(UpdateCameraPosSmooth(targetPos));
                //transform.position = targetPos;


            // Handle camera rotation
            if (Input.GetKeyDown(KeyCode.E))
                DoRotate(true);
            else if (Input.GetKeyDown(KeyCode.Q))
                DoRotate(false);    
        }
	}

    void DoRotate(bool isRotateRight)
    {
        isRotating = true;
        float rotateAmt = isRotateRight ? -90f : 90f;

        // Rotate camera offset 
        currentTargetOffset = Quaternion.Euler(0, rotateAmt, 0) * currentTargetOffset;
        StartCoroutine(RotateAround(rotateAmt));

        // Update sorting axis
        cam.transparencySortAxis = Quaternion.Euler(0, rotateAmt, 0) * cam.transparencySortAxis;

        // Send out action to other scripts (e.g. BillboardEntity)
        rotateCamera?.Invoke(rotateAmt);
    }

    IEnumerator RotateAround(float rotateAmt)
    {
        // Target Camera Position
        // Adapted from: https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
        // Rotate vector around a point
        Vector3 camStartingPos = transform.position;
        Vector3 dir = camStartingPos - target.position;
        dir = Quaternion.Euler(0, rotateAmt, 0) * dir;
        Vector3 cameraTargetPos = dir + target.position;

        // Target Camera Rotation
        Quaternion camStartingRot = transform.rotation;
        Quaternion camTargetRot = Quaternion.Euler(0, rotateAmt, 0) * camStartingRot;

        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * ((Time.timeScale) / (1 / cameraLerpSpeed));

            //float smoothT = Mathf.SmoothStep(0f, 1f, t);


            transform.position = Vector3.Lerp(camStartingPos, cameraTargetPos, easeCurve.Evaluate(t));
            transform.rotation = Quaternion.Lerp(camStartingRot, camTargetRot, easeCurve.Evaluate(t));
            yield return null;
        }

        isRotating = false;
    }

    IEnumerator UpdateCameraPosSmooth(Vector3 targetPos)
    {
        float t = 0;

        Vector3 startingPos = transform.position;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / (1 / cameraLerpSpeed));
            transform.position = Vector3.Lerp(startingPos, targetPos, t);

            yield return null;
        }
    }
}
