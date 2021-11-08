using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public enum TYPE_FILE_ACCESS {Save, Load}

namespace GoogleAPI
{
    public class GoogleApi : SingletonClass<GoogleApi>
    {

        public delegate void EventDelegate(bool success);

        MsgDelegate msgDel;

        EventDelegate eventDel;

        public GoogleApi()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
                //.RequestEmail()
                //        .RequestIdToken()


            .RequestServerAuthCode(false) //절대 새로고침 하지마시오

            .Build();

            PlayGamesPlatform.InitializeInstance(config);

            PlayGamesPlatform.Activate();

            PlayGamesPlatform.DebugLogEnabled = true;
            Debug.Log("Activate");

        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="eventDel">로그인 확인 여부 메소드</param>
        public void setLogin(EventDelegate eventDel)
        {


            //로그인
            Social.localUser.Authenticate((success) =>
                {
                    eventDel(success);

                    /*
#if UNITY_EDITOR

                    #region firebase 인증 단계
                    ///
                    {
                        //인증
                        var authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                        Firebase.Auth.Credential credential =
                            Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
                        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                            {
                                if (task.IsCanceled)
                                {
                                    Debug.Log("canceled");
                                    return;
                                }
                                if (task.IsFaulted)
                                {
                                    Debug.Log("failed");
                                    return;
                                }

                                Firebase.Auth.FirebaseUser newUser = task.Result;

                                Debug.LogFormat("user {0} {1}", newUser.DisplayName, newUser.UserId);


                            });

                        //첫 로그인
                        //Firebase UID를 가져올 수 있음
                        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
                        if (user != null)
                        {
                            string playerName = user.DisplayName;
                            string uid = user.UserId;
                        }

                        //로그아웃
                        //auth.SignOut();

                    }

                    #endregion
                     * 
#endif
                    */
                }
            );

            Debug.Log("authenticated : " + Social.localUser.authenticated);

        }

        /// <summary>
        /// 로그 아웃
        /// </summary>
        public void setLogOut()
        {
            PlayGamesPlatform.Instance.SignOut();
        }



        /// <summary>
        /// 플레이어 통계 가져오기
        /// </summary>
        public void getStats()
        {
            ((PlayGamesLocalUser)Social.localUser).GetStats((rc, stats) =>
                {
                    //                if(rc <= 0 && stats.HasDaysSinceLastPlayed())

                }
            );
        }


        #region ######################## 업적 #######################


        /// <summary>
        /// 업적 공개
        /// </summary>
        /// <param name="achieveKey"></param>
        /// <param name="value"></param>
        /// <param name="eventDel"></param>
        public void reportProgress(string achieveKey, float value, EventDelegate eventDel)
        {
            Social.ReportProgress(achieveKey, value, (success) =>
            {
                eventDel(success);
            });
        }


        /// <summary>
        /// 업적 증가
        /// </summary>
        /// <param name="achieveKey"></param>
        /// <param name="value"></param>
        /// <param name="eventDel"></param>
        public void incrementAchievement(string achieveKey, int step, EventDelegate eventDel)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(achieveKey, step, (success) =>
            {
                eventDel(success);
            });
        }

        #endregion

        #region ######################## 리더보드 #######################

        /// <summary>
        /// 리더보드 스코어 삽입
        /// </summary>
        /// <param name="leaderboardKey"></param>
        /// <param name="l_value"></param>
        /// <param name="eventDel"></param>
        public void reportScore(string leaderboardKey, long l_value, EventDelegate eventDel)
        {

            PlayGamesPlatform.Instance.ReportScore(l_value, leaderboardKey, (success) =>
            {
                eventDel(success);
            });
        }


        /// <summary>
        /// 리더보드 보기
        /// </summary>
        /// <param name="key"></param>
        public void showLeaderBoard(string key = "")
        {
            if (key == "")
                Social.ShowLeaderboardUI();
            else
                PlayGamesPlatform.Instance.ShowLeaderboardUI(key, (status) =>
                {
                    Debug.Log("UIStatus : " + status);
                });
        }

        #endregion


        #region ######################## 데이터 #######################



        void setMsg(string msg)
        {
            Debug.Log("메시지 : " + msg);
            if (msgDel != null) msgDel(msg);
        }

        /// <summary>
        /// 저장된 게임 열기
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void openSavedGame(string fileName, TYPE_FILE_ACCESS typeFileAccess, EventDelegate eventDel, MsgDelegate msgDel)
        {
            this.msgDel = msgDel;
            this.eventDel = eventDel;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            setMsg("저장된 게임 열기 실행");

            if (savedGameClient != null)
            {
                switch (typeFileAccess)
                {
                    case TYPE_FILE_ACCESS.Save:
                        savedGameClient.OpenWithAutomaticConflictResolution(
                            fileName,
                            DataSource.ReadCacheOrNetwork,
                            ConflictResolutionStrategy.UseLongestPlaytime,
                            OnSavedGameWriteOpened
                        );
                        break;
                    case TYPE_FILE_ACCESS.Load:
                        savedGameClient.OpenWithAutomaticConflictResolution(
                            fileName,
                            DataSource.ReadCacheOrNetwork,
                            ConflictResolutionStrategy.UseLongestPlaytime,
                            OnSavedGameReadOpened
                        );
                        break;
                }
            }
        }

        void OnSavedGameWriteOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            Debug.Log("savedGameOpened : " + status);
            if (status == SavedGameRequestStatus.Success)
            {
                setMsg("기록중...");

                //저장된 게임 쓰기
                //델리게이트에 로딩중 표시
                //new byte[100] = IOData에서 처리된 것을 가져와야 함
                //계정 데이터를 가져와야 함
                byte[] data = IOPackage.IOData.GetInstance.DataConvertSerialToByte(Account.GetInstance.convertSeriel());

                saveGameData(game, data, DateTime.Now.TimeOfDay);
            }
            else
            {
                setMsg("저장된 데이터 열기 실패");
            }
            //에러
        }

        void OnSavedGameReadOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            Debug.Log("savedGameOpened : " + status);
            //저장된 게임 열기
            if (status == SavedGameRequestStatus.Success)
            {
                //저장된 게임 읽기 성공
                setMsg("저장된 게임 열기 성공!");
                loadGameData(game);
            }
            else
            {
                setMsg("저장된 게임 열기 실패");
            }
            //에러
        }

        /// <summary>
        /// 게임 저장하기
        /// </summary>
        /// <param name="game"></param>
        /// <param name="savedData"></param>
        /// <param name="totalPlayTime"></param>
        void saveGameData(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlayTime)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();


            builder = builder
                      .WithUpdatedPlayedTime(totalPlayTime)
                      .WithUpdatedDescription("SavedGame " + DateTime.Now);


            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            savedGameClient.CommitUpdate(game,
                    updatedMetadata,
                    savedData,
                    OnSavedGameWrite
                    );

        }

        void OnSavedGameWrite(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                setMsg("데이터 저장 완료!");
                eventDel(true);
                //            Debug.Log("데이터 쓰기 완료");
            }
            else
            {
                setMsg("데이터 쓰기 실패");
                //로그 남기기
                eventDel(false);
            }
        }

        /// <summary>
        /// 게임 불러오기
        /// </summary>
        /// <param name="game"></param>
        void loadGameData(ISavedGameMetadata game)
        {
            setMsg("불러오기 시작");
            //파일 데이터와 비교


            if (IOPackage.IOData.GetInstance.getLastWriteTime(PrepClass.fileName) != null)
            {
                DateTime fileTime = IOPackage.IOData.GetInstance.getLastWriteTime(PrepClass.fileName).Value;

                Debug.Log("DateTime = " + fileTime + " " + game.LastModifiedTimestamp + " " + fileTime.CompareTo(game.LastModifiedTimestamp));




//                game.LastModifiedTimestamp;

                if (fileTime.CompareTo(game.LastModifiedTimestamp) > 0)
                {
                    //파일로 불러오기
                    msgDel("동기화 : 저장된 파일");
                    eventDel(false);
                    return;
                }
            }

            //구글로 불러오기
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);

//            game.LastModifiedTimestamp 

        }

        void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
        {

            Debug.Log("GameDataLength : " + data.Length);

            if (status == SavedGameRequestStatus.Success)
            {
                try
                {
                    setMsg("진행중 : 불러오기");
                    //읽은 데이터를 시리얼로 변환하여 계정에 삽입

                    //Debug.Log("loadData : " + data.Length);

                    AccountSerial accountSerial = IOPackage.IOData.GetInstance.DataConvertByteToSerial(data);
                    //불러온 데이터보다 파일 데이터가 최신이면 파일 데이터로 불러오기


                    if (Account.GetInstance.setAccount(accountSerial))
                    {
                        setMsg("불러오기 성공!");
                        //Debug.Log("데이터 읽기 성공");
                        eventDel(true);
                    }
                    else
                        throw new Exception();
                }
                catch (Exception e)
                {
                    setMsg("불러오기 실패 : 데이터 오류");
                    //Debug.Log("불러오기 오류 : " + e.Message);
                    eventDel(false);
                }
            }
            else
            {
                setMsg("불러오기 실패 : 데이터 없음");
                //Debug.LogError("데이터 로드 실패 : " + status);
                eventDel(false);
            }
            //에러
        }

        /// <summary>
        /// 게임 삭제하기
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteGameData(string fileName)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(fileName,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                OnDeleteSavedGame
                );
        }

        void OnDeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            //        if(status == SavedGameRequestStatus.Success)
            //삭제 명령
            //        else
            //에러
        }

        #endregion
    }

}