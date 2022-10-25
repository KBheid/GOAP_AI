using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillInTent : Action
{
	private Chaser chaser;
	private Building target;
	private bool cancelled = false;
	private float maxChillDuration = 10f;
	private float curDur = 0f;

	public ChillInTent() : base() {
		prerequisites.Add(new KeyValuePair<string, bool>("isCreature", true));
		prerequisites.Add(new KeyValuePair<string, bool>("wantsRelax", true));

		effects.Add(new KeyValuePair<string, bool>("chill", true));
	}

	public override Action Clone()
	{
		ChillInTent ret = new ChillInTent();
		prerequisites = ret.prerequisites;
		effects = ret.effects;

		return ret;
	}

	public override void OnCompletion()
	{
		base.OnCompletion();
		
		CreatureState cState = state as CreatureState;
		cState.chillValue += maxChillDuration;
	}

	public override void OnStart()
	{
		state.TryGetComponent(out chaser);
		FindTents();
	}

	private void FindTents()
	{
		Building[] tents = Object.FindObjectsOfType<Building>();

		float distance = 1000;
		foreach (Building tent in tents)
		{
			float newDist = (tent.transform.position - state.transform.position).magnitude;
			if (newDist < distance)
			{
				target = tent;
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

		curDur += Time.deltaTime;
		if (curDur >= maxChillDuration)
			OnCompletion();

		return true;
	}
}
