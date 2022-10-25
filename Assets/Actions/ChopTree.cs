using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTree : Action
{
	float chopTimer = 7f;
	float timeChopped = 0f;
	float chopDistance = 2f;

	private Chaser chaser;
	private BerryTree target;
	private bool cancelled = false;

	public ChopTree() : base()
	{
		prerequisites.Add(new KeyValuePair<string, bool>("isCreature", false));

		effects.Add(new KeyValuePair<string, bool>("needsWood", false));
	}

	public override Action Clone()
	{
		ChopTree ret = new ChopTree();
		prerequisites = ret.prerequisites;
		effects = ret.effects;

		return ret;
	}

	public override void OnCompletion()
	{
		base.OnCompletion();
		Object.Destroy(target.gameObject);
		state.woodCount += 5;
	}

	public override void OnStart()
	{
		state.TryGetComponent(out chaser);
		FindTrees();

	}

	private void FindTrees() 
	{
		BerryTree[] trees = Object.FindObjectsOfType<BerryTree>();

		if (trees.Length == 0)
		{
			cancelled = true;
			return;
		}

		target = trees[0];
		float distance = (trees[0].transform.position - state.transform.position).magnitude;
		foreach (BerryTree tree in trees)
		{
			float newDist = (tree.transform.position - state.transform.position).magnitude;
			if ( newDist < distance) {
				target = tree;
				distance = newDist;
			}
		}

		chaser.target = target.transform.position;
		chaser.target.y = 0.5f;
		chaser.Paused = false;
	}

	public override bool OnTick()
	{
		if (cancelled)
			return false;

		if (target == null)
		{
			FindTrees();
			timeChopped = 0;
			return true;
		}

		// Check distance
		if ((state.transform.position - chaser.target).magnitude < chopDistance)
		{
			timeChopped += Time.deltaTime;
		}

		if (timeChopped >= chopTimer)
			OnCompletion();

		return true;
	}
}
