using System;
using System.Collections.Generic;

[Serializable]
public class ExecutePaymentResponse {

	public string id, intent, state, cart, create_time;
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
		public List<RelatedResources> related_resources;
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
		public Details details;
	}

	[Serializable]
	public class Payee
	{
		public string merchant_id, email;
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
		public ShippingAddress shipping_address;
	}

	[Serializable]
	public class RelatedResources
	{
		public Sale sale;
	}

	[Serializable]
	public class Sale
	{
		public string id, state, payment_mode, protection_eligibility, protection_eligibility_type, parent_payment, create_time, update_time;
		public Amount amount;
		public TransactionFee transaction_fee;
		public List<Link> links;
	}

	[Serializable]
	public class Details
	{
		public string subtotal;
	}

	[Serializable]
	public class TransactionFee
	{
		public string value, currency;
	}
}