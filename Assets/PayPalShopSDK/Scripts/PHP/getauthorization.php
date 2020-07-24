<?php
$_post = (array) filter_input_array(INPUT_POST, FILTER_SANITIZE_SPECIAL_CHARS);
$_get = (array) filter_input_array(INPUT_GET, FILTER_SANITIZE_SPECIAL_CHARS);
if(!isset($_post["authorization"]) || !isset($_get['mode'])){
    echo "Error 403: Forbidden";
    exit;
}

$ch = curl_init();
if($_get['mode'] == 'live')
    $url = 'https://api.paypal.com/v1/oauth2/token';
else
    $url = 'https://api.sandbox.paypal.com/v1/oauth2/token';
curl_setopt($ch, CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_POST, 1);
curl_setopt($ch, CURLOPT_POSTFIELDS, 'grant_type=client_credentials');
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

$headers = [
    'Accept: application/json',
    'Accept-Language: en_US',
    'Authorization: Basic ' . $_post['authorization'],
];

curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);

$server_output = curl_exec ($ch);

curl_close ($ch);

echo $server_output;

?>