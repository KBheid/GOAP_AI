using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Action
{
	private Chaser chaser;

	public Wander() : base() {
		prerequisites.Add(new KeyValuePair<string, bool>("isCreature", true));
		prerequisites.Add(new KeyValuePair<string, bool>("wantsWander", true));
		cost = 2;

		effects.Add(new KeyValuePair<string, bool>("chill", true));
	}

	public override Action Clone()
	{
		Wander ret = new Wander();
		prerequisites = ret.prerequisites;
		effects = ret.effects;

		return ret;
	}

	public override void OnCompletion()
	{
		base.OnCompletion();
		
		CreatureState cState = state as CreatureState;
		cState.chillValue -= 5;
	}

	public override void OnStart()
	{
		state.TryGetComponent(out chaser);
		FindTarget();
	}

	private void FindTarget()
	{
		chaser.target = new Vector3(Random.Range(-15, 15), 0.5f, Random.Range(-15, 15));
		chaser.Paused = false;
	}

	public override bool OnTick()
	{

		if ((state.transform.position - chaser.target).magnitude < 1.5f)
			OnCompletion();

		return true;
	}
}
