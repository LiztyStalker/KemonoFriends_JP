
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace IOPackage
{
    public class IOData : SingletonClass<IOData>
    {
        byte[] key = new byte[24];// = {1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4};// new byte[8];// {1, 1, 1, 1, 1, 1, 1, 1};

        const int myIterations = 1000;

        //암호화 알고리즘 랜덤 난수
        RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        //암호 데이터
        byte[] salt = new byte[96];//{8, 7, 6, 5, 4, 3, 2, 1};//System.Text.Encoding.ASCII.GetBytes ("saltTestTextData");


        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider(); // 키24개 필요
        //	DESCryptoServiceProvider des = new DESCryptoServiceProvider(); // 키 8개 필요

        string m_filePath;

        //	FileStream file;
        //	CryptoStream cryptoStream;

        public IOData()
        {
            //저장 및 불러오기
            m_filePath = Application.persistentDataPath;

//            Debug.Log("FilePath : " + m_filePath);

            //키 가져오기 - 시스템 디바이스의 고유번호는 현재 고유번호의 인덱스값
            key = System.Text.Encoding.ASCII.GetBytes(SystemInfo.deviceUniqueIdentifier.Substring(0, 24));
            //123456789
            //

            //키의 길이만큼 반복
            for (int i = 0; i < key.Length; i++) { 
                //삽입 - 고유번호가 인덱스가 되어 현재 고유번호의 인덱스 값을 가져옴.
                key[i] = (byte)(Convert.ToByte(SystemInfo.deviceUniqueIdentifier[(int)key[i] % SystemInfo.deviceUniqueIdentifier.Length]) - 32); 
            }

            //솔트값 삽입
            rngCsp.GetBytes(salt);
            //Debug.Log("rngCsp : " + Convert.ToBase64String(salt));
        }


        //	private byte[] getKey(){
        ////		SystemInfo.deviceUniqueIdentifier;
        ////		key[] = 
        //
        //	}

        /// <summary>
        /// 계정 해쉬 데이터 가져오기
        /// 없을 경우 딱 한번만 실행
        /// </summary>
        /// <returns>The account hash.</returns>
        //	public string getAccountHash(string hash){
        //
        //		//초반 생성일 경우
        //		if (hash == null) {
        //
        //		} 
        //
        //		return hash;
        //	}

        /// <summary>
        /// 파일의 유무 판별
        /// </summary>
        /// <returns><c>true</c>, if file was ised, <c>false</c> otherwise.</returns>
        /// <param name="fileName">File name.</param>
        public bool isFile(string fileName)
        {
            return File.Exists(string.Format("{0}/{1}.dat", m_filePath, fileName));
        }


        /// <summary>
        /// 암호화 하기 .dat
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="fileName">File name.</param>
        //	public CryptoStream encrypting(string filePath, string fileName){
        //		return encrypting(filePath, fileName, "dat");
        //	}
        //
        //	public CryptoStream encrypting(string filePath, string fileName, string fileExt){
        //		using(file = new FileStream(string.Format("{0}/{1}.{2}", m_filePath, fileName, fileExt), FileMode.Create, FileAccess.Write)){
        //			using(CryptoStream cryptoStream = new CryptoStream(file, tdes.CreateEncryptor(key, salt), CryptoStreamMode.Write)){
        //				//					using(CryptoStream cryptoStream = new CryptoStream(file, des.CreateEncryptor(key, salt), CryptoStreamMode.Write))
        //				return cryptoStream;
        //			}
        //		}
        //		return null;
        //	}


        /// <summary>
        /// 저장 데이터 변환하기 시리얼 -> Byte
        /// 암호화
        /// </summary>
        /// <param name="account"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] DataConvertSerialToByte(AccountSerial account)
        {
            byte[] data = new byte[0];

            try
            {
              
                using (MemoryStream memory = new MemoryStream())
                {



                    IFormatter bf = new BinaryFormatter();
                    ////XML방식으로 저장하는 것을 권장


                    ////cryptoStream에 직렬화 하기
                    bf.Serialize(memory, account);
                    data = memory.ToArray();

//                    Debug.Log("dataLength : " + memory.Length);

//                    string str = Convert.ToBase64String(memory.ToArray());

//                    Debug.Log("WriteDataBuilder : " + str);
                    
                    memory.Close();

//                    using (CryptoStream cryptoStream = new CryptoStream(memory, tdes.CreateEncryptor(key, salt), CryptoStreamMode.Write))
//                    {


////                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccountSerial));

////                        xmlSerializer.Serialize(cryptoStream, account);

////                        data = new byte[cryptoStream.Length];
////                        cryptoStream.Write(data, 0, (int)cryptoStream.Length);
////                        data = memory.ToArray();


//                        IFormatter bf = new BinaryFormatter();
//                        ////XML방식으로 저장하는 것을 권장


//                        ////cryptoStream에 직렬화 하기
//                        bf.Serialize(cryptoStream, account);

//                        data = memory.ToArray();

//                        //using (FileStream file = new FileStream(string.Format("{0}/{1}_t.dat", m_filePath, "friend"), FileMode.Create, FileAccess.Write))
//                        //{
//                        //    file.Write(data, 0, data.Length);
//                        //    file.Close();
//                        //}
                                               

//                        Debug.Log("dataLength : " + memory.Length);

//                        string str = Convert.ToBase64String(memory.ToArray());

//                        Debug.Log("WriteDataBuilder : " + str);
//                        cryptoStream.Close();
//                        memory.Close();
//                    }
                }



                //{



                //    using (CryptoStream cryptoStream = new CryptoStream(memory, tdes.CreateEncryptor(key, salt), CryptoStreamMode.Write))
                //    {

                //        //                        Debug.Log("bf");
                //        IFormatter bf = new BinaryFormatter();
                //        //XML방식으로 저장하는 것을 권장


                //        //                        Debug.Log("can : " + file.CanWrite + " " + cryptoStream);
                //        //                        bf.Serialize(file, account);
                //        bf.Serialize(cryptoStream, account);

                //        data = memory.ToArray();

                //        Debug.Log("dataLength : " + data.Length);
                        
                //        string str = Convert.ToBase64String(data);

                //        Debug.Log("WriteDataBuilder : " + str);

                //        cryptoStream.Close();
                //        memory.Close();
                //        //                        Debug.Log("close");

                //    }
                //}
            }
            catch (Exception e)
            {
                Debug.LogError("Serial -> Byte 변환 오류 : " + e.Message);
            }

            return data;
        }

        /// <summary>
        /// 저장 데이터 변환하기 - Byte -> 시리얼
        /// 복호화
        /// </summary>
        /// <param name="dataByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public AccountSerial DataConvertByteToSerial(byte[] data)
        {

            try
            {

                //using (MemoryStream memory = new MemoryStream(data, false))
                //{

                //    Debug.Log("pos : " + memory.Position);
                //    memory.Position = 0;

                //    string str = Convert.ToBase64String(data);

                //    Debug.Log("convert : " + str);

                //    IFormatter bf = new BinaryFormatter();

                //    //                            AccountSerial accountData = (AccountSerial)bf.Deserialize(file);
                //    AccountSerial accountData = (AccountSerial)bf.Deserialize(memory);
                //    Debug.Log("불러오기 완료 : " + accountData.GetType());



                //    memory.Close();
                //    return accountData;
                //}


                //using (FileStream file = new FileStream(string.Format("{0}/{1}_t.dat", m_filePath, "friend"), FileMode.Open, FileAccess.Read))
                //{
                //    byte[] temData = new byte[data.Length];
                //    file.Read(temData, 0, temData.Length);

                //    using (CryptoStream cryptoStream = new CryptoStream(file, tdes.CreateDecryptor(key, salt), CryptoStreamMode.Read))
                //    {
                //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccountSerial));
                //        AccountSerial accountSerial = (AccountSerial)xmlSerializer.Deserialize(cryptoStream);

                //        Debug.Log("AccountSerial : " + accountSerial.name);

                //        file.Close();
                //    }

                //}

                using (MemoryStream memory = new MemoryStream())
                {
                    memory.Write(data, 0, data.Length);
                    memory.Position = 0;
//                    Debug.Log("length : " + memory.Length);


                    IFormatter bf = new BinaryFormatter();

//                    string str = Convert.ToBase64String(memory.ToArray());
//                    Debug.Log("convert : " + str);

                    AccountSerial accountData = (AccountSerial)bf.Deserialize(memory);

                    memory.Close();
                    return accountData;




//                    using (CryptoStream cryptoStream = new CryptoStream(memory, tdes.CreateDecryptor(key, salt), CryptoStreamMode.Read))
//                    {


////                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccountSerial));
                        


//                        IFormatter bf = new BinaryFormatter();

//                        string str = Convert.ToBase64String(memory.ToArray());
//                        Debug.Log("convert : " + str);

//                        AccountSerial accountData = (AccountSerial)bf.Deserialize(cryptoStream);
//                        //AccountSerial accountData = (AccountSerial)bf.Deserialize(cryptoStream);
//                        //Debug.Log("불러오기 완료 : " + accountData.GetType());


//                        cryptoStream.Close();
//                        memory.Close();
//                        //return accountData;

//                        //AccountSerial accountSerial = (AccountSerial)xmlSerializer.Deserialize(cryptoStream) ;

//                        Debug.Log("accountSerial : " + accountData);

//                        return accountData;

//                    }
                }


                //Debug.Log("convert : " + data.Length);

                //using (MemoryStream memory = new MemoryStream(data))
                //{
                //    using (CryptoStream cryptoStream = new CryptoStream(memory, tdes.CreateDecryptor(key, salt), CryptoStreamMode.Read))
                //    {

                //        IFormatter bf = new BinaryFormatter();

                //        //                            AccountSerial accountData = (AccountSerial)bf.Deserialize(file);
                //        AccountSerial accountData = (AccountSerial)bf.Deserialize(cryptoStream);
                //        Debug.Log("불러오기 완료 : " + accountData.GetType());


                //        string str = Convert.ToBase64String(data);

                //        Debug.Log("ReaddataBuilder : " + str);


                //        cryptoStream.Close();
                //        memory.Close();

                //        return accountData;
                //    }
                //}
            }
            catch (Exception e)
            {
                Debug.LogError("Byte -> Serial 변환 오류 : " + e.Message);
            }
                        
            return null;
        }

        /// <summary>
        /// 백업파일 저장하기
        /// </summary>
        /// <param name="bf"></param>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool saveBackupData(IFormatter bf, Stream stream, string filename)
        {
            try
            {
                using (FileStream file = new FileStream(string.Format("{0}/{1}_b.dat", m_filePath, filename), FileMode.Create, FileAccess.Write))
                {
                    bf.Serialize(file, stream);
                    file.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("저장 오류 : " + e.Message);

                return false;
            }
        }

        /// <summary>
        /// 데이터 저장 - Formatter
        /// </summary>
        /// <returns><c>true</c>, if data was saved, <c>false</c> otherwise.</returns>
        /// <param name="account">Account.</param>
        public bool saveData(AccountSerial account, string fileName)
        {
            try
            {
                using (FileStream file = new FileStream(string.Format("{0}/{1}.dat", m_filePath, fileName), FileMode.Create, FileAccess.Write))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(file, tdes.CreateEncryptor(key, salt), CryptoStreamMode.Write))
                    {


//                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccountSerial));

//                        xmlSerializer.Serialize(cryptoStream, account);





//                        Debug.Log("bf");
                        IFormatter bf = new BinaryFormatter();


////                        Debug.Log("can : " + file.CanWrite + " " + cryptoStream);
////                        bf.Serialize(file, account);
                        bf.Serialize(cryptoStream, account);
//                        Debug.Log("저장 완료 : " + account.GetType());

                        //백업파일 저장하기
//                        if (saveBackupData(bf, cryptoStream, fileName))
//                            Debug.Log("백업파일 저장 완료");

                        cryptoStream.Close();
                        file.Close();
//                        Debug.Log("close");


                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("저장 오류 : " + e.Message);

                return false;
            }

        }

        /// <summary>
        /// 마지막 저장한 시간 가져오기
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Nullable<DateTime> getLastWriteTime(string fileName)
        {
            if (isFile(fileName))
            {
                return TimeZoneInfo.ConvertTimeToUtc(File.GetLastWriteTime(string.Format("{0}/{1}.dat", m_filePath, fileName)));
            }

            return null;
        }


        /// <summary>
        /// 데이터 불러오기
        /// </summary>
        /// <returns><c>true</c>, if data was loaded, <c>false</c> otherwise.</returns>
        public AccountSerial loadData(string fileName)
        {
            //파일 유무 판단
            if (isFile(fileName))
            {

                try
                {


                    using (FileStream file = new FileStream(string.Format("{0}/{1}.dat", m_filePath, fileName), FileMode.Open, FileAccess.Read))
                    {
                        

                        using (CryptoStream cryptoStream = new CryptoStream(file, tdes.CreateDecryptor(key, salt), CryptoStreamMode.Read))
                        {

                            IFormatter bf = new BinaryFormatter();


//                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccountSerial));
//                            AccountSerial accountData = (AccountSerial)xmlSerializer.Deserialize(cryptoStream);


//                            AccountSerial accountData = (AccountSerial)bf.Deserialize(file);
                            AccountSerial accountData = (AccountSerial)bf.Deserialize(cryptoStream);

                            Debug.Log("파일 불러오기 완료 : " + accountData.GetType());

                            cryptoStream.Close();
                            
                            file.Close();

                            return accountData;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("불러오기 오류 : " + e.Message);
                    Debug.LogError("백업 파일 불러오기");
                    return loadData(fileName + "_b");
                }

            }
            
            Debug.Log("파일이 없습니다.");
            return null;
            

        }

        private string DESEncrypting(string data)
        {


            //salt = new byte[8];

            //		using (rngCsp = new RNGCryptoServiceProvider()) {
            //			rngCsp.GetBytes(salt);
            //		}

            //		int myIterations = 1000;

            try
            {
                Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(key, salt, myIterations);
                //			TripleDES encAlg = TripleDES.Create ();
                //			encAlg.Key = k1.GetBytes(16);

                MemoryStream ms = new MemoryStream();

                StreamWriter sw = new StreamWriter(new CryptoStream(ms, new RijndaelManaged().CreateEncryptor(k1.GetBytes(32), k1.GetBytes(16)), CryptoStreamMode.Write));

                //			byte[] utfd1 = new System.Text.UTF8Encoding(false).GetBytes(data);

                sw.Write(data);

                sw.Close();

                return Convert.ToBase64String(ms.ToArray());

                //			k1.Reset();
            }
            catch (Exception e)
            {
                Debug.LogError("암호화 오류 : " + e.Message);
            }

            return null;

            //		RijndaelManaged rijndaelCipher = new RijndaelManaged ();
            //
            //		//입력받은 데이터를 바이트 배열로 변환
            //		byte[] plainText = System.Text.Encoding.Unicode.GetBytes (data);
            //
            //		//딕셔너리 공격을 대비하여 키를 더 풀기 어렵게 만들기 위한 Salt 사용
            //		byte[] salt = System.Text.Encoding.ASCII.GetBytes (key.Length.ToString ());
            //
            //
            //		Rfc2898DeriveBytes secretKey = new Rfc2898DeriveBytes (key, salt);
            //
            //		//내부적인 오류로 인해 PasswordDeriveBytes는 권장하지 않음 - MS
            ////		PasswordDeriveBytes secretKey = new PasswordDeriveBytes (key, salt);
            //
            //
            //
            //		ICryptoTransform Encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            //
            //		MemoryStream memoryStream = new MemoryStream();
            //
            //		CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            //
            //		cryptoStream.Write(plainText, 0, plainText.Length);
            //
            //		cryptoStream.FlushFinalBlock();
            //
            //		byte[] cipherBytes = memoryStream.ToArray();
            //
            //		memoryStream.Close();
            //		cryptoStream.Close();
            //
            //		string EncrytedData = Convert.ToBase64String(cipherBytes);
            //
            //		return EncrytedData;
        }



        private string DESDecrypting(string data)
        {

            //		RijndaelManaged rijndaelCipher = new RijndaelManaged ();


            //salt = new byte[8];

            //		using (rngCsp = new RNGCryptoServiceProvider()) {
            //			rngCsp.GetBytes(salt);
            //		}

            //		int myIterations = 1000;
            //		rngCsp.Dispose ();

            try
            {

                Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(key, salt, myIterations);

                //			TripleDES decAlg = TripleDES.Create ();
                //			decAlg.Key = k1.GetBytes(16);
                //			decAlg.IV = 

                ICryptoTransform cTransform = new RijndaelManaged().CreateDecryptor(k1.GetBytes(32), k1.GetBytes(16));

                byte[] bytes = Convert.FromBase64String(data);

                return new StreamReader(new CryptoStream(new MemoryStream(bytes), cTransform, CryptoStreamMode.Read)).ReadToEnd();

                //			MemoryStream decryptionStreamBacking = new MemoryStream();
                //			CryptoStream decrypt = new CryptoStream(decryptionStreamBacking, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
                //
                //			byte[] data1 = System.Text.Encoding.ASCII.GetBytes(data);
                //
                //			decrypt.Write(data1, 0, data1.Length);
                //
                //			decrypt.Flush();
                //
                //			decrypt.Close ();
                //
                //			string data2 = new System.Text.UTF8Encoding(false).GetString(decryptionStreamBacking.ToArray());
                //
                //			return data2;
                //

            }
            catch (Exception e)
            {
                Debug.LogError("복호화 오류 : " + e.Message);
            }
            return null;


            //		RijndaelManaged rijndaelCipher = new RijndaelManaged ();
            //
            //		byte[] EncryptedData = Convert.FromBase64String (data);
            //		byte[] salt = System.Text.Encoding.ASCII.GetBytes (key.Length.ToString ());
            //
            //		Rfc2898DeriveBytes secretKey = new Rfc2898DeriveBytes (key, salt);
            //
            //		ICryptoTransform Decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            //
            //		MemoryStream memoryStream = new MemoryStream (EncryptedData);
            //
            //		CryptoStream cryptoStream = new CryptoStream (memoryStream, Decryptor, CryptoStreamMode.Read);
            //
            //		byte[] plainText = new byte[EncryptedData.Length];
            //
            //		int DecryptedCount = cryptoStream.Read (plainText, 0, plainText.Length);
            //
            //		memoryStream.Close ();
            //		cryptoStream.Close ();
            //
            //		string DecryptedData = System.Text.Encoding.Unicode.GetString (plainText, 0, DecryptedCount);
            //
            //		return DecryptedData;
        }

    }
}


