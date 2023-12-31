using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CookingEvent : MonoBehaviour
{
    Quest nowQuest;
    public GameObject questImagePrefab;
    private GameObject questImage;
    private Quest newQuest;
    public static bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (QuestManager.Instance != null)
        {
            if (QuestManager.Instance.IsActive && !isActive)
            {
                // 현재 퀘스트 가져오기
                nowQuest = QuestManager.Instance.GetNowQuest();

                // 퀘스트의 제목이 "요리"인지 확인
                if (nowQuest.title == "요리")
                {
                    isActive = true;
                    SetQuestImage();
                }
            }
        }

        if (Cooking.isSuccess)
        {
            Debug.Log("요리 성공");

            Cooking.isSuccess = false;
            QuestManager.CompleteQuest();
            Destroy(questImage);
        }

    }

    private void SetQuestImage()
    {
        // Fishing House 위에 퀘스트 이미지 발생
        Debug.Log("요리 퀘스트 발생");

        Vector3 questPosition = transform.position;
        questPosition.y += 6;
        questPosition.x += 6;
        questImage = Instantiate(questImagePrefab, questPosition, Quaternion.identity);
        questImage.transform.SetParent(transform);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (QuestManager.Instance.IsActive && isActive)
            {
                Debug.Log("요리 퀘스트 이동");
                // 현재 씬의 이름을 저장
                string currentSceneName = SceneManager.GetActiveScene().name;

                // 새로운 씬을 additive 모드로 로드
                SceneManager.LoadScene("Cooking", LoadSceneMode.Additive);

                // 로드한 씬을 활성화
                Scene loadedScene = SceneManager.GetSceneByName("Cooking");
                SceneManager.SetActiveScene(loadedScene);

                // 현재 씬 유지를 위해 이전 씬의 상태를 유지
                SceneManager.MergeScenes(loadedScene, SceneManager.GetSceneByName(currentSceneName));


            }
        }
    }
}
