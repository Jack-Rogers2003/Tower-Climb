using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    // Firebase reference
    private static DatabaseReference reference;
    private static bool isFirebaseInitialized = false;
    private static string uri = "https://csc384leaderboard-default-rtdb.europe-west1.firebasedatabase.app/";


    // Singleton instance to access the FirebaseManager from anywhere
    public static DatabaseManager Instance { get; private set; }

    public static void Initialize()
    {
        if (isFirebaseInitialized)
        {
            return;
        }
        // Check and resolve Firebase dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase ready");
                FirebaseApp.DefaultInstance.Options.DatabaseUrl =
                    new System.Uri(uri);
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                isFirebaseInitialized = true;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public static async Task<List<string>> ReadData()
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogWarning("Firebase is not initialized yet. Please call FirebaseManager.Initialize() first.");
            return null;
        }

        try
        {
            var snapshot = await reference.Child("test").GetValueAsync();
            Debug.Log(snapshot.Value);
            List<string> keyValues = new List<string>();

            keyValues.Add((string)snapshot.Value);


            return keyValues;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error getting children: " + ex.Message);
            return null;
        }

    }
}
