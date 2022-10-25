using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
	public GameObject upgradePrefab;
	public int overlapsForUpgrade = 3;

	private int curOverlaps = 0;

	private bool priority = false;

	private void Start()
	{
		transform.rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.TryGetComponent(out Building b))
		{
			// If they upgrade to the same thing, upgrade ourself and destroy other
			if (b.upgradePrefab == upgradePrefab)
			{
				priority = !b.priority;

				if (priority)
				{
					curOverlaps += b.curOverlaps + 1;
					Destroy(b.gameObject);
					CheckUpgrade();
				}
			}
		}
	}

	private void CheckUpgrade()
	{
		if (upgradePrefab == null)
			return;

		if (curOverlaps >= overlapsForUpgrade)
		{
			GameObject go = Instantiate(upgradePrefab);
			go.transform.position = transform.position;
			Destroy(gameObject);
		}
	}
}
