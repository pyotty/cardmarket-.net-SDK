Partial Public Class Mkm

    Public Structure Enumerators

        Public Enum Condition
            MT ' Mint
            NM ' Near Mint (default value when not passed)
            EX ' Excellent
            GD ' Good
            LP ' Light-played
            PL ' Played
            PO ' Poor
        End Enum

        Public Enum IdLanguage
            English = 1
            French = 2
            German = 3
            Spanish = 4
            Italian = 5
            Simplified_Chinese = 6
            Japanese = 7
            Portuguese = 8
            Russian = 9
            Korean = 10
            Traditional_Chinese = 11
        End Enum

        Public Enum IsCommercial
            private_user = 0
            commercial_user = 1
            powerseller = 2
        End Enum

        Public Enum userType
            [private] 'for private sellers only
            commercial 'for all commercial sellers including powersellers
            powerseller 'for powersellers only
        End Enum

        Public Enum riskGroup
            no_risk = 0
            low_risk = 1
            high_risk = 2
        End Enum

        Public Enum Reputation
            not_enough_sells_to_rate = 0
            outstanding_seller = 1
            very_good_seller = 2
            good_seller = 3
            average_seller = 4
            bad_seller = 5
        End Enum

        Public Enum ShipsFast
            normal_shipping_speed = 0
            ships_very_fast = 1
            ships_fast = 2
        End Enum

        Public Enum UserScore
            Outstanding = 1
            VeryGood = 2
            Good = 3
            Average = 4
            Bad = 5
        End Enum
    End Structure

End Class