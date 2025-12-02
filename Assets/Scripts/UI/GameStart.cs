using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)                   //아무 키나 눌리면
        {
            SceneManager.LoadScene("MainScene"); // 다음 씬 이름
        }
    }
}
