using UnityEngine;
using System.Collections;

public class FaceNode : MonoBehaviour
{
	int side;
	Vector3 start;
	public FaceNode mirror = null;

	void Awake()
	{
		side = transform.position.x == 0f ? 0 : (int)Mathf.Sign(transform.position.x);
		start = transform.position;

		if (transform.position.z != 0f)
			Debug.Log("WARNING: Node starting at wrong Z plane!", gameObject);
	}

	public int Side
	{
		get { return side; }
	}
}
