
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("게임 설정")]
    public string gameSceneName = "GameScene";

    [Header("코인 UI")]
    [SerializeField] private TMP_Text coinsText;

    private void Start()
    {
        UpdateCoinsUI();
    }

    // 보유 코인 표시
    public void UpdateCoinsUI()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager를 찾을 수 없습니다.");
            return;
        }

        if (coinsText != null)
        {
            coinsText.text =
                "Coin: " + GameManager.Instance.coins;
        }
    }

    // 게임 시작
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
        Debug.Log("시작 버튼 클릭 성공!");
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
