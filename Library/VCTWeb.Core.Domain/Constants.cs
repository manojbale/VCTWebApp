using System;
using System.Collections.Generic;

namespace VCTWeb.Core.Domain
{

    public class Constants
    {

        #region Stored Procedure Constants

        //Stored Procedure names


        ////Configuration
        public const string USP_GET_ALL_CONFIGURATIONS = "usp_GetAllConfigurations";
        public const string USP_GET_EDITABLE_CONFIGURATIONS = "usp_GetEditableConfigurations";
        public const string USP_UPDATE_CONFIGURATION = "usp_UpdateConfiguration";
        public const string USP_GET_CONFIGURATION_KEY_VALUE = "usp_GetConfigurationKeyValue";

        public const string USP_GET_ALL_CONFIGURATION_GROUPS = "usp_GetAllConfigurationGroups";
        public const string USP_GET_CONFIGURATIONS_BY_GROUP = "usp_GetConfigurationsByGroup";

        public const string USP_SAVE_LOCATION_AND_ADDRESS = "usp_SaveLocationAndAddress";
        //public const string USP_SAVE_LOCATION = "usp_SaveLocation";
        public const string USP_DELETE_LOCATION = "usp_DeleteLocation";
        public const string USP_DeleteItemFromLocationPartDetail = "usp_DeleteItemFromLocationPartDetail";
        public const string USP_PUBLISH_BOM = "usp_PublishBOM";
        public const string USP_SAVE_LOCATION_ADDRESS = "usp_SaveLocationAddress";
        public const string USP_GET_LOCATION_ADDRESS = "usp_GetLocationAddress";
        public const string USP_GET_PARTY_ADDRESS = "usp_GetPartyAddress";

        public const string USP_GET_DUPLICATE_LOCATIONCODE = "usp_GetDuplicateLocationCode";
        public const string USP_GET_DUPLICATE_BOM = "usp_GetDuplicateBOM";
        public const string USP_GET_DUPLICATE_KIT_FAMILY = "usp_GetDuplicateKitFamily";
        public const string USP_CheckInUseKitFamily = "usp_CheckInUseKitFamily";
        public const string USP_CheckInUseParty = "usp_CheckInUseParty";
        public const string USP_CheckIsPartyExists = "usp_CheckIsPartyExists";
        public const string USP_CheckInUseLocation = "usp_CheckInUseLocation";
        public const string USP_CheckInUseUser = "usp_CheckInUseUser";

        public const string USP_GET_ROLE_PERMISSIONS = "usp_GetRolePermissions";
        public const string USP_GET_ROLE_PERMISSIONS_BY_ROLEID = "usp_GetRolePermissionsByRoleId";
        public const string USP_DELETE_PERMISSIONS_BY_ROLEID = "usp_DeletePermissionsByRoleId";
        public const string USP_DELETE_ROLE_BY_ROLEID = "usp_DeleteRoleByRoleId";
        public const string USP_SAVE_ROLE_PERMISSIONS = "usp_SaveRolePermissions";
        public const string USP_GET_ROLES = "usp_GetRoles";
        public const string USP_CHECKUSERNAME = "usp_CheckUserName";

        public const string USP_GET_PASSWORDVALUE_FOR_USERNAME = "usp_GetPasswordValueForUserName";
        public const string USP_CHECK_USERNAME_AND_PASSWORD = "usp_CheckUserNameAndPassword";
        public const string USP_UPDATE_USER_PASSWORD = "usp_UpdateUserPassword";

        ////COUNTRY AND STATE
        public const string USP_GET_COUNTRY_LIST = "usp_GetCountryList";
        public const string USP_GET_STATE_LIST = "usp_GetStateList";

        public const string USP_GETIMPLIEDPERMISSIONS = "usp_GetImpliedPermissions";
        public const string USP_GETPERMISSION = "usp_GetPermission";
        public const string USP_GETLISTOFPERMISSIONBYUSERNAME = "usp_GetListOfPermissionByUserName";
        public const string USP_GETLISTOFPERMISSIONBYROLES = "usp_GetListOfPermissionByRoles";
        public const string USP_SAVEROLE = "usp_SaveRole";
        public const string USP_GETLISTOFROLEBYUSERNAME = "usp_GetListOfRoleByUserName";
        public const string USP_GETLISTOFROLEBYADGROUPS = "usp_GetListOfRoleByADGroups";
        public const string USP_SAVE_USER_AND_ROLE_MEMEBERSHIP = "usp_SaveUserAndRoleMembership";
        public const string USP_SAVE_BOM = "usp_SaveBOM";
        public const string USP_SAVECONTACT = "usp_SaveContact";
        public const string USP_CONTACTEXISTS = "usp_IsContactExists";
        public const string USP_SAVEKITFAMILY = "usp_SaveKitFamily";
        public const string USP_SAVEPARTY = "usp_SaveParty";
        public const string USP_SaveInventoryCountReconcile = "usp_SaveInventoryCountReconcile";
        public const string USP_DELETEROLEBYUSERNAME = "usp_DeleteRoleByUserName";
        public const string USP_DELETELOCATIONCONTACTBYLOCATIONID = "usp_DeleteLocationContactByLocationId";
        public const string USP_SAVEROLEMEMBERSHIP = "usp_SaveRoleMembership";
        public const string USP_SAVELOCATIONCONTACT = "usp_SaveLocationContact";
        public const string USP_GETUSERINFORMATION = "usp_GetUserInformation";
        public const string USP_GETLISTOFUSERS = "usp_GetListOfUsers";
        public const string USP_GetListOfSalesRepByLocationId = "usp_GetListOfSalesRepByLocationId";
        public const string USP_GETLISTOFCONTACTS = "usp_GetListOfContacts";
        public const string USP_GETCONTACTBYCONTACTID = "usp_GetContactByContactId";
        public const string USP_GETLOCATIONCONTACTBYCONTACTID = "usp_GetLocationContactByContactId";
        public const string USP_GETLISTOFKITFAMILY = "usp_GetListOfKitFamily";
        public const string USP_GetActiveKitFamiliesByLocationId = "usp_GetActiveKitFamiliesByLocationId";
        public const string USP_GetKitFamilyById = "usp_GetKitFamilyById";
        public const string USP_GetKitFamilyPartsById = "usp_GetKitFamilyPartsById";
        public const string USP_GetKitFamilyPartsByKitFamilyName = "usp_GetKitFamilyPartsByKitFamilyName";
        public const string USP_GetKitStockLevelByLocationId = "usp_GetKitStockLevelByLocationId";
        public const string USP_GetKitDetailStockLevelByLocationAndKitFamily = "usp_GetKitDetailStockLevelByLocationAndKitFamily";
        public const string USP_GetKitDetailStockLevelByLocationAndParty = "usp_GetKitDetailStockLevelByLocationAndParty";
        public const string USP_GetPartStockLevelByLocationId = "usp_GetPartStockLevelByLocationId";
        public const string USP_GetPartDetailStockLevelByLocationAndPart = "usp_GetPartDetailStockLevelByLocationAndPart";
        public const string USP_GETLISTOFCONTACTSBYLOCATIONID = "usp_GetListOfContactsByLocationId";
        public const string USP_DELETEUSER = "usp_DeleteUser";
        public const string USP_INSERT_LOGGGING_DETAILS = "usp_InsertLogggingDetails";
        public const string USP_GETUSERROLEBYUSERNAME = "usp_GetUserRoleByUserName";
        public const string USP_VERIFYSECURITYQUESTIONANSWER = "usp_VerifySecurityQuestionAnswer";
        public const string USP_GetKitFamilyLocationsById = "usp_GetKitFamilyLocationsById";
        public const string USP_GET_DICTIONARY_LIST_BY_PREFIX = "usp_GetDictionaryListByPrefix";
        public const string USP_GET_DICTIONARY_RULE = "usp_GetDictionaryRule";
        public const string USP_GetVirtualBuildKitByKitNumber = "usp_GetVirtualBuildKitByKitNumber";
        public const string USP_GETLISTOFREGIONS = "usp_GetListOfRegions";
        public const string USP_GETLISTOFSALESOFFICES = "usp_GetListOfSalesOffices";
        public const string USP_GETLOTNUMBERSLISTBYKITNUMBER = "usp_GetLotNumbersByKitNumber";
        public const string USP_GETKITNUMBERDESCBYKITNUMBER = "usp_GetKitNumberDescByKitNumber";
        public const string USP_GETLOTNUMBERSTOASSIGNED = "usp_GetLotNumbersToAssigned";
        public const string USP_GetLocationsByLocationType = "usp_GetLocationsByLocationType";
        public const string USP_GetLocationTypes = "usp_GetLocationTypes";

        public const string USP_GETKITSBYPROCEDUREANDCATALOG = "usp_GetKitsByProcedureAndCatalog";
        public const string USP_GETKITSBYKITNUMBER = "usp_GetKitsByKitNumber";
        public const string USP_GetKitsByKitNumberOrDesc = "usp_GetKitsByKitNumberOrDesc";
        public const string USP_GetKitByKitNumber = "usp_GetKitByKitNumber";
        public const string USP_GETKITSBYKITNAME = "usp_GetKitsByKitName";
        public const string USP_GETPROCEDURESBYPROCEDURENAME = "usp_GetProceduresByProcedureName";

        public const string USP_GETPPHYSICIANBYPARTYNAME = "usp_GetPhysicianByPartyName";
        public const string USP_GETLOTSBYLOTNUMBER = "usp_GetLotsByLotNumber";
        public const string USP_GETCATALOGBYCATALOGNUMBER = "usp_GetCatalogByCatalogNumber";
        public const string USP_GetPartyAvailableQtyById = "usp_GetPartyAvailableQtyById";
        public const string USP_GETLISTOFCATALOGNUMBERS = "usp_GetListOfCatalogNumbers";
        public const string USP_GETLISTOFCATALOGNUMBERSBYBOMID = "usp_GetListOfCatalogNumbersByBOMId";
        public const string USP_GETLISTOFTRAYTYPES = "usp_GetListOfTrayTypes";
        public const string USP_GETLISTOFKITTYPES = "usp_GetListOfKitTypes";
        public const string USP_GETLISTOFPUBLISHEDBOMS = "usp_GetListOfPublishedBOMs";
        public const string USP_GETLISTOFUNPUBLISHEDBOMS = "usp_GetListOfUnPublishedBOMs";
        public const string USP_GetBOMByBOMId = "usp_GetBOMByBOMId";
        public const string USP_GETPARTYBYPARTYNAME = "usp_GetPartyByPartyName";
        public const string USP_GetPartyLocationByPartyId = "usp_GetPartyLocationByPartyId";
        public const string USP_GetRevenueProjectionByLocationId = "usp_GetRevenueProjectionByLocationId";
        public const string USP_GetRevenueProjectionByParentLocationId = "usp_GetRevenueProjectionByParentLocationId";
        public const string USP_GETLISTOFPARTIES = "usp_GetListOfParties";
        public const string USP_GetPartyCycleCountByPartyId = "usp_GetPartyCycleCountByPartyId";
        public const string USP_GetExptectedInventoryCountByPartyId = "usp_GetExptectedInventoryCountByPartyId";
        public const string USP_GetPartyCycleCountMatchByPartyId = "usp_GetPartyCycleCountMatchByPartyId";
        public const string USP_GetDispositionTypesByCategory = "usp_GetDispositionTypesByCategory";
        public const string USP_GetCaseListByPartAndLotNum = "usp_GetCaseListByPartAndLotNum";
        public const string USP_GetPartyById = "usp_GetPartyById";
        public const string USP_GETLOCATIONTYPEFORPARTY = "usp_GetLocationTypeForParty";
        public const string USP_GETLOCATIONTYPEFORBRANCH = "usp_GetLocationTypeForBranch";
        public const string USP_GETLOCATIONTYPEFORSATELLITE = "usp_GetLocationTypeForSatellite";
        //public const string USP_GETLISTOFPARTYLOCATIONS = "usp_GetListOfPartyLocations";
        public const string USP_GETPARTYLOCATIONBYPARTYID = "usp_GetPartyLocationByPartyId";
        public const string USP_GETLISTOFLOCATIONSBYLOCATIONTYPEID = "usp_GetListOfLocationsByLocationTypeId";
        //public const string USP_GETPARTYLOCATIONBYLOCATIONID = "usp_GetPartyLocationByLocationId";
        public const string USP_GETLOCATIONBYLOCATIONID = "usp_GetLocationByLocationId";
        public const string USP_GetNearExpiryItemsByLocationId = "usp_GetNearExpiryItemsByLocationId";
        public const string USP_GetLocationByParentLocationId = "usp_GetLocationByParentLocationId";
        public const string USP_FetchAllPendingCasesByCaseType = "usp_FetchAllPendingCasesByCaseType";
        public const string USP_GetKitFamilyByCaseId = "usp_GetKitFamilyByCaseId";
        public const string USP_GETLISTOFPARTYTYPES = "usp_GetListOfPartyTypes";
        public const string USP_SAVENEWREQUEST = "usp_SaveNewRequest";
        public const string USP_SENDREQUEST = "usp_SendRequest";
        public const string USP_SENDREQUESTFORCASE = "usp_SendRequestForCase";
        public const string USP_SAVEREQUESTDETAILS = "usp_SaveRequestDetails";
        public const string USP_SAVEREQUESTTRANSACTION = "usp_SaveRequestTransaction";
        public const string USP_GetLocationsByKitFamilyId = "usp_GetLocationsByKitFamilyId";
        public const string USP_GetPartyByLocation = "usp_GetPartyByLocation";
        public const string USP_GetRMAPartsByCatalogNumber = "usp_GetRMAPartsByCatalogNumber";
        public const string USP_GetRMACasesByCaseNum = "usp_GetRMACasesByCaseNum";

        public const string USP_GETREQUESTSBYUSER = "usp_GetRequestsByUser";
        public const string USP_GETREQUESTSBYREGIONLOCATION = "usp_GetRequestsByRegionLocation";
        public const string USP_GETREQUESTSUMMARYBYREGION = "usp_GetRequestSummaryByRegion";
        //public const string USP_GETSEARCHRESULTBYREQUESTID = "usp_GetSearchResultByRequestId";
        public const string USP_GETSEARCHRESULTBYCASEID = "usp_GetSearchResultByCaseId";
        //public const string USP_GETSEARCHRESULTFORREQUESTEDLOCATION = "usp_GetSearchResultForRequestedLocation";
        public const string USP_GETSEARCHRESULTFORREQUESTEDLOCATIONBYCASEID = "usp_GetSearchResultForRequestedLocationByCaseId";
        //public const string USP_GETSEARCHRESULTFORSHIPTOLOCATION = "usp_GetSearchResultForShipToLocation";
        public const string USP_GETSEARCHRESULTFORSHIPTOLOCATIONBYCASEID = "usp_GetSearchResultForShipToLocationByCaseId";


        public const string USP_GetKitListingByLocationIdAndKitFamilyId = "usp_GetKitListingByLocationIdAndKitFamilyId";
        public const string USP_CaseDetailsCancel = "usp_CaseDetailsCancel";
        public const string USP_GetCaseKitDetailByCaseId = "usp_GetCaseKitDetailByCaseId";
        public const string USP_GetKitDetailByCaseId = "usp_GetKitDetailByCaseId";
        public const string USP_GetCaseItemsListByCaseId = "usp_GetCaseItemsListByCaseId";
        public const string USP_SaveNewProductTransfer = "usp_SaveNewProductTransfer";
        public const string USP_GetManuallyAddedKits = "usp_GetManuallyAddedKits";
        public const string USP_GetDuplicateKitNumber = "usp_GetDuplicateKitNumber";
        public const string USP_GetKitTableByKitNumber = "usp_GetKitTableByKitNumber";
        public const string USP_SaveKitListing = "usp_SaveKitListing";
        public const string USP_ModifyKitTable = "usp_ModifyKitTable";
        public const string USP_GetCaseStatus = "usp_GetCaseStatus";
        public const string USP_GetCaseType = "usp_GetCaseType";
        public const string USP_SaveCase = "usp_SaveCase";
        public const string USP_CancelCase = "usp_CancelCase";
        public const string USP_DeleteKitListing = "usp_DeleteKitListing";
        public const string USP_GetCaseByCaseId = "usp_GetCaseByCaseId";
        public const string USP_GetInvoiceAdvisoryByCaseId = "usp_GetInvoiceAdvisoryByCaseId";
        public const string USP_GetListOfCasesByFilter = "usp_GetListOfCasesByFilter";
        public const string USP_GetCaseStatusDetailByCaseId = "usp_GetCaseStatusDetailByCaseId";
        public const string USP_GetCatalogByKitNumber = "usp_GetCatalogByKitNumber";
        public const string USP_GetPhysicians = "usp_GetListofPhysicians";
        public const string USP_ApplyCaseFilters = "usp_ApplyCaseFilters";
        public const string USP_GetListOfAllLocations = "usp_GetListOfAllLocations";
        public const string USP_GetListOfRequestStausByUser = "usp_GetListOfRequestStausByUser";
        public const string USP_GETFILTEREDREQUESTSBYUSER = "usp_GetFilteredRequestsByUser";
        public const string USP_UpdateRequestStatusById = "usp_UpdateRequestStatusById";
        public const string USP_GetListOfRequestStausByRegion = "usp_GetListOfRequestStausByRegion";
        public const string USP_GETFILTEREDREQUESTSBYREGION = "usp_GetFilteredRequestsByRegion";
        public const string USP_GetKitFamilyByLocationId = "usp_GetKitFamilyByLocationId";
        public const string USP_GetKitTableListByKitNumber = "usp_GetKitTableListByKitNumber";
        public const string USP_GetKitTableListByKitFamilyId = "usp_GetKitTableListByKitFamilyId";
        public const string USP_GetSelectBuildKitByKitNumber = "usp_GetSelectBuildKitByKitNumber";
        public const string USP_GetListOfPendingBuildKit = "usp_GetListOfPendingBuildKit";
        public const string USP_GetRMAKitDetailsByCaseId = "usp_GetRMAKitDetailsByCaseId";
        public const string USP_GetKitTableByKitFamily = "usp_GetKitTableByKitFamily";

        public const string USP_GetLocationPARLevelByLocationId = "usp_GetLocationPARLevelByLocationId";
        public const string USP_GetPartyPARLevelByPartyId = "usp_GetPartyPARLevelByPartyId";
        public const string USP_GetReplenishmentTransferByLocationId = "usp_GetReplenishmentTransferByLocationId";
        public const string USP_GetReplenishmentTransferByPartyId = "usp_GetReplenishmentTransferByPartyId";
        public const string USP_SavePartyPARLevel = "usp_SavePartyPARLevel";
        public const string USP_SaveLocationPARLevel = "usp_SaveLocationPARLevel";
        public const string USP_DeletePartyPARLevel = "usp_DeletePartyPARLevel";
        public const string USP_DeleteLocationPARLevel = "usp_DeleteLocationPARLevel";
        public const string USP_SavePartyReplenishmentTransfer = "usp_SavePartyReplenishmentTransfer";
        public const string USP_SaveLocationReplenishmentTransfer = "usp_SaveLocationReplenishmentTransfer";
        public const string USP_SaveKitFamilyPartDetail = "usp_SaveKitFamilyPartDetail";
        public const string USP_GetCasePartDetailByCaseId = "usp_GetCasePartDetailByCaseId";
        public const string USP_GetKitHistoryByKitNumberAndLocationId = "usp_GetKitHistoryByKitNumberAndLocationId";
        public const string USP_GetGroupedCasePartDetailByCaseId = "usp_GetGroupedCasePartDetailByCaseId";
        public const string USP_GetGroupedCaseKitFamilyDetailByCaseId = "usp_GetGroupedCaseKitFamilyDetailByCaseId";
        public const string USP_GetKitFamilyItemsByCaseId = "usp_GetKitFamilyItemsByCaseId";
        public const string USP_UpdateKitFamilyPartQty = "usp_UpdateKitFamilyPartQty";
        public const string USP_DeleteKitFamilyPartById = "usp_DeleteKitFamilyPartById";
        public const string USP_SAVEVIRTUALBUILDKIT = "usp_SaveVirtualBuildKit";
        public const string USP_GetCasesSummaryByCaseType = "usp_GetCasesSummaryByCaseType";
        public const string USP_GetCasesListByCaseType = "usp_GetCasesListByCaseType";

        public const string USP_GetCasesListByCaseType_Pagination = "usp_GetCasesListByCaseType_Pagination";
        public const string USP_GetLocationDetailLocationId = "usp_GetLocationDetailLocationId";
        public const string USP_GetMapShippingDetail = "usp_GetMapShippingDetail";
        public const string USP_GetPendingCasesListBySalesPerson = "usp_GetPendingCasesListBySalesPerson";
        public const string USP_GetKitFamilyByNumber = "usp_GetKitFamilyByNumber";
        public const string USP_GetKitFamilyByNumberAndLocation = "usp_GetKitFamilyByNumberAndLocation";
        public const string USP_GetLocationDetailsByLocationId = "usp_GetLocationDetailsByLocationId";
        public const string USP_GetKitFamilyByLocationAndNumber = "usp_GetKitFamilyByLocationAndNumber";

        public const string USP_GetAllProcedures = "usp_GetAllProcedures";
        public const string USP_GetMappedKitsByKitNumber = "usp_GetMappedKitsByKitNumber";
        public const string USP_GetKitNumbersToBeAssigned = "usp_GetKitNumbersToBeAssigned";
        public const string USP_GetKitNumbersToBeAssignedByCaseId = "usp_GetKitNumbersToBeAssignedByCaseId";
        public const string USP_AssignKitInventory = "usp_AssignKitInventory";
        //public const string USP_GetLotNumbersToBeAssigned = "usp_GetLotNumbersToBeAssigned";
        public const string USP_GetLotNumbersToBeAssignedByCaseId = "usp_GetLotNumbersToBeAssignedByCaseId";
        public const string USP_AssignPartInventory = "usp_AssignPartInventory";
        public const string USP_GetPartsHighOrderList = "usp_GetPartsHighOrderList";
        public const string USP_GetShippingPendingCasesByLocId = "usp_GetShippingPendingCasesByLocId";
        public const string USP_GetShippingPartsByCaseId = "usp_GetShippingPartsByCaseId";
        public const string USP_GetShippingKitsByCaseId = "usp_GetShippingKitsByCaseId";
        public const string USP_SaveShippingDetails = "usp_SaveShippingDetails";
        public const string USP_GetBuildKitById = "usp_GetBuildKitById";
        public const string USP_GetCheckedInBuildKitById = "usp_GetCheckedInBuildKitById";
        public const string USP_GetBuildKitItemsByLocationAndKitFamily = "usp_GetBuildKitItemsByLocationAndKitFamily";
        public const string USP_GetBuildKitItemsByLocationAndParty = "usp_GetBuildKitItemsByLocationAndParty";
        public const string USP_GetMapAgingOrdersList = "usp_GetMapAgingOrdersList";
        public const string USP_GetCheckedInPendingCasesByLocId = "usp_GetCheckedInPendingCasesByLocId";
        public const string USP_GetReCheckedInPendingCasesByLocId = "usp_GetReCheckedInPendingCasesByLocId";
        public const string USP_SaveCheckedInDetails = "usp_SaveCheckedInDetails";
        public const string USP_SaveReCheckedInDetails = "usp_SaveReCheckedInDetails";
        public const string USP_SaveInventoryTransferDetails = "usp_SaveInventoryTransferDetails";
        public const string USP_GetUnutilizePartsListByDays = "usp_GetUnutilizePartsListByDays";
        public const string USP_GetUnutilizeKitsListByDays = "usp_GetUnutilizeKitsListByDays";
        public const string USP_GetCheckOutBuildKitById = "usp_GetCheckOutBuildKitById";
        public const string USP_SaveReturnInventoryRMA = "usp_SaveReturnInventoryRMA";
        public const string USP_GetCheckOutKitByCaseKitId = "usp_GetCheckOutKitByCaseKitId";
        public const string USP_SaveHospitalInventoryTransferDetails = "usp_SaveHospitalInventoryTransferDetails";

        public const string USP_GetInventoryReportData = "usp_GetInventoryReportData";

        public const string USP_GetKitsFamilyByProcedureName = "usp_GetKitsFamilyByProcedureName";

        public const string USP_GetLocationAndPartyTypes = "usp_GetLocationAndPartyTypes";
        public const string USP_GetReplenishmentPlanning_Data = "usp_GetReplenishmentPlanning_Data";


        //Added by Suraj Namdeo

        public const string Usp_EppFetchDistinctColumnValueForTable = "Usp_EppFetchDistinctColumnValueForTable";
        public const string Usp_EppFetchProductLinePartCategoryHierarchy = "Usp_EppFetchProductLinePartCategoryHierarchy";
        public const string USP_EppGetCustomerByAccountNumber = "usp_EppGetCustomerByAccountNumber";
        public const string usp_EppSaveCustomer = "usp_EppSaveCustomer";
        public const string usp_EppFetchListOfFilteredCustomers = "usp_EppFetchListOfFilteredCustomers";
        public const string Usp_EppGetListOfSystemUsers = "Usp_EppGetListOfSystemUsers";
        public const string Usp_EppGetListOfSystemUsersByRoleName = "Usp_EppGetListOfSystemUsersByRoleName";
        public const string Usp_EppUpdateParLevelQuantityForRefNum = "Usp_EppUpdateParLevelQuantityForRefNum";
        public const string Usp_EppFetchPartCatalogByPartNum = "Usp_EppFetchPartCatalogByPartNum";
        public const string Usp_EppFetchCustomerForReportFilterDropdown = "Usp_EppFetchCustomerForReportFilterDropdown";
        public const string usp_EppFetchCustomerProductLineByProductLine = "usp_EppFetchCustomerProductLineByProductLine";
        public const string Usp_EppFetchAllCustomerProductLine = "Usp_EppFetchAllCustomerProductLine";
        public const string Usp_EppFetchUserDetailForAccountNumber = "Usp_EppFetchUserDetailForAccountNumber";
        public const string usp_EppFetchLowInventoryReport = "usp_EppFetchLowInventoryReport";
        public const string usp_EppFetchInventoryAmountReport = "usp_EppFetchInventoryAmountReport";
        public const string Usp_EppFetchOffCartRateForConsumedItems = "Usp_EppFetchOffCartRateForConsumedItems";
        public const string Usp_EppFetchManualConsumptionReport = "Usp_EppFetchManualConsumptionReport";
        public const string Usp_EppFetchTransactionReport = "Usp_EppFetchTransactionReport";
        public const string Usp_EppFetchTagHistoryByTagId = "Usp_EppFetchTagHistoryByTagId";
        public const string usp_EppFetchConsumptionRate = "usp_EppFetchConsumptionRate";
        public const string usp_EppFetchShipAndBillReport = "usp_EppFetchShipAndBillReport";
        public const string usp_EppSaveOrderAdjustment = "usp_EppSaveOrderAdjustment";
        public const string UspEppFetchAllOrderAdjustmentByOrderId = "usp_EppFetchAllOrderAdjustmentByOrderId";
        public const string UspEppFetchAllItemStatus = "usp_EppFetchAllItemStatus";
        public const string UspEppInitiateManualScan = "usp_EppInitiateManualScan";
        public const string UspEppSaveNotificationDetail = "usp_EppSaveNotificationDetail";
        public const string UspEppUpdateNotificationDetail = "Usp_EppUpdateNotificationDetail";
        public const string UspEppFetchNotificationDetailByKey = "usp_EppFetchNotificationDetailByKey";
        public const string UspEppFetchAllNotificationDetailByAccountNumber = "usp_EppFetchAllNotificationDetailByAccountNumber";
        public const string UspEppDeleteNotificationDetail = "usp_EppDeleteNotificationDetail";
        public const string UspEppFetchAllProductLinePartDetail = "Usp_EppFetchAllProductLinePartDetail";
        public const string UspEppFetchAllProductLine = "usp_EppFetchAllProductLine";
        public const string UspEppGetDispositionTypesByCategory = "USP_EppGetDispositionTypesByCategory";
        public const string UspEppSaveManualConsumption = "usp_EppSaveManualConsumption";
        public const string UspEppIsManuallyCompleted = "usp_EppIsManualScanCompleted";
        public const string UspEppRevertManualConsumption = "Usp_EppRevertManualConsumption";



        public const string Usp_EppFetchCustomerShelfForDashBoard = "Usp_EppFetchCustomerShelfForDashBoard";
        public const string Usp_FetchCustomerShelfByCustomerShelfId = "Usp_FetchCustomerShelfByCustomerShelfId";
        public const string Usp_EppFetchCustomerShelfPropertyByCustomerShelfId = "Usp_EppFetchCustomerShelfPropertyByCustomerShelfId";
        public const string Usp_EppFetchCustomerShelfAntennaPropertyByCustomerShelfAntennaId = "Usp_EppFetchCustomerShelfAntennaPropertyByCustomerShelfAntennaId";
        public const string Usp_EppFetchDistinctAntennaByCustomerShelfId = "Usp_EppFetchDistinctAntennaByCustomerShelfId";
        public const string Usp_EppSaveModifiedReaderAntennaValues = "Usp_EppSaveModifiedReaderAntennaValues";
        

        
        #endregion

        #region Other Constants

        //   //for Hybrid Pallet Label Printng
        //   public const string GTINONPALLET = "GTIN";
        //   public const string TAGNAME = "TAG";
        //   public const string GTINHR = "HR";

        //   //Constants for Key related to NumberSequence
        //   public const string LOT_NUMBER = "Lot";
        //   public const string PALLET_NUMBER = "Pallet";
        //   public const string TICKET_NUMBER = "Ticket";

        //   //Constants for Application Identifiers
        //   public const string AISTART = "(";
        //   public const string AISTOP = ")";
        //   public const string AI_GTIN = "01";
        //   public const string AI_LOT_NUMBER = "10";
        //   public const string AI_QUANTITY = "30";
        //   public const string FNC1 = "^1";

        //   //Authentication
        //   public const string CURRENT_PERSON_ID = "CurrentPersonId";
        //   public const string CURRENT_USER_PERMISSIONS = "CurrentUserPermissions";
        //   public const string CURRENT_USER_NAME = "CurrentUserName";
        //   public const string CURRENT_USER_ROLES = "CurrentUserRoles";

        public const string SEARCH_PATH_PREFIX = "LDAP://"; //This would be prefixed with the domain name

        public const string SaltKey = "ITS";
        public const string ROOT_USER = "root";
        //   public const string MSGBOX_MISSING_INFO = "capMissingInfo";
        //   public const string MSGBOX_INVALID_INFO = "capInvalidInfo";
        //   public const string MSGBOX_ERROR_INFO = "capError";

        //   public const string DATABASE_ERROR = "msgDatabaseError";
        //   public const string INVALID_USERNAME_OR_PASSWORD = "msgNoValidUserNameOrPassword";
        //   public const string WRONG_USERNAME_OR_PASSWORD = "msgWrongUserNameAndPassword";
        //   public const string ACTIVE_DIRECTORY_AUTHENTICATION_FAILED = "msgActiveDirectoryAuthenticationFailed";

        //   public const string SYSTEM_ADMINISTRATOR = "System Administrator";
        //   public const string DOMAIN_USER_SEPARATOR = "\\";
        //   public const string FIRST_LEVEL_SEPARATOR = ",";
        //   public const string SECOND_LEVEL_SEPARATOR = ";";

        //   public const string MODE = "Mode";
        //   public const string ACTION_MODE = "ActionMode";

        //   public const string DICTIONARY_LIST_PREFIX = "AlertMail";

        //   public const string NUMBER_SEQUENCE_KEYS = "Lot,Pallet,Ticket,ShipOrder,VINASSIGN";
        //   public const string NUMBER_SEQUENCE_KEYS_WITHOUT_TICKET = "Lot,Pallet,ShipOrder,VINASSIGN";

        //   public const string CULTURERESOURCEBASENAME = "WIPProduct.RFID.LabelPrint.Resources.LocalizedCulture";

        //   //  RFID encoding types
        //   public const string SGTIN96 = "Sgtin96Encoding";
        //   public const string SSCC96 = "Sscc96Encoding";
        public const string RFID_USER = "RFID";

        //   // Advanced Shipping Notice EDI

        //   public const string ST01 = "ST01";
        //   public const string BSN01 = "BSN01";
        //   public const string BSN02 = "BSN02";
        //   public const string BSN06 = "BSN06";
        //   public const string BSN07 = "BSN07";
        //   public const string N106FR = "N106FR";
        //   public const string N106TO = "N106TO";

        //   //for Email Validation
        public const string EMAIL_VALIDATION = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
       + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        //   public static string USP_GETNUMBERSEQUENCEWIDTHBYPROCESS = "usp_GetNumberSequenceWidthByProcess";

        #endregion

        #region Enums & Structs
        //public enum RevisionHistoryTableName
        //{
        //    Product,
        //    StepComplexity,
        //    WorkStation,
        //    line,
        //    Tools,
        //    Measurable,
        //    Procedures,
        //    ProcedureConfiguration,
        //    ActivityReason,
        //    Location,
        //    UnitOfMeasure,
        //    ProductType,
        //    KitPreferredLine,
        //    WorkStationType,
        //    Shift,
        //    KitStructure
        //}

        //public enum AlertDictionary
        //{
        //    AlertMailHost,
        //    AlertMailPort,
        //    AlertMailFrom,
        //    AlertMailRecipient,
        //    AlertMailSubjectProduct,
        //    AlertMailSubjectHarvestStop,
        //    AlertMailSubjectTicketReceive,
        //    AlertMailMessageProduct,
        //    AlertMailMessageHarvestStop,
        //    AlertMailMessageTicketReceive
        //}

        public enum CaseType
        {
            InventoryTransfer,
            NewProductTransfer,
            ReplenishmentTransfer,
            ReturnInventoryRMATransfer,
            RoutineCase,
            InternalRequest
        }

        public enum Dictionary
        {
            RefreshTime,
            RefreshTimeMonitorDashboard,
            GenricGridPageSize,
            ContentGridPageSize,
            Language,
            TaktTimePriorNotificationTime
        }

        public enum RequestStatus
        {
            Open,
            Cancelled,
            Rejected,
            SentForConfirmation,
            Confirmed,
            Closed,
            Shipped,
            CheckedIn
        }

        public enum DispositionCategory
        {
            InventoryCountReconcile,
            CaseCancel,
            ReturnInventoryRMA,
            InventoryCountReconcilePositiveVariance,
            InventoryCountReconcileNegativeVariance
        }

        public enum CaseStatus
        {
            New,
            InventoryAssigned,
            PartiallyInventoryAssigned,
            Shipped,
            PartiallyShipped,
            Delivered,
            PartiallyDelivered,
            CheckedIn,
            PartiallyCheckedIn,
            InternallyRequested,
            Cancelled
        }

        public enum KitStatus
        {
            Available,
            AssignedToCase,
            Shipped,
            Delivered,
            Received
        }

        public enum PartStatus
        {
            AssignedToCase,
            AssignedToKit,
            Available,
            Consumed,
            Damaged,
            Deleted,
            Delivered,
            Invoiced,
            Missing,
            Shipped
        }

        public enum InventoryType
        {
            Kit,
            Part
        }

        //public enum AlertNotificationType
        //{
        //    ReceiveTicket,
        //    HarvestClose,
        //    Product
        //}

        //public enum PartyTypeName
        //{
        //    SHIPPER,
        //    GROWER,
        //    CUSTOMER,
        //    COMPANY,
        //    VENDOR
        //}

        //public enum PartyTypeForLabelFormat
        //{
        //    GROWER,
        //    CUSTOMER,
        //    GROWER_OWNER
        //}

        //public enum StatusCode
        //{
        //    Open,
        //    Close,
        //    Cancel,
        //    Ready,
        //    Ship,
        //    Received,
        //    RecPallet,
        //    None
        //}

        //public enum TicketType
        //{
        //    Defective,
        //    Good
        //}

        public enum ResultStatus
        {
            MissingParentID,
            SelectProductType,
            SelectLocationType,
            SelectLocation,
            SelectParty,
            SelectPartyType,
            SelectUOMName,
            SelectShipmentTypeCode,
            SelectContainerTypeName,
            Saved,
            Cancelled,
            Deleted,
            Published,
            MissingDetails,
            MissingPropertyName,
            MissingDataType,
            MissingPropertyValue,
            Ok,
            MissingAddress,
            InvalidPropertyValue,
            Duplicate,
            CanNotDeleteMandatoryProperty,
            DuplicatePropertyName,
            Error,
            Created,
            Updated,
            DuplicateProduct,
            DuplicateGS1Code,
            DuplicateCompanyPrefix,
            DuplicateLocationName,
            DuplicateBOM,
            DuplicateKitFamily,
            SelectAtleastOneItem,
            SelectOnlyOneItem,
            SomeItemsNotUpdated,
            MissingUOMName,
            MissingShipmetTypeCode,
            MissingContainerType,
            MissingNumberSequencePrefix,
            MissingNumberSequenceWidth,
            DuplicateLastSequencesNumber,
            ConfigurationTypeMismatch,
            PropertyDescritorInUse,
            RFIDSerialNumber,
            MarkedAsDeleted,
            MissingStepComplexityName,
            MissingWorkStationname,
            MissingWorkStationMacAddress,
            NotMac,
            MissingSequence,
            MissingDocname,
            MissingDocDescription,
            MissingDocFilePath,
            MissingToolName,
            MissingMeasurableName,
            Locked,
            Unlocked,
            MissingPlantName,
            InvalidComboValue,
            MissingActivityReasonCodeName,
            MissingShiftName,
            InvalidLineSequence,
            Undo,
            InvalidComboForProduct,
            InvalidComboForLine,
            MissingQuantity,
            MissingKitSturcturename,
            Moved,
            Assigned,
            RequestSent,
            InUse,
            Failed,
            DuplicateContact
        }

        public enum CurrentMode
        {
            Add, Edit, Delete, View
        }

        //public enum OperationType
        //{
        //    Print,
        //    Preview
        //}

        //public enum HarvestEvent
        //{
        //    HarvestEventStart, HarvestEventEditOrClose, CaseLabel
        //}

        //public enum AuthenticationType
        //{
        //    Database, ActiveDirectory
        //}

        //public enum PrintMode
        //{
        //    New,
        //    RePrint
        //}

        //public enum ProductCategory
        //{
        //    RAW,
        //    FINISHEDGOOD,
        //    SUBASSEMBLY
        //}

        //[Serializable]
        //public struct TicketsInPallet
        //{
        //    public string GS1Code;
        //    public string LotNumber;
        //    public int Quantity;
        //}

        //public enum ReportName
        //{
        //    VehicleActivity,
        //    MultipleVehicleActivity,
        //    Throughput,
        //    UsersReport,
        //    WriteInReport,
        //    StationTimeException,
        //    MeasurementReport,
        //    IncomingCount,
        //    HelpRequests,
        //    ConcernsReport,
        //    CompletionCount,
        //    ChangesReport,
        //    BuildCombinationReport,
        //    DashboardReport,
        //    MonitorWriteIn,
        //    MonitorHelpRequests,
        //    MonitorSkips,
        //    AllReports,
        //    AVSLogReport
        //}
        #endregion

        #region Public Methods

        //public static List<AddressRole> GetAddressRole()
        //{
        //    List<AddressRole> _addressRole = new List<AddressRole>();
        //    _addressRole.Add(new AddressRole() { PartyAddressRole = "Billing" });
        //    _addressRole.Add(new AddressRole() { PartyAddressRole = "Shipping" });
        //    _addressRole.Add(new AddressRole() { PartyAddressRole = "Home" });
        //    _addressRole.Add(new AddressRole() { PartyAddressRole = "Office" });

        //    return _addressRole;
        //}

        //public static List<PartyRoleForProduct> GetPartyProductRole()
        //{
        //    List<PartyRoleForProduct> _partyRoleForProduct = new List<PartyRoleForProduct>();
        //    _partyRoleForProduct.Add(new PartyRoleForProduct() { PartyProductRole = "Grower" });
        //    _partyRoleForProduct.Add(new PartyRoleForProduct() { PartyProductRole = "Shipper" });
        //    _partyRoleForProduct.Add(new PartyRoleForProduct() { PartyProductRole = "Distributor" });
        //    _partyRoleForProduct.Add(new PartyRoleForProduct() { PartyProductRole = "Retailer" });

        //    return _partyRoleForProduct;
        //}

        //public static List<PropertyDataType> GetPropertyDataType()
        //{
        //    List<PropertyDataType> _propertyDataTypes = new List<PropertyDataType>();
        //    _propertyDataTypes.Add(new PropertyDataType() { PropertyDataTypes = "Date" });
        //    _propertyDataTypes.Add(new PropertyDataType() { PropertyDataTypes = "Numeric" });
        //    _propertyDataTypes.Add(new PropertyDataType() { PropertyDataTypes = "String" });
        //    _propertyDataTypes.Add(new PropertyDataType() { PropertyDataTypes = "Boolean" });

        //    return _propertyDataTypes;
        //}

        #endregion



    }
}
