using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class WorkoutManager : MonoBehaviour
{
    public Button[] workoutButtons;
    public TextMeshProUGUI workoutNameText;
    public TextMeshProUGUI workoutDescriptionText;
    public GameObject ballPrefab;
    public Transform ballSpawnParent;
    public BallSpawner ballSpawner;

    void Start()
    {
        // Assign button click listeners
        for (int i = 0; i < workoutButtons.Length; i++)
        {
            int index = i; // Necessary to capture the correct index in the lambda expression
            workoutButtons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    void OnButtonClick(int index)
    {
        // Display workout info
        workoutNameText.text = ballSpawner.workoutInfo[index].workoutName;
        workoutDescriptionText.text = ballSpawner.workoutInfo[index].description;

        // Spawn balls
        ballSpawner.SpawnBalls(index, ballPrefab, ballSpawnParent);
    }
}
