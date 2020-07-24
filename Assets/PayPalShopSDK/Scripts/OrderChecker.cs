using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderChecker : MonoBehaviour
{

	public string payID;
	public string accessToken;
	public float checkInterval;
	public enum CheckState
	{
		NOT_STARTED,
		CHECKING,
		VERIFIED,
		SUCCESS,
		FAILURE,
	};
	public CheckState checkStatus;

	private void Start()
	{
		StartChecking();
	}

	private void Update()
	{
		if (checkStatus == CheckState.CHECKING)
		{
			GetPaymentResponse();
		}
		if (checkStatus == CheckState.VERIFIED)
		{
			ExecutePaymentResponse();
		}
	}

	public void StartChecking()
	{
		checkStatus = CheckState.CHECKING;
		InvokeRepeating("GetPayment", 0f, checkInterval);
	}

	private void GetPayment()
	{
		GetPayment previousCall = GetComponent<GetPayment>();
		if (previousCall != null)
		{
			Destroy(previousCall);
		}
		GetPayment getPayment = gameObject.AddComponent<GetPayment>();
		getPayment.accessToken = accessToken;
		getPayment.payID = payID;
	}

	private void ExecutePayment()
	{
		ExecutePayment previousCall = GetComponent<ExecutePayment>();
		if (previousCall != null)
		{
			Destroy(previousCall);
		}
		ExecutePayment executePayment = gameObject.AddComponent<ExecutePayment>();
		executePayment.accessToken = accessToken;
		executePayment.paymentID = payID;
		executePayment.payerID = GetComponent<GetPayment>().API_Success.payer.payer_info.payer_id;
	}

	private void GetPaymentResponse()
	{
		GetPayment previousCall = GetComponent<GetPayment>();
		if (previousCall != null && previousCall.API_Success != null && previousCall.API_Success.payer.status == "VERIFIED")
		{
			CancelInvoke("GetPayment");
			Verify();
		}
	}

	private void ExecutePaymentResponse()
	{
		ExecutePayment executePayment = this.gameObject.GetComponent<ExecutePayment>();
		if (executePayment != null && executePayment.API_Success != null && executePayment.API_Success.state == "approved")
		{
			Finish();
		}
		if (executePayment != null && executePayment.API_Success != null && executePayment.API_Success.state == "failed")
		{
			Fail();
		}
	}

	private void Verify()
	{
		checkStatus = CheckState.VERIFIED;
		InvokeRepeating("ExecutePayment", 0f, checkInterval);
	}

	private void Finish()
	{
		checkStatus = CheckState.SUCCESS;
		CancelInvoke("ExecutePayment");
	}

	private void Fail()
	{
		checkStatus = CheckState.FAILURE;
		CancelInvoke("ExecutePayment");
	}
}
