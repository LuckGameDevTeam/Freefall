<?php

#This file is part of the "Simple IAP System" project by Rebound Games.
#You are only allowed to use these resources if you've bought them directly or indirectly
#from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
#distribute or otherwise make available to any third party the Service or the Content. 

$store = $_POST['store'];   //store to verify against (IOS or Android)
$pid = $_POST['pid'];   //product_id (iOS) - productId (Android)
$tid = $_POST['tid'];   //orderId (Android)
$rec = $_POST['rec'];   //receipt data (iOS) - purchase token (Android)
$ios_bid = 'YOUR IOS APP BUNDLE IDENTIFIER';  //bundle identifier for this app (iOS)
$android_bid = 'YOUR ANDROID APP BUNDLE IDENTIFIER'; //bundle identifier for this app (Android)

if(!isset($store))
{
    echo 'false';
    return;
}

if($store == 'IOS')
{
    verifyReceiptIOS($rec, false);
}
elseif($store == 'Android')
{
    verifyReceiptAndroid($rec);
}


function verifyReceiptIOS($receipt, $sandbox)
{
	$secret = 'YOUR ITUNES SHARED SECRET';

    if ($sandbox)
    {
        $url = 'https://sandbox.itunes.apple.com/verifyReceipt';
    }
    else
    {
        $url = 'https://buy.itunes.apple.com/verifyReceipt';
    }

    $postData = json_encode(array('receipt-data' => $receipt,
								  'password' => $secret));

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, $postData);
    $encodedResponse = curl_exec($ch);
    curl_close($ch);

    if ($encodedResponse == false)
    {
        echo 'false';
        return;
    }

    global $pid, $ios_bid;
    $response = json_decode($encodedResponse);
    $status = $response->{'status'};
    $server_pid = $response->{'receipt'}->{'product_id'};
    $server_bid = $response->{'receipt'}->{'bid'};

    if($status == 21007)
    {
        verifyReceiptIOS($receipt, true);
    }
    elseif (!isset($pid, $ios_bid) || $status != 0
        || $pid != $server_pid || $ios_bid != $server_bid)
    {
        echo 'false';
    }
    else
    {
        echo 'true';
    }
}


function verifyReceiptAndroid($receipt)
{
    $path = 'access_token.php';

    date_default_timezone_set("UTC");
    $time = time() * 1000;

    //older than 30 minutes (60 minutes expiration)
    if(!file_exists($path) || ($time - filemtime($path)) > 1800)
    {
        requestAccessTokenAndroid($path);
    }

    $fh = fopen($path, 'r');
    if (flock($fh, LOCK_SH))
    {
        fgets($fh);
        $access = base64_decode(fgets($fh));
        fclose($fh);
    }

    global $pid, $tid, $android_bid;
    $type = $_POST['type'];

    if($type == "subs")
    {
        $url =  "https://www.googleapis.com/androidpublisher/v1.1/applications/$android_bid/subscriptions/$pid/purchases/$receipt";
    }
    else
    {
        $url =  "https://www.googleapis.com/androidpublisher/v1.1/applications/$android_bid/inapp/$pid/purchases/$receipt";
    }

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, array("Authorization: Bearer $access"));
    $encodedResponse = curl_exec($ch);
    curl_close($ch);

    if ($encodedResponse == false)
    {
        echo 'false';
        return;
    }

    $response = json_decode($encodedResponse);
    $type = $response->{'kind'};

    if(!isset($type))
    {
        echo 'false';
        return;
    }

    if($type == 'androidpublisher#subscriptionPurchase')
    {
        $expiration = $response->{'validUntilTimestampMsec'};
        if($time > $expiration)
        {
            echo 'false';
        }
        else
        {
            echo 'true';
        }
    }
    elseif($type == 'androidpublisher#inappPurchase')
    {
        $status = $response->{'purchaseState'};
        if ($status != 0)
        {
            echo 'false';
        }
        else
        {
            echo 'true';
        }
    }
    else
    {
        echo 'true';
    }
}


function requestAccessTokenAndroid($path)
{
    $refresh = 'YOUR GOOGLE ACCOUNT REFRESH TOKEN';
    $client_id = 'YOUR GOOGLE ACCOUNT CLIENT ID';
    $client_secret = 'YOUR GOOGLE ACCOUNT CLIENT SECRET';

    $url = 'https://accounts.google.com/o/oauth2/token';

    $postData = array('grant_type' => 'refresh_token',
        'client_id'  => $client_id,
        'client_secret' => $client_secret,
        'refresh_token' => $refresh);

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, $postData);
    $encodedResponse = curl_exec($ch);
    curl_close($ch);

    $response = json_decode($encodedResponse);
    $token = $response->{'access_token'};

    if(!isset($token)) return;

    $fh = fopen($path, 'w');
    if (flock($fh, LOCK_EX))
    {
        fwrite($fh, "<?php header('HTTP/1.0 404 Not Found'); die(); ?>\n");
        fwrite($fh, base64_encode($token));
        fclose($fh);
    }
}