using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitSpawn : MonoBehaviour {

    [SerializeField]
    private Transform[] points;
    public GameObject rabbit;
    public float createTime = 1.7f;
    private float delay = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        delay += Time.deltaTime;
        if (delay >= createTime)
        {
            StartCoroutine(CreateRabbit());
            delay = 0;
        }
    }
    

    IEnumerator CreateRabbit()
    {
        points = GameObject.Find("Spawnpoints").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        Instantiate(rabbit, points[idx].position, Quaternion.Euler(0,-180,0));
        yield return null;
    }

    
}
