using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    void Start()
    {
        // Check and resolve Firebase dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase ready");
                FirebaseApp.DefaultInstance.Options.DatabaseUrl =
                    new System.Uri("https://csc384leaderboard-default-rtdb.europe-west1.firebasedatabase.app/");
                StartDatabase();  // Only start the database after Firebase is ready
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    void StartDatabase()
    {
        // Ensure Firebase database reference is properly initialized
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("test").SetValueAsync("Hello from Unity on Windows!");
    }
}
