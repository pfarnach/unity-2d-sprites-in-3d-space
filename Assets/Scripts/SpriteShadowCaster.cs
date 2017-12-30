using UnityEngine;
using System.Collections;

// Adapted from: https://forum.unity.com/threads/animated-sprites-with-cast-shadows-and-receive-shadow.322862/
// Another approach here: https://forum.unity.com/threads/sprite-receive-shadow.357705/
// And here: https://github.com/anlev/Unity-2D-Sprite-cast-and-receive-shadows/blob/master/SpriteShadow.shader


[ExecuteInEditMode]
public class SpriteShadowCaster : MonoBehaviour {

	void Awake () {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        spriteRenderer.receiveShadows = true;
	}

}
