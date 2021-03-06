                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IDAStar : Agent
{
	Vector3 target;
	List<Point>  myGraph;
	
	public override void Start()
	{
		base.Start();
		myGraph = graphStructure.myGraph;      
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		MouseHandle ();
	}
	
	public List<Point> Deliberate(Vector3 start, Vector3 goal)
	{
		graphStructure.AddNode(start);
		myGraph[myGraph.Count - 1].myWeights.Add(0);
		myGraph[myGraph.Count - 1].g = 0;
		myGraph[myGraph.Count - 1].h = Vector3.Distance(start, goal);
		myGraph[myGraph.Count -1].f = Vector3.Distance(start, goal);
		graphStructure.AddNode(goal);

		float bound = Vector3.Distance (start, goal);
		while (true)
		{
			float t = Search(myGraph[myGraph.Count -2], 0f, bound);
			if (t == 0f) 
				return ReconstructPath (myGraph[myGraph.Count -1]);
			if (t == float.PositiveInfinity )
				return new List<Point>();
			bound = t;

		}
	}

	public float Search(Point node, float g, float bound)
	{
		node.g = g;
		node.f = node.g +  Vector3.Distance(node.Position, myGraph[myGraph.Count - 1].Position);
		if (node.f > bound)
			return node.f;
		if (node == myGraph [myGraph.Count - 1])
			return 0;
		float min = float.PositiveInfinity;
		int i = 0;
		foreach (Point neighbour in node.myNeighbours) 
		{
			neighbour.myParent = node;
			float t = Search(neighbour, g + node.myWeights[i++], bound);
			if ( t == 0) 
				return 0;
			if(t < min)
				min = t;

		}
		return min;

	}
	
	public List<Point> ReconstructPath(Point end)
	{
		
		List<Point> path = new List<Point>();
		path.Add(end);
		Point start = myGraph[myGraph.Count - 2];
		Point current = end;
		while(current != start)
		{
			Debug.Log (current.Position);
			path.Add(current.myParent);
			current = current.myParent;
		}
		
		Debug.Log (current.Position);
		return path;
		
	}
	
	void MouseHandle()
	{
		if(Input.GetMouseButton(0))
		{
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit r;
			if(Physics.Raycast(ray, out r))
			{
				target = r.point;
				Debug.Log (r.point);	
			}
			if (!(transform.position == target))
				MoveTo(target, transform.position );
			
		}
		base.Update();

	}
	
	
}
