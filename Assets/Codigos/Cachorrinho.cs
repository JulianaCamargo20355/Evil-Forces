using UnityEngine;

public class Cachorrinho : MonoBehaviour
{
    public GameObject projet; //bala

    public Transform gatilho; //posição que sai o tiro

    private bool tiro; //imput do tiro 

    public float forcaTiro; //velocidade do projetil

    private bool flipX;

    bool esq = true;

    public float moveSpeed;

    public float tiroCD;

    bool podeAtirar = false;


    void Start()
    {
        Invoke(nameof(ResetarTiro), tiroCD);
    }


    void Update()
    {
        if (esq == true)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 180, 0);

            if(podeAtirar == true)
            {
                Atirar();
            }
        }

        else
        {
            transform.eulerAngles = Vector3.zero;
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Tiro"))
        {
            Destroy(col.gameObject); // Destruir o tiro
            Destroy(this.gameObject); // Destruir o inimigo
        }

        if (col.gameObject.CompareTag("Esq"))
        {
            esq = false;
        }
        if (col.gameObject.CompareTag("Dir"))
        {
            esq = true;
        }
    }

    void Atirar()
    {
        podeAtirar = false;

        GameObject temp = Instantiate(projet);
        temp.transform.position = gatilho.position;
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaTiro, 0);
        Destroy(temp, 3f);

        Invoke(nameof(ResetarTiro), tiroCD);
    }

    void ResetarTiro()
    {
        podeAtirar = true;     
    }
}
