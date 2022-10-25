using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkWater : Action
{
	float drinkDistance = 3f;
	float drinkTimer = 2f;
	float timeDrank = 0f;
	private Chaser chaser;

	public DrinkWater() : base() {
		effects.Add(new KeyValuePair<string, bool>("needsWater", false));
	}

	public override Action Clone()
	{
		DrinkWater ret = new DrinkWater();
		prerequisites = ret.prerequisites;
		effects = ret.effects;

		return ret;
	}

	public override void OnCompletion()
	{
		base.OnCompletion();
		state.thirst -= 50f;
	}

	public override void OnStart()
	{
		state.TryGetComponent(out chaser);

		// Get nearest source
		WaterTank[] water = Object.FindObjectsOfType<WaterTank>();

		WaterTank nearestWater = water[0];
		float distance = (water[0].transform.position - state.transform.position).magnitude;
		foreach (WaterTank waterTank in water)
		{
			float newDist = (waterTank.transform.position - state.transform.position).magnitude;
			if (newDist < distance)
			{
				nearestWater = waterTank;
				distance = newDist;
			}
		}

		chaser.target = nearestWater.transform.position;
		chaser.target.y = 0.5f;
		chaser.Paused = false;
	}

	public override bool OnTick()
	{

		if ((state.transform.position - chaser.target).magnitude < drinkDistance)
		{
			timeDrank += Time.deltaTime;
		}

		if (timeDrank >= drinkTimer)
			OnCompletion();


		return true;
	}
}
