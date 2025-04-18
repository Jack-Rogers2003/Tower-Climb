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
    private static readonly string uri = "https://csc384leaderboard-default-rtdb.europe-west1.firebasedatabase.app/";


    // Singleton instance to access the FirebaseManager from anywhere
    public static void Initialize()
    {
        // Check and resolve Firebase dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Once Firebase is initialized, set the database URL and reference
                FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(uri);
                reference = FirebaseDatabase.DefaultInstance.RootReference;

                // Set the initialization flag to true

                Debug.Log("Firebase Initialized and Database URL set!");
            }
            else
            {
                // Log any issues with Firebase initialization
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }


    public static async Task<List<(string, string, string)>> ReadData()
    {
        List<(string, string, string)> keyValues = new();

        var task = reference.GetValueAsync();
        await task.ContinueWithOnMainThread(t =>
        {
            if (t.IsCompleted)
            {
                DataSnapshot snapshot = t.Result;
                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {

                    Debug.Log($"{childSnapshot.Key}");
                    keyValues.Add((childSnapshot.Key, childSnapshot.Child("Username").Value.ToString(), childSnapshot.Child("Battles").Value.ToString()));
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

    public static void UpdateRank()
    {
        string winCount = PlayerPrefs.GetString("BattleCount", "0");
        string id = PlayerPrefs.GetString("id", "0");

        reference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string highestWinCount = snapshot.Child(id).Child("Battles").Value?.ToString() ?? "0";

                if (int.Parse(highestWinCount) < int.Parse(winCount))
                {
                    reference.Child(id).Child("Battles").SetValueAsync(winCount);
                }
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

    public static async Task<List<(string, string)>> GetBattles()
    {
        List<(string, string)> keyValues = new();

        var task = reference.GetValueAsync();
        await task.ContinueWithOnMainThread(t =>
        {
            if (t.IsCompleted)
            {
                DataSnapshot snapshot = t.Result;
                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    keyValues.Add((childSnapshot.Key, childSnapshot.Child("Battles").Value?.ToString() ?? "0"));
                }
            }
            else
            {
                Debug.LogError("Error getting data: " + t.Exception);
            }
        });

        return keyValues;
    }

}
