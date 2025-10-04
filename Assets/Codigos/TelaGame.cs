using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaGame : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("Game"); 
    }
}