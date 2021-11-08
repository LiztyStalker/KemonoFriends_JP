using System;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Messaging;


namespace FirebaseAPI
{

    public class FirebaseCloudMessaging
    {

        FirebaseApi firebaseAPI;

        //파이어베이스 상태
        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

        public FirebaseCloudMessaging(FirebaseApi parent)
        {
            firebaseAPI = parent;
            InitializeFirebase();


            //파이어베이스 연동
            //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            //{
            //    dependencyStatus = task.Result;

            //    //파이어베이스 연동 성공 상태이면
            //    if (dependencyStatus == Firebase.DependencyStatus.Available)
            //    {
            //        //초기화
            //        InitializeFirebase();
            //    }
            //    else
            //    {
            //        Debug.LogError(
            //          "Could not resolve all Firebase dependencies: " + dependencyStatus);
            //    }
            //});
            //            FirebaseMessaging.TokenReceived += OnTokenReceived;
            //            FirebaseMessaging.MessageReceived += OnMessageReceived;

            
        }


        void InitializeFirebase()
        {
            //계정 초기화
            //DebugLog("Setting up Firebase Auth");
            //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            //auth.StateChanged += AuthStateChanged;
            //auth.IdTokenChanged += IdTokenChanged;
            //// Specify valid options to construct a secondary authentication object.
            //if (otherAuthOptions != null &&
            //    !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) ||
            //      String.IsNullOrEmpty(otherAuthOptions.AppId) ||
            //      String.IsNullOrEmpty(otherAuthOptions.ProjectId)))
            //{
            //    try
            //    {
            //        otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
            //          otherAuthOptions, "Secondary"));
            //        otherAuth.StateChanged += AuthStateChanged;
            //        otherAuth.IdTokenChanged += IdTokenChanged;
            //    }
            //    catch (Exception)
            //    {
            //        DebugLog("ERROR: Failed to initialize secondary authentication object.");
            //    }
            //}
            //AuthStateChanged(this, null);

            //메시징 초기화
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            Firebase.Messaging.FirebaseMessaging.Subscribe("topic");
            //        DebugLog("Firebase Messaging Initialized");

        }

        //
        public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
        {
            Debug.Log("Received Registration Token : " + token.Token);
        }

        //메시지 구독
        public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Debug.Log("Received a new message");

            var notification = e.Message.Notification;
            if (notification != null)
            {
                //알림 있음
                Debug.Log("title : " + notification.Title);
                Debug.Log("body : " + notification.Body);
            }

            //요청자
            if (e.Message.From.Length > 0)
                Debug.Log("from : " + e.Message.From);

            //링크
            if (e.Message.Link != null)
            {
                Debug.Log("link: " + e.Message.Link.ToString());
            }

            //데이터 카운트
            if (e.Message.Data.Count > 0)
            {
                Debug.Log("data : ");
                foreach (KeyValuePair<string, string> iter in e.Message.Data)
                {
                    Debug.Log(" " + iter.Key + " : " + iter.Value);
                }
            }

        }



    }
}