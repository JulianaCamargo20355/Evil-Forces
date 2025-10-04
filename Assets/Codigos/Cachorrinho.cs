using UnityEngine;

public class Cachorrinho : MonoBehaviour
{
    public GameObject projet; // bala
    public Transform gatilho; // posição que sai o tiro
    public float forcaTiro; // velocidade do projetil
    public float moveSpeed;
    public float tiroCD;

    private bool tiro;
    private bool flipX;
    private bool esq = true;
    private bool podeAtirar = false;

    void Start()
    {
        Invoke(nameof(ResetarTiro), tiroCD);
    }

    void Update()
    {
        if (esq)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 180, 0);

            if (podeAtirar)
                Atirar();
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
            Destroy(col.gameObject); // destruir o tiro
            Destroy(this.gameObject); // destruir o inimigo

            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.inimigoDerrotado = true;
                player.VerificarVitoria();
            }
        }

        if (col.gameObject.CompareTag("Esq"))
            esq = false;
        if (col.gameObject.CompareTag("Dir"))
            esq = true;
    }

    void Atirar()
    {
        podeAtirar = false;

        GameObject temp = Instantiate(projet, gatilho.position, Quaternion.identity);
        temp.GetComponent<Rigidbody2D>().velocity = new Vector2(forcaTiro, 0);
        Destroy(temp, 3f);

        Invoke(nameof(ResetarTiro), tiroCD);
    }

    void ResetarTiro()
    {
        podeAtirar = true;
    }
}
