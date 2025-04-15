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
    private static readonly string uri = "https://csc384leaderboard-default-rtdb.europe-west1.firebasedatabase.app/";


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

    public static async Task<List<(string, string)>> ReadData()
    {
        List<(string, string)> keyValues = new List<(string, string)>();

        var task = reference.GetValueAsync();
        await task.ContinueWithOnMainThread(t =>
        {
            if (t.IsCompleted)
            {
                DataSnapshot snapshot = t.Result;
                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    keyValues.Add((childSnapshot.Key, childSnapshot.Child("Username").Value.ToString()));
                }
            }
            else
            {
                Debug.LogError("Error getting data: " + t.Exception);
            }
        });

        return keyValues;
    }



    public static void CreateNewUser(string username)
    {
        reference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int id = (int)snapshot.ChildrenCount + 1;
                reference.Child((id).ToString()).Child("Username").SetValueAsync(username);
                PlayerPrefs.SetString("id", id.ToString());
            }
            else
            {
                Debug.LogError("Error adding data: " + task.Exception);
            }
        });
    }

    public static void UpdateUsername(string username)
    {
        reference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string id = PlayerPrefs.GetString("id");
                reference.Child((id).ToString()).Child("Username").SetValueAsync(username);
            }
            else
            {
                Debug.LogError("Error adding data: " + task.Exception);
            }
        });
    }

}
