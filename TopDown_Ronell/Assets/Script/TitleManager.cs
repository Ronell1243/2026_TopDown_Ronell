using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public void GameStartButton()
    {
        GameManager.Instance.StartGame();
    }

    public void GameExitButton()
    {
        GameManager.Instance.StartExit();
        Debug.Log("게임 종료");
    }    
}
