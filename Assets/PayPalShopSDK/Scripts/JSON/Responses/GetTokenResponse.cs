using System;

[Serializable]
public class GetTokenResponse
{
	public string scope, access_token, token_type, app_id, expires_in;
}