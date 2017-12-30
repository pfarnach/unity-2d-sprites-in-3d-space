using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeeThrough : MonoBehaviour {

    [SerializeField]
    private Transform target;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    [Range (0f, 1f)]
    private float seeThroughOpacity = 0.3f;

    [SerializeField]
    private float fadeTime = 0.5f;

    private GameObject lastHit;


	void Update () {
        RaycastHit hitInfo = new RaycastHit();
        Ray rayToTarget = new Ray(transform.position, target.position - transform.position);

        //Debug.DrawRay(transform.position, target.position - transform.position, Color.red);

        if (Physics.Raycast(rayToTarget, out hitInfo, (transform.position - target.position).magnitude, layerMask))
        {
            GameObject newHit = hitInfo.collider.gameObject;

            // No other currently faded objects
            if (lastHit == null)
            {
                StartCoroutine(SetOpacity(newHit, seeThroughOpacity));
                lastHit = newHit;
            }
            // Don't set opacity of sprite that's already had its opacity set
            else if (lastHit.GetInstanceID() != newHit.GetInstanceID())
            {
                StartCoroutine(SetOpacity(newHit, seeThroughOpacity));
                StartCoroutine(SetOpacity(lastHit, 1f));
                lastHit = null;
            }
        }
        // No hit, so reset last hit to fully opaque
        else if (lastHit != null)
        {
            StartCoroutine(SetOpacity(lastHit, 1f));
            lastHit = null;
        }
	}

    IEnumerator SetOpacity(GameObject go, float targetOpacity)
    {
        float lerpTime = 0f;
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color targetColor = spriteRenderer.color;
        targetColor.a = targetOpacity;

        while (lerpTime <= fadeTime)
        {
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, lerpTime / fadeTime);
            lerpTime += Time.deltaTime;
            yield return null;
        }
    }

}

