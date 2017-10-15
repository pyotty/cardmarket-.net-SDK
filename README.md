A SDK for Cardmarket RESTful API v 2.0

Copyright (c) 2017 Licensed under the MIT license (http://opensource.org/licenses/mit-license.php)

*** Usage *** 
  Const appToken = "your string"
  Const appSecret = "your string"
  Const accessToken = "your string"
  Const accessSecret = "your string"
  
  Dim mkm As New Mkm(appToken, appSecret, accessToken, accessSecret)

  *******************
  *** MarketPlace ***
  *******************

    Games - Returns all games supported by MKM and you can sell and buy products for.
    Dim lg = mkm.Marketplace_Games
    Dim IdGame = lg.game.Single(Function(f) f.abbreviation = "MtG").IdGame

    Expansions - Returns all expansions with single cards for the specified game.
    Dim le = mkm.Marketplace_Expansions(IdGame)
    Dim IdExp = le.expansion.Single(Function(f) f.enName = "Onslaught").idExpansion

    Expansion Singles - Returns all single cards for the specified expansion.
    Dim ls = mkm.Marketplace_ExpansionSingles(IdExp)
    Dim IdProduct = ls.single.First(Function(f) f.enName = "Wellwisher").idProduct

    Products - Returns a product specified by its ID
    Dim p = mkm.MarketPlace_Product(IdProduct)

    Product List (File) - Returns a CSV file with all relevant products available at Cardmarket.
    mkm.MarketPlace_ProductList("C:\temp\")

    Price Guides (File)
    NotImplemented

    Find Products - Searches for products by a given search string
    Dim lp = mkm.MarketPlace_FindProducts("swamp", False, IdGame, Mkm.Enumerators.IdLanguage.English)

    Articles - Returns all available articles for a specified product.
    Dim la = mkm.MarketPlace_Articles(IdProduct)

    Metaproducts - Returns the Metaproduct entity for the metaproduct specified by its ID.
    NotImplemented

    Find Metaproducts - Searches for metaproducts and returns the Metaproduct entity of the metaproducts found.
    NotImplemented

    Users - Returns the User entity for the user specified by its ID or exact name.
    NotImplemented

    Find Users - Returns User entities for the users found.
    NotImplemented

    Article Users - 
    NotImplemented

  *** MarketPlace ***

  *************
  *** Stock ***
  *************

    GET - 	Returns the Article entities in the authenticated user's stock.
    Dim lsa1 = mkm.Stock_List()

    POST - Adds new articles to the user's stock.
    Dim l1 As New List(Of Mkm.Objects.article)
    Dim a1 As New Mkm.Objects.article With {
        .idProduct = p.Product.idProduct,
        .idLanguage = Mkm.Enumerators.IdLanguage.Italian,
        .count = 1,
        .price = 100,
        .condition = Mkm.Enumerators.Condition.NM.ToString()
    }
    l1.Add(a1)
    Dim li1 As Mkm.Objects.inserted = mkm.Stock_Add(l1)
    Dim test1 = li1.inserted.First
    Dim r2 = test1.IdArticle

    PUT - Changes articles in the user's stock.
    Dim l2 As New List(Of Mkm.Objects.article)
    Dim a2 As New Mkm.Objects.article With {
        .idArticle = r2.idArticle,
        .idLanguage = Mkm.Enumerators.IdLanguage.Italian,
        .count = 2,
        .price = 100,
        .condition = Mkm.Enumerators.Condition.NM.ToString()
    }
    l2.Add(a2)
    Dim li2 As Mkm.Objects.updated = mkm.Stock_Update(l2)

    DELETE - Removes articles from the user's stock
    Dim l3 As New List(Of Mkm.Objects.article)
    Dim a3 As New Mkm.Objects.article With {
        .idArticle = r2.idArticle,
        .count = 1
    }
    l3.Add(a3)
    Dim li3 = mkm.Stock_Delete(l3)

    Stock (File) - Returns a CSV file with all articles in the authenticated user's stock, further specified by a game and language.
    NotImplemented

    Stock in Shopping Carts - Returns the Article entities of the authenticated user's stock that are currently in other user's shopping carts.
    NotImplemented

    Stock Article - Returns a single Article entity in the authenticated user's stock specified by its article ID.
    NotImplemented

    Change Stock Article Quantity - Changes quantities for articles in authenticated user's stock
    NotImplemented

    Find Stock Articles - Searches for and returns articles specified by the article's name and game.
    Dim lsa2 = mkm.Stock_FindArticle("jace", IdGame)

  *** Stock ***
