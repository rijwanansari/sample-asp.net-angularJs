Imports sampleh4f.DAL
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Device.Location

Public Class StoreInfoServices
    Dim googleKey As String = "" ''AppSettings("GoogleAPIKey")

    Private dtStoreInfoG As New DataTable()


    Public Function GetActiveStoreInfoList() As List(Of StoreInfo)
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Stores = Stores.Where(Function(a) a.Active_Ind = "Y")
            Return Stores.ToList()
        End Using
    End Function

    Public Function GetAllActiveStore() As DataTable
        Dim distance As Double = 0
        Dim dtStoreInfo As New DataTable()
        dtStoreInfo.Columns.AddRange(New DataColumn(26) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)), New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)), New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)), New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)), New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)), New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)), New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)), New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)), New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)), New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)), New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)), New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)), New DataColumn("strLogoImage", GetType(String)), New DataColumn("HasIndividualWebsite", GetType(Boolean)), New DataColumn("IndividualWebsite", GetType(String))})
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Stores = Stores.Where(Function(a) a.Active_Ind = "Y")
            For Each store As StoreInfo In Stores
                dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh, store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance, store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo, store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage, store.HasIndividualWebsite, store.IndividualWebsite)
            Next
            Return dtStoreInfo
        End Using
    End Function
    Public Function GetCoordinateFromAddres(ByVal strAddress As String) As GeoCoordinate
        Dim lat As Double = 0
        Dim lng As Double = 0
        Dim retCoordinate As GeoCoordinate
        Dim url As String = "https://maps.google.com/maps/api/geocode/xml?address=" + strAddress + "&sensor=false&key=" + googleKey

        Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        Dim responseWeb As WebResponse = request.GetResponse()
        Dim dataStream = responseWeb.GetResponseStream()
        Dim sreader As StreamReader = New StreamReader(dataStream)
        Dim responsereader As String = sreader.ReadToEnd()
        responseWeb.Close()

        Dim dsResult As New DataSet()
        Dim tempsr = New StringReader(responsereader)
        Dim tempxtr = New XmlTextReader(tempsr)
        dsResult.ReadXml(tempxtr)
        Dim dtCoordinates As New DataTable()
        dtCoordinates.Columns.AddRange(New DataColumn(3) {New DataColumn("Id", GetType(Integer)), New DataColumn("Address", GetType(String)), New DataColumn("Latitude", GetType(String)), New DataColumn("Longitude", GetType(String))})
        For Each row As DataRow In dsResult.Tables("result").Rows
            Dim geometry_id As String = dsResult.Tables("geometry").[Select]("result_id = " + row("result_id").ToString())(0)("geometry_id").ToString()
            Dim location As DataRow = dsResult.Tables("location").[Select](Convert.ToString("geometry_id = ") & geometry_id)(0)
            dtCoordinates.Rows.Add(row("result_id"), row("formatted_address"), location("lat"), location("lng"))
            strAddress = row("formatted_address").ToString
            lat = Convert.ToDouble(location("lat"))
            lng = Convert.ToDouble(location("lng"))
            Exit For
        Next

        'Dim request As WebRequest = WebRequest.Create(url)
        'Using response As WebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        '    Using reader As New StreamReader(response.GetResponseStream(), Encoding.UTF8)
        '        Dim dsResult As New DataSet()
        '        dsResult.ReadXml(reader)
        '        Dim dtCoordinates As New DataTable()
        '        dtCoordinates.Columns.AddRange(New DataColumn(3) {New DataColumn("Id", GetType(Integer)), New DataColumn("Address", GetType(String)), New DataColumn("Latitude", GetType(String)), New DataColumn("Longitude", GetType(String))})
        '        For Each row As DataRow In dsResult.Tables("result").Rows
        '            Dim geometry_id As String = dsResult.Tables("geometry").[Select]("result_id = " + row("result_id").ToString())(0)("geometry_id").ToString()
        '            Dim location As DataRow = dsResult.Tables("location").[Select](Convert.ToString("geometry_id = ") & geometry_id)(0)
        '            dtCoordinates.Rows.Add(row("result_id"), row("formatted_address"), location("lat"), location("lng"))
        '            strAddress = row("formatted_address").ToString
        '            lat = Convert.ToDouble(location("lat"))
        '            lng = Convert.ToDouble(location("lng"))
        '            Exit For
        '        Next
        '    End Using
        'End Using
        retCoordinate = New GeoCoordinate(lat, lng)
        Return retCoordinate
    End Function
    Public Function GetStoreListByDistance(ByVal decDistance As Double, ByVal strSearchAddress As String) As DataTable
        Dim dtStoreInfo As New DataTable()
        Try
            Dim sCordinate As GeoCoordinate = GetCoordinateFromAddres(strSearchAddress)
            Dim dCordinate As GeoCoordinate
            Dim distance As Double = 0
            Dim DeliveryDistance As Double = 0
            CreateStoreDataTable()
            dtStoreInfo = dtStoreInfoG
            Using _dbContext As New Hungry4FoodDataContext
                Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
                Stores = Stores.Where(Function(a) a.Active_Ind = "Y")
                For Each store As StoreInfo In Stores
                    dCordinate = GetCoordinateFromAddres(store.StoreStrtAdd + ", " + store.StoreCity + ", " + store.StoreState + ", " + store.StoreZipCd)
                    DeliveryDistance = store.DeliveryDistance
                    distance = sCordinate.GetDistanceTo(dCordinate)
                    If distance <= decDistance * 1609.34 Then
                        dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh, store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance,
                                             store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo, store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage, store.HasIndividualWebsite, store.IndividualWebsite)
                    End If
                Next
            End Using
        Catch ex As Exception
            dtStoreInfo = Nothing
        End Try
        Return dtStoreInfo
    End Function

    ''Sorting Datatable
    Public Function SortDataTable(ByVal dtIn As DataTable, ByVal strColName As String, ByVal strDirection As String) As DataTable
        Dim dtOut As DataTable = Nothing
        dtIn.DefaultView.Sort = strColName + " " + strDirection
        dtOut = dtIn.DefaultView.Table()
        Return dtOut
    End Function

    Public Function GetOnlineOrderingStore(ByVal decDistance As Double, ByVal strSearchAddress As String) As DataTable
        Dim sCordinate As GeoCoordinate = GetCoordinateFromAddres(strSearchAddress)
        Dim dCordinate As GeoCoordinate
        Dim distance As Double = 0
        Dim DeliveryDistance As Double = 0
        Dim dtStoreInfo As New DataTable()
        dtStoreInfo.Columns.AddRange(New DataColumn(24) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)), New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)), New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)), New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)), New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)), New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)), New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)), New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)), New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)), New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)), New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)), New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)), New DataColumn("strLogoImage", GetType(String))})
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Stores = Stores.Where(Function(a) a.IsOnlineOrder = "Y")
            For Each store As StoreInfo In Stores
                dCordinate = GetCoordinateFromAddres(store.StoreStrtAdd + ", " + store.StoreCity + ", " + store.StoreState + ", " + store.StoreZipCd)
                DeliveryDistance = store.DeliveryDistance
                distance = sCordinate.GetDistanceTo(dCordinate)
                If distance < decDistance * 1609.34 Then
                    dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh, store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance, store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo, store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage)
                End If
            Next
        End Using
        Return dtStoreInfo
    End Function

    Public Function GetPhoneOrderingStore(ByVal decDistance As Double, ByVal strSearchAddress As String) As DataTable
        Dim sCordinate As GeoCoordinate = GetCoordinateFromAddres(strSearchAddress)
        Dim dCordinate As GeoCoordinate
        Dim distance As Double = 0
        Dim DeliveryDistance As Double = 0
        Dim dtStoreInfo As New DataTable()
        dtStoreInfo.Columns.AddRange(New DataColumn(24) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)), New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)), New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)), New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)), New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)), New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)), New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)), New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)), New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)), New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)), New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)), New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)), New DataColumn("strLogoImage", GetType(String))})
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Stores = Stores.Where(Function(a) a.IsOnlineOrder = "N")
            For Each store As StoreInfo In Stores
                dCordinate = GetCoordinateFromAddres(store.StoreStrtAdd + ", " + store.StoreCity + ", " + store.StoreState + ", " + store.StoreZipCd)
                DeliveryDistance = store.DeliveryDistance
                distance = sCordinate.GetDistanceTo(dCordinate)
                If distance < decDistance * 1609.34 Then
                    dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh, store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance, store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo, store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage)
                End If
            Next
        End Using
        Return dtStoreInfo
    End Function

    'satya for Price 
    Public Function GetStoreByPriceRange(ByVal decDistance As Double, ByVal strSearchAddress As String, ByVal strPrice As String) As DataTable
        Dim sCordinate As GeoCoordinate = GetCoordinateFromAddres(strSearchAddress)
        Dim dCordinate As GeoCoordinate
        Dim distance As Double = 0
        Dim DeliveryDistance As Double = 0
        Dim dtStoreInfo As New DataTable()
        dtStoreInfo.Columns.AddRange(New DataColumn(24) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)), New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)), New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)), New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)), New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)), New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)), New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)), New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)), New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)), New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)), New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)), New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)), New DataColumn("strLogoImage", GetType(String))})
        Try
            Using _dbContext As New Hungry4FoodDataContext
                Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
                Stores = Stores.Where(Function(a) a.PriceRating.Length <= strPrice.Length)
                For Each store As StoreInfo In Stores
                    dCordinate = GetCoordinateFromAddres(store.StoreStrtAdd + ", " + store.StoreCity + ", " + store.StoreState + ", " + store.StoreZipCd)
                    DeliveryDistance = store.DeliveryDistance
                    distance = sCordinate.GetDistanceTo(dCordinate)
                    If distance < decDistance * 1609.34 Then
                        dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh, store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance, store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo, store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage)
                    End If
                Next
            End Using
        Catch ex As Exception
        End Try

        Return dtStoreInfo
    End Function
    Public Function GetStoreByCuisine(ByVal decDistance As Double, ByVal strSearchAddress As String, ByVal strCuisine As String) As DataTable
        Dim sCordinate As GeoCoordinate = GetCoordinateFromAddres(strSearchAddress)
        Dim dCordinate As GeoCoordinate
        Dim distance As Double = 0
        Dim DeliveryDistance As Double = 0
        Dim dtStoreInfo As New DataTable()
        CreateStoreDataTable()
        dtStoreInfo = dtStoreInfoG
        ''dtStoreInfo.Columns.AddRange(New DataColumn(24) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)), New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)), New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)), New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)), New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)), New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)), New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)), New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)), New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)), New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)), New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)), New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)), New DataColumn("strLogoImage", GetType(String))})
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Dim StoreFilter As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Dim cuisines As String() = strCuisine.Split({","c})
            For Each cuisine As String In cuisines


                StoreFilter = Stores.Where(Function(a) a.Cuisines.ToUpper.Contains(cuisine.ToUpper))
                For Each store As StoreInfo In StoreFilter
                    dCordinate = GetCoordinateFromAddres(store.StoreStrtAdd + ", " + store.StoreCity + ", " + store.StoreState + ", " + store.StoreZipCd)
                    DeliveryDistance = store.DeliveryDistance
                    distance = sCordinate.GetDistanceTo(dCordinate)
                    If distance < decDistance * 1609.34 Then
                        dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh,
                                             store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance,
                                             store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo,
                                             store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage, store.HasIndividualWebsite, store.IndividualWebsite)
                    End If
                Next
            Next
        End Using
        dtStoreInfo = dtStoreInfo.DefaultView.ToTable(True)
        Return dtStoreInfo

    End Function
    Public Function GetStoreSortingByPriceRating(ByVal decDistance As Double, ByVal strSearchAddress As String) As DataTable
        Dim sCordinate As GeoCoordinate = GetCoordinateFromAddres(strSearchAddress)
        Dim dCordinate As GeoCoordinate
        Dim distance As Double = 0
        Dim DeliveryDistance As Double = 0
        Dim dtStoreInfo As New DataTable()
        dtStoreInfo.Columns.AddRange(New DataColumn(24) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)), New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)), New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)), New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)), New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)), New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)), New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)), New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)), New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)), New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)), New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)), New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)), New DataColumn("strLogoImage", GetType(String))})
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Stores = Stores.Where(Function(a) a.Active_Ind = "Y")
            Stores = Stores.OrderBy(Function(a) a.PriceRating)
            For Each store As StoreInfo In Stores
                dCordinate = GetCoordinateFromAddres(store.StoreStrtAdd + ", " + store.StoreCity + ", " + store.StoreState + ", " + store.StoreZipCd)
                DeliveryDistance = store.DeliveryDistance
                distance = sCordinate.GetDistanceTo(dCordinate)
                If distance <= decDistance * 1609.34 Then
                    dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh, store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance, store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo, store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage)
                End If
            Next
        End Using
        Return dtStoreInfo
    End Function
    Public Function GetStoreSortingByTopRating(ByVal decDistance As Double, ByVal strSearchAddress As String) As DataTable
        Dim sCordinate As GeoCoordinate = GetCoordinateFromAddres(strSearchAddress)
        Dim dCordinate As GeoCoordinate
        Dim distance As Double = 0
        Dim DeliveryDistance As Double = 0
        Dim dtStoreInfo As New DataTable()
        dtStoreInfo.Columns.AddRange(New DataColumn(24) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)), New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)), New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)), New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)), New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)), New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)), New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)), New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)), New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)), New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)), New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)), New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)), New DataColumn("strLogoImage", GetType(String))})
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Stores = Stores.Where(Function(a) a.Active_Ind = "Y")
            Stores = Stores.OrderBy(Function(a) a.Rating)
            For Each store As StoreInfo In Stores
                dCordinate = GetCoordinateFromAddres(store.StoreStrtAdd + ", " + store.StoreCity + ", " + store.StoreState + ", " + store.StoreZipCd)
                DeliveryDistance = store.DeliveryDistance
                distance = sCordinate.GetDistanceTo(dCordinate)
                If distance <= decDistance * 1609.34 Then
                    dtStoreInfo.Rows.Add(store.StoreID, store.StoreName, store.StoreStrtAdd, store.StoreCity, store.StoreState, store.StoreZipCd, store.StorePh, store.Rating, store.Reviews, store.TimeZone_Offset, store.IsOnlineOrder, store.IsDelivery, store.DeliveryDistance, store.DlvryRate, store.DlvryMinOrderAmt, store.DlvryEstTime, store.CouponCnt, store.CashCouponPercent, store.CashCouponInfo, store.Cuisines, store.PriceRating, store.PopularDishes, store.MoreInfomation, distance, store.StoreLogoImage)
                End If
            Next
        End Using
        Return dtStoreInfo
    End Function


    Public Function GetStoreName(ByVal intStoreID As Integer) As String
        Dim strRet As String = String.Empty
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            strRet = Stores.Select(Function(a) a.StoreName).SingleOrDefault()
            Return strRet
        End Using
    End Function
    Public Function GetTimeZoneUpset(ByVal intStoreID As Integer) As String
        Dim intRet As Decimal = 0
        Try
            Using _dbContext As New Hungry4FoodDataContext
                Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
                intRet = Stores.Select(Function(a) a.TimeZone_Offset).SingleOrDefault()
                Return intRet
            End Using
        Catch ex As Exception
            Return intRet
        End Try
    End Function

    Public Function GetStoreEmail(ByVal intStoreID As Integer) As String
        Dim strRet As String = String.Empty
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            strRet = Stores.Select(Function(a) a.Email).SingleOrDefault()
            Return strRet
        End Using
        Return strRet
    End Function

    Public Function IsOnlineOrderStore(ByVal intStoreID As Integer) As Boolean
        Dim blnRet As Boolean = True
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            Dim val = Stores.Select(Function(a) a.IsOnlineOrder).SingleOrDefault()
            If val.ToString.ToUpper <> "Y" Then
                blnRet = False
            End If
            Return blnRet
        End Using
        Return blnRet
    End Function

    Public Function IsSingleStoreInfo() As Boolean
        Dim blnRet As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.Active_Ind.ToString.ToUpper.Contains("Y"))
            If Stores.Count = 1 Then
                blnRet = True
            End If
            Return blnRet
        End Using
        Return blnRet
    End Function
    Public Function GetSingleStoreID() As String
        Dim strRet As String = String.Empty
        Using _dbContext As New Hungry4FoodDataContext
            Dim Store As StoreInfo = _dbContext.StoreInfos.Where(Function(a) a.Active_Ind.ToString.ToUpper.Contains("Y")).Take(1).SingleOrDefault()
            strRet = Store.StoreID
            Return strRet
        End Using
        Return strRet
    End Function

    Public Function GetStorePhone(ByVal intStoreID As Integer) As String
        Dim strRet As String = String.Empty
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            strRet = Stores.Select(Function(a) a.StorePh).SingleOrDefault()
            Return strRet
        End Using
        Return strRet
    End Function

    Public Sub CreateStoreDataTable()
        dtStoreInfoG.Columns.AddRange(New DataColumn(26) {New DataColumn("intStoreID", GetType(Integer)), New DataColumn("strStoreName", GetType(String)),
                                      New DataColumn("strStreetAdd", GetType(String)), New DataColumn("strCity", GetType(String)),
                                      New DataColumn("strStoreState", GetType(String)), New DataColumn("strZipCd", GetType(String)),
                                      New DataColumn("strPhone", GetType(String)), New DataColumn("DecRating", GetType(Decimal)),
                                      New DataColumn("intReviews", GetType(Integer)), New DataColumn("decTimeZoneOffSet", GetType(Decimal)),
                                      New DataColumn("IsOnlineOrder", GetType(String)), New DataColumn("IsDelivery", GetType(String)),
                                      New DataColumn("decDeliveryDistance", GetType(Decimal)), New DataColumn("strDeliveryRate", GetType(String)),
                                      New DataColumn("decMinDeliveryAmount", GetType(Decimal)), New DataColumn("DeliveryEstTime", GetType(String)),
                                      New DataColumn("intCouponCount", GetType(Integer)), New DataColumn("decCashCouponPercent", GetType(Decimal)),
                                      New DataColumn("strCashCouponInfo", GetType(String)), New DataColumn("strCuisines", GetType(String)),
                                      New DataColumn("strPriceRating", GetType(String)), New DataColumn("strPopularDishes", GetType(String)),
                                      New DataColumn("strMoreInformation", GetType(String)), New DataColumn("calDistance", GetType(String)),
                                      New DataColumn("strLogoImage", GetType(String)), New DataColumn("HasIndividualWebsite", GetType(Boolean)),
                                      New DataColumn("IndividualWebsite", GetType(String))})

    End Sub

#Region "satya for admin"
    'satya for
    Public Function GetAllStoreInfoList() As List(Of StoreInfo)
        Using _dbContext As New Hungry4FoodDataContext
            Dim StoreInfos As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            ' StoreInfos = StoreInfos.Select(Function(p) p.StoreName).SingleOrDefault
            _dbContext.DeferredLoadingEnabled = False
            Return StoreInfos.ToList()
        End Using
    End Function

    Public Function GetStoreInfoListByStoreID(ByVal lstStoreID As List(Of Long)) As List(Of StoreInfo)
        Dim lstStoreInfo As New List(Of StoreInfo) '= Nothing
        Using _dbContext As New Hungry4FoodDataContext
            For Each storeid As Long In lstStoreID
                Dim StoreInfo As StoreInfo = _dbContext.StoreInfos.Where(Function(a) a.StoreID = storeid).SingleOrDefault()
                lstStoreInfo.Add(StoreInfo)
            Next
            Return lstStoreInfo
        End Using
    End Function

    'StoreInfo Insert
    Public Function InsertStoreInfo(ByVal objStoreInfo As StoreInfo) As Boolean
        Dim blnRslt As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            _dbContext.StoreInfos.InsertOnSubmit(objStoreInfo)
            _dbContext.SubmitChanges()
            blnRslt = True

        End Using
        Return blnRslt
    End Function
    Public Function GetStoreID() As Long
        Dim intRet As Long = 0
        Using _dbContext As New Hungry4FoodDataContext
            Dim StoreIds As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            If StoreIds.Count > 0 Then
                intRet = StoreIds.Max(Function(p) p.StoreID)
            End If
        End Using
        Return intRet
    End Function
    'for Update
    Public Function UpdateStoreInfo(ByVal intStoreID As Integer, ByVal objStoreInfo As StoreInfo) As Boolean
        Dim blnRet As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim ItemStoreInfos As IQueryable(Of StoreInfo) = _dbContext.StoreInfos
            Dim results = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID).Single()
            results.BusinessName = objStoreInfo.BusinessName
            results.StoreName = objStoreInfo.StoreName
            results.StoreLogoImage = objStoreInfo.StoreLogoImage
            results.StorePh = objStoreInfo.StorePh
            results.StoreStrtAdd = objStoreInfo.StoreStrtAdd
            results.StoreCity = objStoreInfo.StoreCity
            results.StoreState = objStoreInfo.StoreState
            results.StoreZipCd = objStoreInfo.StoreZipCd
            results.Fax = objStoreInfo.Fax
            results.FaxUserID = objStoreInfo.FaxUserID
            results.FaxPassword = objStoreInfo.FaxPassword
            results.IsDelivery = objStoreInfo.IsDelivery
            results.IsOnlineOrder = objStoreInfo.IsOnlineOrder

            results.DlvryRate = objStoreInfo.DlvryRate
            results.DlvryEstTime = objStoreInfo.DlvryEstTime
            results.DlvryMinOrderAmt = objStoreInfo.DlvryMinOrderAmt
            results.DeliveryDistance = objStoreInfo.DeliveryDistance
            results.TimeZone_Offset = objStoreInfo.TimeZone_Offset
            results.CashCouponInfo = objStoreInfo.CashCouponInfo
            results.CashCouponPercent = objStoreInfo.CashCouponPercent
            results.CouponCnt = objStoreInfo.CouponCnt
            results.Cuisines = objStoreInfo.Cuisines
            results.PopularDishes = objStoreInfo.PopularDishes
            results.PriceRating = objStoreInfo.PriceRating
            results.Reviews = objStoreInfo.Reviews
            results.Rating = objStoreInfo.Rating
            results.Active_Ind = objStoreInfo.Active_Ind
            results.Email = objStoreInfo.Email
            results.SMS = objStoreInfo.SMS
            results.MoreInfomation = objStoreInfo.MoreInfomation
            results.HasIndividualWebsite = objStoreInfo.HasIndividualWebsite
            results.IndividualWebsite = objStoreInfo.IndividualWebsite


            _dbContext.SubmitChanges()
            blnRet = True
        End Using
        Return blnRet
    End Function
    Public Function DeleteStoreInfo(ByVal intStoreID As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim StoreInfos As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            _dbContext.StoreInfos.DeleteAllOnSubmit(StoreInfos)
            _dbContext.SubmitChanges()
            blnRet = True
        End Using
        Return blnRet
    End Function
#End Region

    Public Function CheckIsDeliveryStore(ByVal intStoreID As Integer) As Boolean
        Dim strRet As String = String.Empty
        Dim blnRet As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            strRet = Stores.Select(Function(a) a.IsDelivery).SingleOrDefault().ToString
            If strRet.ToUpper = "Y" Then
                blnRet = True
            End If
            Return blnRet
        End Using
        Return blnRet
    End Function

    Public Function GetDeliveryDistanceFromStoreID(ByVal intStoreID As Integer) As Decimal
        Dim decRet As Decimal = 0
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            decRet = Stores.Select(Function(a) a.DeliveryDistance).SingleOrDefault()
            Return decRet
        End Using
        Return decRet
    End Function
    Public Function GetStoreAddressFromStoreID(ByVal intStoreID As Integer) As String
        Dim strRet As String = String.Empty
        Using _dbContext As New Hungry4FoodDataContext
            Dim Stores As IQueryable(Of StoreInfo) = _dbContext.StoreInfos.Where(Function(a) a.StoreID = intStoreID)
            strRet = Stores.Select(Function(a) a.StoreStrtAdd).SingleOrDefault() + ", " + Stores.Select(Function(a) a.StoreCity).SingleOrDefault() + ", " + Stores.Select(Function(a) a.StoreState).SingleOrDefault() + ", " + Stores.Select(Function(a) a.StoreZipCd).SingleOrDefault()
            Return strRet
        End Using
        Return strRet
    End Function

    Public Function GetStoreById(ByVal storeId As Integer) As StoreInfo
        Dim storeInfo As StoreInfo = New StoreInfo()
        Using _dbContext As New Hungry4FoodDataContext
            storeInfo = _dbContext.StoreInfos.Where(Function(a) a.StoreID = storeId).Take(1).SingleOrDefault()
            Return storeInfo
        End Using
        Return storeInfo
    End Function
End Class
