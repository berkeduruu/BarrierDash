using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference databaseRef;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text feedbackText;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            }else
            {
                Debug.Log("Database Error: " + task.Exception);
            }
        });
    }
    
    public void LoginAttempt()
    {
        string email = emailLoginField.text;
        string password = passwordLoginField.text;

        databaseRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                feedbackText.text = "Error getting data";
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var user in snapshot.Children)
                {
                    string userEmail = user.Child("email").Value.ToString();
                    string userPassword = user.Child("password").Value.ToString();

                    if (userEmail == email && userPassword == password)
                    {
                        feedbackText.text = "Login Successful!";
                        SceneManager.LoadScene("GameScene");
                        break;
                    }
                }
                
                feedbackText.text = "Invalid email or password";
            }
        });
    }
}

