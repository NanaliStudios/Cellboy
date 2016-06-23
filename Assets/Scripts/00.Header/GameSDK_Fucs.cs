using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Collections;

class GameSDK_Funcs
{
	public static byte[] CurrentSaveDAta = null;

	public static void Initialize()
	{
		#if UNITY_ANDROID
		GooglePlayConnection.Instance.Connect ();
		AndroidInAppPurchaseManager.Client.AddProduct("coin_200");
		AndroidInAppPurchaseManager.Client.Connect ();
	
		AndroidInAppPurchaseManager.ActionProductPurchased += delegate {
			if(AndroidInAppPurchaseManager.Client.IsConnected)
			{
				if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_200"))
				{
					GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>().m_Gamedata.m_iHaveCoin += 200;
					AndroidInAppPurchaseManager.Client.Consume("coin_200");
					Debug.Log("coin_200 Purchase success");
				}
			}

		};  


		//GooglePlay CloudSave Set Delegates
		GooglePlaySavedGamesManager.ActionGameSaveLoaded += delegate(GP_SpanshotLoadResult result) {

			Debug.Log("Cloud Save Loaded Complete");
			CurrentSaveDAta = result.Snapshot.bytes;

		};

		#elif UNITY_IOS
		Social.localUser.Authenticate( sucess => {
			if(sucess)
				Debug.Log("IOS Gamecenter login sucess");
			else
				Debug.Log("IOS Gamecenter login failed");
		});
		#endif
	}

	public static bool isInitialized()
	{
		#if UNITY_ANDROID
		if (GooglePlayConnection.Instance.IsConnected)
			return true;
		#elif UNITY_IOS
		if(Social.localUser.authenticated)
		return true;
		#endif

		return false;
	}

	public static void Show_CloudSaveUI()
	{
		#if UNITY_ANDROID
		GooglePlaySavedGamesManager.Instance.ShowSavedGamesUI ("MySaved", 3);
		#elif UNITY_IOS

		#endif
	}

	public static void Show_LeaderBoard()
	{
		#if UNITY_ANDROID
		GooglePlayManager.Instance.ShowLeaderBoardById ("CgkI-5Pv_oYcEAIQAQ");
		#endif

		#if UNITY_IOS
		GameCenterPlatform.ShowLeaderboardUI("CellboyScore", UnityEngine.SocialPlatforms.TimeScope.AllTime);
		#endif
	}

	public static void SubmitScore_LeaderBoard(int iScore)
	{
		#if UNITY_ANDROID
		GooglePlayManager.Instance.SubmitScoreById("CgkI-5Pv_oYcEAIQAQ" , iScore);
		#endif

		#if UNITY_IOS
		Social.ReportScore(iScore, "CellboyScore", success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
		#endif
	}

	public static void Do_CloudSave(byte[] Data)
	{


		#if UNITY_ANDROID
		Debug.Log ("Try Save GameData to GoogleCloud");
		GooglePlaySavedGamesManager.Instance.CreateNewSnapshot ("SaveData", "",
		                                                        new Texture2D( 100, 100, TextureFormat.RGB24, false ),
		                                                        Data, 0);
		#elif UNITY_IOS
		Debug.Log ("Try Save GameData to ICloud");
		P31CloudFile.writeAllBytes("SaveData", Data);
		#endif
	}

	public static void Do_CloudLoad()
	{
		#if UNITY_ANDROID
		Debug.Log ("Try Load GameData to GoogleCloud");
		GooglePlaySavedGamesManager.instance.LoadSpanshotByName("SaveData");
		#elif UNITY_IOS
		Debug.Log ("Try Load GameData to ICloud");
		CurrentSaveDAta = P31CloudFile.readAllBytes("SaveData");
		#endif
	}

	//Purchase
	public static void Purcahse_Item(string strID)
	{
		#if UNITY_ANDROID
		AndroidInAppPurchaseManager.Client.Connect();
		AndroidInAppPurchaseManager.Client.Purchase (strID);
		#endif

		#if UNITY_IOS
		#endif
	}

	public static bool Check_IsPurchased(string strID)
	{

		#if UNITY_ANDROID
		if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased (strID)) 
			return true;
		#endif
		
		#if UNITY_IOS
		#endif

		return false;
	}

}
