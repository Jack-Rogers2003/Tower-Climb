using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using Firebase.Auth;


public class DatabaseManager : MonoBehaviour
{
    // Firebase reference
    private static DatabaseReference reference;
    private static readonly string uri = "https://csc384leaderboard-default-rtdb.europe-west1.firebasedatabase.app/";
    private static FirebaseAuth auth;

    // Singleton instance to access the FirebaseManager from anywhere
    public static void Initialize()
    {
        auth = FirebaseAuth.DefaultInstance;
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

    public static string GetCurrentUserID()
    {
        return auth.CurrentUser.UserId;
    }

    public static void LogOut()
    {
        auth.SignOut();
    }

    public static bool IsLoggedIn()
    {
        try
        {
            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                Debug.Log("email:" + user.Email);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch 
        {
            return false;
        }
    }


    public static async Task<List<(string, string, string)>> ReadData()
    {
        List<(string, string, string)> keyValues = new();

        var task = reference.GetValueAsync();
        await task.ContinueWithOnMainThread(t =>
        {
            if (!t.IsCompleted || t.IsFaulted || t.Exception != null)
            {
                Debug.LogError("Task failed: " + t.Exception);
                return;
            }

            DataSnapshot snapshot = t.Result;

            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                try
                {
                    var key = childSnapshot.Key;
                    var usernameNode = childSnapshot.Child("Username");
                    var battlesNode = childSnapshot.Child("Battles");

                    if (usernameNode == null || battlesNode == null)
                    {
                        Debug.LogWarning($"One of the fields is null for key: {key}");
                        continue;
                    }

                    var username = usernameNode.Value?.ToString();
                    var battles = battlesNode.Value?.ToString();

                    if (username == null || battles == null)
                    {
                        Debug.LogWarning($"Value was null for key: {key}, Username: {username}, Battles: {battles}");
                        continue;
                    }

                    keyValues.Add((key, username, battles));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception for key: {childSnapshot.Key} => {ex}");
                }
            }
        });

        return keyValues;
    }

    public static void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCompleted)
            {
                reference.GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        foreach (DataSnapshot childSnapshot in snapshot.Children)
                        {
                            if (childSnapshot.Child("email").Value == null)
                            {
                                continue;
                            }
                            if (childSnapshot.Child("email").Value.ToString() == email)
                            {
                                PlayerPrefs.SetString("id", childSnapshot.Key.ToString());
                                PlayerPrefs.SetString("UserName", childSnapshot.Child("Username").Value.ToString());
                                PlayerPrefs.Save(); 
                                return;
                            }
                        }
                    }
                });
            }
        });
    }

    public static void CreateNewUser(string email, string password, string username)
    {
        Debug.Log("Creating");
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (!task.IsCompleted && task.IsFaulted)
                Debug.LogError("Error: " + task.Exception);
            else
            {
                CreateNewUser(username);
            }
        });
       }

    public static void CreateNewUser(string username)
    {
        reference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseUser user = auth.CurrentUser;
                DataSnapshot snapshot = task.Result;
                int id = (int)snapshot.ChildrenCount + 1;
                reference.Child((id).ToString()).Child("id").SetValueAsync(user.UserId);
                reference.Child((id).ToString()).Child("Username").SetValueAsync(username);
                reference.Child((id).ToString()).Child("Battles").SetValueAsync("0");

                PlayerPrefs.SetString("id", id.ToString());
                PlayerPrefs.SetString("UserName", username.ToString());

                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError("Error adding data: " + task.Exception);
            }
        });
    }

    public async static Task<bool> UpdateRank()
    {
        string winCount = PlayerPrefs.GetString("BattleCount", "0");
        string id = PlayerPrefs.GetString("id", "0");
        bool flag = false;

        var tcs = new TaskCompletionSource<bool>();


        await reference.GetValueAsync().ContinueWithOnMainThread(async task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                string highestWinCount = snapshot.Child(id).Child("Battles").Value?.ToString() ?? "0";

                if (int.Parse(highestWinCount) < int.Parse(winCount))
                {
                    List<(string, string, string)> ranksPre = await ReadData();
                    ranksPre = ranksPre.OrderByDescending(x => int.Parse(x.Item3)).ToList();

                    await reference.Child(id).Child("Battles").SetValueAsync(winCount);


                    //Key - Username - Battle
                    List<(string, string, string)> ranksPost = await ReadData();
                    ranksPost = ranksPost.OrderByDescending(x => int.Parse(x.Item3)).ToList();


                    if (ranksPost.FindIndex(pair => pair.Item1 == PlayerPrefs.GetString("id")) < ranksPre.FindIndex(pair => pair.Item1 == PlayerPrefs.GetString("id")))
                    {
                        flag = true;
                        tcs.SetResult(flag);
                    }
                }
            }
            else
            {
                Debug.LogError("Error adding data: " + task.Exception);
            }
        });

        return await tcs.Task;
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
