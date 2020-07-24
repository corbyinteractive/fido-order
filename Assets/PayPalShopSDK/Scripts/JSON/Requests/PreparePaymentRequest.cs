using System;
using System.Collections.Generic;

[Serializable]
public class PreparePaymentRequest
{
	public string intent;
	public Payer payer;
	public List<Transaction> transactions;
	public RedirectUrls redirect_urls;

	[Serializable]
	public class Payer
	{
		public string payment_method;
	}

	[Serializable]
	public class Transaction
	{
		public Amount amount;
		public ItemList item_list;
		public string description, invoice_number;
	}

	[Serializable]
	public class Amount
	{
		public string total, currency;
	}

	[Serializable]
	public class Item
	{
		public string name, description, quantity, price, currency;
	}

	[Serializable]
	public class ItemList
	{
		public List<Item> items;
	}

	[Serializable]
	public class RedirectUrls
	{
		public string return_url, cancel_url;
	}
}
	