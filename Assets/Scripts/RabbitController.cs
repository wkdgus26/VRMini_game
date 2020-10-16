using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour {
    [SerializeField]
    private ParticleSystem particle;
    private Vector3 goalPos;
    private Vector3 originPos;

    private Animator anim;
    // Use this for initialization
    void Start () {
        originPos = this.transform.position;
        goalPos = new Vector3(this.transform.position.x, 56.065f, this.transform.position.z);
        anim = GetComponent<Animator>();
        StartCoroutine(MoveCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
        //StartCoroutine(MoveCoroutine());
	}

    IEnumerator MoveCoroutine()
    {
        int count = 0;
        while (true)
        {
            count++;
            this.transform.position = Vector3.Lerp(this.transform.position, goalPos, 0.2f);
            if (count > 25)
            {
                break;
            }
            yield return null;
        }
        this.transform.position = goalPos;
        //if(collider.gameObject.tag=="hammer")
        yield return new WaitForSeconds(1f);
        StartCoroutine(OriginMoveCoroutine());
    }

    IEnumerator OriginMoveCoroutine()
    {
        int count = 0;
        while (true)
        {
            count++;
            this.transform.position = Vector3.Lerp(this.transform.position , originPos, 0.1f);
            if (count > 50)
            {
                break;
            }
            yield return null;
        }
        this.transform.position = originPos;
        Destroy(gameObject);
    }

    IEnumerator DeadCoroutine()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        particle.Play();
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "hammer")
        {
            StopAllCoroutines();
            StartCoroutine(DeadCoroutine());
        }
    }


}
