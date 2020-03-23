using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject transitionEnd;
    [SerializeField]
    private GameObject backGround;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private AudioClip audioStar;
    [SerializeField]
    private AudioClip audioJump;
    [SerializeField]
    private AudioClip audioFall;
    [SerializeField]
    private AudioClip audioWin;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float delayJump = 0f;
    private bool isPlay;
    private Score score;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
        isPlay = true;
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    void Update()
    {
        if (isPlay)
        {
            if (delayJump >= 0f)
                delayJump -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Space) && isGrounded && delayJump <= 0f)
            {
                rb.AddForce(Vector2.up * 200f);
                isGrounded = false;
                delayJump = 0.5f;
                audio.PlayOneShot(audioJump);
            }
            float inputHorizontal = Input.GetAxis("Horizontal");
            rb.AddForce(Vector2.right * 200f * inputHorizontal * Time.deltaTime);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -4f, 4f), rb.velocity.y);
            float xCam = Mathf.Clamp(transform.position.x, 0f, 16.9f);
            cam.transform.position = new Vector3(xCam, 0f, -10f);
            backGround.transform.position = new Vector3(xCam / 1.05f + 0.4f, 0f);
            if (transform.position.y < -1.9f)
            {
                audio.PlayOneShot(audioFall);
                StartCoroutine(ReloadGame());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Star")
        {
            audio.PlayOneShot(audioStar);
            score.UpdateScore(10);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Flag")
        {
            audio.PlayOneShot(audioWin);
            BoxCollider2D collider = collision.gameObject.GetComponent<BoxCollider2D>();
            Destroy(collider);
            collision.gameObject.GetComponentInChildren<Animator>().Play("Flag_Raise");
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        isPlay = false;
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(ReloadGame());
    }

    IEnumerator ReloadGame()
    {
        isPlay = false;
        GameObject obj = Instantiate(transitionEnd, new Vector2(cam.transform.position.x + 0.027f, cam.transform.position.y - 0.005734801f), Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level_1");
    }
}
