using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] GameObject treePrefab;

    float growDuration = 60f;
    float curDuration = 0;

    // Update is called once per frame
    void Update()
    {
        curDuration += Time.deltaTime;

        if (curDuration >= growDuration)
		{
            GameObject go = Instantiate(treePrefab);
            go.transform.position = transform.position;

            Destroy(gameObject);
		}
    }
}
