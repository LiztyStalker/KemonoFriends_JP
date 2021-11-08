using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class UIFirebaseAuth : MonoBehaviour {
       

    protected Firebase.Auth.FirebaseAuth auth;
    
    protected Firebase.Auth.FirebaseAuth otherAuth;

    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
      new Dictionary<string, Firebase.Auth.FirebaseUser>();

    public GUISkin fb_GUISkin;
    
    private string logText = "";

    protected string email = "";
    protected string password = "";
    protected string displayName = "";
    protected string phoneNumber = "";
    protected string receivedCode = "";
    // Flag set when a token is being fetched.  This is used to avoid printing the token
    // in IdTokenChanged() when the user presses the get token button.
    private bool fetchingToken = false;
    // Enable / disable password input box.
    // NOTE: In some versions of Unity the password input box does not work in
    // iOS simulators.
    public bool usePasswordInput = false;
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;
    bool UIEnabled = true;

    // Set the phone authentication timeout to a minute.
    private uint phoneAuthTimeoutMs = 60 * 1000;
    // The verification id needed along with the sent code for phone authentication.
    private string phoneAuthVerificationId;

    // Options used to setup secondary authentication object.
    private Firebase.AppOptions otherAuthOptions = new Firebase.AppOptions
    {
        ApiKey = "",
        AppId = "",
        ProjectId = ""
    };

    const int kMaxLogSize = 16382;


    //파이어베이스 상태
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

	// Use this for initialization
	void Start () {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }


    void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");

        //파이어베이스 연결된 인스턴스 가져오기
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //계정 변경 상태 이벤트 연결
        auth.StateChanged += AuthStateChanged;

        //ID Token 변경 상태 이벤트 연결
        auth.IdTokenChanged += IdTokenChanged;


        // Specify valid options to construct a secondary authentication object.
        //특수 계정 옵션이 있으면 - 재해석 필요
        if (otherAuthOptions != null &&
            //ApiKey가 비어있지 않고
            !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) ||
            //AppID가 비어있거나
              String.IsNullOrEmpty(otherAuthOptions.AppId) ||
            //ProjectId가 비어있으면
              String.IsNullOrEmpty(otherAuthOptions.ProjectId)))
        {
            try
            {
                //계정 옵션 가져오기
                otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
                  otherAuthOptions, "Secondary"));

                //계정 옵션 상태 이벤트 연결
                otherAuth.StateChanged += AuthStateChanged;
                otherAuth.IdTokenChanged += IdTokenChanged;
            }
            catch (Exception)
            {
                Debug.Log("ERROR: Failed to initialize secondary authentication object.");
            }
        }

        //초기화
        AuthStateChanged(this, null);
    }

    //계정 상태 변경자
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        //계정 가져오기
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;

        //user 값 가져오기
        if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);

        //보낸계정과 계정이 같고 보낸유저와 유저가 같지 않으면
        if (senderAuth == auth && senderAuth.CurrentUser != user)
        {
            //유저와 보낸 계정의 현재 유저가 같지 않고 보낸계정의 현재 유저가 비어있지 않으면
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;

            //로그인 안되어있으면
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            //보낸 계정의 현재 유저 등록
            user = senderAuth.CurrentUser;

            //사용계정에 현재 유저 등록
            userByAuth[senderAuth.App.Name] = user;

            //로그인 되어있으면
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                displayName = user.DisplayName ?? "";
                //DisplayDetailedUserInfo(user, 1);
            }
        }
    }

    //ID Token 변경자
    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        //보낸자 계정 등록
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;

        //토큰을 보내지 않았고 보낸 계정이 같고 보낸 계정의 현재 유저가 있으면
        if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        {
            //현재 유저의 토큰 동기화 false 후 continue with Thread 실행
            senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
              task => Debug.Log(string.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        }
    }


    #region ###################### 계정 생성 ##############################

    /// <summary>
    /// 계정 만들기
    /// 계정 생성 동기화
    /// </summary>
    /// <returns></returns>
    public Task CreateUserAsync()
    {
        Debug.Log(String.Format("Attempting to create user {0}...", email));
        //DisableUI();

        // This passes the current displayName through to HandleCreateUserAsync
        // so that it can be passed to UpdateUserProfile().  displayName will be
        // reset by AuthStateChanged() when the new user is created and signed in.
        string newDisplayName = displayName;
        return auth.CreateUserWithEmailAndPasswordAsync(email, password)
          .ContinueWith((task) =>
          {
              return HandleCreateUserAsync(task, newDisplayName: newDisplayName);
          }).Unwrap();
    }

    /// <summary>
    /// 계정 생성 동기화
    /// </summary>
    /// <param name="authTask"></param>
    /// <param name="newDisplayName"></param>
    /// <returns></returns>
    Task HandleCreateUserAsync(Task<Firebase.Auth.FirebaseUser> authTask,
                             string newDisplayName = null)
    {
//        EnableUI();

        //작업이 완료되었으면
        if (LogTaskCompletion(authTask, "User Creation"))
        {
            if (auth.CurrentUser != null)
            {
                Debug.Log(String.Format("User Info: {0}  {1}", auth.CurrentUser.Email,
                                       auth.CurrentUser.ProviderId));
                return UpdateUserProfileAsync(newDisplayName: newDisplayName);
            }
        }
        // Nothing to update, so just return a completed Task.
        return Task.FromResult(0);
    }


    /// <summary>
    /// 유저 프로필 업데이트 동기화
    /// </summary>
    /// <param name="newDisplayName"></param>
    /// <returns></returns>
    public Task UpdateUserProfileAsync(string newDisplayName = null)
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to update user profile");
            return Task.FromResult(0);
        }
        displayName = newDisplayName ?? displayName;
        Debug.Log("Updating user profile");
        //        DisableUI();
        return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
        {
            DisplayName = displayName,
            PhotoUrl = auth.CurrentUser.PhotoUrl,
        }).ContinueWith(HandleUpdateUserProfile);
    }

    /// <summary>
    /// 유저 프로필 업데이트 핸들러
    /// </summary>
    /// <param name="authTask"></param>
    void HandleUpdateUserProfile(Task authTask)
    {
        //        EnableUI();
        if (LogTaskCompletion(authTask, "User profile"))
        {
            DisplayDetailedUserInfo(auth.CurrentUser, 1);
        }
    }

    #endregion

    #region ############## 공통 #####################

    /// <summary>
    /// 작업 완료 점검
    /// </summary>
    /// <param name="task"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        //작업 취소 됨
        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        //작업 실패됨
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encounted an error.");
            //작업 예외 출력하기
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                      ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                }
                Debug.Log(authErrorCode + exception.ToString());
            }
        }
        //작업 종료됨
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
            complete = true;
        }
        return complete;
    }

    #endregion


#region ################### 로그인 ############################

    /// <summary>
    /// 익명 로그인
    /// </summary>
    /// <returns></returns>
    public Task SigninAnonymouslyAsync() {
        Debug.Log("Attempting to sign anonymously...");
//        DisableUI();
        return auth.SignInAnonymouslyAsync().ContinueWith(HandleSigninResult);
    }


    /// <summary>
    /// 이메일 로그인
    /// </summary>
    /// <returns></returns>
    public Task SigninAsync() {
        Debug.Log(String.Format("Attempting to sign in as {0}...", email));
//        DisableUI();
        return auth.SignInWithEmailAndPasswordAsync(email, password)
          .ContinueWith(HandleSigninResult);
    }
    

    /// <summary>
    /// 자격증명 로그인
    /// </summary>
    /// <returns></returns>
    public Task SigninWithCredentialAsync() {
        Debug.Log(String.Format("Attempting to sign in as {0}...", email));
        //DisableUI();
        Firebase.Auth.Credential cred = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
        return auth.SignInWithCredentialAsync(cred).ContinueWith(HandleSigninResult);
    }


    /// <summary>
    /// 로그인 핸들러
    /// </summary>
    /// <param name="authTask"></param>
    void HandleSigninResult(Task<Firebase.Auth.FirebaseUser> authTask)
    {
        //        EnableUI();
        LogTaskCompletion(authTask, "Sign-in");
    }
#endregion

#region ############## 자격증명 연결 ###############################
    void LinkWithCredential() {
        if (auth.CurrentUser == null) {
          Debug.Log("Not signed in, unable to link credential to user.");
          return;
        }
        Debug.Log("Attempting to link credential to user...");
        Firebase.Auth.Credential cred = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
        auth.CurrentUser.LinkWithCredentialAsync(cred).ContinueWith(HandleLinkCredential);
    }

    void HandleLinkCredential(Task authTask) {
        if (LogTaskCompletion(authTask, "Link Credential")) {
          DisplayDetailedUserInfo(auth.CurrentUser, 1);
        }
    }
#endregion


#region ############### 재연결 #######################
    public void ReloadUser() {
        if (auth.CurrentUser == null) {
          Debug.Log("Not signed in, unable to reload user.");
          return;
        }
        Debug.Log("Reload User Data");
        auth.CurrentUser.ReloadAsync().ContinueWith(HandleReloadUser);
    }


    void HandleReloadUser(Task authTask) {
        if (LogTaskCompletion(authTask, "Reload")) {
          DisplayDetailedUserInfo(auth.CurrentUser, 1);
        }
    }
#endregion


    #region ############### 토큰 가져오기 #######################
    /// <summary>
    /// 토큰 가져오기
    /// </summary>
    public void GetUserToken()
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to get token.");
            return;
        }
        Debug.Log("Fetching user token");
        fetchingToken = true;
        auth.CurrentUser.TokenAsync(false).ContinueWith(HandleGetUserToken);
    }

    /// <summary>
    /// 토큰 핸들러
    /// </summary>
    /// <param name="authTask"></param>
    void HandleGetUserToken(Task<string> authTask)
    {
        fetchingToken = false;
        if (LogTaskCompletion(authTask, "User token fetch"))
        {
            Debug.Log("Token = " + authTask.Result);
        }
    }
    #endregion


    #region ############# 유저 정보 가져오기 #####################
    void GetUserInfo()
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to get info.");
        }
        else
        {
            Debug.Log("Current user info:");
            DisplayDetailedUserInfo(auth.CurrentUser, 1);
        }
    }
    #endregion


    #region ############# 로그아웃 #####################
    public void SignOut()
    {
        Debug.Log("Signing out.");
        auth.SignOut();
    }
    #endregion


    #region ########### 유저 삭제하기 #############

    /// <summary>
    /// 유저 정보 삭제 동기화
    /// </summary>
    /// <returns></returns>
    public Task DeleteUserAsync()
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log(String.Format("Attempting to delete user {0}...", auth.CurrentUser.UserId));
//            DisableUI();
            return auth.CurrentUser.DeleteAsync().ContinueWith(HandleDeleteResult);
        }
        else
        {
            Debug.Log("Sign-in before deleting user.");
            // Return a finished task.
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// 삭제 핸들러
    /// </summary>
    /// <param name="authTask"></param>
    void HandleDeleteResult(Task authTask)
    {
//        EnableUI();
        LogTaskCompletion(authTask, "Delete user");
    }

    #endregion


    #region ############## 이메일 공급자 ###################

    /// <summary>
    /// 이메일 공급자 보이기
    /// </summary>
    public void DisplayProvidersForEmail()
    {
        auth.FetchProvidersForEmailAsync(email).ContinueWith((authTask) =>
        {
            if (LogTaskCompletion(authTask, "Fetch Providers"))
            {
                Debug.Log(String.Format("Email Providers for '{0}':", email));
                foreach (string provider in authTask.Result)
                {
                    Debug.Log(provider);
                }
            }
        });
    }

    #endregion


    #region ############## 비밀번호 재설정 메일 보내기 ###################
    public void SendPasswordResetEmail()
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWith((authTask) =>
        {
            if (LogTaskCompletion(authTask, "Send Password Reset Email"))
            {
                Debug.Log("Password reset email sent to " + email);
            }
        });
    }

    #endregion

    #region ################# 폰으로 증명하기 #####################

    /// <summary>
    /// 폰으로 증명
    /// </summary>
    //public void VerifyPhoneNumber()
    //{
    //    var phoneAuthProvider = Firebase.Auth.PhoneAuthProvider.GetInstance(auth);
    //    phoneAuthProvider.VerifyPhoneNumber(phoneNumber, phoneAuthTimeoutMs, null,
    //      verificationCompleted: (cred) =>
    //      {
    //          DebugLog("Phone Auth, auto-verification completed");
    //          auth.SignInWithCredentialAsync(cred).ContinueWith(HandleSigninResult);
    //      },
    //      verificationFailed: (error) =>
    //      {
    //          Debug.Log("Phone Auth, verification failed: " + error);
    //      },
    //      codeSent: (id, token) =>
    //      {
    //          phoneAuthVerificationId = id;
    //          Debug.Log("Phone Auth, code sent");
    //      },
    //      codeAutoRetrievalTimeOut: (id) =>
    //      {
    //          Debug.Log("Phone Auth, auto-verification timed out");
    //      });
    //}

    ///// <summary>
    ///// 증명 받기
    ///// </summary>
    //public void VerifyReceivedPhoneCode()
    //{
    //    var phoneAuthProvider = Firebase.Auth.PhoneAuthProvider.GetInstance(auth);
    //    // receivedCode should have been input by the user.
    //    var cred = phoneAuthProvider.GetCredential(phoneAuthVerificationId, receivedCode);
    //    auth.SignInWithCredentialAsync(cred).ContinueWith(HandleSigninResult);
    //}

    #endregion



    #region ########## 기본 계정 바꾸기 ####################
    public void SwapAuthFocus()
    {
        if (!HasOtherAuth) return;
        var swapAuth = otherAuth;
        otherAuth = auth;
        auth = swapAuth;
        Debug.Log(String.Format("Changed auth from {0} to {1}",
                               otherAuth.App.Name, auth.App.Name));
    }

    public bool HasOtherAuth { get { return auth != otherAuth && otherAuth != null; } }
    #endregion



    #region ########################## 디스플레이 #######################################


    /// <summary>
    /// 유저 정보 자세히 보이기
    /// </summary>
    /// <param name="user"></param>
    /// <param name="indentLevel"></param>
    void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
    {
        DisplayUserInfo(user, indentLevel);
        Debug.Log("  Anonymous: " + user.IsAnonymous);
        Debug.Log("  Email Verified: " + user.IsEmailVerified);
        var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
        if (providerDataList.Count > 0)
        {
            Debug.Log("  Provider Data:");
            foreach (var providerData in user.ProviderData)
            {
                DisplayUserInfo(providerData, indentLevel + 1);
            }
        }
    }

    /// <summary>
    /// 유저 정보 보이기
    /// </summary>
    /// <param name="userInfo"></param>
    /// <param name="indentLevel"></param>
    void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        var userProperties = new Dictionary<string, string> {
              {"Display Name", userInfo.DisplayName},
              {"Email", userInfo.Email},
              {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
              {"Provider ID", userInfo.ProviderId},
              {"User ID", userInfo.UserId}
            };
        foreach (var property in userProperties)
        {
            if (!String.IsNullOrEmpty(property.Value))
            {
                Debug.Log(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
            }
        }
    }

    #endregion
}
