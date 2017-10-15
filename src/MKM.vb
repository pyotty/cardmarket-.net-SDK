Partial Public Class Mkm

    Private _LimitCount As Integer
    Public ReadOnly Property LimitCount As Integer
        Get
            Return _LimitCount
        End Get
    End Property

    Private ReadOnly appToken As String
    Private ReadOnly appSecret As String
    Private ReadOnly accessToken As String
    Private ReadOnly accessSecret As String
    Private ReadOnly BaseUrl = "https://www.mkmapi.eu/ws/v2.0"


    Sub New(appToken As String, appSecret As String, accessToken As String, accessSecret As String, SandBox As Boolean)
        Me.appToken = appToken
        Me.appSecret = appSecret
        Me.accessToken = accessToken
        Me.accessSecret = accessSecret
        Me.BaseUrl = If(SandBox, "https://sandbox.mkmapi.eu/ws/v2.0/", "https://www.mkmapi.eu/ws/v2.0")
    End Sub

#Region "Funzioni"

#Region "   Marketplace"

    ''' <summary>
    ''' Returns all games supported by MKM and you can sell and buy products for.
    ''' </summary>
    ''' <returns></returns>
    Public Function Marketplace_Games() As Objects.RootGame
        Return ConsumeAPI(Of Objects.RootGame)(New Uri(BaseUrl, "games"), "GET")
    End Function

    ''' <summary>
    ''' Returns all expansions with single cards for the specified game.
    ''' </summary>
    ''' <param name="IdGame"></param>
    ''' <returns></returns>
    Public Function Marketplace_Expansions(IdGame As Integer) As Objects.RootExpansions
        Return ConsumeAPI(Of Objects.RootExpansions)(New Uri(BaseUrl, $"{IdGame}/expansions"), "GET")
    End Function

    ''' <summary>
    '''  Returns all single cards for the specified expansion.
    ''' </summary>
    ''' <param name="idExpansion"></param>
    ''' <returns></returns>
    Public Function Marketplace_ExpansionSingles(idExpansion As Integer) As Objects.RootExpansionSingle
        Return ConsumeAPI(Of Objects.RootExpansionSingle)(New Uri(BaseUrl, $"expansions/{idExpansion}/singles"), "GET")
    End Function

    ''' <summary>
    ''' Returns a product specified by its ID
    ''' </summary>
    ''' <param name="idProduct"></param>
    ''' <returns></returns>
    Public Function MarketPlace_Product(idProduct As Integer) As Objects.RootProduct
        Return ConsumeAPI(Of Objects.RootProduct)(New Uri(BaseUrl, $"products/{idProduct}"), "GET")
    End Function

    ''' <summary>
    '''  Returns a CSV file with all relevant products available at Cardmarket.
    ''' </summary>
    ''' <param name="path"></param>
    Public Sub MarketPlace_ProductList(path As String)
        Dim pge = ConsumeAPI(Of Objects.PriceGuideEntity)(New Uri(BaseUrl, $"productlist"), "GET")

        Dim b As Byte() = Convert.FromBase64String(pge.productsfile)

        Using m = New IO.MemoryStream(b)
            Using gz = New IO.Compression.GZipStream(m, IO.Compression.CompressionMode.Decompress)
                Using fs As New IO.FileStream(IO.Path.Combine(path, "PriceGuideEntity.csv"), IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                    gz.CopyTo(fs)
                End Using
            End Using
        End Using

    End Sub

    ''' <summary>
    ''' Searches for products by a given search string
    ''' </summary>
    ''' <param name="search"></param>
    ''' <param name="exact"></param>
    ''' <param name="idGame"></param>
    ''' <param name="idLanguage"></param>
    ''' <param name="start"></param>
    ''' <param name="maxResults"></param>
    ''' <returns></returns>
    Public Function MarketPlace_FindProducts(search As String,
                                             Optional exact As Boolean? = Nothing,
                                             Optional idGame As Integer? = Nothing,
                                             Optional idLanguage As Integer? = Nothing,
                                             Optional start As Integer? = Nothing,
                                             Optional maxResults As Integer? = Nothing) As Objects.Products

        Dim url As New Uri(BaseUrl, $"products/find?search={search}")
        '{"&exact={exact}&idGame={idGame}&idLanguage={idLanguage}&start={start}&maxResults={maxResults}")

        Dim uriBuilder As New UriBuilder(url)
        Dim query = Web.HttpUtility.ParseQueryString(uriBuilder.Query)
        If exact.HasValue Then query("exact") = exact.Value.ToString.ToLower
        If idGame.HasValue Then query("idGame") = idGame.Value
        If idLanguage.HasValue Then query("idLanguage") = idLanguage.Value
        If start.HasValue Then query("start") = start.Value
        If maxResults.HasValue Then query("maxResults") = maxResults.Value

        uriBuilder.Query = query.ToString
        url = uriBuilder.Uri

        Return ConsumeAPI(Of Objects.Products)(url, "GET")
    End Function

    ''' <summary>
    ''' Returns all available articles for a specified product. You can specify parameters for start and maximum results returned. If the response would have more than 1.000 entities a Temporary Redirect is returned. You can specify several filter parameters.
    ''' </summary>
    ''' <param name="idProduct"></param>
    ''' <param name="start"></param>
    ''' <param name="maxResults"></param>
    ''' <param name="userType"></param>
    ''' <param name="minUserScore"></param>
    ''' <param name="idLanguage"></param>
    ''' <param name="minCondition"></param>
    ''' <param name="isFoil"></param>
    ''' <param name="isSigned"></param>
    ''' <param name="isAltered"></param>
    ''' <param name="minAvailable"></param>
    ''' <returns></returns>
    Public Function MarketPlace_Articles(idProduct As Integer,
                                         Optional start As Integer? = Nothing,
                                         Optional maxResults As Integer? = Nothing,
                                         Optional userType As Enumerators.userType? = Nothing,
                                         Optional minUserScore As Enumerators.UserScore? = Nothing,
                                         Optional idLanguage As Enumerators.IdLanguage? = Nothing,
                                         Optional minCondition As Enumerators.Condition? = Nothing,
                                         Optional isFoil As Boolean? = Nothing, '(true|false)
                                         Optional isSigned As Boolean? = Nothing, '(true|false)
                                         Optional isAltered As Boolean? = Nothing, '(true|false)
                                         Optional minAvailable As Integer? = Nothing) As Objects.RootArticles

        Dim url As New Uri(BaseUrl, $"articles/{idProduct}")

        Dim uriBuilder As New UriBuilder(url)
        Dim query = Web.HttpUtility.ParseQueryString(uriBuilder.Query)
        If start.HasValue Then query("start") = start.Value.ToString
        If maxResults.HasValue Then query("maxResults") = maxResults.Value.ToString
        If userType.HasValue Then query("userType") = userType.Value.ToString
        If minUserScore.HasValue Then query("minUserScore") = minUserScore.Value
        If idLanguage.HasValue Then query("idLanguage") = idLanguage.Value
        If minCondition.HasValue Then query("minCondition") = minCondition.Value.ToString
        If isFoil.HasValue Then query("isFoil") = isFoil.Value.ToString.ToLower
        If isSigned.HasValue Then query("isSigned") = isSigned.Value.ToString.ToLower
        If isAltered.HasValue Then query("isAltered") = isAltered.Value.ToString.ToLower
        If minAvailable.HasValue Then query("minAvailable") = minAvailable.Value.ToString

        uriBuilder.Query = query.ToString
        url = uriBuilder.Uri

        Return ConsumeAPI(Of Objects.RootArticles)(url, "GET")

    End Function

    ''' <summary>
    ''' 	Returns the Metaproduct entity for the metaproduct specified by its ID.
    ''' </summary>
    ''' <returns></returns>
    Public Function MarketPlace_Metaproducts() As Object
        Throw New NotImplementedException
    End Function

    ''' <summary>
    ''' Searches for metaproducts and returns the Metaproduct entity of the metaproducts found. Search parameters are provided as query parameters and can be a search string (search) for the metaproduct's name; a flag exact indicating if the search string must exactly match the metaproduct's name, a parameter indicating the game (idGame); and a parameter indicating the language the search string is provided in (idLanguage).
    ''' </summary>
    ''' <returns></returns>
    Public Function MarketPlace_FindMetaproducts() As Object
        Throw New NotImplementedException
    End Function

    ''' <summary>
    ''' Returns the User entity for the user specified by its ID or exact name.
    ''' </summary>
    ''' <returns></returns>
    Public Function MarketPlace_Users() As Object
        Throw New NotImplementedException
    End Function

    ''' <summary>
    ''' Returns User entities for the users found. A search parameter (search) must be specified for the user's name.
    ''' </summary>
    ''' <returns></returns>
    Public Function MarketPlace_FindUsers() As Object
        Throw New NotImplementedException
    End Function

    ''' <summary>
    ''' Due to performance reasons responses with more than 1.000 entities are temporarily redirected to a request URI specifying a maximum of 1.000 entities returned (see Temporary Redirect for more information). Additional (optional) query parameters serve as paginating filters, where you can set the number of articles returned as well as a starting parameter (see Partial Responses for more information).
    ''' </summary>
    ''' <returns></returns>
    Public Function MarketPlace_UserArticles() As Object
        Throw New NotImplementedException
    End Function

#End Region

#Region "   Stock"

    ''' <summary>
    ''' Returns the Article entities in the authenticated user's stock.
    ''' </summary>
    ''' <returns></returns>
    Public Function Stock_List() As Objects.articles
        Return ConsumeAPI(Of Objects.articles)(New Uri(BaseUrl, "stock"), "GET")
    End Function

    ''' <summary>
    ''' Adds new articles to the user's stock
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns></returns>
    Public Function Stock_Add(l As List(Of Objects.article)) As Objects.inserted

        Dim a As New Objects.articles
        a.articles.AddRange(l)

        Const method = "POST"
        Dim url As New Uri(BaseUrl, "stock")

        Dim request = Net.WebRequest.CreateHttp(url)
        Dim header As New OAuthHeader(Me.appToken, Me.appSecret, Me.accessToken, Me.accessSecret)
        request.Headers.Add(Net.HttpRequestHeader.Authorization, header.getAuthorizationHeader(method, url))
        request.Method = method
        Net.ServicePointManager.Expect100Continue = False

        'Using streamWriter As New IO.StreamWriter(request.GetRequestStream())
        '    Dim jss As New Web.Script.Serialization.JavaScriptSerializer()
        '    Dim json = jss.Serialize(l)
        '    streamWriter.Write(json)
        '    streamWriter.Flush()
        '    streamWriter.Close()
        'End Using
        'Using response = request.GetResponse()
        '    Using streamReader As New IO.StreamReader(response.GetResponseStream())
        '        Dim result = streamReader.ReadToEnd()
        '        MsgBox (result)
        '    End Using
        'End Using


        Using textWriter As New Utf8StringWriter
            Dim ser As New Xml.Serialization.XmlSerializer(l.GetType())
            Dim ns As New Xml.Serialization.XmlSerializerNamespaces
            ns.Add("", "")
            ser.Serialize(textWriter, l, ns)
            Dim b = textWriter.ToString()
            Dim postBytes As Byte() = Text.Encoding.UTF8.GetBytes(b)
            Using dataStream As IO.Stream = request.GetRequestStream()
                dataStream.Write(postBytes, 0, postBytes.Length)
            End Using
        End Using


        Using response = TryCast(request.GetResponse(), Net.HttpWebResponse)
            Using s As IO.Stream = response.GetResponseStream()
                Using sr As New IO.StreamReader(s)
                    Dim jss As New Web.Script.Serialization.JavaScriptSerializer()
                    Dim c As String = sr.ReadToEnd()

                    Dim b = jss.Deserialize(Of Objects.inserted)(c)
                    Return b
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Changes articles in the user's stock.
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns></returns>
    Public Function Stock_Update(l As List(Of Objects.article)) As Objects.updated
        Dim a As New Objects.articles
        a.articles.AddRange(l)

        Const method = "PUT"
        Dim url As New Uri(BaseUrl, "stock")

        Dim request = Net.WebRequest.CreateHttp(url)
        Dim header As New OAuthHeader(Me.appToken, Me.appSecret, Me.accessToken, Me.accessSecret)
        request.Headers.Add(Net.HttpRequestHeader.Authorization, header.getAuthorizationHeader(method, url))
        request.Method = method
        Net.ServicePointManager.Expect100Continue = False

        'Using streamWriter As New IO.StreamWriter(request.GetRequestStream())
        '    Dim jss As New Web.Script.Serialization.JavaScriptSerializer()
        '    Dim json = jss.Serialize(l)
        '    streamWriter.Write(json)
        '    streamWriter.Flush()
        '    streamWriter.Close()
        'End Using
        'Using response = request.GetResponse()
        '    Using streamReader As New IO.StreamReader(response.GetResponseStream())
        '        Dim result = streamReader.ReadToEnd()
        '        MsgBox (result)
        '    End Using
        'End Using


        Using textWriter As New Utf8StringWriter
            Dim ser As New Xml.Serialization.XmlSerializer(l.GetType())
            Dim ns As New Xml.Serialization.XmlSerializerNamespaces
            ns.Add("", "")
            ser.Serialize(textWriter, l, ns)
            Dim b = textWriter.ToString()
            Dim postBytes As Byte() = Text.Encoding.UTF8.GetBytes(b)
            Using dataStream As IO.Stream = request.GetRequestStream()
                dataStream.Write(postBytes, 0, postBytes.Length)
            End Using
        End Using


        Using response = TryCast(request.GetResponse(), Net.HttpWebResponse)
            Using s As IO.Stream = response.GetResponseStream()
                Using sr As New IO.StreamReader(s)
                    Dim jss As New Web.Script.Serialization.JavaScriptSerializer()
                    Dim c As String = sr.ReadToEnd()
                    Dim b = jss.Deserialize(Of Objects.updated)(c)
                    Return b
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Removes articles from the user's stock.
    ''' </summary>
    ''' <param name="l"></param>
    ''' <returns></returns>
    Public Function Stock_Delete(l As List(Of Objects.article)) As Objects.deleted
        Dim a As New Objects.articles
        a.articles.AddRange(l)

        Const method = "DELETE"
        Dim url As New Uri(BaseUrl, "stock")

        Dim request = Net.WebRequest.CreateHttp(url)
        Dim header As New OAuthHeader(Me.appToken, Me.appSecret, Me.accessToken, Me.accessSecret)
        request.Headers.Add(Net.HttpRequestHeader.Authorization, header.getAuthorizationHeader(method, url))
        request.Method = method
        Net.ServicePointManager.Expect100Continue = False

        'Using streamWriter As New IO.StreamWriter(request.GetRequestStream())
        '    Dim jss As New Web.Script.Serialization.JavaScriptSerializer()
        '    Dim json = jss.Serialize(l)
        '    streamWriter.Write(json)
        '    streamWriter.Flush()
        '    streamWriter.Close()
        'End Using
        'Using response = request.GetResponse()
        '    Using streamReader As New IO.StreamReader(response.GetResponseStream())
        '        Dim result = streamReader.ReadToEnd()
        '        MsgBox (result)
        '    End Using
        'End Using


        Using textWriter As New Utf8StringWriter
            Dim ser As New Xml.Serialization.XmlSerializer(l.GetType())
            Dim ns As New Xml.Serialization.XmlSerializerNamespaces
            ns.Add("", "")
            ser.Serialize(textWriter, l, ns)
            Dim b = textWriter.ToString()
            Dim postBytes As Byte() = Text.Encoding.UTF8.GetBytes(b)
            Using dataStream As IO.Stream = request.GetRequestStream()
                dataStream.Write(postBytes, 0, postBytes.Length)
            End Using
        End Using


        Using response = TryCast(request.GetResponse(), Net.HttpWebResponse)
            Using s As IO.Stream = response.GetResponseStream()
                Using sr As New IO.StreamReader(s)
                    Dim jss As New Web.Script.Serialization.JavaScriptSerializer()
                    Dim c As String = sr.ReadToEnd()
                    Dim b = jss.Deserialize(Of Objects.deleted)(c)
                    Return b
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Searches for and returns articles specified by the article's name and game.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="IdGame"></param>
    ''' <returns></returns>
    Public Function Stock_FindArticle(name As String, IdGame As Integer) As Objects.RootArticles
        Return ConsumeAPI(Of Objects.RootArticles)(New Uri(BaseUrl, $"stock/articles/{name}/{IdGame}"), "GET")
    End Function

#End Region

#End Region


    Private Function ConsumeAPI(Of T)(url As Uri, method As String) As T

        Dim request = Net.WebRequest.CreateHttp(url)
        Dim header As New OAuthHeader(Me.appToken, Me.appSecret, Me.accessToken, Me.accessSecret)
        request.Headers.Add(Net.HttpRequestHeader.Authorization, header.getAuthorizationHeader(method, url))
        request.Method = method

        Using response = TryCast(request.GetResponse(), Net.HttpWebResponse)
            Using s As IO.Stream = response.GetResponseStream()
                Me._LimitCount = Convert.ToInt32(response.Headers.Item("X-Request-Limit-Count"))
                Using sr As New IO.StreamReader(s)
                    Dim jss As New Web.Script.Serialization.JavaScriptSerializer()
                    Dim a As String = sr.ReadToEnd()
                    Dim o = jss.Deserialize(Of T)(a)
                    Return o
                End Using
            End Using
        End Using

    End Function







End Class