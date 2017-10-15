## A SDK for Cardmarket RESTful API v 2.0

*Copyright (c) 2017 Licensed under the MIT license (http://opensource.org/licenses/mit-license.php)*

## Usage

### Define yours 4 keys
```ruby
Const appToken = "your string"
Const appSecret = "your string"
Const accessToken = "your string"
Const accessSecret = "your string"
```

### Inizialize class
```ruby
  Dim mkm As New Mkm(appToken, appSecret, accessToken, accessSecret)
```

### Call method

**MarketPlace**



'''ruby
Games - Returns all games supported by MKM and you can sell and buy products for
Dim lg = mkm.Marketplace_Games
Dim IdGame = lg.game.Single(Function(f) f.abbreviation = "MtG").IdGame
'''

    
