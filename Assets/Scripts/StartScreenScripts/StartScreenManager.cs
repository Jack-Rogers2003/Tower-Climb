using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public TextMeshProUGUI header;
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;
    public TMP_InputField signupEmail;
    public TMP_InputField signupPassword;
    public TMP_InputField signupPassword2;
    public TMP_InputField username;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DatabaseManager.Initialize();
        SceneManager.LoadScene("AudioManagerService", LoadSceneMode.Additive);

        if (DatabaseManager.IsLoggedIn())
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    // Quit the game
    public void ExitScreen()
    {
        Application.Quit(); 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public async void Login()
    {
        if (string.IsNullOrWhiteSpace(loginEmail.text))
        {
            header.text = "Please Enter Your Email";
        }
        else if (string.IsNullOrWhiteSpace(loginPassword.text))
        {
            header.text = "Please Enter Your Password";
        }
        else
        {
            await DatabaseManager.Login(loginEmail.text, loginPassword.text);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    private bool IsEmailValid(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public async void SignUp()
    {
        if (string.IsNullOrEmpty(signupEmail.text))
        {
            header.text = "Please Enter Your Email";

        }
        else if (!IsEmailValid(signupEmail.text))
        {
            header.text = "Please Enter a Valid Email";

        }
        else if (string.IsNullOrEmpty(username.text))
        {
            header.text = "Please Enter Your Username";

        }
        else if (string.IsNullOrEmpty(signupPassword.text))
        {
            header.text = "Please Enter Your Password";
        }
        else if (string.IsNullOrEmpty(signupPassword2.text))
        {
            header.text = "Please Re-Enter Your Password";
        }
        else if (signupPassword.text != signupPassword2.text)
        {
            header.text = "Your Passwords are not the same";
        }
        else if (signupPassword.text.Length < 6)
        {
            header.text = "Password must be of length 6 or greater";
        }
        else { 
            await DatabaseManager.CreateNewUser(signupEmail.text, signupPassword.text, username.text);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    public void GuestButtonClicked()
    {
        PlayerPrefs.SetString("id", "");
        PlayerPrefs.SetString("UserName", "");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
