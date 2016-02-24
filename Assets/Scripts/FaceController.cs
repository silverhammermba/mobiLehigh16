using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Edge
{
	public FaceNode n1;
	public FaceNode n2;
	public float strength = 2f;

	public Edge(FaceNode _n1, FaceNode _n2, float _strength = 2f)
	{
		n1 = _n1;
		n2 = _n2;
		strength = _strength;
	}
}

public class FaceController : MonoBehaviour
{
	public GameObject node;
	public List<Edge> edges;

	void Start ()
	{
		// mirror all (non-center) nodes
		for (int i = 0; i < transform.childCount; ++i)
		{
			FaceNode n1 = transform.GetChild(i).GetComponent<FaceNode>();

			if (n1.Side == 0)
			{
				n1.mirror = n1;
				continue;
			}

			GameObject newNode = Instantiate(node, new Vector3(-n1.transform.position.x, n1.transform.position.y, n1.transform.position.z), Quaternion.identity) as GameObject;
			newNode.name = n1.gameObject.name + " (mirror)";
			//newNode.transform.SetParent(transform);

			FaceNode n2 = newNode.GetComponent<FaceNode>();
			n1.mirror = n2;
			n2.mirror = n1;
		}

		// duplicate edges for mirrored nodes
		int numEdges = edges.Count;
		for (int i = 0; i < numEdges; ++i)
		{
			edges.Add(new Edge(edges[i].n1.mirror, edges[i].n2.mirror, edges[i].strength));
		}
	}
}
