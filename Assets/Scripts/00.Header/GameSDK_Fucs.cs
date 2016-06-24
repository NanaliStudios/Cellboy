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
		AndroidInAppPurchaseManager.Client.AddProduct ("coin_200");
		AndroidInAppPurchaseManager.Client.AddProduct ("coin_500");
		AndroidInAppPurchaseManager.Client.AddProduct ("coin_1000");
		AndroidInAppPurchaseManager.Client.AddProduct ("addoff");
		AndroidInAppPurchaseManager.Client.Connect ();
	
		//IAP Purchase delegate
		AndroidInAppPurchaseManager.ActionProductPurchased += delegate(BillingResult obj){
			if (AndroidInAppPurchaseManager.Client.IsConnected) {
				PlayerData _PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData> ();

				if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_200")) {
					_PlayerData.m_Gamedata.m_iHaveCoin += 200;
					AndroidInAppPurchaseManager.Client.Consume ("coin_200");
					TapjoyManager.Instance.TrackInappPurchase_ForAndroid ("coin_200", "", "");
					Debug.Log ("coin_200 Purchase success");
				} else if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_500")) {
					_PlayerData.m_Gamedata.m_iHaveCoin += 500;
					AndroidInAppPurchaseManager.Client.Consume ("coin_500");
					TapjoyManager.Instance.TrackInappPurchase_ForAndroid ("coin_500", "", "");
					Debug.Log ("coin_500 Purchase success");
				} else if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_1000")) {
					_PlayerData.m_Gamedata.m_iHaveCoin += 1000;
					AndroidInAppPurchaseManager.Client.Consume ("coin_1000");
					TapjoyManager.Instance.TrackInappPurchase_ForAndroid ("coin_1000", "", "");
					Debug.Log ("coin_1000 Purchase success");
				}
			
				if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("adoff")) {
					PlayerPrefs.SetInt("Adoff", 1);
					Application.LoadLevel ("00_Logo");
				}

				_PlayerData.GameData_Save ();
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
			
		EasyStoreKit.LoadProducts();


		EasyStoreKit.transactionPurchasedEvent += delegate(string productIdentifier) {

		PlayerData _PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData>();

		if(productIdentifier == "coin_200")
		{
		_PlayerData.m_Gamedata.m_iHaveCoin += 200;
		Debug.Log("coin_200 Purchase success");
		}
		else if(productIdentifier == "coin_500")
		{
		_PlayerData.m_Gamedata.m_iHaveCoin += 500;
		Debug.Log("coin_500 Purchase success");
		}
		else if(productIdentifier == "coin_1000")
		{
		_PlayerData.m_Gamedata.m_iHaveCoin += 1000;
		Debug.Log("coin_1000 Purchase success");
		}
		else if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("adoff")) {
		_PlayerData.m_Gamedata.m_bAdOff = true;
		Application.LoadLevel ("00_Logo");
		}

		_PlayerData.GameData_Save ();

		};
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
		#elif UNITY_IOS
		EasyStoreKit.BuyProductWithIdentifier ("coin_200", 1);
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
