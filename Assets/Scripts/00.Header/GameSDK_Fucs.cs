using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Collections;
using System.Runtime.InteropServices;

class GameSDK_Funcs
{
	public static byte[] CurrentSaveDAta = null;

	#if UNITY_IOS
//	[DllImport ("__Internal")]
//	private static extern bool StorekitCellboy_Initialize();
//	[DllImport ("__Internal")]
//	private static extern bool StorekitCellboy_GetStoreItemInformation(string storeItemID);
//	[DllImport ("__Internal")]
//	private static extern int StorekitCellboy_BeginPurchase(string storeItemID);
//	[DllImport ("__Internal")]
//	private static extern bool StorekitCellboy_FinishPurchase(string storeItemID);
//	[DllImport ("__Internal")]
//	private static extern void StorekitCellboy_ProcessErrorPurchase();

	#endif

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

				if(obj.isSuccess)
				{
					if(obj.purchase.SKU == "coin_200")
					{
						_PlayerData.m_Gamedata.m_iHaveCoin += 200;
						AndroidInAppPurchaseManager.Client.Consume ("coin_200");
					}
						
					if(obj.purchase.SKU == "coin_500")
					{
						_PlayerData.m_Gamedata.m_iHaveCoin += 500;
						AndroidInAppPurchaseManager.Client.Consume ("coin_500");
					}

					if(obj.purchase.SKU == "coin_5000")
					{
						_PlayerData.m_Gamedata.m_iHaveCoin += 5000;
						AndroidInAppPurchaseManager.Client.Consume ("coin_5000");
					}

					if(obj.purchase.SKU == "adoff")
					{
						PlayerPrefs.SetInt("Adoff", 1);
						Application.LoadLevel ("00_Logo");
					}

					TapjoyManager.Instance.TrackInappPurchase_ForAndroid (obj.purchase.SKU, obj.purchase.originalJson, obj.purchase.signature);
					_PlayerData.GameData_Save ();
				}
				else if(obj.isFailure)
				{
					//Debug.Log("Purchased fail");
					return;
				}
			}
		};


		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += delegate(BillingResult obj) {
		
			PlayerData _PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData> ();

			if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_200")) {
				_PlayerData.m_Gamedata.m_iHaveCoin += 200;
				AndroidInAppPurchaseManager.Client.Consume ("coin_200");
			}

			if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_500")) {
				_PlayerData.m_Gamedata.m_iHaveCoin += 500;
				AndroidInAppPurchaseManager.Client.Consume ("coin_500");
			}

			if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("coin_5000")) {
				_PlayerData.m_Gamedata.m_iHaveCoin += 5000;
				AndroidInAppPurchaseManager.Client.Consume ("coin_5000");
			}

			if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased ("adoff")) {

				if(PlayerPrefs.GetInt("Adoff") == 1)
					return;

				PlayerPrefs.SetInt("Adoff", 1);
				Application.LoadLevel ("00_Logo");
			}
		};

		//GooglePlay CloudSave Set Delegates
		GooglePlaySavedGamesManager.ActionGameSaveLoaded += delegate(GP_SpanshotLoadResult result) {

			//Debug.Log("Cloud Save Loaded Complete");
			CurrentSaveDAta = result.Snapshot.bytes;

		};

		#elif UNITY_IOS

		Social.localUser.Authenticate( sucess => {
			//if(sucess)
				//Debug.Log("IOS Gamecenter login sucess");
			//else
				//Debug.Log("IOS Gamecenter login failed");
		});

		//StorekitCellboy_GetStoreItemInformation("coin_200");
		//StorekitCellboy_Initialize();

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
			//Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
		#endif
	}

	public static void Do_CloudSave(byte[] Data)
	{


		#if UNITY_ANDROID
		//Debug.Log ("Try Save GameData to GoogleCloud");
		GooglePlaySavedGamesManager.Instance.CreateNewSnapshot ("SaveData", "",
		                                                        new Texture2D( 100, 100, TextureFormat.RGB24, false ),
		                                                        Data, 0);
		#elif UNITY_IOS
		//Debug.Log ("Try Save GameData to ICloud");
		P31CloudFile.writeAllBytes("SaveData", Data);
		#endif
	}

	public static void Do_CloudLoad()
	{
		#if UNITY_ANDROID
		//Debug.Log ("Try Load GameData to GoogleCloud");
		GooglePlaySavedGamesManager.instance.LoadSpanshotByName("SaveData");
		#elif UNITY_IOS
		//Debug.Log ("Try Load GameData to ICloud");
		CurrentSaveDAta = P31CloudFile.readAllBytes("SaveData");
		#endif
	}

	//Purchase
	public static void Purcahse_Item(string strID)
	{
		#if UNITY_ANDROID
		AndroidInAppPurchaseManager.Client.Purchase (strID);
		#elif UNITY_IOS
		//StorekitCellboy_BeginPurchase(strID);
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


	//for ios 
	static public void SuccessPurchase(string strID)
	{		
		//StorekitCellboy_FinishPurchase (strID);
	}

}
