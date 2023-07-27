param (
    [string]
    $filePath,
    [string]
    $appVersion,
    [string]
    $blobUrl,
    [string]
    $sasToken
)

$fileName = Split-Path $filePath -leaf
$putUrl = "$blobUrl$fileName$sasToken"

$headers = @{ 'x-ms-meta-AppVersion' = $appVersion; 'x-ms-blob-type' = 'BlockBlob' }
$contentType = 'application/x-zip-compressed'
Invoke-WebRequest $putUrl -Method 'PUT' -ContentType $contentType -Headers $headers -InFile $filePath -UseBasicParsing