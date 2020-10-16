using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    [SerializeField]
    private Transform teleport_Main;
    [SerializeField]
    private Transform teleport_Basketball;
    [SerializeField]
    private Transform teleport_CatchRabbit;
    [SerializeField]
    private FadeOut fadeOut;
    [SerializeField]
    private FadeIn fadeIn;

    private void Awake()
    {

    }
    // Use this for initialization
    void Start() {
        fadeIn.StartFadeAnim();
    }

    // Update is called once per frame
    void Update() {
        SceneChange();
        //Test();
    }

    private void Test()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            //StartCoroutine(MoveBasket());
            StartCoroutine(MoveCatch());
        }
    }

    private void SceneChange()
    {
        if ((this.transform.position.x <= -5f && this.transform.position.x >= -12f) && (this.transform.position.z >= 47f && this.transform.position.z <= 54f))
        //if (Input.GetKeyDown("1")) 
        {
            StartCoroutine(MoveBasket());
        }

        if ((this.transform.position.x >= 17f && this.transform.position.x <= 27f) && (this.transform.position.z <= 77.5f && this.transform.position.z >= 67.5f))
        //if(Input.GetKeyDown("2"))
        {
            StartCoroutine(MoveCatch());
        }
    }

    public void MainButton()
    {
        StartCoroutine(ChangeMainCoroutine());
    }

    IEnumerator CatchRabbitCoroutine()
    {
        fadeOut.StartFadeAnim();
        yield return new WaitForSeconds(2f);
        Debug.Log("z");
        SceneManager.LoadScene("CatchRabbit");
    }

    IEnumerator BasketBallCoroutine()
    {
        fadeOut.StartFadeAnim();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("BasketBall");
    }

    IEnumerator MainCoroutine()
    {
        fadeOut.StartFadeAnim();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator MoveCatch()
    {
        int count = 0;
        while (true)
        {
            count++;
            this.transform.position = Vector3.Lerp(this.transform.position, teleport_CatchRabbit.transform.position, 0.05f);
            if (count > 50)
            {
                break;
            }
            yield return null;
        }
        this.transform.position = teleport_CatchRabbit.transform.position;
        StartCoroutine(CatchRabbitCoroutine());
    }

    IEnumerator MoveBasket()
    {
        int count = 0;
        while (true)
        {
            count++;
            this.transform.position = Vector3.Lerp(this.transform.position, teleport_Basketball.transform.position, 0.05f);
            if (count > 50)
            {
                break;
            }
            yield return null;
        }
        this.transform.position = teleport_Basketball.transform.position;
        StartCoroutine(BasketBallCoroutine());
    }

    IEnumerator ChangeMainCoroutine()
    {
        int count = 0;
        while (true)
        {
            count++;
            this.transform.position = Vector3.Lerp(this.transform.position, teleport_Main.transform.position, 0.02f);
            if (count > 100)
            {
                break;
            }
            yield return null;
        }
        this.transform.position = teleport_Main.transform.position;
        StartCoroutine(MainCoroutine());
    }

}
