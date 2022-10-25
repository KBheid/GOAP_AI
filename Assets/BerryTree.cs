using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryTree : MonoBehaviour
{
    public float rechargeTime = 5f;
    public GameObject berryParent;

	public bool eaten { get; private set; }
    private float timeSinceEaten = 0f;

	private void Start()
	{
		transform.rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
	}

	private void Update()
	{
		if (eaten)
		{
			timeSinceEaten += Time.deltaTime;

			if (timeSinceEaten >= rechargeTime)
			{
				eaten = false;
				berryParent.SetActive(true);
			}
		}
	}

	public void Eat()
	{
		eaten = true;
		timeSinceEaten = 0f;
		berryParent.SetActive(false);
	}
}
