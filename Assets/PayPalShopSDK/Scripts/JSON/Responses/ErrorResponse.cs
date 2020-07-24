using System;
using System.Collections.Generic;

[Serializable]
public class ErrorResponse {

	public string name, message, information_link, debug_id;
	public List<Detail> details;

	[Serializable]
	public class Detail
	{
		public string field, issue;
	}
}