''' <summary>
''' Class encapsulates tokens and secret to create OAuth signatures and return Authorization headers for web requests.
''' </summary>
Public Class OAuthHeader
    Private ReadOnly appSecret As String
    Private ReadOnly accessSecret As String

    Private Const signatureMethod As String = "HMAC-SHA1"
    Private Const version As String = "1.0"

    ''' <summary>All Header params compiled into a Dictionary</summary>
    Private ReadOnly headerParams As IDictionary(Of String, String)

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New(appToken As String, appSecret As String, accessToken As String, accessSecret As String)
        Me.appSecret = appSecret
        Me.accessSecret = accessSecret

        ' Initialize all class members
        Me.headerParams = New Dictionary(Of String, String) From {
            {"oauth_consumer_key", appToken},
            {"oauth_token", accessToken},
            {"oauth_nonce", Guid.NewGuid().ToString("n")},
            {"oauth_timestamp", Convert.ToInt32((DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1))).TotalSeconds).ToString()},
            {"oauth_signature_method", signatureMethod},
            {"oauth_version", version}
            }
    End Sub

    ''' <summary>
    ''' Pass request method and URI parameters to get the Authorization header value
    ''' </summary>
    ''' <param name="method">Request Method</param>
    ''' <param name="url">Request URI</param>
    ''' <returns>Authorization header value</returns>
    Public Function getAuthorizationHeader(method As String, url As Uri) As String
        Dim path As String = url.GetLeftPart(UriPartial.Path)

        ' Add the realm parameter to the header params
        Me.headerParams.Add("realm", path)

        'Add query parameter
        Dim query = Web.HttpUtility.ParseQueryString(url.Query)
        For Each p As String In From p1 As String In query Where Not String.IsNullOrWhiteSpace(query.Get(p1))
            Me.headerParams.Add(p, query.Get(p))
        Next

        ' Start composing the base string from the method and request URI
        Dim baseString As String = $"{method.ToUpper}&{Uri.EscapeDataString(path)}&"

        ' Gather, encode, and sort the base string parameters
        Dim encodedParams As New SortedDictionary(Of String, String)
        For Each p As KeyValuePair(Of String, String) In From p1 In Me.headerParams Where Not p1.Key.Equals("realm")
            encodedParams.Add(Uri.EscapeDataString(p.Key), Uri.EscapeDataString(p.Value))
        Next

        ' Expand the base string by the encoded parameter=value pairs
        Dim paramStrings As List(Of String) = (From p In encodedParams Select $"{p.Key}={p.Value}").ToList()
        Dim paramString As String = Uri.EscapeDataString(String.Join(Of String)("&", paramStrings))
        baseString &= paramString

        ' Create the OAuth signature
        Dim signatureKey As String = Uri.EscapeDataString(Me.appSecret) & "&" & Uri.EscapeDataString(Me.accessSecret)
        Dim hasher As Security.Cryptography.HMAC = Security.Cryptography.HMAC.Create()
        hasher.Key = Text.Encoding.UTF8.GetBytes(signatureKey)
        Dim rawSignature As Byte() = hasher.ComputeHash(Text.Encoding.UTF8.GetBytes(baseString))
        Dim oAuthSignature As String = Convert.ToBase64String(rawSignature)

        ' Include the OAuth signature parameter in the header parameters array
        Me.headerParams.Add("oauth_signature", oAuthSignature)

        ' Construct the header string
        Dim headerParamStrings As List(Of String) = (From p In Me.headerParams Select $"{p.Key}=""{p.Value}""").ToList()

        Dim authHeader As String = $"OAuth {String.Join(Of String)(", ", headerParamStrings)}"

        Return authHeader
    End Function
End Class