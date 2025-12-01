using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainScene"); // ∞‘¿” æ¿ ¿Ã∏ß
        }
    }
}
