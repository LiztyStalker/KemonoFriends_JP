using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

using UnityEngine;



namespace FirebaseAPI
{
	public class FirebaseDB
	{

        public delegate void TaskDelegate(AggregateException agrException, Task<DataSnapshot> task);
       

        //
        FirebaseApi firebaseAPI;

        //리더보드 리스트 
        ArrayList leaderBoard = new ArrayList();


        Vector2 scrollPosition = Vector2.zero;
        private Vector2 controlsScrollViewVector = Vector2.zero;

        public GUISkin fb_GUISkin;

        //출력 수
        private const int MaxScores = 5;
        private string logText = "";
        private string email = "name";
        private int score = 100;
        private Vector2 scrollViewVector = Vector2.zero;
        protected bool UIEnabled = true;

        const int kMaxLogSize = 16382;

        //상태
        //DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

        readonly string dbPath = "api-project-60908316";

        public FirebaseDB(FirebaseApi parent)
        {
            firebaseAPI = parent;
            InitializeFirebase();
        }

        

        /// <summary>
        /// DB 연결 초기화
        /// Firebase DB가 여러개 있을 수 있나?
        /// </summary>
        protected virtual void InitializeFirebase()
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            // NOTE: You'll need to replace this url with your Firebase App's database
            // path in order for the database connection to work correctly in editor.

            //데이터베이스 초기화
            //firebase.database 링크 연결
            app.SetEditorDatabaseUrl(string.Format("https://{0}.firebaseio.com/", dbPath));

            //app 옵션이 db주소이면 연결
            if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
            
            StartListener();
        }

        /// <summary>
        /// DB 데이터 초기화
        /// </summary>
        protected void StartListener()
        {
            //DB 읽기
            //데이터 정렬
            FirebaseDatabase.DefaultInstance
              .GetReference("Leaders").OrderByChild("score")
              .ValueChanged += HandleListener;

            //ValueChanged
            // 경로의 전체 내용에 대한 변경을 읽고 수신 대기
            // Snapshot에 대해 Value를 호출하면 Dic<str, obj>가 반환.
            // 데이터가 없는 경우 Value를 호출하면 null 반환
            
                //ex
                //FirebaseDatabase.DefaultInstance
                //    .GetReference("Leaders")
                //    .ValueChanged += HandleValueChanged;
                //}

                //void HandleValueChanged(object sender, ValueChangedEventArgs args) {
                //  if (args.DatabaseError != null) {
                //    Debug.LogError(args.DatabaseError.Message);
                //    return;
                //  }
                //   Do something with the data in args.Snapshot
                //}

            //하위 이벤트는 노드의 하위에서 발생하는 특정 작업에 대응하여 발생
                //ChildAdded
                //항목 목록을 검색하거나 항목 목록에 대한 추가를 수신 대기
                //ChildChanged 및 ChildRemoved와 함께 사용하여 목록의 변경을 모니터링

                //ChildChanged
                //목록의 항목에 대한 변경을 수신 대기. 
                //ChildAdded 및 ChildRemoved와 함께 사용하여 모니터링

                //ChildRemoved
                //목록의 항목 삭제를 수신 대기. 
                //ChildAdded 및 ChildChanged와 함께 사용하여 목록의 변경을 모니터링

                //ChildMoved
                //순서 있는 목록의 항목 순서 변경을 수신대기. 
                //ChildMoved 이벤트가 현재의 정렬 기준에 따라 항목 순서 변경의 원인이 된 ChildChanged의 이벤트를 항상 뒤따름

              
              
              //(object sender2, ValueChangedEventArgs e2) =>
              //{
              //    //DB 에러 났으면
              //    if (e2.DatabaseError != null)
              //    {
              //        Debug.LogError(e2.DatabaseError.Message);
              //        return;
              //    }

              //    Debug.Log("Received values for Leaders.");
              //    string title = leaderBoard[0].ToString();
              //    leaderBoard.Clear();
              //    leaderBoard.Add(title);
              //    if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
              //    {
              //        foreach (var childSnapshot in e2.Snapshot.Children)
              //        {
              //            if (childSnapshot.Child("score") == null
              //              || childSnapshot.Child("score").Value == null)
              //            {
              //                Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
              //                break;
              //            }
              //            else
              //            {
              //                Debug.Log("Leaders entry : " +
              //                  childSnapshot.Child("email").Value.ToString() + " - " +
              //                  childSnapshot.Child("score").Value.ToString());
              //                leaderBoard.Insert(1, childSnapshot.Child("score").Value.ToString()
              //                  + "  " + childSnapshot.Child("email").Value.ToString());
              //            }
              //        }
              //    }
              //};
        }


        void HandleListener(object sender2, ValueChangedEventArgs e2)
        {
                        
            //DB 에러 났으면
            if (e2.DatabaseError != null)
            {
                Debug.LogError(e2.DatabaseError.Message);
                return;
            }

            Debug.Log("Received values for Leaders.");
//            string title = leaderBoard[0].ToString();
            leaderBoard.Clear();
//            leaderBoard.Add(title);
            if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
            {
                Debug.Log("Snapshot");
                foreach (var childSnapshot in e2.Snapshot.Children)
                {
                    if (childSnapshot.Child("score") == null
                        || childSnapshot.Child("score").Value == null)
                    {
                        Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                        break;
                    }
                    else
                    {
                        Debug.Log("Leaders entry : " +
                            childSnapshot.Child("email").Value.ToString() + " - " +
                            childSnapshot.Child("score").Value.ToString());
                        leaderBoard.Insert(1, childSnapshot.Child("score").Value.ToString()
                            + "  " + childSnapshot.Child("email").Value.ToString());
                    }
                }


            }

        }


        #region ################ 점수 삽입하기 ####################

        /// <summary>
        /// 데이터 추가하기
        /// </summary>
        /// <param name="adventureName">모험 이름</param>
        /// <param name="accountName">계정 이름</param>
        /// <param name="characterName">캐릭터 이름</param>
        /// <param name="value">값 점수</param>
        /// <param name="taskDel">콜백</param>
        public void AddScore(
            string adventureName, 
            string accountName,
//            string characterName, 
            int value, 
            TaskDelegate taskDel
            )
        {
            if (score == 0 || string.IsNullOrEmpty(email))
            {
                Debug.Log("invalid score or email.");
                return;
            }

            //Debug.Log(String.Format("Attempting to add score {0} {1}",
            //  email, score.ToString()));

            //데이터베이스 참조
            //DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Leaders");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(adventureName);

            Debug.Log("Running Transaction...");
            // Use a transaction to ensure that we do not encounter issues with
            // simultaneous updates that otherwise might create more than MaxScores top scores.


            //데이터 삭제
            //reference.RemoveValueAsync();
            //reference.SetValueAsync(null);
            //reference.UpdateChildrenAsync(null);

            //정의된 경로에 데이터를 쓰거나 대체
            //reference.SetValueAsync();

            //원시 Json 으로 데이터를 쓰거나 대체
            //reference.SetRawJsonValueAsync();

            //데이터 목록에 추가. 고유키를 생성
            //reference.Push();

            //정의된 경로에서 모든 데이터를 대체하지 않고 일부키 업데이트
            //reference.UpdateChildrenAsync();

            //동시 업데이트에 의해 손상될 수 있는 복잡한 데이터를 업데이트
            //reference.RunTransaction();

            //트랜잭션으로 저장
            reference.RunTransaction(AddScoreTransaction)
              .ContinueWith(task =>
              {
                  if (task.Exception != null)
                  {
                      Debug.Log(task.Exception.ToString());
                  }
                  else if (task.IsCompleted)
                  {
                      Debug.Log("Transaction complete.");
                  }

                  taskDel(task.Exception, task);
              });


        }

        //리스트 보이기


        /// <summary>
        /// 점수 삽입 트랜잭션으로 저장
        /// </summary>
        /// <param name="mutableData"></param>
        /// <returns></returns>
        TransactionResult AddScoreTransaction(MutableData mutableData)
        {

            ///MutableData - DB에 저장된 데이터로 추정
            //MutableData 값을 List로 변환
            List<object> leaders = mutableData.Value as List<object>;
            
            //비어있으면 초기화
            if (leaders == null)
            {
                leaders = new List<object>();
            }
            //자식 숫자가 최대 숫자보다 많으면 - 필터링
            else if (mutableData.ChildrenCount >= MaxScores)
            {
                // If the current list of scores is greater or equal to our maximum allowed number,
                // we see if the new score should be added and remove the lowest existing score.

                //최소점수 = 최대점수
                long minScore = long.MaxValue;

                //최소값 = null
                object minVal = null;

                //모든 자식 돌기
                foreach (var child in leaders)
                {
                    //child가 Dic<str, obj> 형이 아니면
                    if (!(child is Dictionary<string, object>))
                        continue;

                    //score 키의 object를 long으로 형변환
                    long childScore = (long)((Dictionary<string, object>)child)["score"];

                    //최소값이 더 크면
                    if (childScore < minScore)
                    {
                        //최소스코어 = 자식스코어
                        minScore = childScore;
                        //최소값 = 자식
                        minVal = child;
                    }
                }

                //최소스코어가 스코어보다 크면 - abort 상태 - 데이터를 불러오지 못함
                // If the new score is lower than the current minimum, we abort.
                if (minScore > score)
                {
                    ///
                    return TransactionResult.Abort();
                }

                // Otherwise, we remove the current lowest to be replaced with the new score.

                //사용된 자식값 지우기
                leaders.Remove(minVal);
            }

            // Now we add the new score as a new entry that contains the email address and score.

            //점수 리스트 초기화
            Dictionary<string, object> newScoreMap = new Dictionary<string, object>();

            //점수와 이메일 삽입
            newScoreMap["score"] = score;
            newScoreMap["email"] = email;

            //등록된 점수 삽입하기
            leaders.Add(newScoreMap);

            // You must set the Value to indicate data at that location has changed.
            //mutableData 값을 현재 등록된 점수 리스트로 삽입
            mutableData.Value = leaders;

            ///
            //트랜잭션 값 성공
            //DB에 저장
            return TransactionResult.Success(mutableData);
        }

        #endregion




        #region ############## 하위 이벤트 ######################
        
        //var ref = FirebaseDatabase.DefaultInstance.GetReference("GameSessionComments");
        //ref.ChildAdded += HandleChildAdded;
        //ref.ChildChanged += HandleChildChanged;
        //ref.ChildRemoved += HandleChildRemoved;
        //ref.ChildMoved += HandleChildMoved;

        void HandleChildAdded(object sender, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            // Do something with the data in args.Snapshot
        }

        void HandleChildChanged(object sender, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            // Do something with the data in args.Snapshot
        }

        void HandleChildRemoved(object sender, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            // Do something with the data in args.Snapshot
        }

        void HandleChildMoved(object sender, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            // Do something with the data in args.Snapshot
        }

        #endregion
    }
}
