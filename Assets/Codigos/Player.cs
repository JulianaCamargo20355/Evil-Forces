using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float andar;
    public float forcaPulo;

    public float climbSpeed = 4f;
    private bool isClimbing = false;
    private bool inWall = false;

    public GameObject projet;
    public Transform gatilho;
    public float forcaTiro;
    private bool flipX = false;

    public Animator anime;
    Rigidbody2D rig;

    private int cont = 0;
    private bool morreu = false;
    private Vector3 posInic;

    public int contReset;

    public int contCoin = 0;
    public int totalCoins = 5; 
    public bool inimigoDerrotado = false;

    void Start()
    {
        posInic = transform.position;
        rig = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        cont = 2; 
    }

    void Update()
    {
        if (!morreu)
        {
            Andar();
            Pular();
            Escalada();

            bool andando = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f;
            anime.SetBool("isRun", andando);

            if (Input.GetKeyDown(KeyCode.Z))
                Atirar(andando);
        }

        anime.SetBool("isDead", morreu);
        anime.SetBool("isClimbing", isClimbing);
    }

    void Andar()
    {
        float h = Input.GetAxis("Horizontal");
        Vector3 movimento = new Vector3(h, 0f, 0f);
        transform.position += movimento * Time.deltaTime * andar;

        if (flipX && h > 0)
            Flipar();
        if (!flipX && h < 0)
            Flipar();
    }

    void Pular()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cont > 0)
        {
            rig.velocity = new Vector2(rig.velocity.x, forcaPulo);
            cont--;

            if (cont == 1)
            {
                anime.SetBool("isJump", true);
                anime.SetBool("isDoubleJump", false);
            }
            else if (cont == 0)
            {
                anime.SetBool("isJump", false);
                anime.SetBool("isDoubleJump", true);
            }
        }
    }

    void Escalada()
    {
        if (inWall && Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
            isClimbing = true;

        if (isClimbing)
        {
            float v = Input.GetAxis("Vertical");
            rig.velocity = new Vector2(rig.velocity.x, v * climbSpeed);
            rig.gravityScale = 0;

            if (Mathf.Abs(v) < 0.1f)
                rig.velocity = new Vector2(rig.velocity.x, 0);
        }
        else
        {
            rig.gravityScale = 1;
        }
    }

    void Atirar(bool andando)
    {
        if (andando)
            anime.SetBool("isFireRun", true);
        else
            anime.SetBool("isFire", true);

        GameObject temp = Instantiate(projet, gatilho.position, Quaternion.identity);
        float direcao = flipX ? -1 : 1;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaTiro * direcao, 0);
        Destroy(temp, 3f);

        if (andando)
            Invoke(nameof(ResetFireRunAnim), 0.2f);
        else
            Invoke(nameof(ResetFireAnim), 0.2f);
    }

    void ResetFireRunAnim() => anime.SetBool("isFireRun", false);
    void ResetFireAnim() => anime.SetBool("isFire", false);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            cont = 2;
            anime.SetBool("isJump", false);
            anime.SetBool("isDoubleJump", false);
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            morreu = true;
            Invoke(nameof(ResetPosition), 2f);
            contReset++;
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Coin"))
        {
            contCoin++;
            Destroy(trigger.gameObject);
            VerificarVitoria();
        }

        if (trigger.gameObject.CompareTag("Osso"))
        {
            Destroy(trigger.gameObject);
            morreu = true;
            Invoke(nameof(ResetPosition), 2f);
            contReset++;
        }

        if (trigger.gameObject.CompareTag("Parede"))
            inWall = true;
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Parede"))
        {
            inWall = false;
            isClimbing = false;
        }
    }

    void ResetPosition()
    {
        morreu = false;
        isClimbing = false;
        inWall = false;
        transform.position = posInic;
        rig.velocity = Vector2.zero;
    }

    void Flipar()
    {
        flipX = !flipX;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public void VerificarVitoria()
    {
        if (contCoin == totalCoins && inimigoDerrotado)
        {
            GameManager.instance.LoadScene("win"); // carrega a cena de vitória
        }
    }
}
