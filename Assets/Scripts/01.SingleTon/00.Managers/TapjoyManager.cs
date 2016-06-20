using UnityEngine;
using System.Collections;
using TapjoyUnity;

public class TapjoyManager : MonoBehaviour
{
	public int m_iTapjoyCurrency = 0;
	public TJPlacement m_TjNotice = null;

	private static TapjoyManager s_Instance = null;
	public static TapjoyManager Instance
	{
		get
		{
			if (s_Instance == null)
				s_Instance =  FindObjectOfType(typeof (TapjoyManager)) as TapjoyManager;
			return s_Instance;
		}
	}
	
	private bool IsPossible_ShowContent0 = false;
	private bool IsPossible_ShowContent1 = false;
	private bool IsPossible_ShowContent2 = false;
	
	//연결 시도 후에 연결이 완료되면 Notice를 호출, 만약 5초이상 무응답이면 호출하지않고 종료.
	private IEnumerator Start()
	{
		DontDestroyOnLoad (this);

		//콜벡관련 등록.
		TJPlacement.OnRequestSuccess += HandlePlacementRequestSuccess;
		TJPlacement.OnRequestFailure += HandlePlacementRequestFailure;
		TJPlacement.OnContentReady += HandlePlacementContentReady;
		TJPlacement.OnContentShow += HandlePlacementContentShow;
		TJPlacement.OnContentDismiss += HandlePlacementContentDismiss;
		
		TJPlacement.OnPurchaseRequest += HandleOnPurchaseRequest;
		TJPlacement.OnRewardRequest += HandleOnRewardRequest;

		Tapjoy.OnGetCurrencyBalanceResponse += delegate(string currencyName, int balance){

			m_iTapjoyCurrency = balance;



		};
		
		while(true)
		{
			if(!Tapjoy.IsConnected)
			{
				Tapjoy.Connect();
				yield return new WaitForFixedUpdate();
			}
			else
			{
				m_TjNotice = ContentsReady("Notice");
				break;
			}
		}
	}
	
	//CreatePlacement가 완료되었는지 확인.
	public bool IsReady(string key)
	{
		bool value = false;
		switch(key)
		{
		case "Notice": 			   value = IsPossible_ShowContent0; break;
		case "getfreecoin1": value = IsPossible_ShowContent1; break;
		}
		
		return value;
	}
	
	//CreatePlacement. 이미 컨텐츠가 호출되어있으면 다시 요청하지 않음.
	public TJPlacement ContentsReady(string targetKey)
	{
		TJPlacement place = TJPlacement.CreatePlacement(targetKey);
		if (!place.IsContentReady ()) {
			place.RequestContent ();
		}

		return place;
	}
	
	//컨텐츠 대기상태 설정.
	private void SetReady(string name, bool isReady)
	{
		switch(name)
		{
		case "Notice": 			   IsPossible_ShowContent0 = isReady; break;
		case "getfreecoin1": IsPossible_ShowContent1 = isReady; break;
		}
	}
	
	public void HandlePlacementRequestSuccess(TJPlacement placement)
	{
		//컨텐츠 요청 성공시 리턴.
		Debug.Log ("HandlePlacementRequestSuccess : "+placement.GetName());
	}
	
	public void HandlePlacementRequestFailure(TJPlacement placement, string error)
	{
		//컨텐츠 요청 실패시 리턴. 컨텐츠별 대기상태를 false로 설정.
		Debug.Log ("HandlePlacementRequestFailure : "+placement.GetName());
		SetReady(placement.GetName(),false);
	}
	
	public void HandlePlacementContentReady(TJPlacement placement)
	{
		//컨텐츠가 준비완료 되었을 때 리턴.
		Debug.Log ("HandlePlacementContentReady : "+placement.GetName());
		SetReady(placement.GetName(),true);
	}
	
	public void HandlePlacementContentShow(TJPlacement placement)
	{
		//ShowContent를 수행하는 순간 리턴.
		Debug.Log ("HandlePlacementContentShow : "+placement.GetName());
	}
	
	public void HandlePlacementContentDismiss(TJPlacement placement)
	{
		//컨텐츠 종료시 리턴. 플레이스먼트 아이디별로 다른 동작을 취해주면 됨.
		Debug.Log ("HandlePlacementContentDismiss : "+placement.GetName());
		SetReady(placement.GetName(),false);
		if (placement.GetName () == "Notice")
			AdFunctions.m_bTjNoticeDismiss = true;
	}
	
	
	public void HandleOnPurchaseRequest(TJPlacement placement, TJActionRequest request, string productId)
	{
		//결제연동시 사용.
	}
	
	public void HandleOnRewardRequest(TJPlacement placement, TJActionRequest request, string itemId, int quantity)
	{
		//리워드관련 컨텐츠 연동시 사용.
	}
	
	public void TrackInappPurchase_ForApple(string itemName, string currency, double price, string transactionID)
	{
		//iOS 인앱결제 연동. (구매하는 아이템 이름, 화폐, 가격, 영수증ID)
		#if UNITY_IOS
		Tapjoy.TrackPurchaseInAppleAppStore(itemName, currency, price, transactionID);
		#endif
	}
	
	public void TrackInappPurchase_ForAndroid(string skuDetails, string purchaseData, string dataSignature)
	{
		//Android 인앱결제 연동. (변수들은 검색해서 사용 (안드로이드 기본 데이터))
		#if UNITY_ANDROID
		Tapjoy.TrackPurchaseInGooglePlayStore(skuDetails,purchaseData,dataSignature);
		#endif
	}
	
	public void TrackCustomEvent(string category, string eventName, string param1, string param2)
	{
		//커스텀 이벤트. ex = TrackCustomEvent(“GUN”,”Get”,”price”,”10”);
		Tapjoy.TrackEvent(category,eventName,param1,param2);
	}
}
