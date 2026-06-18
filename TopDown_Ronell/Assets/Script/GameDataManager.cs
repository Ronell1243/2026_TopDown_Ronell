using System.IO;
using UnityEngine;


public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public GameSettingData gameSettingData;
    public SaveData saveData;
    public int isTutorialFinished;
    private string savePath;
    // 현재 스테이지 실시간 데이터
    public int currentItemCount = 0;   // 현재 모은 아이템 개수
    public float elapsedTime = 0f;     // 흘러간 시간 (초 단위)

    // 게임 시작할 때 데이터를 초기화해주는 함수
    public void ResetStageData()
    {
        currentItemCount = 0;
        elapsedTime = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/saveData.json";

            LoadJsonData();
            LoadPlayerPrefs();

        }
        else
        {
            Destroy(gameObject);      // 중복 방지
        }
    }

    //ScriptableObject 파트
    public int GetPlayerHp()
    {
        int baseHP = gameSettingData.startHp;
        int bonusHP = gameSettingData.hpBonusPerDeath;

        return baseHP + bonusHP * saveData.deathCount;
    }

    public int GetPlayerAttack()
    {
        int baseAttack = gameSettingData.startAttack;
        int bonusAttack = gameSettingData.atkBonusPerDeath;
        return baseAttack + bonusAttack * saveData.deathCount;
    }

    public float GetPlayerMoveSpeed()
    {
        return gameSettingData.playerMoveSpeed;
    }

    public void GainItemJson()
    {
        currentItemCount++;               // 이번 판 수집량 증가
        saveData.totalCollectedItems++;    // JSON 데이터 내부 누적 총 획득량 증가

        SaveJsonData();                   // 변경된 데이터를 json 파일로 즉시 저장!
    }

    public void CheckAndSaveBestTime()
    {
        // 현재 걸린 시간이 기존 최고 기록보다 더 빠르다면 기록 갱신
        if (elapsedTime < saveData.bestClearTime)
        {
            saveData.bestClearTime = elapsedTime;
            Debug.Log($"최고 기록! 변경된 시간: {elapsedTime:F2}초");
        }

        SaveJsonData(); // 파일 저장
    }


    //Json 파트
    public void SaveGameResult()
    {
        saveData.deathCount++;

        SaveJsonData();
    }

    public void SaveJsonData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("JSON 저장 완료: " + savePath);
    }

    public void LoadJsonData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            saveData = new SaveData();
            SaveJsonData();
        }
    }

    public void DeleteJsonData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        saveData = new SaveData();
        SaveJsonData();

        Debug.Log("JSON 저장 데이터 삭제");
    }

    //PlayerPrefs 파트

    public void LoadPlayerPrefs()
    {
        isTutorialFinished = PlayerPrefs.GetInt("TUTORIAL", 0);
    }

    public void SavePlyerPrefs()
    {
        PlayerPrefs.SetInt("TUTORIAL", isTutorialFinished);
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs 저장 완료");
    }
    
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("TUTORIAL");
        LoadPlayerPrefs();

        Debug.Log("PlayerPrefs 삭제 완료");
    }
}
