using UnityEngine;
using System;
using System.Collections;
using Coherent.UI;
using Coherent.UI.Binding;
using Assets.Scripts.Configs;
using UnityEngine.SceneManagement;
using Assets.Scripts.Logic.UI.Training;
using Assets.Scripts.Controller;
using Assets.Scripts.DBLogic;
using Newtonsoft.Json;
using System.Collections.Generic;
using Assets.Scripts.VO;
using Assets.LSE.TL;
using Utility;
using System.IO;
using Assets.Scripts.Tools;
using System.Text;
using System.Threading;

/*
 * 
 */
public class UICommunicationsScript : MonoBehaviour
{
    private CoherentUIView ViewComponent;
    private TLGradeManager gradeManager = new TLGradeManager();
	private string userName=null;
    // Use this for initialization
    void Start()
    {
        //gradeManager = new TLGradeManager();
        ViewComponent = GetComponent<CoherentUIView>();
        if (ViewComponent)
        {
            ViewComponent.OnReadyForBindings += this.RegisterBindings;
        }

        ViewComponent.ReceivesInput = true;
        //InvokeRepeating("TestData1", 1, 15);
        InvokeRepeating("TestData2", 4, 15);
		TestData1();
	}
    void TestData1()
    {
        //登录测试
        //ReceiveWeb("{\"Action\":\"UserLogin\",\"param1\":\"001\",\"param2\":\"123\",\"param3\":\"0\"}");
        //音乐测试
        // ReceiveWeb("{\"Action\":\"MusicOnOff\",\"param1\":\"0\"}");
        //音量测试
        //ReceiveWeb("{\"Action\":\"VoiceControl\",\"param1\":\"10\"}");
        ////获取链接数据-知识界面测试
        // ReceiveWeb("{\"Action\":\"GetExternalLinks\",\"param1\":\"ppt\"}");
        //打开外部文件测试
        // ReceiveWeb("{\"Action\":\"OpenLink\",\"param1\":\"ppt\",\"param2\":\"C:/Users/Leo/Desktop/全息路虎.mp4\"}");

        //ToolsDBHelper tool = new ToolsDBHelper();
        //tool.getTools();
        //ReceiveWeb("{\"Action\":\"updateIsExam\",\"param1\":\"1\",\"param2\":\"0\"}");
        //ReceiveWeb("{\"Action\":\"getCircuits\",\"param1\":\"1\"}");
    }
    void TestData2()
    {
        //登录测试
        //ReceiveWeb("{\"Action\":\"UserLogin\",\"param1\":\"001\",\"param2\":\"123\",\"param3\":\"0\"}");
        //音乐测试
        //ReceiveWeb("{\"Action\":\"MusicOnOff\",\"param1\":\"1\"}");

        //注册测试
        string date = "{ \"name\":5454,\"sex\":2,\"phone\":3,\"ide\":4,\"term\":5,\"workshop\":6,\"team\":7,\"id\":8,\"pwd\":9}";
        // ReceiveWeb("{\"Action\":\"StudentEdit\",\"param1\":" + date+"}");

        //获取学员信息
        // ReceiveWeb("{\"Action\":\"GetStuInfo\",\"param1\":\"1\"}");
    }

    void Update()
    {

    }

    private void RegisterBindings(int frame, string url, bool isMain)
    {
        if (isMain)
        {
            var view = ViewComponent.View;
            if (view != null)
            {
                view.BindCall("ReceiveWeb", (Action<string>)this.ReceiveWeb);
            }
        }
    }
    
    public void ReceiveWeb(string json)
    {
        Debug.LogError("ReceiveWebAction:" + json);
        Dictionary<string, System.Object> dic = JsonConvert.DeserializeObject<Dictionary<string, System.Object>>(json);
        string action = dic["Action"].ToString();
        switch (action)
        {
            case "UserLogin":
                string callback;
                UserDBHelper userDBLogin = new UserDBHelper();
                try
                {
                    User user = userDBLogin.getUser(dic["param1"].ToString(), dic["param2"].ToString(), dic["param3"].ToString());
                    string userStr = JsonConvert.SerializeObject(user);
					if (userName==null)
						userName = user.Name;
                    if (user != null)
                    {
                        callback = "{\"Action\":\"LoginCallback\",\"param1\":\"success\",\"param2\":" + userStr + "}";
                        MySession.user = user;
                    }
                    else
                    {
                        callback = "{\"Action\":\"LoginCallback\",\"param1\":\"failed\",\"param2\":\"用户名密码错误！\"}";
                        MySession.user = null;
                    }
                }
                catch (Exception)
                {
                    callback = "{\"Action\":\"LoginCallback\",\"param1\":\"failed\",\"param2\":\"服务器配置错误！\"}";
                }
                UnityToWeb(callback);
                break;
            case "MusicOnOff": //音乐开关
                if (dic["param1"].ToString() == "0")
                    AudioManager.getInstance().AudioPause(global::AudioManager.MusicNumType.groundMusic);
                else
                    AudioManager.getInstance().AudioPlay(global::AudioManager.MusicNumType.groundMusic);
                break;
            case "VoiceControl": //音量控制
                AudioManager.getInstance().SetAudioVolume(global::AudioManager.MusicNumType.groundMusic, int.Parse(dic["param1"].ToString()) / 20.0f);
                break;
            case "GetExternalLinks": //获取链接数据-知识界面
                string callbackExternalLinks;
                InfoLinksDBHelper externalLinksDB = new InfoLinksDBHelper();
                string externalLinksStr = externalLinksDB.getGetLinks(dic["param1"].ToString(), int.Parse(dic["param2"].ToString()), int.Parse(dic["param3"].ToString()));
                int totalNum = externalLinksDB.getGetTotalLinksNum(dic["param1"].ToString());
                callbackExternalLinks = "{\"Action\":\"GetExternalLinksCallback\",\"param1\":" + externalLinksStr + ",\"param2\":\"" + totalNum + "\"}";
                Debug.LogError("callback:" + callbackExternalLinks);
                UnityToWeb(callbackExternalLinks);
                break;
            case "GetExternalLinksByPage": //通过page获取链接数据 -知识界面
                string callbackLinksByPage;
                InfoLinksDBHelper linkByPageDB = new InfoLinksDBHelper();
                string linkByPageStr = linkByPageDB.getGetLinks(dic["param1"].ToString(), int.Parse(dic["param2"].ToString()), int.Parse(dic["param3"].ToString()));
                callbackLinksByPage = "{\"Action\":\"GetExternalLinksByPageCallback\",\"param1\":\"" + linkByPageStr + "\"}";
                UnityToWeb(callbackLinksByPage);
                break;
            case "DeleteExternalLinks": //通过page获取链接数据 -知识界面
                string DeleteExternalLinksResult = "";
                InfoLinksDBHelper DeleteExternalLinksDB = new InfoLinksDBHelper();
                bool DeleteExternalLinksisSuccessed = DeleteExternalLinksDB.deleteLink(int.Parse(dic["param1"].ToString()));
                if (DeleteExternalLinksisSuccessed)
                {
                    DeleteExternalLinksResult = "success";
                }
                else
                {
                    DeleteExternalLinksResult = "faild";
                }
                string DeleteExternalLinksCallback = "{\"Action\":\"DeleteExternalLinksCallback\",\"param1\":\"" + DeleteExternalLinksResult + "\"}";
                UnityToWeb(DeleteExternalLinksCallback);
                break;
            case "AddExternalLinks":
                string AddExternalLinksResult = "";
                InfoLinksDBHelper AddExternalLinksDB = new InfoLinksDBHelper();
                LinkInfo info = new LinkInfo();
                info.Type = dic["param1"].ToString();
                info.Address = dic["param2"].ToString();
                bool AddExternalLinksisSuccessed = AddExternalLinksDB.addLink(info);
                if (AddExternalLinksisSuccessed)
                {
                    AddExternalLinksResult = "success";
                }
                else
                {
                    AddExternalLinksResult = "faild";
                }
                string AddExternalLinksCallback = "{\"Action\":\"AddExternalLinksCallback\",\"param1\":\"" + AddExternalLinksResult + "\",\"param2\":\"" + info.Type + "\"}";
                UnityToWeb(AddExternalLinksCallback);
                break;
            case "EditExternalLinks":
                Dictionary<string, string> EditExternalLinksdicTemp = JsonConvert.DeserializeObject<Dictionary<string, string>>(dic["param1"].ToString());
                string EditExternalLinksResult = "";
                InfoLinksDBHelper EditExternalLinksDB = new InfoLinksDBHelper();
                LinkInfo EditExternalLinksinfo = new LinkInfo();
                EditExternalLinksinfo.Id = int.Parse(EditExternalLinksdicTemp["Id"]);
                //EditExternalLinksinfo.Type = EditExternalLinksdicTemp["type"];
                EditExternalLinksinfo.Address = EditExternalLinksdicTemp["Address"];
                bool EditExternalLinksisSuccessed = EditExternalLinksDB.updateLink(EditExternalLinksinfo);
                if (EditExternalLinksisSuccessed)
                {
                    EditExternalLinksResult = "success";
                }
                else
                {
                    EditExternalLinksResult = "faild";
                }
                string EditExternalLinksCallback = "{\"Action\":\"AddExternalLinksCallback\",\"param1\":\"" + EditExternalLinksResult + "\"}";
                UnityToWeb(EditExternalLinksCallback);
                break;
            case "OpenExternalLinks": //根据type，link打开外部链接
                OpenLink(dic["param1"].ToString(), dic["param2"].ToString());
                break;
            case "updateLinks":
                if ((dic["param1"].ToString()).Equals("ppt"))
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Ppt, updateLinks, dic["param2"].ToString());
                else if ((dic["param1"].ToString()).Equals("word"))
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Word, updateLinks, dic["param2"].ToString());
                else if ((dic["param1"].ToString()).Equals("excel"))
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Excel, updateLinks, dic["param2"].ToString());
                else if ((dic["param1"].ToString()).Equals("video"))
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Vedio, updateLinks, dic["param2"].ToString());
                break;
            case "UpExternalLinks":
                if ((dic["param1"].ToString()).Equals("ppt"))
                {
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Ppt, UpLinks);
                }
                else if ((dic["param1"].ToString()).Equals("word"))
                {
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Word, UpLinks);
                }
                else if ((dic["param1"].ToString()).Equals("excel"))
                {
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Excel, UpLinks);
                }
                else if ((dic["param1"].ToString()).Equals("video"))
                {
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Vedio, UpLinks);
                }
                else if ((dic["param1"].ToString()).Equals("all"))
                {
                    UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.AllType, UpLinks);
                }
                break;
            case "EnterScene": //根据进入培训场景
                switch (dic["param2"].ToString())
                {
                    case "sbrz":
                        UIManager.getInstance().enterEquipKnow("DeviceKnow1", (TrainUI.TrainMode)int.Parse(dic["param3"].ToString()));
                        break;
                    case "dlyl":
                        UIManager.getInstance().enterTrain(dic["param1"].ToString(), "电路原理", (TrainUI.TrainMode)int.Parse(dic["param3"].ToString()));
                        break;
                    case "sbcj":
                        UIManager.getInstance().enterTrain(dic["param1"].ToString(), "设备拆解", (TrainUI.TrainMode)int.Parse(dic["param3"].ToString()));
                        break;
                    case "sbzz":
                        UIManager.getInstance().enterTrain(dic["param1"].ToString(), "设备组装", (TrainUI.TrainMode)int.Parse(dic["param3"].ToString()));
                        break;
                    case "sbxs":
                        UIManager.getInstance().enterTrain(dic["param1"].ToString(), "设备巡视", (TrainUI.TrainMode)int.Parse(dic["param3"].ToString()));
                        break;
                    case "sbjx":
                        UIManager.getInstance().enterTrain(dic["param1"].ToString(), "设备检修", (TrainUI.TrainMode)int.Parse(dic["param3"].ToString()));
                        break;
                    case "DBQgz":
                        UIManager.getInstance().enterTrain(dic["param1"].ToString(), "设备故障", (TrainUI.TrainMode)int.Parse(dic["param3"].ToString()));
                        break;
                }
                break;
            case "StudentRegister":
                Dictionary<string, string> dicTemp = JsonConvert.DeserializeObject<Dictionary<string, string>>(dic["param1"].ToString());
                UserDBHelper userDBStu = new UserDBHelper();
                User userStu = new User();
                userStu.Name = dicTemp["name"];
                userStu.Sex = dicTemp["sex"];
                userStu.Phone = dicTemp["phone"];
                userStu.IdCard = dicTemp["ide"];
                userStu.Term = dicTemp["term"];
                userStu.Workshop = dicTemp["workshop"];
                userStu.Group = dicTemp["team"];
                userStu.AccountID = dicTemp["id"];
                userStu.Pwd = dicTemp["pwd"];
                bool isSuccessed = userDBStu.addUser(userStu);
                //“success”:成功 “faild”:失败
                string result = "";
                if (isSuccessed)
                {
                    result = "success";
                }
                else
                {
                    result = "faild";
                }
                string callBackStudentRegister = "{\"Action\":\"StudentRegisterCallback\",\"param1\":\"" + result + "\"}";
                UnityToWeb(callBackStudentRegister);
                break;
            case "StudentEdit":
                Dictionary<string, string> dicTempStuEdit = JsonConvert.DeserializeObject<Dictionary<string, string>>(dic["param1"].ToString());
                UserDBHelper userDBStuEdit = new UserDBHelper();
                User userStuEdit = new User();
                userStuEdit.Name = dicTempStuEdit["name"];
                userStuEdit.Sex = dicTempStuEdit["sex"];
                userStuEdit.Phone = dicTempStuEdit["phone"];
                userStuEdit.IdCard = dicTempStuEdit["ide"];
                userStuEdit.Term = dicTempStuEdit["term"];
                userStuEdit.Workshop = dicTempStuEdit["workshop"];
                userStuEdit.Group = dicTempStuEdit["team"];
                userStuEdit.AccountID = dicTempStuEdit["id"];
                userStuEdit.Pwd = dicTempStuEdit["pwd"];
                bool isSuccessedStuEdit = userDBStuEdit.updateUser(userStuEdit);

                //“success”:成功 “faild”:失败
                string resultStuEdit = "";
                if (isSuccessedStuEdit)
                {
                    resultStuEdit = "success";
                }
                else
                {
                    resultStuEdit = "faild";
                }
                string callBackStuEdit = "{\"Action\":\"StudentEditCallback\",\"param1\":\"" + resultStuEdit + "\"}";
                UnityToWeb(callBackStuEdit);
                break;
            case "StudentDelete":
                UserDBHelper userInfoDBStudentDelete = new UserDBHelper();
                bool isSuccessedStudentDelete = userInfoDBStudentDelete.deleteUser(int.Parse(dic["param1"].ToString()));
                string resultisSuccessedStudentDelete = "";
                if (isSuccessedStudentDelete)
                    resultisSuccessedStudentDelete = "success";
                else
                    resultisSuccessedStudentDelete = "faild";
                string callBackStudentDelete = "{\"Action\":\"StudentDeleteCallback\",\"param1\":\"" + resultisSuccessedStudentDelete + "\"}";
                UnityToWeb(callBackStudentDelete);
                break;
            case "GetStuInfo":
                UserDBHelper userInfoDB = new UserDBHelper();
                int userNum = userInfoDB.getTotalXueyuanUserNum();
                List<User> users = userInfoDB.getXueyuanUserByPage(int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString()));
                string callBackStuInfo = "{\"Action\":\"GetStuInfoCallback\",\"param1\":" + JsonConvert.SerializeObject(users) + ",\"param2\":\"" + userNum + "\"}";
				Debug.Log(callBackStuInfo);
                UnityToWeb(callBackStuInfo);
                break;
            case "GetStuScoreInfo":
                string stuScoreBackJson = gradeManager.gradeManager(dic["param1"].ToString(), int.Parse(dic["param2"].ToString()), int.Parse(dic["param3"].ToString()));
                UnityToWeb(stuScoreBackJson);
                break;
            case "TestBack":
                //当前成绩返回
                if (UIManager.getInstance().getCurrentUIType() == UIType.TrainUI)
                    UIManager.getInstance().trainUI.backMenu();
                else if (UIManager.getInstance().getCurrentUIType() == UIType.CircuitUI)
                    UIManager.getInstance().circuitUI.backMenu();
                break;
            case "TestAgin":
                //当前成绩页面再考一次
                if (UIManager.getInstance().getCurrentUIType() == UIType.TrainUI)
                    UIManager.getInstance().trainUI.afreshExam();
                else if (UIManager.getInstance().getCurrentUIType() == UIType.CircuitUI)
                    UIManager.getInstance().circuitUI.afreshExam();
                break;
			case "CheckCircuitErrorInfo":
				break;
			case "CheckErrorInfo":
				if (int.Parse(dic["param1"].ToString())==2)
					UIManager.getInstance().circuitUI.cause(int.Parse(dic["param2"].ToString()));
				else
					UIManager.getInstance().trainUI.cause(int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString()));
				break;
			case "getTwoQuestionTest":
                string topicsJson = JsonConvert.SerializeObject(gradeManager.getTopic2Ds(int.Parse(dic["param1"].ToString())));
                float examTime = gradeManager.getExamTimeById(int.Parse(dic["param1"].ToString()));
                UnityToWeb("getTwoQuestionTest", topicsJson, examTime.ToString());
                break;
            case "getTwoTestScore":
                string twoScore = gradeManager.examGrade2D(dic["param1"].ToString());
                UnityToWeb("getTwoTestScoreBack", twoScore);
                break;
            case "AddTwoQuestion":
                string twoQuestionBack = "";
                if (gradeManager.topic2DResolve(dic["param1"].ToString()))
                    twoQuestionBack = "success";
                else twoQuestionBack = "failed";
                UnityToWeb("AddTwoQuestionCallback", twoQuestionBack);
                break;
            case "AddTwoQuestionImg":
                UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Image, uploadImg, dic["param1"].ToString());
                break;
            case "GetTwoQuestion":
                int currentPage = int.Parse(dic["param1"].ToString());
                int pageNum = int.Parse(dic["param2"].ToString());
                int SubjectId = int.Parse(dic["SubjectId"].ToString());
                List<Topic2D> topics = gradeManager.getTopic2Ds(SubjectId, currentPage, pageNum);
                int topicCount = gradeManager.getTopic2DCount(SubjectId);
                float scoreCount = gradeManager.getTopic2DScoreCount(SubjectId);
                string questionCallbackJson = "{\"Action\":\"GetTwoQuestionCallback\",\"param1\":" + JsonConvert.SerializeObject(topics) + ",\"param2\":\"" + topicCount.ToString() + "\",\"param3\":\"" + scoreCount.ToString() + "\"}";
                UnityToWeb(questionCallbackJson);
                break;
            case "DeleteTwoQuestion":
                if (gradeManager.deleteTopic2D(int.Parse(dic["param1"].ToString())))
                    UnityToWeb("DeleteTwoQuestionCallback", "success");
                else UnityToWeb("DeleteTwoQuestionCallback", "failed");
                break;
            case "UpdateTwoQuestion":
                if (gradeManager.updateTopic2DResolve(dic["param1"].ToString()))
                    UnityToWeb("UpdateTwoQuestionCallback", "success");
                else UnityToWeb("UpdateTwoQuestionCallback", "failed");
                break;
            case "getTestScorePage":
                string scorePageJson = gradeManager.getHistoryGrade(int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString()));
                UnityToWeb("getTestScoreBack", scorePageJson);
                break;
            case "getTestCharts":
                string chartsJson = gradeManager.getGradeCharts(int.Parse(dic["param1"].ToString()));
                UnityToWeb("getTestChartsBack", chartsJson);
                break;

                ///update score;
            case "updateTwoScore":
                UnityToWeb("updateTwoScoreCallback", gradeManager.updateTopic2DScore(int.Parse(dic["param1"].ToString()), float.Parse(dic["param2"].ToString()), int.Parse(dic["param3"].ToString())).ToString());
                break;
			case "updateThreeScore":
				 UnityToWeb("updateThreeScoreCallback", gradeManager.updateTopic3DScore(int.Parse(dic["param1"].ToString()), float.Parse(dic["param2"].ToString()), int.Parse(dic["param3"].ToString())).ToString());
				break;
            case "updateFourScore":
                UnityToWeb("updateFourScoreCallback", gradeManager.updateTopic3DScore(int.Parse(dic["param1"].ToString()), float.Parse(dic["param2"].ToString()), int.Parse(dic["param3"].ToString())).ToString());
                break;

                ////param1 : score//15 5 2   14 1 2
            case "updateCircuitScore":
                //DataBackup();
                //UnityToWeb("updateCircuitScoreCallback", gradeManager.updateTopicCircuitScore(int.Parse(dic["param2"].ToString()), float.Parse(dic["param4"].ToString()), int.Parse(dic["param1"].ToString()), dic["param3"].ToString(), char.Parse(dic["param5"].ToString())).ToString());
                UnityToWeb("updateCircuitScoreCallback", gradeManager.updateTopicCircuitScore(int.Parse(dic["param2"].ToString()),float.Parse(dic["param1"].ToString()), int.Parse(dic["param3"].ToString())).ToString());
                break;
            case "getCircuits":
                UnityToWeb("getCircuitsCallback", JsonConvert.SerializeObject(gradeManager.getCircuitTopics()));
                break;
			case "updateIsExam":
				UnityToWeb("updateIsExamCallback", gradeManager.updateIsExam(int.Parse(dic["param1"].ToString()),char.Parse(dic["param2"].ToString())).ToString());
				break;
			case "GetThreeQuestion":
                List<Topic3D> topic3Ds = gradeManager.getTopic3Ds(int.Parse(dic["param3"].ToString()), int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString()));
                int topic3DCount = gradeManager.getTopic3DCount(int.Parse(dic["param3"].ToString()));
                float topic3DScore = gradeManager.getTopic3DScoreCount(int.Parse(dic["param3"].ToString()));
                string ThreeQuestionCallbackJson = "{\"Action\":\"GetThreeQuestionCallback\",\"param1\":" + JsonConvert.SerializeObject(topic3Ds) + ",\"param2\":\"" + topic3DCount.ToString() + "\",\"param3\":\"" + topic3DScore.ToString() + "\",\"param4\":\"" + dic["param3"].ToString() + "\",\"param5\":\"" + (gradeManager.getExamSubject(int.Parse(dic["param3"].ToString())).IsExam ? "1" : "0") + "\"}";
                UnityToWeb(ThreeQuestionCallbackJson);
                break;

                ////four
            case "GetFourQuestion":
                List<CircuitTopicNew> topic4Ds = gradeManager.getTopicCircuitTopicNew(int.Parse(dic["param3"].ToString()));
                int topic4DCount = gradeManager.getTopicCircuitCount(int.Parse(dic["param3"].ToString()));
                float topic4DScore = gradeManager.getTopicCircuitScoreCount(int.Parse(dic["param3"].ToString()));
                string FourQuestionCallbackJson = "{\"Action\":\"GetFourQuestionCallback\",\"param1\":" + JsonConvert.SerializeObject(topic4Ds) + ",\"param2\":\"" + topic4DCount.ToString() + "\",\"param3\":\"" + topic4DScore.ToString() + "\",\"param4\":\"" + dic["param3"].ToString() + "\",\"param5\":\"" + (gradeManager.getExamSubject(int.Parse(dic["param3"].ToString())).IsExam ? "1" : "0") + "\"}";
                UnityToWeb(FourQuestionCallbackJson);
                break;
                //new four
                ////


            case "updateSubjectExam":
                if(gradeManager.updateSubjectExam(int.Parse(dic["param1"].ToString()), dic["param2"].ToString()))
                    UnityToWeb("updateSubjectExamCallback", "success");
                else UnityToWeb("updateSubjectExamCallback", "faild");
                break;
            case "BackToSbrzStudy":
                UIManager.getInstance().equipKnowUI.changeMode(TrainUI.TrainMode.Study);
                break;
            case "BackToMenu":
                UIManager.getInstance().equipKnowUI.backMenu();
                break;
            case "ImportStuInfo":
                UIManager.getInstance().GetComponent<OpenFile>().OpenFileByType(OpenFile.FileType.Excel, InputData);
                break;
			case "PlayMusicByID":
				PlayMusicByID(int.Parse(dic["param1"].ToString()));
				break;
			case "updateAllTopic3DScore":
				gradeManager.updateAllTopic3DScore(int.Parse(dic["param3"].ToString()));
				List<Topic3D> topic3ds = gradeManager.getTopic3Ds(int.Parse(dic["param3"].ToString()), int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString()));
				int topic3dCount = gradeManager.getTopic3DCount(int.Parse(dic["param3"].ToString()));
				float topic3dScore = gradeManager.getTopic3DScoreCount(int.Parse(dic["param3"].ToString()));
				string updateAllTopic3DScoreJson = "{\"Action\":\"updateAllTopic3DScoreCallBack\",\"param1\":" + JsonConvert.SerializeObject(topic3ds) + ",\"param2\":\"" + topic3dCount.ToString() + "\",\"param3\":\"" + topic3dScore.ToString() + "\",\"param4\":\"" + dic["param3"].ToString() + "\",\"param5\":\"" + (gradeManager.getExamSubject(int.Parse(dic["param3"].ToString())).IsExam ? "1" : "0") + "\"}";
				UnityToWeb(updateAllTopic3DScoreJson);
				break;
			case "updateAllTopic2DScore":
				gradeManager.updateAllTopic2DScore(int.Parse(dic["param3"].ToString()));
				List<Topic2D> topic2ds = gradeManager.getTopic2Ds(int.Parse(dic["param3"].ToString()), int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString()));
				int topic2dCount = gradeManager.getTopic2DCount(int.Parse(dic["param3"].ToString()));
				float topic2dScore = gradeManager.getTopic2DScoreCount(int.Parse(dic["param3"].ToString()));
				string updateAllTopic2DScoreJson = "{\"Action\":\"updateAllTopic2DScoreCallBack\",\"param1\":" + JsonConvert.SerializeObject(topic2ds) + ",\"param2\":\"" + topic2dCount.ToString() + "\",\"param3\":\"" + topic2dScore.ToString() + "\",\"param4\":\"" + dic["param3"].ToString() + "\",\"param5\":\"" + (gradeManager.getExamSubject(int.Parse(dic["param3"].ToString())).IsExam ? "1" : "0") + "\"}";
				UnityToWeb(updateAllTopic2DScoreJson);
				break;
			case "closeSystem":
				UIManager.getInstance().exitSystem();
				break;
            case "StopMusicByID":
                AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
                break;
			case "GetDurationCharts":
				Dictionary<string, int> grades = gradeManager.GetDurationCharts(int.Parse(dic["param1"].ToString()));
				string[] ExamTime = new string[grades.Count];
				float[] Grade = new float[grades.Count];
				int count = 0;
				foreach (KeyValuePair<string, int> kvp in grades)
				{
					ExamTime[count] = kvp.Key;
					Grade[count] = kvp.Value;
					count++;
				}
				string GetDurationChartsJson = "{\"Action\":\"GetDurationChartsCallback\",\"param1\":" + JsonConvert.SerializeObject(ExamTime) + ",\"param2\":" + JsonConvert.SerializeObject(Grade) + "}";
				Debug.Log(GetDurationChartsJson);
				UnityToWeb(GetDurationChartsJson);
				break;
			case "GetScoreCharts":
				Dictionary<string,float> gradeScore = gradeManager.GetScoreCharts(int.Parse(dic["param1"].ToString()));
				string[] ExamTimeScore = new string[gradeScore.Count];
				float[] GradeScore = new float[gradeScore.Count];
				int countScore = 0;
				foreach(KeyValuePair<string, float> kvp in gradeScore)
				{
					ExamTimeScore[countScore] = kvp.Key;
					GradeScore[countScore] = kvp.Value;
					countScore++;
				}
				string GetScoreChartsJson = "{\"Action\":\"GetScoreChartsCallback\",\"param1\":" + JsonConvert.SerializeObject(ExamTimeScore) + ",\"param2\":" + JsonConvert.SerializeObject(GradeScore) + "}";
				Debug.Log(GetScoreChartsJson);
				UnityToWeb(GetScoreChartsJson);
				break;
			case "getSumStuDate":
				string getSumStuCallbackJson = "{\"Action\":\"getSumStuCallback\",\"param1\":" + JsonConvert.SerializeObject(gradeManager.getSumStuDate(dic["param1"].ToString())) + "}";
				UnityToWeb(getSumStuCallbackJson);
				break;
			case "getSumCurveDate":
				string getSumCurveCallbackJson = "{\"Action\":\"getSumCurveCallback\",\"param1\":" + JsonConvert.SerializeObject(gradeManager.getSumCurveDate()) + "}";
				UnityToWeb(getSumCurveCallbackJson);
				break;
			case "SubmitReportDate":
				string wordPath=createWord(dic["param1"].ToString());
				string submitBack;
				string SubmitReportDateCallbackJson;
				if (wordPath == null || wordPath == "")
					submitBack = "failed";
				else
					submitBack = "success";
				SubmitReportDateCallbackJson = "{\"Action\":\"SubmitReportDateCallback\",\"param1\":\"" + submitBack + "\",\"param2\":\"" + wordPath+ "\"}";
				UnityToWeb(SubmitReportDateCallbackJson);
				break;
			case "SystemBackup":
				DoBackup();
				string BackupJson= "{\"Action\":\"SystemBackupCallback\",\"param1\":\"1\"}";
				UnityToWeb(BackupJson);
				break;
			case "RestoreBackup":
				restore(int.Parse(dic["param1"].ToString()));
				string RestoreJson = "{\"Action\":\"RestoreBackupCallback\",\"param1\":\"1\"}";
				UnityToWeb(RestoreJson);
				break;
			case "GetSystemBackup":
				//string GetSystemBackupJson = "{\"Action\":\"GetSystemBackupCallback\",\"param1\":" + JsonConvert.SerializeObject(GetSystemBackup(int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString()))) + "}";
				UnityToWeb(GetSystemBackup(int.Parse(dic["param1"].ToString()), int.Parse(dic["param2"].ToString())));
				break;
			case "DeleteBackup":
				string back = "";
				if (DeleteBackup(int.Parse(dic["param1"].ToString())))
					back = "success";
				else
					back = "failed";
				string deleteBackupJson= "{\"Action\":\"DeleteBackupCallback\",\"param1\":\"" + back+ "\"}";
				UnityToWeb(deleteBackupJson);
				break;
		}
    }

    public void uploadImg(string path, string option)
    {
        string fileName = "";
        if (path == null || path.Equals(""))
            return;
        string fileType = path.Substring(path.LastIndexOf(".") + 1, (path.Length - path.LastIndexOf(".") - 1));
        fileName = Utils.CreateId() + "." + fileType;
        string fileNameTemp = GlobalConfig.SaveFileImgPath + "temp/" + fileName;
        FileUtils.CopyFile(path, fileNameTemp);
        string json = "{\"Action\":\"AddTwoQuestionImgCallback\",\"param1\":\"temp/" + fileName + "\",\"param2\":\"" + option + "\"}";
        UnityToWeb(json);
    }

    public void updateLinks(string linkUrl, string linkId)
    {
        InfoLinksDBHelper EditExternalLinksDB = new InfoLinksDBHelper();
        LinkInfo EditExternalLinksinfo = new LinkInfo();
        EditExternalLinksinfo.Id = int.Parse(linkId);
        EditExternalLinksinfo.Address = linkUrl;
        if (EditExternalLinksDB.updateLink(EditExternalLinksinfo))
            UnityToWeb("updateLinksCallback", "success");
        else UnityToWeb("updateLinksCallback", "faild");
    }
    public void UpLinks(string linkUrl)
    {
        linkUrl = linkUrl.Replace("\\", "/");
        string UpExternalLinksCallback = "{\"Action\":\"UpExternalLinksCallback\",\"param1\":\"" + linkUrl + "\"}";
        UnityToWeb(UpExternalLinksCallback);
    }
    public void UnityToWeb(string json)
    {

		Debug.LogError("UnityToWeb:" + json);
		var view = ViewComponent.View;
		//Debug.Log(ViewComponent.View);
        view.TriggerEvent("ReceiveUnity", json);
    }

    public void UnityToWeb(string action, string param)
    {
        string message = "";
        if(param.Equals(""))
            message = "{\"Action\":\"" + action + "\",\"param1\":\"\"}";
        else if (param.Substring(0, 1).Equals("{") || param.Substring(0, 1).Equals("["))
            message = "{\"Action\":\"" + action + "\",\"param1\":" + param + "}";
        else message = "{\"Action\":\"" + action + "\",\"param1\":\"" + param + "\"}";
        Debug.LogError("UnityToWeb:" + message);
		if (ViewComponent != null)
		{
			var view = ViewComponent.View;
			if (view != null)
				view.TriggerEvent("ReceiveUnity", message);
			else
				print("View为空");
		}
    }

    public void UnityToWeb(string action, string param1, string param2)
    {
        string message = "{\"Action\":\"" + action + "\",\"param1\":" + param1 + ",\"param2\":" + param2 + "}";
        if (param1.Equals(""))
            message = "{\"Action\":\"" + action + "\",\"param1\":\"\"";
        else if (param1.Substring(0, 1).Equals("{") || param1.Substring(0, 1).Equals("["))
            message = "{\"Action\":\"" + action + "\",\"param1\":" + param1;
        else message = "{\"Action\":\"" + action + "\",\"param1\":\"" + param1 + "\"";
        if(param2.Equals(""))
            message += ",\"param2\":\"\"}";
        else if (param2.Substring(0, 1).Equals("{") || param2.Substring(0, 1).Equals("["))
            message += ",\"param2\":" + param2 + "}";
        else message += ",\"param2\":\"" + param2 + "\"}";
        Debug.LogError("UnityToWeb:" + message);
        var view = ViewComponent.View;
        view.TriggerEvent("ReceiveUnity", message);
    }

    //打开链接
    private void OpenLink(string type, string link)
    {
        OpenExternalLink openLinkOperation = new OpenExternalLink();

        if (type == "ppt")
        {
            openLinkOperation.OpenLinksByType(OpenExternalLink.LinkType.ppt, link);
        }
        else if (type == "word")
        {
            openLinkOperation.OpenLinksByType(OpenExternalLink.LinkType.word, link);
        }
        else if (type == "ie")
        {
            openLinkOperation.OpenLinksByType(OpenExternalLink.LinkType.ie, link);
        }
        else if (type == "video")
        {
            openLinkOperation.OpenLinksByType(OpenExternalLink.LinkType.baofeng, link);
        }
        else if (type == "excel")
        {
            openLinkOperation.OpenLinksByType(OpenExternalLink.LinkType.excel, link);
        }
    }

    private void DataBackup()
    {
        try
        {
            //String command = "mysqldump --quick --host=localhost --default-character-set=gb2312 --lock-tables --verbose  --force --port=端口号 --user=用户名 --password=密码 数据库名 -r 备份到的地址";

            //构建执行的命令
            StringBuilder sbcommand = new StringBuilder();

            StringBuilder sbfileName = new StringBuilder();
            sbfileName.AppendFormat("{0}", DateTime.Now.ToString()).Replace("-", "").Replace(":", "").Replace(" ", "");
            String fileName = sbfileName.ToString();

            sbcommand.AppendFormat("mysqldump -h192.168.2.110 -uroot -p123456 railwaydatabase> c:/a" + Utils.CreateId() + ".sql");
            String command = sbcommand.ToString();

            //获取mysqldump.exe所在路径
            String appDirecroty = Application.streamingAssetsPath;

			Cmd.StartCmd(appDirecroty, command);
        }
        catch (Exception ex)
        {
            Debug.LogError("数据库备份失败！"+ex.Message);
        }
        finally
        {


        }
    }

    private void UnityReceive()
    {

    }
	public void InputData(string path)
	{
		UserDBHelper excelInfo = new UserDBHelper();
		if (excelInfo.importUser(path))
		{
			UnityToWeb("ImportStuInfoCallback", "success");
			Debug.Log("导入，成功");
		}
		else
		{
			UnityToWeb("ImportStuInfoCallback", "failed");
			Debug.Log("导入，失败");
		}
    }
	public void PlayMusicByID(int ID)
	{
		AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
		if (ID < 1 || ID > 6)
			return;
		string aduioPath=null;
		switch (ID)
		{
			case 1:
				aduioPath = "shineijifanganquanguanlizhidu";
				break;
			case 2:
				aduioPath = "shangdaozuoyeanquanguanlizhidu";
				break;
			case 3:
				aduioPath = "xiaofangguanlizhidu";
				break;
			case 4:
				aduioPath = "daolufa";
				break;
			case 5:
				aduioPath = "tieluanquanguanlitiaoli";
				break;
			case 6:
				aduioPath = "tielujishuguanliguiceng";
				break;
            default:
                AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
                return;
		}
		AudioManager.getInstance().AudioPlay("zhishi/"+ID+"/" + aduioPath, AudioManager.MusicNumType.realtimeMusic);
	}
	public string getUsetName()
	{
		return userName;
	}
	/// <summary>
	/// 创建word文档，保存评估信息
	/// </summary>
	public string createWord(string content)
	{
		string path = Application.dataPath + "/StreamingAssets/Report/" + Utils.CreateId() + ".doc";
		FileStream fs= File.Create(path);
		fs.Close();
		StreamWriter sw = new StreamWriter(path);
		sw.Write(content);
		sw.Close();
		return path;
	}
	/// <summary>
	/// 备份数据库
	/// </summary>
	public void DoBackup()
	{
		string sqlName = Utils.CreateId();
		string sqlPath = Application.streamingAssetsPath + "/MysqlBackup/" + sqlName + ".sql";
		string cmdStr = Application.dataPath + "/StreamingAssets/mysqldump -h192.168.2.110 -uroot -p123456 railwaydatabase>"+ sqlPath;
		string fileName = "cmd.exe";
		Debug.Log(sqlPath);
		runCmd(fileName,cmdStr);
		writeTxt(sqlName);
	}
	/// <summary>
	/// 将备份信息写入Txt
	/// </summary>
	/// <param name="content">写入内容</param>
	public void writeTxt(string content)
	{
		StreamWriter fs = new StreamWriter(Application.streamingAssetsPath + "/mysqlBackupInfo.txt", true);
		fs.WriteLine(content);
		fs.Close();
	}
	/// <summary>
	/// 逐行读取txt文件并返回数组
	/// </summary>
	/// <returns></returns>
	public string[] readerTxt()
	{
		string[] filelist = File.ReadAllLines(Application.streamingAssetsPath + "/mysqlBackupInfo.txt", Encoding.Default);
		return filelist;
	}

	/// <summary>
	/// 调用cmd
	/// </summary>
	/// <param name="fileName">程序名</param>
	/// <param name="cmdStr">命令行</param>
	public void runCmd(string fileName, string cmdStr)
	{
		bool Flag = true;
		Debug.Log(cmdStr);
		System.Diagnostics.Process proc = new System.Diagnostics.Process();
		proc.StartInfo.FileName = fileName;
		proc.StartInfo.UseShellExecute = false;
		proc.StartInfo.RedirectStandardInput = true;
		proc.StartInfo.RedirectStandardOutput = true;
		proc.StartInfo.RedirectStandardError = true;
		proc.StartInfo.CreateNoWindow = false;
		try
		{
			proc.StartInfo.Arguments = "/c " + cmdStr;
			proc.Start();//启动程序
						 //向cmd窗口发送输入信息
		}
		catch (Exception ex)
		{
			Debug.Log(ex.ToString());
			Flag = false;
		}
		string output = proc.StandardOutput.ReadToEnd();
		proc.WaitForExit();//等待程序执行完退出进程
		proc.Close();
		Debug.Log(output);
	}
	/// <summary>
	/// 还原数据库
	/// </summary>
	/// <param name="path">数据库地址</param>
	public void restore(int path)
	{
		string cmdStr = Application.streamingAssetsPath+ "/mysql -h192.168.2.110 -uroot -p123456 railwaydatabase < "+ Application.streamingAssetsPath + "/MysqlBackup/"+readerTxt()[path].ToString() +".sql";
		string fileName = "cmd.exe";
		runCmd(fileName, cmdStr);
	}
	public string GetSystemBackup(int page,int pagenum)
	{
		List<MysqlBackupData> backupData = new List<MysqlBackupData>();
		string[] filelist = readerTxt();
		string[] returnFileList=new string[pagenum];
		for (int i = 0; i < pagenum; i++)
		{
			if ((page - 1) * pagenum +i < filelist.Length)
			{
				MysqlBackupData data = new MysqlBackupData();
				returnFileList[i] = filelist[(page - 1) * pagenum +i];
				data.Id = (page - 1) * pagenum + i + 1;
				data.Url = Application.streamingAssetsPath + "/MysqlBackup/" + returnFileList[i];
				data.BackupTime= returnFileList[i].Substring(0, 4)+"/"+ returnFileList[i].Substring(4, 2)+"/"+ returnFileList[i].Substring(6, 2)+"-"+ returnFileList[i].Substring(8, 2)+":"+ returnFileList[i].Substring(10, 2)+":"+ returnFileList[i].Substring(12, 2);
				backupData.Add(data);
			}
		}
		string GetSystemBackupJson = "{\"Action\":\"GetSystemBackupCallback\",\"param1\":" + JsonConvert.SerializeObject(backupData) +",\"param2\":\""+ filelist.Length + "\"}";
		return GetSystemBackupJson;
	}
	/// <summary>
	/// 删除备份数据及其信息
	/// </summary>
	/// <param name="id"></param>
	public bool DeleteBackup(int id)
	{
		List<string> lines = new List<string>(File.ReadAllLines(Application.streamingAssetsPath + "/mysqlBackupInfo.txt"));
		try
		{
			File.Delete(Application.streamingAssetsPath + "/MysqlBackup/" + lines[id-1] + ".sql");
			lines.RemoveAt(id-1);
			File.WriteAllLines(Application.streamingAssetsPath + "/mysqlBackupInfo.txt", lines.ToArray());
			return true;
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			return false;
		}
	}
}
