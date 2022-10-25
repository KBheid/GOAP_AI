using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionPlanner
{
    static List<Action> actions = new List<Action>();

	private static List<Node> constructedGraph;

	public static void Initialize()
	{
		actions.Clear();

		// Humanoids
		actions.Add(new ScavangeTree());
		actions.Add(new BuildHouse());
		actions.Add(new ChopTree());

		// Creatures
		actions.Add(new ChillInTent());
		actions.Add(new Wander());
		actions.Add(new Poo());

		// All
		actions.Add(new DrinkWater());

		RefreshBaseActions();

	}
	public static void RefreshBaseActions()
	{
		constructedGraph = new List<Node>();

		// Create nodes
		foreach (Action a in actions)
		{
			constructedGraph.Add(new Node(a.cost, a));
		}

		// Connect all nodes by adding granters to a list that grants the desired effect
		foreach (Node n in constructedGraph)
		{
			List<Node> otherNodes = new List<Node>(constructedGraph);
			otherNodes.Remove(n);

			foreach (KeyValuePair<string, bool> prereq in n.action.prerequisites)
			{
				foreach (Node n2 in otherNodes)
					if (n2.action.HasEffect(prereq))
						n2.AddConnection(n);
			}
		}
	}

	public static List<Action> GetValidActions(State s)
	{
		List<Action> valid = new List<Action>();

		foreach (Action a in actions)
		{
			if (a.MeetsPrerequisites(s))
				valid.Add(a);
		}

		return valid;
	}


	public static List<Action> FindActionsTowardsGoal(KeyValuePair<string, bool> goal, List<Action> validActions)
	{
		List<Node> allNodes = new List<Node>(constructedGraph);

		// Create a root node and connect to all valid actions
		Node root = new Node(0, null);

		List<Node> goalNodes = new List<Node>();

		// Using the preconstructed graph, add in our start point and find our end points
		foreach (Node n in allNodes)
		{
			if (validActions.Contains(n.action))
				root.AddConnection(n);

			if (n.action.HasEffect(goal))
				goalNodes.Add(n);
		}

		allNodes.Add(root);

		List<Node> orderedPath = Dijkstra(allNodes, root, goalNodes);
		List<Action> orderedActions = new List<Action>();

		orderedPath.Remove(root);
		orderedPath.ForEach(n => { orderedActions.Add(n.action.Clone()); });

		foreach (Node n in orderedPath)
			n.Print();

		return orderedActions;
	}

	static List<Node> Dijkstra(List<Node> graph, Node start, List<Node> validTargets)
	{
		List<Node> visited  = new List<Node>();
		List<Node> frontier = new List<Node>();

		frontier.Add(start);

		start.supposedDistance = 0;

		while (frontier.Count>0)
		{
			// Get lowest distance
			Node cur = frontier[0];
			foreach (Node n in frontier)
			{
				if (n.supposedDistance < cur.supposedDistance)
					cur = n;
			}

			// Remove from frontier
			frontier.Remove(cur);
			visited.Add(cur);

			// Bail as soon as we have reached a target and added it to visited
			if (validTargets.Contains(cur))
				break;

			// Update connections in frontier
			foreach (Node n in cur.connections)
			{
				if (visited.Contains(n))
					continue;

				if (frontier.Contains(n))
				{
					if (n.supposedDistance < cur.supposedDistance + n.cost)
					{
						n.supposedDistance = cur.supposedDistance + n.cost;
						n.prevNode = cur;
					}
				}
				else
				{
					frontier.Add(n);
					n.supposedDistance = cur.supposedDistance + n.cost;
					n.prevNode = cur;
				}
			}
		}

		if (visited.Count == 0)
			return null;


		// Last item in list is a goal
		Node last = visited[visited.Count - 1];

		List<Node> orderedNodes = new List<Node>();
		while (last != null) {
			orderedNodes.Add(last);
			last = last.prevNode;
		}

		orderedNodes.Reverse();
		return orderedNodes;
	}


	class Node
	{
		public Node prevNode;
		public List<Node> connections;
		public float cost;
		public float supposedDistance;
		public Action action;

		public Node(float cost, Action action)
		{
			this.connections = new List<Node>();
			this.cost = cost;
			this.action = action;
		}

		public void AddConnection(Node n)
		{
			connections.Add(n);
		}

		public void Print()
		{
			string s = "";
			s += action?.ToString() + "\n ---------------- \n";
			foreach (Node n2 in connections)
			{
				s += n2.action.ToString() + ", ";
			}
			s += "\n\n";
			Debug.Log(s);
		}
	}
}
