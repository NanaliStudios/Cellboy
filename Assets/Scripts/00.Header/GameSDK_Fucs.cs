using UnityEngine;
using System.Collections;

class GameSDK_Funcs
{
	public static void Initialize()
	{
		#if UNITY_ANDROID
		GooglePlayConnection.Instance.Connect ();
		AndroidInAppPurchaseManager.Client.AddProduct("coin_200");
		AndroidInAppPurchaseManager.Client.Connect ();
	
		//GooglePlaySavedGamesManager.Instance.ShowSavedGamesUI ("MySaved", 3);


		//GooglePlay CloudSave Set Delegates
		GooglePlaySavedGamesManager.ActionGameSaveLoaded += delegate(GP_SpanshotLoadResult result) {

		};

		#elif UNITY_IOS

		Social.localUser.Authenticate(delegate {

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
		Social.ShowLeaderboardUI();
		#endif
	}

	public static void SubmitScore_LeaderBoard(int iScore)
	{
		#if UNITY_ANDROID
		GooglePlayManager.Instance.SubmitScoreById("CgkI-5Pv_oYcEAIQAQ" , iScore);
		#endif

		#if UNITY_IOS
		//Social.ReportScore(iScore, "BoardName");
		#endif
	}

	public static void Do_CloudSave(byte[] Data)
	{


		#if UNITY_ANDROID
		GooglePlaySavedGamesManager.Instance.CreateNewSnapshot ("SaveData","",null, Data, 0);
		#elif UNITY_IOS
		#endif
	}

	public static void Do_CloudLoad()
	{

		#if UNITY_ANDROID
		GooglePlaySavedGamesManager.instance.LoadAvailableSavedGames();
		#elif UNITY_IOS
		#endif
	}

	public static void Get_CloudSave()
	{
		#if UNITY_ANDROID
		//GooglePlaySavedGamesManager.action
		#endif
		
		#if UNITY_IOS
		#endif
	}

	public static void Gamedata_CloudSave()
	{
		#if UNITY_ANDROID
		#endif
		
		#if UNITY_IOS
		#endif
	}

	//Purchase
	public static void Purcahse_Item(string strID)
	{
		#if UNITY_ANDROID
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
