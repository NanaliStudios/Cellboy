using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class GameSDKManager
{
	public static byte[] CurrentSaveDAta = null;

	#if UNITY_IOS
	[DllImport ("__Internal")]
	private static extern bool StorekitCellboy_Initialize();
	[DllImport ("__Internal")]
	private static extern bool StorekitCellboy_GetStoreItemInformation(string storeItemID);
	[DllImport ("__Internal")]
	private static extern int StorekitCellboy_BeginPurchase(string storeItemID);
	[DllImport ("__Internal")]
	private static extern bool StorekitCellboy_FinishPurchase(string storeItemID);
	[DllImport ("__Internal")]
	private static extern void StorekitCellboy_ProcessErrorPurchase();

	#endif

	// ****** Common ******
	private static List<ProductInfo> listProducts=new List<ProductInfo>(); //상품의 정보 : 한번 불러오면 게임이 끝날때까지 계속 들고 있음. (IOS,Android 공통사용).
	private static bool IsFake=false;

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

		StorekitCellboy_GetStoreItemInformation("coin_200");
		StorekitCellboy_Initialize();

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
		StorekitCellboy_BeginPurchase(strID);
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

	//FOR IOS

	// Return : 0 Success  1: Not complete get Item Information 2: Not complete past purchase 3: ?? 4: Network Error
	static public int BeginPurchase(string strItemID)
	{
		#if UNITY_WEBPLAYER
		return 0;
		#endif
		// TODO: Block-screen

		CurItemID = strItemID;
		IsFake = false;

		return StorekitCellboy_BeginPurchase (CurItemID);
	}

	public void Callback_StorekitCellboy_OnStoreItemInformationResponse(string storeItemInformationJSON)
	{
		listProducts.Clear();
	
		Hashtable responseJSON= MiniJSONV.Json.Deserialize(storeItemInformationJSON) as Hashtable;
		if(responseJSON!=null)
		{
			ArrayList arraylist=new ArrayList();
			#if UNITY_IPHONE
			arraylist= (ArrayList)responseJSON["id"];
			#else
			arraylist= (ArrayList)responseJSON["datas"];
			#endif

			for(int i=0;i<arraylist.Count;i++)
			{
				Hashtable ht=(Hashtable)arraylist[i];
				if(ht!=null)
				{
					string currencyCode="";
					string itemPrice="";
					string storeItemID="";

					#if UNITY_IPHONE
					currencyCode = ht["currency"] as string;
					itemPrice = ht["price"] as string;
					storeItemID = ht["storeItemID"] as string;
					#else
					currencyCode = ht["price_currency_code"] as string;
					itemPrice = ht["price"] as string;
					storeItemID = ht["productId"] as string;
					#endif

					ProductInfo productInfo=new ProductInfo(itemPrice,storeItemID,currencyCode);
					listProducts.Add(productInfo);

					Debug.Log (productInfo.Currency + " : " + productInfo.Price + " : " + productInfo.ProductId);
				}
			}

			//listProducts 를 이용하여 가격을 표시할부분에 가격 표시.
		}
	}

	public static ProductInfo GetProductInfo(string strItemID)
	{
		ProductInfo productInfo=new ProductInfo("","","");

		if(listProducts.Count>0)
		{
			string TargetProductId=strItemID;
			for(int i=0;i<listProducts.Count;i++)
				if(string.Equals(listProducts[i].ProductId,TargetProductId))
				{
					productInfo=listProducts[i];
					break;
				}
		}

		//IOS or Android에서 받아온 상품에 대한 정보들이 없으면, 로컬에서 직접 상품가격을 넣어줌.
		if(string.IsNullOrEmpty(productInfo.ProductId))
		{
			//			productInfo=CV.GetDefaultProductInto(storeItemID);
			productInfo.ProductId=(strItemID);
		}

		return productInfo;
	}


	// ****** Result Purchase ******
	static public bool FinishPurchase(string strItemID)
	{
		return StorekitCellboy_FinishPurchase (strItemID);
	}

	static public bool FinishPurchase()
	{
		return StorekitCellboy_FinishPurchase (CurItemID);
	}

	static public bool FakeFinishPurchase()
	{
		IsFake = true;
		return StorekitCellboy_FinishPurchase (CurItemID);
	}

	static public void ProcessErrorPurchase()
	{
		StorekitCellboy_ProcessErrorPurchase();
	}

	public void OnErrorPurchase_ForNotIOS(string error)
	{
		var ht=new Hashtable();
		ht["storeItemID"]=CurItemID;
		ht["errorDescription"]=error;
		ht["errorCode"]="";
		string json=MiniJSONV.Json.Serialize(ht);
		Debug.Log (json);
		Callback_StorekitCellboy_OnErrorPurchaseResponse(json);
	}

	// Native Callback Functions ( called back from Native Side using UnitySendMessage(string method, string message)
	void Callback_StorekitCellboy_OnErrorPurchaseResponse(string errorInfoJSON)
	{		
		// TODO: Remove Block-screen

		Hashtable errorInformation = MiniJSONV.Json.Deserialize (errorInfoJSON) as Hashtable;

		string storeItemID = "";
		if(errorInformation ["storeItemID"] != null)
			storeItemID = errorInformation ["storeItemID"] as string;

		string errorDescription = "";
		if(errorInformation ["errorDescription"] != null)
			errorDescription = errorInformation ["errorDescription"] as string;

		string errorCode = "";
		if(errorInformation ["errorCode"] != null)
			errorCode = errorInformation ["errorCode"] as string;

		Debug.Log ("StoreItemID:" + storeItemID + "ErrorDescription:" + errorDescription);

		//        ShowPopUpWindow ("Store Error", errorDescription);
		CurItemID = "";
	}


	void Callback_StorekitCellboy_OnBeginPurchaseResponse(string json)
	{
		//        Callback_StorekitCellboy_OnProcessErrorPurchase (json);
		// And than Finish        
		// To Do : Produce Item to In-Game-Side
		string purchasedStoreItemID = (CurItemID);

		Hashtable ht=MiniJSONV.Json.Deserialize(json) as Hashtable;
		string storeItemID = "";
		string transactionID = "";
		string idfaID = "";
		string receipt = "";
		string signature = "";
		// *** User.
		if(ht != null)
		{
			storeItemID = ht["storeItemID"].ToString();
			transactionID = ht["transactionID"].ToString();
			idfaID = ht["idfaID"].ToString();
			receipt = ht["receipt"].ToString();
			signature = ht["signature"].ToString();
		}

		Debug.Log ("___________________________________________________________________________________________________________________________");

		SuccessPurchase(storeItemID);
	}

	static public void OnFakeFinishPurchase_ForNotIOS()
	{
		// TODO: Remove Block-screen

		CurItemID = "";
	}

	public void OnFinishPurchase_ForNotIOS(string transactionID)
	{
		var ht=new Hashtable();
		ht["storeItemID"]=CurItemID;
		ht["transactionID"]=transactionID;
		ht["idfaID"]="It's not iOS Device.";
		string json= MiniJSONV.Json.Serialize(ht);
		Debug.Log (json);
		this.Callback_StorekitCellboy_OnFinishPurchaseResponse(json);
	}

	void Callback_StorekitCellboy_OnFinishPurchaseResponse(string json) //Final Dst.
	{
		Debug.Log ("Callback_StorekitCellboy_OnFinishPurchaseResponse");
		// TODO: Remove Block-screen

		if(!IsFake)
		{
			Hashtable ht= MiniJSONV.Json.Deserialize(json) as Hashtable;
			string storeItemID = "";
			string transactionID = "";
			string idfaID = "";
			// *** User.
			if(ht != null)
			{
				storeItemID = ht["storeItemID"].ToString();
				transactionID = ht["transactionID"].ToString();
				idfaID = ht["idfaID"].ToString();
			}
		}

		CurItemID = "";
	}

	static public string CurItemID="";
	void Callback_StorekitCellboy_OnProcessErrorPurchase(string json)
	{
		Hashtable ht= MiniJSONV.Json.Deserialize(json) as Hashtable;
		string storeItemID = "";
		string transactionID = "";
		//        string receipt = "";
		// *** User.
		if(ht != null)
		{
			storeItemID = ht["storeItemID"].ToString();
			transactionID = ht["transactionID"].ToString();
			//            receipt = ht["receipt"].ToString();
		}

		Debug.Log (storeItemID + " : " + transactionID);

		// To Do : Produce Item to In-Game-Side

		SuccessPurchase(transactionID);
	}

	static public void SuccessPurchase(string transactionId)
	{		
		// To Do : Produce Item to In-Game-Side
		string purchasedStoreItemID = CurItemID;
		if(purchasedStoreItemID== null)
		{
			Debug.Log("Invalid StoreItemID : "+CurItemID);
			//			return;
		}

		//get reawrd.

		PlayerData _PlayerData = GameObject.Find ("PlayerData(Clone)").GetComponent<PlayerData> ();

		switch (purchasedStoreItemID) {
		case "coin_200":
			_PlayerData.m_Gamedata.m_iHaveCoin += 200;
			AndroidInAppPurchaseManager.Client.Consume ("coin_200");
			break;
		case "coin_500":
			break;
		case "coin_1000":
			break;
		case "adoff":
			break;
		default:
			break;
		}

		_PlayerData.GameData_Save ();
		FinishPurchase (purchasedStoreItemID);
	}
}
	
public struct ProductInfo
{
	public string Price;
	public string ProductId;
	public string Currency;

	public ProductInfo(string TmpPrice,string TmpProductId,string TmpCurrency)
	{
		Price=TmpPrice;
		ProductId=TmpProductId;
		Currency=TmpCurrency;
	}
}
