using UnityEngine;
using System.Collections;

[System.Serializable]
public class WorkoutData
{
    public WorkoutInfo[] workoutInfo;
}

[System.Serializable]
public class WorkoutInfo
{
    public int workoutID;
    public string workoutName;
    public string description;
    public WorkoutDetail[] workoutDetails;
}

[System.Serializable]
public class WorkoutDetail
{
    public int ballId;
    public float speed;
    public float ballDirection;
}

public class BallSpawner : MonoBehaviour
{
    public WorkoutInfo[] workoutInfo;

    void Start()
    {
        string json = Resources.Load<TextAsset>("workoutData").text;
        WorkoutData data = JsonUtility.FromJson<WorkoutData>(json);
        workoutInfo = data.workoutInfo;
    }

    public void SpawnBalls(int index, GameObject ballPrefab, Transform parent)
    {
        StartCoroutine(SpawnAndSettleBalls(index, ballPrefab, parent));
    }

    private IEnumerator SpawnAndSettleBalls(int index, GameObject ballPrefab, Transform parent)
    {
        // Get the selected workout details
        WorkoutDetail[] details = workoutInfo[index].workoutDetails;

        // Spawn balls
        foreach (var detail in details)
        {
            // Calculate position based on ballDirection
            Vector3 spawnPosition = new Vector3(detail.ballDirection * 5f, 0f, 0f); // Adjust multiplier as needed

            // Instantiate ball prefab
            GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity, parent);

            // Set ball attributes
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set speed
                rb.velocity = Vector3.zero; // Set initial velocity to zero
                rb.angularVelocity = Vector3.zero; // Set initial angular velocity to zero
                rb.useGravity = true; // Enable gravity to make balls settle
            }
            else
            {
                Debug.LogWarning("Rigidbody component not found on ball prefab.");
            }

            yield return null; // Yield to the next frame
        }

        // Check if all balls have settled
        yield return new WaitForSeconds(2f); // Adjust the delay duration as needed
        CheckAndDisableGravity(parent);
    }

    private void CheckAndDisableGravity(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null && rb.velocity.magnitude > 0.01f) // Adjust threshold as needed
            {
                // If any ball is still moving, wait and check again
                StartCoroutine(WaitAndCheckAgain(parent));
                return;
            }
        }

        // All balls have settled, disable gravity for each ball
        foreach (Transform child in parent)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
            }
        }
    }

    private IEnumerator WaitAndCheckAgain(Transform parent)
    {
        yield return new WaitForSeconds(1f); // Adjust the delay duration as needed
        CheckAndDisableGravity(parent);
    }
}
