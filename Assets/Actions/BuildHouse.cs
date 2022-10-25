using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHouse : Action
{
	private Chaser chaser;
	private Object housePrefab;
	private float buildDistance = 0.75f;

	public BuildHouse() : base() {
		prerequisites.Add(new KeyValuePair<string, bool>("needsWood", false));
		prerequisites.Add(new KeyValuePair<string, bool>("isCreature", false));

		effects.Add(new KeyValuePair<string, bool>("houseBuilt", true));
	}

	public override Action Clone()
	{
		BuildHouse ret = new BuildHouse();
		prerequisites = ret.prerequisites;
		effects = ret.effects;

		return ret;
	}

	public override void OnCompletion()
	{
		base.OnCompletion();
		GameObject go = Object.Instantiate(housePrefab) as GameObject;
		go.transform.position = chaser.target;

		state.woodCount -= 1;
	}

	public override void OnStart()
	{
		state.TryGetComponent(out chaser);

		housePrefab = Resources.Load("Tent");

		// Get a location to build
		chaser.target = new Vector3(Random.Range(-15, 15), 0.5f, Random.Range(-15, 15));
		chaser.Paused = false;
	}

	public override bool OnTick()
	{
		// Cancel if somehow we run out of wood in the middle
		if (state.woodCount < 1)
			return false;

		if ( (state.transform.position - chaser.target).magnitude < buildDistance )
		{
			OnCompletion();
		}

		return true;
	}
}
