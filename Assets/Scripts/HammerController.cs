using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HammerController : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private FadeOut fadeOut;
    [SerializeField]
    private Transform teleport_Main;
    [SerializeField]
    private GameObject hammer;
    [SerializeField]
    private GameObject respawan;
    [SerializeField]
    private GameObject text1;
    [SerializeField]
    private GameObject text2;
    [SerializeField]
    private float hammerRate = 0.1f;
    [SerializeField]
    private float maxDistance = 10f;

    private float currentHammerRate;
    private RaycastHit hit;
    private SceneController scene;

    private bool isStart = false;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.blue);
        HammerRateCalc();
    }

    private void MainMove()
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "exit" && isStart == false)
            {
                StartCoroutine(ChangeMainCoroutine());
            }
        }
    }

    private void GameStart()
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "start" && isStart == false)
            {
                isStart = true;
                text1.SetActive(false);
                text2.SetActive(true);
                respawan.SetActive(true);
            }

            if (hit.collider.gameObject.tag == "stop" && isStart == true)
            {
                isStart = false;
                text1.SetActive(true);
                text2.SetActive(false);
                respawan.SetActive(false);
            }
        }
    }

    private void HammerRateCalc()
    {
        if (currentHammerRate > 0)
        {
            currentHammerRate -= Time.deltaTime;
        }
        TryHammer();
    }

    private void TryHammer()
    {
        //if (Input.GetButton("Fire2") && currentHammerRate <= 0)
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && currentHammerRate <= 0)
        {
            GameStart();
            HammerHit();
            MainMove();
        }
    }

    private void HammerHit()
    {
        currentHammerRate = hammerRate;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxDistance))
        {
            HammerMove();
        }
    }

    private void HammerMove()
    {
        
        if (hit.collider.gameObject.tag == "hitter")
        {
            hammer.transform.position = hit.transform.position;
            hammer.transform.position = new Vector3(hit.transform.position.x+0.4f, 57.2f, hit.transform.position.z);
            StartCoroutine(HammerCoroutine());
            
        }

        if (hit.collider.gameObject.tag == "monster")
        {

        }
    }

    IEnumerator HammerCoroutine()
    {

        int count = 0;
        while (true)
        {
            count++;
            hammer.transform.rotation = Quaternion.Slerp(hammer.transform.rotation, Quaternion.Euler(-50f, 90f, -90f), 0.25f);
            if (count > 18)
            {
                break;
            }
            yield return null;
        }
        hammer.transform.rotation = Quaternion.Euler(-50f, 90f, -90f);
        StartCoroutine(OriginHammerCoroutine());
    }

    IEnumerator OriginHammerCoroutine()
    {
        int count = 0;
        while (true)
        {
            count++;
            hammer.transform.rotation = Quaternion.Slerp(hammer.transform.rotation, Quaternion.Euler(50f, 90f, -90f), 0.25f);
            if (count > 18)
            {
                break;
            }
            yield return null;
        }
        hammer.transform.rotation = Quaternion.Euler(50f, 90f, -90f);
    }

    IEnumerator ChangeMainCoroutine()
    {
        int count = 0;
        while (true)
        {
            count++;
            player.transform.position = Vector3.Lerp(player.transform.position, teleport_Main.transform.position, 0.02f);
            if (count > 100)
            {
                break;
            }
            yield return null;
        }
        player.transform.position = teleport_Main.transform.position;
        StartCoroutine(MainCoroutine());
    }

    IEnumerator MainCoroutine()
    {
        fadeOut.StartFadeAnim();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }

}
