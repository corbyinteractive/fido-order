using System;
using System.Collections.Generic;

[Serializable]
public class GetPaymentInfoResponse
{
	public string id, intent, state, cart, create_time, update_time;
	public Payer payer;
	public List<Transaction> transactions;
	public RedirectURLs redirect_urls;
	public List<Link> links;

	[Serializable]
	public class Link
	{
		public string href, rel, method;
	}

	[Serializable]
	public class RedirectURLs
	{
		public string return_url, cancel_url;
	}

	[Serializable]
	public class Transaction
	{
		public Amount amount;
		public Payee payee;
		public string description, invoice_number;
		public ItemList item_list;
	}

	[Serializable]
	public class Item
	{
		public string name, description, price, currency, quantity;
	}

	[Serializable]
	public class ShippingAddress
	{
		public string recipient_name, line1, city, state, postal_code, country_code;
	}

	[Serializable]
	public class ItemList
	{
		public List<Item> items;
		public ShippingAddress shipping_address;
	}

	[Serializable]
	public class Amount
	{
		public string total, currency;
	}

	[Serializable]
	public class Payee
	{
		public string email;
	}

	[Serializable]
	public class Payer
	{
		public string payment_method, status;
		public PayerInfo payer_info; 
	}

	[Serializable]
	public class PayerInfo
	{
		public string email, first_name, payer_id, country_code;
	}
}