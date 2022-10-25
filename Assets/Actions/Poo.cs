using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poo : Action
{
	private Chaser chaser;
	private Object pooPrefab;

	public Poo() : base() {
		prerequisites.Add(new KeyValuePair<string, bool>("isCreature", true));
		prerequisites.Add(new KeyValuePair<string, bool>("wantsWander", true));
		prerequisites.Add(new KeyValuePair<string, bool>("needsPoo", true));

		cost = 1;

		effects.Add(new KeyValuePair<string, bool>("chill", true));
	}

	public override Action Clone()
	{
		Poo ret = new Poo();
		prerequisites = ret.prerequisites;
		effects = ret.effects;

		return ret;
	}

	public override void OnCompletion()
	{
		base.OnCompletion();
		GameObject newPoo = Object.Instantiate(pooPrefab) as GameObject;
		
		Vector3 pos = state.transform.position;
		pos.y = 0;

		newPoo.transform.position = pos;

		state.SetPrereq(new KeyValuePair<string, bool>("needsPoo", false));
	}

	public override void OnStart()
	{
		state.TryGetComponent(out chaser);

		pooPrefab = Resources.Load("Poo");

		FindTarget();
	}

	private void FindTarget()
	{
		float x = Random.Range(-2, 2);
		float y = Random.Range(-2, 2);

		chaser.target = new Vector3(x + Mathf.Sign(x)*15, 0.5f, y + Mathf.Sign(y) * 15);
		chaser.Paused = false;
	}

	public override bool OnTick()
	{

		if ((state.transform.position - chaser.target).magnitude < 1.5f)
			OnCompletion();

		return true;
	}
}
