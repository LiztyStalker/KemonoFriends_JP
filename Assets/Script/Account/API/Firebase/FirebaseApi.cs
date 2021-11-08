using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

namespace FirebaseAPI
{
    public class FirebaseApi : SingletonClass<FirebaseApi>
	{

        //인증 - GoogleAPI - 익명
        //클라우드메시지 - 알림
        //분석기
        //데이터베이스
        //저장소
        //호스팅

        FirebaseCloudMessaging cloudMessaging;
        FirebaseDB firebaseDB;


        DependencyStatus m_dependencyStatus = DependencyStatus.UnavailableOther;

        public DependencyStatus dependencyStatus { get { return m_dependencyStatus; } }
        
        public FirebaseApi()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                m_dependencyStatus = task.Result;

                //파이어베이스 연동 성공 상태이면
                if (m_dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    //초기화
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(
                      "Could not resolve all Firebase dependencies: " + m_dependencyStatus);
                }
            });
        }

        void InitializeFirebase()
        {
            cloudMessaging = new FirebaseCloudMessaging(this);
            firebaseDB = new FirebaseDB(this);
        }
	}
}
