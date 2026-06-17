using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("오디오 소스(스피커)")]
    public AudioSource bgmSource;  // 배경음악 전용 스피커
    public AudioSource sfxSource;  // 효과음 전용 스피커
    
    [Header("씬별 배경음악 mp3 등록")]
    public AudioClip titleBGM;     // 타이틀 씬 브금
    public AudioClip cutsceneBGM;  // 컷씬 씬 브금 (필요 없다면 비워두셔도 됩니다)
    public AudioClip inGameBGM;    // 인게임 씬 브금
    private void Awake()
    {
        // 씬이 바뀌어도 파괴되지 않는 싱글톤 세팅
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        // 오브젝트가 혹시 파괴될 때 등록했던 이벤트를 깔끔하게 해제해 줍니다.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 로드된 씬의 이름을 확인해서 그에 맞는 브금을 틀어줍니다.
        // ⚠️ 빌드 세팅이나 Scene 이름과 대소문자까지 똑같이 적어주셔야 합니다!
        if (scene.name == "TitleScene")
        {
            PlayBGM(titleBGM);
        }
        else if (scene.name == "IntroCutScene") // 만드신 컷씬 이름으로 변경 가능
        {
            PlayBGM(cutsceneBGM);
        }
        else if (scene.name == "HomeScene") // 만드신 인게임 플레이 씬 이름으로 변경
        {
            PlayBGM(inGameBGM);
        }
    }
    // 🎵 배경음악 재생 함수
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmSource == null || bgmClip == null) return;

        // 이미 재생 중인 배경음악과 같은 음악이면 무시
        if (bgmSource.clip == bgmClip && bgmSource.isPlaying) return;

        bgmSource.clip = bgmClip;
        bgmSource.loop = true; // 배경음악은 항상 루프 재생
        bgmSource.Play();
    }


    // 🎵 배경음악 정지 함수
    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxSource == null || sfxClip == null) return;
        sfxSource.PlayOneShot(sfxClip, 1f); // 기본 볼륨 1f로 재생
    }
    // 🔊 효과음 재생 함수 (겹쳐서 재생 가능)
    public void PlaySFX(AudioClip sfxClip, float volume = 1f)
    {
        if (sfxSource == null || sfxClip == null) return;
        sfxSource.PlayOneShot(sfxClip, volume);
    }

    // 🎛️ 배경음악(BGM) 볼륨 조절 함수 (볼륨 바와 연결할 것)
    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume; // 0.0 ~ 1.0 사이의 값이 들어옵니다.
        }
    }

    // 🎛️ 효과음(SFX) 볼륨 조절 함수 (볼륨 바와 연결할 것)
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume; // 0.0 ~ 1.0 사이의 값이 들어옵니다.
        }
    }
}