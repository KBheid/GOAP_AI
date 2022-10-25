using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavangeTree : Action
{
	float eatDistance = 0.75f;
	float eatTimer = 2f;
	float timeEaten = 0f;

	private Chaser chaser;
	private BerryTree target;
	private bool cancelled;

	public ScavangeTree() : base() {
		effects.Add(new KeyValuePair<string, bool>("needsFood", false));
	}

	public override Action Clone()
	{
		ScavangeTree ret = new ScavangeTree();
		prerequisites = ret.prerequisites;
		effects = ret.effects;

		return ret;
	}

	public override void OnCompletion()
	{
		base.OnCompletion();
		target.Eat();
		state.hunger -= 50f;
		state.SetPrereq(new KeyValuePair<string, bool>("needsPoo", true));
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

		float distance = 1000;
		foreach (BerryTree tree in trees)
		{
			if (tree.eaten)
				continue;

			float newDist = (tree.transform.position - state.transform.position).magnitude;
			if (newDist < distance)
			{
				target = tree;
				distance = newDist;
			}
		}

		if (target == null)
		{
			cancelled = true;
			return;
		}

		chaser.target = target.transform.position;
		chaser.target.y = 0.5f;
		chaser.Paused = false;
	}

	public override bool OnTick()
	{
		if (cancelled)
			return false;

		if (target == null || target.eaten)
		{
			FindTrees();
			timeEaten = 0;
			return true;
		}

		// Check distance
		if ((state.transform.position - chaser.target).magnitude < eatDistance)
		{
			timeEaten += Time.deltaTime;
		}

		if (timeEaten >= eatTimer)
			OnCompletion();

		return true;
	}
}
