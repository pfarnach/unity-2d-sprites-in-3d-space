using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEntity : MonoBehaviour {
    
    private float lerpSpeed = 5.5f;

    void Start()
    {
        FindObjectOfType<CameraRotate>().rotateCamera += OnCameraRotate;
    }

    void OnCameraRotate(float rotateAmt)
    {
        //transform.Rotate(0, rotateAmt, 0, Space.World);

        //Quaternion targetRot = Quaternion.Euler(0, rotateAmt, 0);
        //Quaternion targetRot = Quaternion.FromToRotation(transform.rotation.eulerAngles, new Vector3(0, rotateAmt, 0));
        Quaternion targetRot = Quaternion.Euler(0, rotateAmt, 0) * transform.rotation;

        if (Quaternion.Angle(targetRot, transform.rotation) > 1)
            StartCoroutine(UpdateRotation(targetRot));
    }

    IEnumerator UpdateRotation(Quaternion targetRot)
    {
        float t = 0;

        Quaternion startingRot = transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * (Time.timeScale / (1 / lerpSpeed));
            //transform.position = Vector3.Lerp(startingPos, targetPos, t);
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, t);

            yield return null;
        }

    }


}
