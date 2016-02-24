using UnityEngine;
using System.Collections;

public class FaceNode : MonoBehaviour
{
	public FaceNode mirror = null;

	int side;
	Rigidbody2D rb;
	Vector3 start; // starting position
	const float strength = 3f; // the spring constant for this node returning to start

	void Awake()
	{
		side = transform.position.x == 0f ? 0 : (int)Mathf.Sign(transform.position.x);
		start = transform.position;
		rb = GetComponent<Rigidbody2D>();

		if (transform.position.z != 0f)
			Debug.Log("WARNING: Node starting at wrong Z plane!", gameObject);
	}

	public int Side
	{
		get { return side; }
	}

	public Rigidbody2D Body
	{
		get { return rb; }
	}

	public void ApplyForce()
	{
		float diff = Vector3.Distance(transform.position, start);

		// direction from current position to start
		Vector2 dir = (start - transform.position).normalized;

		rb.AddForce(strength * diff * dir);
	}
}
