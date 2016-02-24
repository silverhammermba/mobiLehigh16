using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Edge
{
	public FaceNode n1;
	public FaceNode n2;
	public float strength = 2f; // the spring constant
	public float length; // the spring's equilibrium length, computed from initial position

	public Edge(FaceNode _n1, FaceNode _n2, float _strength = 2f)
	{
		n1 = _n1;
		n2 = _n2;
		strength = _strength;
	}

	public float CurrentLength()
	{
		return Vector3.Distance(n1.transform.position, n2.transform.position);
	}

	public void ApplyForce()
	{
		float diff = CurrentLength() - length;

		// direction from n1 to n2
		Vector2 dir = (n2.transform.position - n1.transform.position).normalized;

		n1.Body.AddForce(strength * diff * dir);
		n2.Body.AddForce(strength * diff * -dir);
	}
}

public class FaceController : MonoBehaviour
{
	public GameObject node;
	public List<Edge> edges;

	void Start ()
	{
		// mirror all (non-center) nodes
		int numNodes = transform.childCount;
		for (int i = 0; i < numNodes; ++i)
		{
			FaceNode n1 = transform.GetChild(i).GetComponent<FaceNode>();

			if (n1.Side == 0)
			{
				n1.mirror = n1;
				continue;
			}

			GameObject newNode = Instantiate(node, new Vector3(-n1.transform.position.x, n1.transform.position.y, n1.transform.position.z), Quaternion.identity) as GameObject;
			newNode.name = n1.gameObject.name + " (mirror)";
			newNode.transform.SetParent(transform);

			FaceNode n2 = newNode.GetComponent<FaceNode>();
			n1.mirror = n2;
			n2.mirror = n1;
		}

		// duplicate edges for mirrored nodes
		int numEdges = edges.Count;
		for (int i = 0; i < numEdges; ++i)
		{
			// if it's between two central nodes, don't duplicate it
			if (edges[i].n1.mirror == edges[i].n1 && edges[i].n2.mirror == edges[i].n2.mirror) continue;

			edges.Add(new Edge(edges[i].n1.mirror, edges[i].n2.mirror, edges[i].strength));
		}

		// compute edges lengths
		for (int i = 0; i < edges.Count; ++i)
		{
			edges[i].length = edges[i].CurrentLength();
		}
	}

	void FixedUpdate()
	{
		for (int i = 0; i < edges.Count; ++i)
		{
			edges[i].ApplyForce();
		}

		for (int i = 0; i < transform.childCount; ++i)
		{
			transform.GetChild(i).GetComponent<FaceNode>().ApplyForce();
		}
	}
}
