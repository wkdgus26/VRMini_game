using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasketBallController : MonoBehaviour {

    [SerializeField]
    private GameObject teleport_Main;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private FadeOut fadeOut;
    [SerializeField]
    private GameObject gameLineMode;
    [SerializeField]
    private GameObject Lever;
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private float buttonRate = 1f;
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private float speed;

    public Transform[] childrenObj;

    private Vector3 ballOriginPos;
    private RaycastHit hit;
    private LineRenderer line;

    private float maxDistance = 10f;
    public float delayTime = 1.7f;
    private float currentButtonRate;
    private float delay = 0;

    private bool isStart = false;
    private bool isBall = false;
    // Use this for initialization
    void Start () {
        line = GetComponent<LineRenderer>();
        ballOriginPos = ball.transform.position;
        childrenObj = ball.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update () {
        ButtonRateCalc();
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

    private void ButtonRateCalc()
    {
        if (currentButtonRate > 0)
        {
            currentButtonRate -= Time.deltaTime;
        }
        TryButton();
    }

    private void TryButton()
    {
        //if (Input.GetButton("Fire2") && currentButtonRate <= 0)
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && currentButtonRate <= 0)
        {
            ButtonHit();
            MainMove();
        }
    }

    private void ButtonHit()
    {
        currentButtonRate = buttonRate;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxDistance))
        {
            Debug.Log(hit.collider);
            if (hit.collider.gameObject.tag == "start" && isStart == false)
            {
                Debug.Log("시작");
                isStart = !isStart;
                StartCoroutine(StartLeverCoroutine());
                wall.SetActive(false);
            }

            else if (hit.collider.gameObject.tag == "start" && isStart == true && !isBall)
            {
                Debug.Log("멈춤");
                isStart = !isStart;
                StartCoroutine(StopLeverCoroutine());
                wall.SetActive(true);
                foreach (Transform a in childrenObj)
                {
                    a.transform.position = ballOriginPos;
                }
            }
            
            else if (hit.collider.gameObject.tag == "ball" && !isBall)
            {
                GetBall();
            }

            //else if (Input.GetButtonDown("Fire2") && isBall)
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && isBall)
            {
                ShootBall();
            }
        }
    }

    private void GetBall()
    {
        hit.rigidbody.isKinematic = true;
        hit.transform.SetParent(this.transform);
        hit.transform.localPosition = new Vector3(0, 0, 0.103f);//Vector3.zero;
        hit.transform.rotation = new Quaternion(0, 0, 0, 0);
        line.enabled = false;
        gameLineMode.SetActive(true);
        isBall = !isBall;
    }

    private void ShootBall()
    {
        this.GetComponentInChildren<Rigidbody>().isKinematic = false;
        this.GetComponentInChildren<Rigidbody>().AddForce(this.transform.forward * speed);
        GameObject a1 = GameObject.FindWithTag("ufo");
        this.transform.DetachChildren();
        a1.transform.SetParent(this.transform);
        line.enabled = true;
        gameLineMode.SetActive(false);
        isBall = !isBall;
    }

    IEnumerator StartLeverCoroutine()
    {

        int count = 0;
        while (true)
        {
            count++;
            Lever.transform.localRotation = Quaternion.Slerp(Lever.transform.localRotation, Quaternion.Euler(-40f, 0, 0), 0.05f);
            if (count > 30)
            {
                break;
            }
            yield return null;
        }
        Lever.transform.localRotation = Quaternion.Euler(-40f, 0, 0);
    }

    IEnumerator StopLeverCoroutine()
    {

        int count = 0;
        while (true)
        {
            count++;
            Lever.transform.localRotation = Quaternion.Slerp(Lever.transform.localRotation, Quaternion.Euler(40f, 0, 0), 0.05f);
            if (count > 30)
            {
                break;
            }
            yield return null;
        }
        Lever.transform.localRotation = Quaternion.Euler(40f, 0, 0);
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
