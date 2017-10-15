'Imports System.Xml.Serialization
Partial Public Class Mkm

    Public Structure Objects

        Public Structure RootGame
            Public Property game As List(Of Game)
            Public Property links As List(Of Link)
        End Structure

        Public Structure Game
            Public Property IdGame As Integer
            Public Property name As String
            Public Property abbreviation As String
            Public Property links As List(Of Link)
        End Structure

        Public Structure RootExpansions
            Public Property expansion As List(Of Expansion)
        End Structure

        Public Structure Expansion
            Public Property idExpansion As Integer
            Public Property enName As String
            Public Property localization As List(Of localization)
            Public Property abbreviation As String
            Public Property icon As Integer
            Public Property releaseDate As Date
            Public Property isReleased As Boolean
            Public Property idGame As Integer
            Public Property links As List(Of Link)
            Public Property expansionIcon As Integer
        End Structure

        Public Structure RootExpansionSingle
            Public Property expansion As Expansion
            Public Property [single] As List(Of Product)
        End Structure

        <Xml.Serialization.XmlRoot("response")>
        Public Structure Products
            <Xml.Serialization.XmlElement("product")>
            Property product As List(Of Product)
        End Structure

        Public Structure RootProduct
            Public Property Product As Product
        End Structure

        Public Structure Product
            Property idProduct As Integer                           'Product ID
            Property idMetaproduct As Integer                       'Metaproduct ID
            Property countReprints As Integer                       'Number of similar products bundled by the metaproduct
            Property enName As String
            Property locName As String
            Property localization As List(Of localization)
            Property website As String                              'URL to the product (relative to MKM's base URL)
            Property image As String                                'Path to the product's image
            Property gameName As String
            Property categoryName As String
            Property idGame As Integer
            'Property number As string?                             ' era INteger? Number of product within the expansion (where applicable)
            Property rarity As String                               'Rarity of product (where applicable)
            Property expansionName As String
            Property expansionIcon As Integer
            Property countArticles As Integer?                      'Number of available articles of this product
            Property countFoils As Integer?                         'Number of available articles in foil of this products
            Property links As List(Of Link)
            'Property category As List(Of category)
            Property priceGuide As priceGuide                       'Price guide entity '''(ATTN: not returned for expansion requests)'''
            Property reprint As List(Of reprint)                    'Reprint entities for each similar product bundled by the metaproduct
            'Property expansion As expansion
        End Structure

        Public Structure productShort
            Property enName As String
            Property locName As String
            Property image As String
            Property expansion As String
            Property nr As Integer?
            Property expIcon As Integer?
            Property rarity As String
        End Structure

        <Serializable>
        <Xml.Serialization.XmlRoot("request")>
        Public Class articles
            '<Xml.Serialization.XmlElement("article")>
            Public Property articles As New List(Of article)
        End Class

        Public Structure RootArticles
            Public Property article As List(Of article)
            'public links As List(Of link)
        End Structure

        <Serializable>
        Public Structure article
            '<XmlIgnore, Web.Script.Serialization.ScriptIgnore> Property idArticle As Integer                          'Article ID
            Property idArticle As Integer                          'Article ID
            Property idProduct As Integer                          'Product ID
            Property idLanguage As Integer?
            '<Xml.Serialization.XmlIgnore, Web.Script.Serialization.ScriptIgnore> Property language As language                         'Language entity
            Property language As language                         'Language entity
            Property comments As String                           'Comments
            Property price As Decimal                    'Price of the article
            Property count As Integer                          'Count (see notes)
            ''<Xml.Serialization.XmlIgnore, Web.Script.Serialization.ScriptIgnore> Property inShoppingCart As Boolean                    'Flag, if that article is currently in a shopping cart
            Property inShoppingCart As Boolean

            '<Xml.Serialization.XmlIgnore, Web.Script.Serialization.ScriptIgnore> Property product As productShort
            '<Xml.Serialization.XmlIgnore, Web.Script.Serialization.ScriptIgnore> Property seller As user                          'Seller's user entity
            Property seller As user                          'Seller's user entity
            ''<Xml.Serialization.XmlIgnore, Web.Script.Serialization.ScriptIgnore> Property lastEdited As Date                        'Date, the article was last updated
            Property condition As String                        'Product's condition, if applicable
            Property isFoil As Boolean                             'Foil flag, if applicable
            Property isSigned As Boolean                           'Signed flag, if applicable
            Property isAltered As Boolean                           'Altered flag, if applicable
            Property isPlayset As Boolean                          'Playset flaf, if applicable
            '<Xml.Serialization.XmlIgnore, Web.Script.Serialization.ScriptIgnore> Private isFirstEd As Boolean                          'First edition flag, if applicable
            '<Xml.Serialization.XmlIgnore, Web.Script.Serialization.ScriptIgnore> Property links As List(Of link)             'HATEOAS links
            Property links As List(Of Link)             'HATEOAS links
        End Structure

        Public Structure user
            Property idUser As Integer                  'user ID
            Property username As String                 'username
            Property registrationDate As Date           'date of registration
            Property isCommercial As Enumerators.IsCommercial
            Property isSeller As Boolean                'indicates if the user can sell; true|false
            Property name As name                       'name entity
            Property address As address                 'Address entity
            Property phone As String                    'phone number; only returned for commercial users
            Property email As String                    'email address; only returned for commercial users
            Property vat As String                      'tax number; only returned for commercial users
            Property riskGroup As Enumerators.riskGroup
            Property reputation As Enumerators.Reputation
            Property shipsFast As Enumerators.ShipsFast
            Property sellCount As Integer?              'number of sales
            Property soldItems As Integer?              'total number of sold items
            Property avgShippingTime As Integer?        'average shipping time
            Property onVacation As Boolean              'true|false
            Property links As List(Of Link)             'HATEOAS links
        End Structure


#Region "Crud"

        Public Structure inserted
            '{"inserted":[{"success":false,"tried":{"idArticle":"0","idProduct":"1","idLanguage":"5","comments":"test2","price":"3","count":"0","condition":"NM","isFoil":"false","isSigned":"false","isAltered":"false","isPlayset":"false","amount":"0"},"error":"You are not authorized to execute this action"}]}
            '{"inserted":[{"success":true,"idArticle":{"idArticle":285182808,"idProduct":1,"language":{"idLanguage":5,"languageName":"Italian"},"comments":"test2","price":3,"count":1,"inShoppingCart":false,"product":{"idGame":1,"enName":"Altar\u0027s Light","locName":"Luce dell\u0027Altare","image":".\/img\/cards\/Mirrodin\/altars_light.jpg","expansion":"Mirrodin","nr":"1","expIcon":"43","rarity":"Uncommon"},"lastEdited":"2017-02-25T18:37:27+0100","condition":"NM","isFoil":false,"isSigned":false,"isPlayset":false,"isAltered":false}}]}
            Property inserted As List(Of insert)
        End Structure

        Public Structure insert
            Property success As Boolean
            Property tried As article
            Property IdArticle As article
            Property [error] As String
        End Structure

        Public Structure updated
            '{"updatedArticles":[],"notUpdatedArticles":[{"success":false,"tried":{"idArticle":"285184665","idProduct":"0","idLanguage":"5","comments":"test2","price":"2","condition":"NM","isFoil":false,"isSigned":false,"isAltered":false,"isPlayset":false}}]}
            Property updatedArticles As List(Of article)
            Property notUpdatedArticles As List(Of article)
        End Structure
        'Public Structure update
        '    Property success As Boolean
        '    Property tried As article
        '    Property IdArticle As article
        '    Property [error] As String
        'End Structure
        Public Structure deleted
            '{"deleted":[{"idArticle":"285159139","count":"1","success":false,"message":"Invalid Article"}]}
            Property deleted As List(Of delete)
        End Structure

        Public Structure delete
            Property idArticle As Integer
            Property count As Integer
            Property success As Boolean
            Property message As String
        End Structure

#End Region

        Public Structure Link
            Public Property rel As String
            Public Property href As String
            Public Property method As String
        End Structure

        Public Structure localization
            Public Property name As String
            Public Property idLanguage As Integer
            Public Property languageName As String
        End Structure

        Public Structure category
            Public Property idCategory As Integer
            Public Property categoryName As String
        End Structure

        Public Structure priceGuide
            Public Property SELL As Decimal         'Average price of articles ever sold of this product
            Public Property LOW As Decimal          'Current lowest non-foil price (all conditions)
            Public Property LOWEX As Decimal        'Current lowest non-foil price (condition EX And better)
            Public Property LOWFOIL As Decimal      'Current lowest foil price
            Public Property AVG As Decimal          'Current average non-foil price of all available articles of this product
            Public Property TREND As Decimal        'Current trend price
        End Structure

        Public Structure reprint
            Public Property idProduct As Integer    'Product ID
            Public Property expansion As String     'Expansion's name
            Public Property expIcon As Integer      'Index of the expansion icon
        End Structure

        Public Structure language                   'Language entity
            Property idLanguage As Integer          'Language ID
            Property languageName As String         'Language's name in English
        End Structure

        Public Structure name
            Property company As String              'company name; only returned for commercial users
            Property firstName As String            'first name
            Property lastName As String             'last name; only returned for commercial users
        End Structure

        Public Structure address
            Property name As String                   'receiver
            Property extra As String                  'extra information
            Property street As String                 'street
            Property zip As String                    'zip
            Property city As String                   'city
            Property country As String                'country
        End Structure

        Public Structure PriceGuideEntity
            Property productsfile As String
            Property mime As String
            Property links As List(Of Link)
        End Structure

    End Structure

End Class