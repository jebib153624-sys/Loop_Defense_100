using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
   public void fuckyouSceneMoveButton()
    {
        SceneManager.LoadScene(2);
        AudioManager.instance.PlaySfx(10);
    }
    public void fuckyouSceneMoveButton1()
    {
        SceneManager.LoadScene(0);
        AudioManager.instance.PlaySfx(10);
    }
}
