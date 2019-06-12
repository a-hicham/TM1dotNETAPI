using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TM1dotNETAPI
{
    public static class TM1API
    {
        // Define a property to allow setting the path to tm1api.dll
        // If this property is not set correctly, the API will not work
        internal static string _TM1DLLPath = null;
        public static string TM1DLLPath
        {
            get
            {
                return _TM1DLLPath;
            }
            set
            {
                _TM1DLLPath = value;
                SetDllDirectory(_TM1DLLPath);
            }
        }

        // The SetDllDirectory function is used to add the path to the TM1API.dll file to 
        // the dll search path.
        //
        // Another method would be to call the LoadLibrary API, but this is problematic, as it
        // required loading all dependent modules individually. This can change between TM1 
        // versions, hence the SetDllDirectory method is preferred. 
        //
        // See Microsoft PInvoke documentation for more details.

        [DllImport("kernel32.dll")]
        public static extern bool SetDllDirectory(string lpPathName);

        public static bool IsError(int hUser, int hObject)
        {
            return (TM1ValType(hUser, hObject) == TM1ValTypeError());
        }

        /// <summary>
        /// Tries to convert a IntPtr to a String
        /// </summary>
        /// <param name="stringHandle"></param>
        /// <returns></returns>
        public static string intPtrToString(int hUser, int stringHandle)
        {
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(TM1ValStringGet(hUser, stringHandle));
        }

        // The rest of this code is a translated version of TM1API.BAS
        // It was translated to C# using an online conversion tool
        // Note: long values were converted to int, as TM1 expects 32-bit values

        // API
        [DllImport("tm1api.dll")]
        public static extern void TM1APIInitialize();

        [DllImport("tm1api.dll")]
        public static extern void TM1APIFinalize();

        // Blob Functions
        [DllImport("tm1api.dll")]
        public static extern int TM1BlobClose(int hPool, int hBlob);

        [DllImport("tm1api.dll")]
        public static extern int TM1BlobCreate(int hPool, int hServer, int sName);

        [DllImport("tm1api.dll")]
        public static extern int TM1BlobGet(int hUser, int hBlob, int x, int n, string buf);

        [DllImport("tm1api.dll")]
        public static extern int TM1BlobOpen(int hPool, int hBlob);

        [DllImport("tm1api.dll")]
        public static extern int TM1BlobPut(int hUser, int hBlob, int x, int n, string buf);

        // Chore functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ChoreExecute(int hPool, int hChore);

        [DllImport("tm1api.dll")]
        public static extern int TM1ChoreCreateEmpty(int hPool, int hServer);

        // Client functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ClientAdd(int hPool, int hServer, int sClientName);

        [DllImport("tm1api.dll")]
        public static extern int TM1ClientGroupAssign(int hPool, int hClient, int hGroup);

        [DllImport("tm1api.dll")]
        public static extern int TM1ClientGroupIsAssigned(int hPool, int hClient, int hGroup);

        [DllImport("tm1api.dll")]
        public static extern int TM1ClientGroupRemove(int hPool, int hClient, int hGroup);

        [DllImport("tm1api.dll")]
        public static extern int TM1ClientHasHolds(int hPool, int hClient);

        [DllImport("tm1api.dll")]
        public static extern int TM1ClientPasswordAssign(int hPool, int hClient, int sPassword);

        // Connection functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionCheck(int hPool, int hConnection);

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionCreate(int hPool, int hServer, int sMasterServerName, int sUsername, int sPassword);

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionDelete(int hPool, int hConnection);

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionSynchronize(int hPool, int hConnection);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectReplicate(int hPool, int hObject);

        // Cube functions
        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpread(int hPool, int hServer, int vArrayOfCells, int vCellReference, int sSpreadData);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadStatusGet(int hPool, int hServer, int hCells, int hCellRange);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadViewArray(int hPool, int hView, int aCellRange, int aCellRef, int sControl);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellValueGet(int hPool, int hCube, int hArrayOfElements);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellValueSet(int hPool, int hCube, int hArrayOfElements, int hValue);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCreate(int hPool, int hServer, int hArrayOfDimensions);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubePerspectiveCreate(int hPool, int hCube, int hArrayOfElementTitles);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubePerspectiveDestroy(int hPool, int hCube, int hArrayOfElementTitles);

        // Dimension functions
        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionCheck(int hPool, int hDimension);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionCreateEmpty(int hPool, int hServer);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionElementComponentAdd(int hPool, int hElement, int hComponent, int rWeight);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionElementComponentDelete(int hPool, int hCElement, int hElement);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionElementComponentWeightGet(int hPool, int hCElement, int hElement);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionElementDelete(int hPool, int hElement);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionElementInsert(int hPool, int hDimension, int hElementBefore, int sName, int iType);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionUpdate(int hPool, int hOldDimension, int hNewDimension);

        // Drill functions
        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellDrillListGet(int hPool, int hCube, int hArrayOfKeys);

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellDrillObjectBuild(int hPool, int hCube, int hArrayOfKeys, int sDrillProcessName);

        [DllImport("tm1api.dll")]
        public static extern int TM1SQLTableGetNextRows(int hPool, int hSQLTable, int bColumnSelection);

        // Group functions
        [DllImport("tm1api.dll")]
        public static extern int TM1GroupAdd(int hPool, int hServer, int sGroupName);

        // Object functions
        // Object Attribute
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectAttributeDelete(int hPool, int hObject, int hAttribute);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectAttributeInsert(int hPool, int hObject, int hAttributeBefore, int sName, int sType);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectAttributeValueGet(int hPool, int hObject, int hAttribute);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectAttributeValueSet(int hPool, int hObject, int hAttribute, int hValue);

        // Object
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectCopy(int hPool, int hSrcObject, int hDstObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectDelete(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectDestroy(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectDuplicate(int hPool, int hObject);

        // Object File
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectFileDelete(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectFileLoad(int hPool, int hServer, int hParent, int iObjectType, int sObjectName);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectFileSave(int hPool, int hObject);

        // Object List
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectListCountGet(int hPool, int hObject, int iPropertyList);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectListHandleByIndexGet(int hPool, int hObject, int iPropertyList, int iIndex);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectListHandleByNameGet(int hPool, int hObject, int iPropertyList, int sName);

        // Object Private
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPrivateDelete(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPrivateListCountGet(int hPool, int hObject, int iPropertyList);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPrivateListHandleByIndexGet(int hPool, int hObject, int iPropertyList, int iIndex);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPrivateListHandleByNameGet(int hPool, int hObject, int iPropertyList, int sName);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPrivatePublish(int hPool, int hObject, int sName);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPrivateRegister(int hPool, int hParent, int hObject, int sName);

        // Object Property
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPropertyGet(int hPool, int hObject, int vProperty);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPropertySet(int hPool, int hObject, int Property_P, int ValRec_V);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectRegister(int hPool, int hParent, int hObject, int sName);

        // Object Security
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityLock(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityRelease(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityReserve(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityRightGet(int hPool, int hObject, int hGroup);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityRightSet(int hPool, int hObject, int hGroup, int iRight);

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityUnLock(int hPool, int hObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1DataReservationAcquire(int hPool, int hCube, int hClient, int hArrayOfElements);

        [DllImport("tm1api.dll")]
        public static extern int TM1DataReservationRelease(int hPool, int hCube, int hClient, int hArrayOfElements);

        [DllImport("tm1api.dll")]
        public static extern int TM1DataReservationGetAll(int hPool, int hCube, int hClient);

        [DllImport("tm1api.dll")]
        public static extern int TM1DataReservationValidate(int hPool, int hCube);

        [DllImport("tm1api.dll")]
        public static extern int TM1DataReservationReleaseAll(int hPool, int hCube, int hClient, int hArrayOfElements);

        [DllImport("tm1api.dll")]
        public static extern int TM1DataReservationGetConflicts(int hPool, int hCube, int hClient, int hArrayOfElements);

        // Process functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessExecute(int hPool, int hProcess, int hParametersArray);

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessExecuteEx(int hPool, int hProcess, int hParametersArray);

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessCheck(int hPool, int hProcess);

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessCreateEmpty(int hPool, int hServer);

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessExecuteSQLQuery(int tPool, int hProcess, int voDatabaseInfoArray);

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessVariableNameIsValid(int hPool, int hProcess, int hVariableName);

        // Rule functions
        [DllImport("tm1api.dll")]
        public static extern int TM1RuleAttach(int hPool, int hRule);

        [DllImport("tm1api.dll")]
        public static extern int TM1RuleCheck(int hPool, int hRule);

        [DllImport("tm1api.dll")]
        public static extern int TM1RuleCreateEmpty(int hPool, int hCube, int hType);

        [DllImport("tm1api.dll")]
        public static extern int TM1RuleDetach(int hPool, int hRule);

        [DllImport("tm1api.dll")]
        public static extern int TM1RuleLineGet(int hPool, int hRule, int iPosition);

        [DllImport("tm1api.dll")]
        public static extern int TM1RuleLineInsert(int hPool, int hRule, int iPosition, int sLine);

        // Server functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ServerBatchUpdateStart(int hPool, int hServer);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerBatchUpdateIsActive(int hPool, int hServer);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerBatchUpdateFinish(int hPool, int hServer, int bDiscard);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerBatchUpdateGetCommitTime(int hPool, int hServer);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerLogClose(int hPool, int hLog);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerLogNext(int hPool, int hLog);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerLogOpen(int hPool, int hServer, int sStartTime, int sCubeFilter, int sUserFilter, int sFlagFilter);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerOpenSQLQuery(int hPool, int hServer, int hDsnInfo);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerPasswordChange(int hPool, int hServer, int sNewPassword);

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerSecurityRefresh(int hPool, int hServer);

        // Subset functions
        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetAll(int hPool, int hSubset);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetCreateEmpty(int hPool, int hDim);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetCreateByExpression(int hPool, int hServer, int sExpression);

        // Subset Element
        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplay(int hPool, int hSubset, int iElement);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplayEll(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplayLevel(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplayLine(int hUser, int vString, int index);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplayMinus(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplayPlus(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplaySelection(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElementDisplayTee(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern double TM1SubsetElementDisplayWeight(int hUser, int vString);

        // Subset Insert
        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetInsertElement(int hPool, int hSubset, int hElement, int iPosition);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetInsertSubset(int hPool, int hSubsetA, int hSubsetB, int iPosition);

        // Subset Select
        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectByAttribute(int hPool, int hSubset, int hAlias, int sValueToMatch, int bSelection);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectByIndex(int hPool, int hSubset, int iPosition, int bSelection);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectByLevel(int hPool, int hSubset, int iLevel, int bSelection);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectByPattern(int hPool, int hSubset, int sPattern, int hElement);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectionDelete(int hPool, int hSubset);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectionInsertChildren(int hPool, int hSubset);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectionInsertParents(int hPool, int hSubset);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectionKeep(int hPool, int hSubset);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSelectNone(int hPool, int hSubset);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSort(int hPool, int hSubset, int bSortDown);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSortByIndex(int hPool, int hSubset, int bSortDown);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSortByHierarchy(int hPool, int hSubset);

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetUpdate(int hPool, int hOldSubset, int hNewSubset);

        // System functions
        [DllImport("tm1api.dll")]
        public static extern void TM1SystemAdminHostSet(int hUser, string AdminHosts);

        [DllImport("tm1api.dll")]
        public static extern void TM1SystemAdminHostGet(int hUser, string AdminHost);

        [DllImport("tm1api.dll")]
        public static extern void TM1SystemBuildNumber();

        [DllImport("tm1api.dll")]
        public static extern void TM1SystemClose(int hUser);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemOpen();

        [DllImport("tm1api.dll")]
        public static extern void TM1SystemProgressHookSet(int hUser, int pHook);

        [DllImport("tm1api.dll")]
        public static extern string TM1SystemServerClientName(int hUser, int index);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerConnect(int hPool, int sServer, int sClient, int sPassword);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerConnectIntegratedLogin(int hPool, int sServerName);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerDisconnect(int hPool, int hServer);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerHandle(int hUser, int name);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerName(int hUser, int index);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerNof(int hUser);

        [DllImport("tm1api.dll")]
        public static extern void TM1SystemServerReload(int hUser);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerStart(int hUser, string szName, string szDataDirectory, string szAdminHost, string szProtocol, int iPortNumber);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerStartEx(int hUser, string szcmdLine);

        [DllImport("tm1api.dll")]
        public static extern int TM1SystemServerStop(int hUser, string szName, int bSave);

        // Value functions
        // Value Array
        [DllImport("tm1api.dll")]
        public static extern int TM1ValArray(int hPool, ref int[] sArray, int MaxSize);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValArrayGet(int hUser, int vArray, int index);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValArrayMaxSize(int hUser, int vArray);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValArraySet(int vArray, int val, int index);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValArraySetSize(int vArray, int Size);

        // Value Bool
        [DllImport("tm1api.dll")]
        public static extern int TM1ValBool(int hPool, int InitBool);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValBoolGet(int hUser, int vBool);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValBoolSet(int vBool, int Bool);

        // Value Error
        [DllImport("tm1api.dll")]
        public static extern int TM1ValErrorCode(int hUser, int vError);

        [DllImport("tm1api.dll")]
        public static extern string TM1ValErrorString(int hUser, int vValue);

        // Value Index
        [DllImport("tm1api.dll")]
        public static extern int TM1ValIndex(int hPool, int InitIndex);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValIndexGet(int hUser, int vIndex);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValIndexSet(int vIndex, int index);

        // Value
        [DllImport("tm1api.dll")]
        public static extern int TM1ValIsUndefined(int hUser, int Value);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValIsUpdatable(int hUser, int Value);

        // Value Object
        [DllImport("tm1api.dll")]
        public static extern int TM1ValObject(int hPool, ref int InitObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValObjectCanRead(int hUser, int vObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValObjectCanWrite(int hUser, int vObject);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValObjectGet(int hUser, int vObject, string pObject);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValObjectSet(int vObject, string pObject);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValObjectType(int hUser, int vObject);

        // Value Pool
        [DllImport("tm1api.dll")]
        public static extern int TM1ValPoolCount(int hPool);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValPoolCreate(int hUser);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValPoolDestroy(int hPool);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValPoolGet(int hPool, int index);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValPoolMemory(int hPool);

        // Value Real
        [DllImport("tm1api.dll")]
        public static extern int TM1ValReal(int hPool, double InitReal);

        [DllImport("tm1api.dll")]
        public static extern double TM1ValRealGet(int hUser, int vReal);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValRealSet(int vReal, double Real);

        // Value String
        [DllImport("tm1api.dll")]
        public static extern int TM1ValString(int hPool, string InitString, int MaxSize);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValStringEncrypt(int hPool, string InitString, int MaxSize);

        [DllImport("tm1api.dll")]
        public static extern int TM1ValStringMaxSize(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern IntPtr TM1ValStringGet(int hUser, int vString);

        [DllImport("tm1api.dll")]
        public static extern void TM1ValStringSet(int vString, string sString);

        // Value Type
        [DllImport("tm1api.dll")]
        public static extern int TM1ValType(int hUser, int Value);

        // View functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayColumnsNof(int hPool, int hView);

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayConstruct(int hPool, int hView);

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayDestroy(int hPool, int hView);

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayRowsNof(int hPool, int hView);

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayValueGet(int hPool, int hView, int iColumn, int iRow);

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewCreate(int hPool, int hCube, int hTitleSubsetArray, int hColumnSubsetArray, int hRowSubsetArray);

        // ViewExtract functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractCreate(int hPool, int hView);

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractDestroy(int hPool, int hView);

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractGetNext(int hPool, int hView);

        // Constant functions
        [DllImport("tm1api.dll")]
        public static extern int TM1ArrayNull();

        // API properties
        [DllImport("tm1api.dll")]
        public static extern int TM1AttributeType();

        [DllImport("tm1api.dll")]
        public static extern int TM1BlobSize();

        [DllImport("tm1api.dll")]
        public static extern int TM1ChoreActive();

        [DllImport("tm1api.dll")]
        public static extern int TM1ChoreFrequency();

        [DllImport("tm1api.dll")]
        public static extern int TM1ChoreStartTime();

        [DllImport("tm1api.dll")]
        public static extern int TM1ChoreSteps();

        [DllImport("tm1api.dll")]
        public static extern int TM1ChoreExecutionMode();

        // Connection properties
        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionChoresUsing();

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionLastSyncTime();

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionLastSyncTimeStar();

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionPassword();

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionSyncErrorCount();

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionSyncPlanetToStar();

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionSyncStarToPlanet();

        [DllImport("tm1api.dll")]
        public static extern int TM1ConnectionUserName();

        // Client properties
        [DllImport("tm1api.dll")]
        public static extern int TM1ClientPassword();

        [DllImport("tm1api.dll")]
        public static extern int TM1ClientStatus();

        // Cube properties
        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadFunctionOk();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadNumericSetOk();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadStringSetOk();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadStatusHeld();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadStatusHeldConsolidation();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellSpreadStatusWritable();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellValueUndefined();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeDimensions();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeLogChanges();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeMeasuresDimension();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubePerspectivesMaxMemory();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubePerspectivesMinTime();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeRule();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeTimeDimension();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeViews();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeReplicationSyncRule();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeReplicationSyncViews();

        [DllImport("tm1api.dll")]
        public static extern int TM1CubeDataReservationMode();

        // Dimension properties
        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionCubesUsing();

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionElements();

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionNofLevels();

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionSubsets();

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionWidth();

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionReplicationSyncSubsets();

        // Drill properties
        [DllImport("tm1api.dll")]
        public static extern int TM1SQLTableColumnNames();

        [DllImport("tm1api.dll")]
        public static extern int TM1SQLTableColumnTypes();

        [DllImport("tm1api.dll")]
        public static extern int TM1SQLTableNumberOfColumns();

        [DllImport("tm1api.dll")]
        public static extern int TM1SQLTableNumberOfRows();

        [DllImport("tm1api.dll")]
        public static extern int TM1SQLTableRowsetSize();

        // Element properties
        [DllImport("tm1api.dll")]
        public static extern int TM1ElementComponents();

        [DllImport("tm1api.dll")]
        public static extern int TM1ElementIndex();

        [DllImport("tm1api.dll")]
        public static extern int TM1ElementLevel();

        [DllImport("tm1api.dll")]
        public static extern int TM1ElementParents();

        [DllImport("tm1api.dll")]
        public static extern int TM1ElementType();

        // Object properties
        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectAttributes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectChangedSinceLoaded();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectLastTimeUpdated();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectMemoryUsed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectName();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectNull();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectParent();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPrivate();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectPublic();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectRegistration();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityOwner();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectSecurityStatus();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectType();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectUnregistered();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectReplicationConnection();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectReplicationSourceObjectName();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectReplicationStatus();

        // Progress properties
        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionCalculatingSubsetAll();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionCalculatingSubsetHierarchy();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionCalculatingView();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionDeletingSelection();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionDuplicatingSubset();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionInsertingSubset();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionKeepingSelection();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionLoadingCube();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionLoadingDimension();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionLoadingSubset();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionRunningQuery();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionSavingSubset();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionSelectingSubsetElements();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressActionSortingSubset();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressMessageClosing();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressMessageOpening();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressMessageRunning();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressTypeCounter();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProgressTypePercent();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessChoresUsing();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessComplete();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataProcedure();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceASCIIDecimalSeparator();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceASCIIDelimiter();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceASCIIHeaderRecords();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceASCIIQuoteCharacter();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceASCIIThousandSeparator();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceCubeView();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceDimensionSubset();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceNameForClient();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceNameForServer();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceOleDbLocation();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceOleDbMdp();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourcePassword();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceQuery();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceType();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessDataSourceUserName();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessEpilogProcedure();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessGrantSecurityAccess();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessMetaDataProcedure();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessParametersDefaultValues();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessParametersNames();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessParametersPromptStrings();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessParametersTypes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessPrologProcedure();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessUIData();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessVariablesEndingBytes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessVariablesNames();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessVariablesPositions();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessVariablesStartingBytes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessVariablesTypes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ProcessVariablesUIData();

        // Rule properties
        [DllImport("tm1api.dll")]
        public static extern int TM1RuleErrorLine();

        [DllImport("tm1api.dll")]
        public static extern int TM1RuleErrorString();

        [DllImport("tm1api.dll")]
        public static extern int TM1RuleNofLines();

        // Security properties
        [DllImport("tm1api.dll")]
        public static extern int TM1SecurityRightAdmin();

        [DllImport("tm1api.dll")]
        public static extern int TM1SecurityRightLock();

        [DllImport("tm1api.dll")]
        public static extern int TM1SecurityRightNone();

        [DllImport("tm1api.dll")]
        public static extern int TM1SecurityRightRead();

        [DllImport("tm1api.dll")]
        public static extern int TM1SecurityRightReserve();

        [DllImport("tm1api.dll")]
        public static extern int TM1SecurityRightWrite();

        // Server properties
        [DllImport("tm1api.dll")]
        public static extern int TM1ServerBlobs();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerClients();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerCubes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerDimensions();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerDirectories();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerGroups();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerLogDirectory();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerNetworkAddress();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerChores();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerProcesses();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerBuildNumber();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerConnections();

        [DllImport("tm1api.dll")]
        public static extern int TM1ServerProcessObjectsSupported();

        // Subset properties
        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetAlias();

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetElements();

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetExpression();

        // System properties
        [DllImport("tm1api.dll")]
        public static extern int TM1SystemVersionGet();

        // Type functions
        [DllImport("tm1api.dll")]
        public static extern int TM1TypeAttribute();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeAttributeAlias();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeAttributeNumeric();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeAttributeString();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeBlob();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeClient();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeCube();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeDimension();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeChore();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeConnection();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeElement();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeElementConsolidated();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeElementSimple();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeElementString();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeGroup();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeProcess();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeRule();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeRuleCalculation();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeRuleDrill();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeServer();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeSQLTable();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeSQLNumericColumn();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeSQLStringColumn();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeSQLNotSupported();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeSubset();

        [DllImport("tm1api.dll")]
        public static extern int TM1TypeView();

        // Value properties
        [DllImport("tm1api.dll")]
        public static extern int TM1ValTypeArray();

        [DllImport("tm1api.dll")]
        public static extern int TM1ValTypeBool();

        [DllImport("tm1api.dll")]
        public static extern int TM1ValTypeError();

        [DllImport("tm1api.dll")]
        public static extern int TM1ValTypeIndex();

        [DllImport("tm1api.dll")]
        public static extern int TM1ValTypeObject();

        [DllImport("tm1api.dll")]
        public static extern int TM1ValTypeReal();

        [DllImport("tm1api.dll")]
        public static extern int TM1ValTypeString();

        // View properties
        // View Array
        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayCellFormatString();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayCellFormattedValue();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayCellOrdinal();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayCellValue();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayMemberDescription();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayMemberName();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewArrayMemberType();

        // View
        [DllImport("tm1api.dll")]
        public static extern int TM1ViewColumnSubsets();

        // View Extract
        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonEQ_A();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonGE_A();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonGE_A_LE_B();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonGT_A();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonGT_A_LT_B();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonLE_A();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonLT_A();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonNE_A();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparisonNone();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractComparison();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractRealLimitA();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractRealLimitB();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractSkipConsolidatedValues();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractSkipZeroes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractSkipRuleValues();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractStringLimitA();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewExtractStringLimitB();

        // View
        [DllImport("tm1api.dll")]
        public static extern int TM1ViewFormat();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewPreConstruct();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewRowSubsets();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewShowAutomatically();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewSuppressZeroes();

        // View Title
        [DllImport("tm1api.dll")]
        public static extern int TM1ViewTitleElements();

        [DllImport("tm1api.dll")]
        public static extern int TM1ViewTitleSubsets();

        // TM1 Errors
        // Error Blob
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorBlobCloseFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorBlobCreateFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorBlobGetFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorBlobNotOpen();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorBlobOpenFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorBlobPutFailed();

        // Error Client
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorClientAlreadyExists();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorClientPasswordNotDefined();

        // Error Cube
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellValueTypeMismatch();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCreationFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeDimensionInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeKeyInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeMeasuresAndTimeDimensionSame();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeNotEnoughDimensions();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeNoTimeDimension();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeNoMeasuresDimension();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeNumberOfKeysInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubePerspectiveAllSimpleElements();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubePerspectiveCreationFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeTooManyDimensions();

        // Error Dimension
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionCouldNotBeCompiled();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionElementAlreadyExists();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionElementComponentAlreadyExists();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionElementComponentDoesNotExist();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionElementComponentNotNumeric();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionElementDoesNotExist();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionElementNotConsolidated();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionHasCircularReferences();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionHasNoElements();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionIsBeingUsedByCube();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionNotChecked();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionNotRegistered();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDimensionUpdateFailedInvalidHierarchies();

        // Error Drill
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeDrillNotFound();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeDrillInvalidStructure();

        // Error Group
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorGroupAlreadyExists();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorGroupMaximunNumberExceeded();

        // Error Object
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectAttributeInvalidType();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectAttributeNotDefined();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectDeleted();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectDuplicationFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectFileInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectFileNotFound();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectFunctionDoesNotApply();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectHandleInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectHasNoParent();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectIncompatibleTypes();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectIndexInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectIsRegistered();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectIsUnregistered();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectListIsEmpty();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectNameExists();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectNameInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectNameIsBlank();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectNotFound();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectNotLoaded();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectPropertyIsList();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectPropertyNotDefined();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectPropertyNotList();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectRegistrationFailed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectSecurityIsLocked();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectSecurityNoAdminRights();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectSecurityNoLockRights();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectSecurityNoReadRights();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectSecurityNoReserveRights();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorObjectSecurityNoWriteRights();

        // Error Rule
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorRuleCubeHasRuleAttached();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorRuleIsAttached();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorRuleIsNotChecked();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorRuleLineNotFound();

        // Error Spreading
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusCubeNoWriteAccess();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusCubeReserved();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusCubeLocked();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusElementIsConsolidated();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusElementNoWriteAccess();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusElementReserved();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusElementLocked();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusRuleApplies();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusNoReservation();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorCubeCellWriteStatusCellReserved();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorDataSpreadFailed();

        // Error Subset
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSubsetIsBeingUsedByView();

        // Error System
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemFunctionObsolete();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemOutOfMemory();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemParameterTypeInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemServerClientAlreadyConnected();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemServerClientNotConnected();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemServerClientNotFound();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemServerClientPasswordInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemServerNotFound();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemUserHandleInvalid();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorSystemValueInvalid();

        // Error Update
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorUpdateNonLeafCellValueFailed();

        // Error View
        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorViewExpressionEmpty();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorViewHasPrivateSubsets();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorViewNotConstructed();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorViewNotMdxView();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorViewNotNativeView();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorChoreModifiedDuringExecution();

        [DllImport("tm1api.dll")]
        public static extern int TM1ErrorChoreDeleted();

        // Obsolete functions
        [DllImport("tm1api.dll")]
        public static extern int TM1CubeCellDrillStringGet(int hPool, int hCube, int hArrayOfElements);

        [DllImport("tm1api.dll")]
        public static extern int TM1DimensionTopElement();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectReplication();

        [DllImport("tm1api.dll")]
        public static extern int TM1ObjectReplicationSourceName();

        [DllImport("tm1api.dll")]
        public static extern int TM1SubsetSubtract(int hPool, int hSubsetA, int hSubsetB);
    }
}