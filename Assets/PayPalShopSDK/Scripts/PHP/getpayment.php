<?php
$_post = (array) filter_input_array(INPUT_POST, FILTER_SANITIZE_SPECIAL_CHARS);
$_get = (array) filter_input_array(INPUT_GET, FILTER_SANITIZE_SPECIAL_CHARS);
if(!isset($_post["authorization"]) || !isset($_get['mode']) || !isset($_post["payID"])){
    echo "Error 403: Forbidden";
    exit;
}

$ch = curl_init();
if($_get['mode'] == 'live')
    $url = 'https://api.paypal.com/v1/payments/payment/' . $_post['payID'];
else
    $url = 'https://api.sandbox.paypal.com/v1/payments/payment/' . $_post['payID'];
curl_setopt($ch, CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

$headers = [
    'Content-Type: application/json',
    'Authorization: Bearer ' . $_post['authorization'],
];

curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);

$server_output = curl_exec ($ch);

curl_close ($ch);

echo $server_output;

?>