using System;
using System.Collections.Generic;

[Serializable]
public class PreparePaymentResponse
{
	public string id, intent, state, create_time;
	public Payer payer;
	public List<Transaction> transactions;
	public List<Link> links;

	[Serializable]
	public class Payer
	{
		public string payment_method;
	}

	[Serializable]
	public class Transaction
	{
		public Amount amount;
		public string description;
	}

	[Serializable]
	public class Amount
	{
		public string total, currency;
	}

	[Serializable]
	public class Link
	{
		public string href, rel, method;
	}
}


