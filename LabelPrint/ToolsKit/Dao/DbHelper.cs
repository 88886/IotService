

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.SessionState;

namespace PrintX.Dev.Utils.ToolsKit
{
    public sealed class Settings
    {
        private static int defaultWebRequestTimeout;

        private static int decimalPrecision;

        private static int decimalScale;

        private static int decimalIntDigits;

        private static int minClientWidth;

        private static int minClientHeight;



        private static string gridColumnDefaultHeaderAlignCache;

        private static string gridRowNoColumnCaption;

        private static string okButtonText;

        private static string closeButtonText;

        private static string cancelButtonText;

        private static bool buttonAutoWidth;

        private static bool numberEditDefaultShowCalculator;



        private static string gotoPageButtonText;

        private static string requiredCPrintVersion;

        private static string cPrintDataDirName;

        private static bool enableDebugJson;

        private static bool cPrintExecuteNotUseIEDialog;

        private static bool cPrintExecuteDirectPreview;

        private static bool enableSaveMDIPageLocation;

        private static bool enableSaveClientLog;

        private static string editFilterChars;

        private static object defaultSelectorValue;

        private static int maxMDIPageCount;

        private static bool usePrintButton;

        private static bool gridPagerDefaultShowRefreshButton;

        private static bool gridDefaultAllowConfig;

        private static bool gridDefaultAllowFrozen;

        private static bool gridDefaultAllowPopup;

        private static bool gridDefaultAllowResize;

        private static bool gridDefaultShowRowIndicator;

        private static bool gridDefaultAllowCopy;

        private static bool gridDefaultAutoPagerPageSize;

        private static bool gridMultiSelectColumnDefaultShowHeaderCheckBox;

        private static int gridDefaultRowHeight;

        private static string logModuleConnectionString;

        private static bool dbPagerDataSourceDefaultUseMemorySort;

        private static string clientWidthContextName;

        private static string clientHeightContextName;



        private static IDbHelperCreator dbHelperCreator;



        private static object registerLock;





        internal static readonly bool DisableErrorAlert;

        internal static readonly bool AddP3PHeader;



        public static int DefaultWebRequestTimeout
        {
            get
            {
                return Settings.defaultWebRequestTimeout;
            }
            set
            {
                Settings.defaultWebRequestTimeout = value;

            }
        }

        public static bool UsePrintButton
        {
            get
            {
                return Settings.usePrintButton;
            }
            set
            {
                Settings.usePrintButton = value;
            
            }
        }



        public static IDbHelperCreator DbHelperCreator
        {
            get
            {
                return Settings.dbHelperCreator;
            }
            set
            {
                Settings.dbHelperCreator = value;
            }
        }




        public static int DecimalPrecision
        {
            get
            {
                return Settings.decimalPrecision;
            }
            set
            {
                Settings.decimalPrecision = value;
            }
        }

        public static int DecimalScale
        {
            get
            {
                return Settings.decimalScale;
            }
            set
            {
                Settings.decimalScale = value;
            }
        }

        public static int MinClientHeight
        {
            get
            {
                return Settings.minClientHeight;
            }
            set
            {
                Settings.minClientHeight = value;
            }
        }

        public static int MinClientWidth
        {
            get
            {
                return Settings.minClientWidth;
            }
            set
            {
                Settings.minClientWidth = value;
            }
        }

        public static int DecimalIntDigits
        {
            get
            {
                return Settings.decimalIntDigits;
            }
            set
            {
                Settings.decimalIntDigits = value;
            }
        }

        internal static int MaxIntLength
        {
            get
            {
                int result;
                if (Settings.decimalIntDigits > 0)
                {
                    result = Settings.decimalIntDigits;
                }
                else
                {
                    int num = Settings.DecimalPrecision - Settings.DecimalScale;
                    result = ((num > 0) ? num : 0);
                }
                return result;
            }
        }

        public static string GridRowNoColumnCaption
        {
            get
            {
                return Settings.gridRowNoColumnCaption;
            }
            set
            {
                Settings.gridRowNoColumnCaption = value;
            }
        }



        internal static string GridColumnDefaultHeaderAlignCache
        {
            get
            {
                return Settings.gridColumnDefaultHeaderAlignCache;
            }
        }

        public static bool GridPagerDefaultShowRefreshButton
        {
            get
            {
                return Settings.gridPagerDefaultShowRefreshButton;
            }
            set
            {
                Settings.gridPagerDefaultShowRefreshButton = value;

            }
        }

        public static bool GridDefaultAllowCopy
        {
            get
            {
                return Settings.gridDefaultAllowCopy;
            }
            set
            {
                Settings.gridDefaultAllowCopy = value;

            }
        }

        public static bool GridDefaultAllowPopup
        {
            get
            {
                return Settings.gridDefaultAllowPopup;
            }
            set
            {
                Settings.gridDefaultAllowPopup = value;

            }
        }

        public static bool GridDefaultAllowConfig
        {
            get
            {
                return Settings.gridDefaultAllowConfig;
            }
            set
            {
                Settings.gridDefaultAllowConfig = value;

            }
        }

        public static bool GridDefaultAllowFrozen
        {
            get
            {
                return Settings.gridDefaultAllowFrozen;
            }
            set
            {
                Settings.gridDefaultAllowFrozen = value;

            }
        }

        public static bool GridDefaultAllowResize
        {
            get
            {
                return Settings.gridDefaultAllowResize;
            }
            set
            {
                Settings.gridDefaultAllowResize = value;

            }
        }

        public static bool GridDefaultShowRowIndicator
        {
            get
            {
                return Settings.gridDefaultShowRowIndicator;
            }
            set
            {
                Settings.gridDefaultShowRowIndicator = value;

            }
        }

        public static bool GridDefaultAutoPagerPageSize
        {
            get
            {
                return Settings.gridDefaultAutoPagerPageSize;
            }
            set
            {
                Settings.gridDefaultAutoPagerPageSize = value;
            }
        }

        public static bool GridMultiSelectColumnDefaultShowHeaderCheckBox
        {
            get
            {
                return Settings.gridMultiSelectColumnDefaultShowHeaderCheckBox;
            }
            set
            {
                Settings.gridMultiSelectColumnDefaultShowHeaderCheckBox = value;

            }
        }

        public static int GridDefaultRowHeight
        {
            get
            {
                return Settings.gridDefaultRowHeight;
            }
            set
            {
                Settings.gridDefaultRowHeight = value;

            }
        }

        public static string CustomHelpHTML
        {
            set
            {

            }
        }

        public static bool ButtonAutoWidth
        {
            get
            {
                return Settings.buttonAutoWidth;
            }
            set
            {
                Settings.buttonAutoWidth = value;

            }
        }

        public static string OkButtonText
        {
            get
            {
                return Settings.okButtonText;
            }
            set
            {
                Settings.okButtonText = value;
            }
        }

        public static string CloseButtonText
        {
            get
            {
                return Settings.closeButtonText;
            }
            set
            {
                Settings.closeButtonText = value;
            }
        }

        public static string CancelButtonText
        {
            get
            {
                return Settings.cancelButtonText;
            }
            set
            {
                Settings.cancelButtonText = value;
            }
        }

        public static bool NumberEditDefaultShowCalculator
        {
            get
            {
                return Settings.numberEditDefaultShowCalculator;
            }
            set
            {
                Settings.numberEditDefaultShowCalculator = value;

            }
        }

        public static string EditFilterChars
        {
            get
            {
                return Settings.editFilterChars;
            }
            set
            {
                Settings.editFilterChars = value;

            }
        }

        public static object DefaultSelectorValue
        {
            get
            {
                return Settings.defaultSelectorValue;
            }
            set
            {

                Settings.defaultSelectorValue = value;
            }
        }

        public static string GotoPageButtonText
        {
            get
            {
                return Settings.gotoPageButtonText;
            }
            set
            {
                Settings.gotoPageButtonText = value;

            }
        }

        public static bool EnableDebugJson
        {
            get
            {
                return Settings.enableDebugJson;
            }
            set
            {
                Settings.enableDebugJson = value;
            }
        }

        public static string RequiredCPrintVersion
        {
            get
            {
                return Settings.requiredCPrintVersion;
            }
            set
            {
                Settings.requiredCPrintVersion = value;

            }
        }

        public static string CPrintDataDirName
        {
            get
            {
                return Settings.cPrintDataDirName;
            }
            set
            {
                Settings.cPrintDataDirName = value;

            }
        }

        public static bool EnableSaveMDIPageLocation
        {
            get
            {
                return Settings.enableSaveMDIPageLocation;
            }
            set
            {
                Settings.enableSaveMDIPageLocation = value;

            }
        }

        public static bool EnableSaveClientLog
        {
            get
            {
                return Settings.enableSaveClientLog;
            }
            set
            {
                Settings.enableSaveMDIPageLocation = value;

            }
        }

        public static bool CPrintExecuteNotUseIEDialog
        {
            get
            {
                return Settings.cPrintExecuteNotUseIEDialog;
            }
            set
            {
            }
        }

        public static bool CPrintExecuteDirectPreview
        {
            get
            {
                return Settings.cPrintExecuteDirectPreview;
            }
            set
            {
                Settings.cPrintExecuteDirectPreview = value;

            }
        }

        public static int MaxMDIPageCount
        {
            get
            {
                return Settings.maxMDIPageCount;
            }
            set
            {

                Settings.maxMDIPageCount = value;
            }
        }

        public static string LogModuleConnectionString
        {
            get
            {
                return Settings.logModuleConnectionString;
            }
            set
            {
                Settings.logModuleConnectionString = value;
            }
        }

        public static string DbConfigStoreConnectionString
        {
            get;
            set;
        }

        public static bool DbPagerDataSourceDefaultUseMemorySort
        {
            get
            {
                return Settings.dbPagerDataSourceDefaultUseMemorySort;
            }
            set
            {
                Settings.dbPagerDataSourceDefaultUseMemorySort = value;
            }
        }

        public static string ClientWidthContextName
        {
            get
            {
                return Settings.clientWidthContextName;
            }
            set
            {
                Settings.clientWidthContextName = value;
            }
        }

        public static string ClientHeightContextName
        {
            get
            {
                return Settings.clientHeightContextName;
            }
            set
            {
                Settings.clientHeightContextName = value;
            }
        }



        internal static System.Collections.Generic.IDictionary<string, int> ClientCacheVersions
        {
            get { return null; }
        }

        public static bool UseCarpaReport
        {
            get;
            set;
        }

        public static bool OnlyUseCarpaReport
        {
            get;
            set;
        }

        public static string ProfileKeySessionVarName
        {
            get;
            set;
        }

        public static bool UseReportDataFromCarpaServer
        {
            get;
            set;
        }





        public static bool DbPagerDataSourceRoundValueForSummary
        {
            get;
            set;
        }





        public static bool HtmlEditorEnableUploadImage
        {
            get;
            set;
        }

        public static int DefaultGridSpace
        {
            get;
            set;
        }

        public static string PrintProductName
        {
            get;
            set;
        }

        static Settings()
        {
            Settings.defaultWebRequestTimeout = 0;
            Settings.decimalPrecision = 14;
            Settings.decimalScale = 4;
            Settings.decimalIntDigits = 0;
            Settings.minClientWidth = 100;
            Settings.minClientHeight = 100;

            Settings.gridColumnDefaultHeaderAlignCache = null;
            Settings.gridRowNoColumnCaption = "行号";
            Settings.okButtonText = "确定";
            Settings.closeButtonText = "关闭";
            Settings.cancelButtonText = "取消";
            Settings.numberEditDefaultShowCalculator = false;

            Settings.gotoPageButtonText = null;
            Settings.requiredCPrintVersion = null;
            Settings.cPrintDataDirName = null;
            Settings.enableDebugJson = false;
            Settings.cPrintExecuteNotUseIEDialog = false;
            Settings.cPrintExecuteDirectPreview = false;
            Settings.enableSaveMDIPageLocation = false;
            Settings.enableSaveClientLog = true;
            Settings.editFilterChars = null;
            Settings.defaultSelectorValue = null;
            Settings.maxMDIPageCount = 20;
            Settings.usePrintButton = false;
            Settings.gridPagerDefaultShowRefreshButton = false;
            Settings.gridDefaultAllowConfig = false;
            Settings.gridDefaultAllowFrozen = true;
            Settings.gridDefaultAllowPopup = false;
            Settings.gridDefaultAllowResize = false;
            Settings.gridDefaultShowRowIndicator = false;
            Settings.gridDefaultAllowCopy = false;
            Settings.gridDefaultAutoPagerPageSize = false;
            Settings.gridMultiSelectColumnDefaultShowHeaderCheckBox = false;
            Settings.gridDefaultRowHeight = 21;
            Settings.logModuleConnectionString = null;
            Settings.dbPagerDataSourceDefaultUseMemorySort = false;
            Settings.clientWidthContextName = null;
            Settings.clientHeightContextName = null;

            Settings.registerLock = new object();
            Settings.DisableErrorAlert = AppSettings.GetBool("Carpa.DisableErrorAlert");
            Settings.AddP3PHeader = AppSettings.GetBool("Carpa.AddP3PHeader");
            int @int = AppSettings.GetInt("Carpa.DefaultWebRequestTimeout");
            if (@int > 0)
            {
                Settings.DefaultWebRequestTimeout = @int;
            }
            bool flag;
            if (Settings.GetConfig("Carpa.EnableDebugJson", out flag))
            {
                Settings.EnableDebugJson = flag;
            }
            if (Settings.GetConfig("Carpa.UseCarpaReport", out flag))
            {
                Settings.UseCarpaReport = flag;
            }
            if (Settings.GetConfig("Carpa.CPrintExecuteNotUseIEDialog", out flag))
            {
                Settings.CPrintExecuteNotUseIEDialog = flag;
            }
            if (Settings.GetConfig("Carpa.CPrintExecuteDirectPreview", out flag))
            {
                Settings.CPrintExecuteDirectPreview = flag;
            }
            if (Settings.GetConfig("Carpa.EnableSaveMDIPageLocation", out flag))
            {
                Settings.EnableSaveMDIPageLocation = flag;
            }
            if (Settings.GetConfig("Carpa.EnableSaveClientLog", out flag))
            {
                Settings.EnableSaveClientLog = flag;
            }
            if (Settings.GetConfig("Carpa.GridDefaultAllowFrozen", out flag))
            {
                Settings.GridDefaultAllowFrozen = flag;
            }
            if (Settings.GetConfig("Carpa.AntiHijack", out flag) && flag)
            {

            }
            if (Settings.GetConfig("Carpa.UseReportDataFromCarpaServer", out flag))
            {
                Settings.UseReportDataFromCarpaServer = flag;
            }
            string @string = AppSettings.GetString("Carpa.WebExportMode");
            if (!string.IsNullOrEmpty(@string))
            {
                if (string.Equals(@string.ToLower(), "npoi", System.StringComparison.OrdinalIgnoreCase))
                {

                }
            }
            Settings.GridMultiSelectColumnDefaultShowHeaderCheckBox = false;

            Settings.HtmlEditorEnableUploadImage = true;
            Settings.DefaultGridSpace = 8;
        }

        private static bool GetConfig(string key, out bool value)
        {
            value = false;
            string @string = AppSettings.GetString(key);
            bool result;
            if (!string.IsNullOrEmpty(@string))
            {
                value = string.Equals(@string, "true", System.StringComparison.OrdinalIgnoreCase);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private Settings()
        {
        }





    }

    internal static class DbParameterHelper
    {
        private static System.Collections.Hashtable dbCaches;

        static DbParameterHelper()
        {
            DbParameterHelper.dbCaches = new System.Collections.Hashtable();
        }

        public static void DeriveParamters(DbHelper helper)
        {
            CommandType commandType = helper.Command.CommandType;
            if (commandType != CommandType.Text)
            {
                if (commandType != CommandType.StoredProcedure)
                {
                    if (commandType == CommandType.TableDirect)
                    {
                        throw new System.InvalidOperationException("当CommandType为TableDirect时不能有参数！");
                    }
                }
                else
                {
                    DbParameterHelper.DeriveSPParameters(helper.Command);
                }
            }
            else
            {
                DbParameterHelper.DeriveSQLParameters();
            }
        }

        private static void DeriveSQLParameters()
        {
            throw new System.InvalidOperationException("暂时不支持！");
        }

        private static void DeriveSPParameters(DbCommand cmd)
        {
            string connectionString = cmd.Connection.ConnectionString;
            System.Collections.Hashtable sPCaches = DbParameterHelper.GetSPCaches(connectionString);
            bool flag;
            System.Collections.Generic.List<SqlParameter> sPParameters = DbParameterHelper.GetSPParameters(sPCaches, cmd, out flag);
            if (!flag)
            {
                cmd.Parameters.AddRange(DbParameterHelper.CloneParamters(sPParameters).ToArray());
            }
        }

        private static System.Collections.Generic.List<SqlParameter> GetSPParameters(System.Collections.Hashtable spCaches, DbCommand cmd, out bool firstDerive)
        {
            string commandText = cmd.CommandText;
            System.Collections.Generic.List<SqlParameter> list = (System.Collections.Generic.List<SqlParameter>)spCaches[commandText];
            firstDerive = false;
            if (list == null)
            {
                lock (spCaches.SyncRoot)
                {
                    list = (System.Collections.Generic.List<SqlParameter>)spCaches[commandText];
                    if (list == null)
                    {
                        SqlCommandBuilder.DeriveParameters((SqlCommand)cmd);
                        if (cmd.Parameters[0].ParameterName == "@RETURN_VALUE")
                        {
                            cmd.Parameters.RemoveAt(0);
                        }
                        list = new System.Collections.Generic.List<SqlParameter>();
                        list = DbParameterHelper.CloneParamters(cmd.Parameters);
                        spCaches[commandText] = list;
                        firstDerive = true;
                    }
                }
            }
            return list;
        }

        private static System.Collections.Generic.List<SqlParameter> CloneParamters(System.Collections.ICollection parameters)
        {
            System.Collections.Generic.List<SqlParameter> list = new System.Collections.Generic.List<SqlParameter>();
            foreach (SqlParameter sqlParameter in parameters)
            {
                list.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.SqlDbType, sqlParameter.Size)
                {
                    Direction = sqlParameter.Direction,
                    Precision = sqlParameter.Precision,
                    Scale = sqlParameter.Scale
                });
            }
            return list;
        }

        private static System.Collections.Hashtable GetSPCaches(string dbKey)
        {
            System.Collections.Hashtable hashtable = (System.Collections.Hashtable)DbParameterHelper.dbCaches[dbKey];
            System.Collections.Hashtable result;
            if (hashtable == null)
            {
                lock (DbParameterHelper.dbCaches.SyncRoot)
                {
                    hashtable = (System.Collections.Hashtable)DbParameterHelper.dbCaches[dbKey];
                    if (hashtable == null)
                    {
                        hashtable = new System.Collections.Hashtable();
                        DbParameterHelper.dbCaches[dbKey] = hashtable;
                        result = hashtable;
                        return result;
                    }
                }
            }
            result = hashtable;
            return result;
        }
    }

    internal enum DbHelperCommandKind
    {
        BeginTransaction,
        ExecuteSQL,
        ExecuteProcedure
    }

    public interface IDbHelperCreator
    {
        DbHelper CreateDbHelper();
    }

    internal sealed class DbHelperState
    {
        private DbConnection connection;

        private string connectionString;

        private DbType dbType;

        private bool hasBegunTransaction;

        private DbHelperCommandKind commandKind;

        private string commandText;

        private bool isSelecting;

        public DbConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
        }

        public DbType DbType
        {
            get
            {
                return this.dbType;
            }
        }

        public bool HasBegunTransaction
        {
            get
            {
                return this.hasBegunTransaction;
            }
        }

        public DbHelperCommandKind CommandKind
        {
            get
            {
                return this.commandKind;
            }
        }

        public string CommandText
        {
            get
            {
                return this.commandText;
            }
        }

        public bool IsSelecting
        {
            get
            {
                return this.isSelecting;
            }
        }

        internal DbHelperState(DbConnection connection, string connectionString, DbType dbType, bool hasBegunTransaction, DbHelperCommandKind commandKind, string commandText, bool isSelecting)
        {
            this.connection = connection;
            this.connectionString = connectionString;
            this.dbType = dbType;
            this.hasBegunTransaction = hasBegunTransaction;
            this.commandKind = commandKind;
            this.commandText = commandText;
            this.isSelecting = isSelecting;
        }
    }


    internal interface IDbCloudProvider
    {
        bool OpenConnection(DbHelperState state);
    }


    public class DbHelper : System.IDisposable
    {
        public class _ScriptDbItemList
        {
            public int dataType = 1;

            public string[] fields;

            public object[] rows;

            public int[] fieldSizes;

            public int[] fieldTypes;
        }

        public delegate void SqlStatementExecutedEventHandler(object sender, string statementText, int line, int position);

        private const string contextDbHelperKey = "__ContextDbHelper";

        private const string dbHelperTrackListKey = "__DbHelperTrackList";

        private string connectionString;

        private DbType dbType;

        private bool fieldNameToLower;

        private DbConnection connection;

        private DbCommand command;

        private bool isDispose = false;

        private bool isDebuggingSql;

        private bool isLoggingSql;

        private bool isTrackingUsage = true;

        private Stopwatch stopwatch;



        private System.DateTime startTime;

        private bool hasError;

        private bool hasMysqlTimeoutError;

        private string lastNonTransactionSql;

        private IHashMap outputParameters;

        private System.Collections.Generic.IDictionary<string, string> specialParameterNames;

        private static readonly bool IsLogSqlEnabled;

        private static readonly int LogSqlElapsedThreshold;

        private static readonly int DefaultCommandTimeout;

        private static readonly int DefaultConnectTimeout;

        private static readonly int MinPoolSize;

        private static readonly int MaxPoolSize;

        private static readonly bool IsSaveDebugSql;

        private static readonly bool IsGlobalTrackingUsage;

        private bool idParamToUlong = AppSettings.GetBool("DbHelper.CheckIdParamAsUlong");

        private static readonly bool CheckIdParamAsUlong;

        private static System.Collections.Hashtable mysqlConnectionInfos;

        private DbHelper.SqlStatementExecutedEventHandler onStatementExecuted;

        private static System.Collections.Generic.IDictionary<int, System.Collections.IList> dbHelperTrackThread;

        private static DbHelper testDbHelper;

        public bool IdParamToUlong
        {
            get
            {
                return this.idParamToUlong;
            }
            set
            {
                this.idParamToUlong = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return this.command.CommandTimeout;
            }
            set
            {
                this.command.CommandTimeout = value;
            }
        }

        public bool IsLoggingSql
        {
            get
            {
                return this.isLoggingSql;
            }
            set
            {
                this.isLoggingSql = value;
            }
        }

        private bool IsTrackingUsage
        {
            get
            {
                return DbHelper.IsGlobalTrackingUsage && this.isTrackingUsage;
            }
        }

        public DbType DbType
        {
            get
            {
                return this.dbType;
            }
        }

        public DbConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        public DbCommand Command
        {
            get
            {
                return this.command;
            }
        }

        public bool FieldNameToLower
        {
            get
            {
                return this.fieldNameToLower;
            }
            set
            {
                this.fieldNameToLower = value;
            }
        }

        public long LastInsertedId
        {
            get
            {
                long result;
            
                
                {
                    if (!(this.command is SqlCommand))
                    {
                        throw new System.InvalidOperationException("不支持LastInsertedId：" + this.command.GetType());
                    }
                    result = System.Convert.ToInt64(this.ExecuteScalerSQL("select @@IDENTITY"));
                }
                return result;
            }
        }

        public bool HasBegunTransaction
        {
            get
            {
                return this.command.Transaction != null;
            }
        }

        public IHashMap OutputParameters
        {
            get
            {
                if (this.outputParameters == null)
                {
                    throw new System.InvalidOperationException("还没有调用过存储过程");
                }
                return this.outputParameters;
            }
        }

        internal static System.Collections.Hashtable MysqlConnectionInfos
        {
            get
            {
                return DbHelper.mysqlConnectionInfos;
            }
        }

        internal static IDbCloudProvider CloudProvider
        {
            get;
            set;
        }

        public bool IsSelecting
        {
            get;
            set;
        }

        static DbHelper()
        {
            DbHelper.IsLogSqlEnabled = AppSettings.GetBool("LogModule.LogSql");
            DbHelper.LogSqlElapsedThreshold = AppSettings.GetInt("LogModule.LogSqlElapsedThreshold");
            DbHelper.DefaultCommandTimeout = AppSettings.GetInt("DbHelper.CommandTimeout");
            DbHelper.DefaultConnectTimeout = AppSettings.GetInt("DbHelper.ConnectTimeout");
            DbHelper.MinPoolSize = AppSettings.GetInt("DbHelper.MinPoolSize");
            DbHelper.MaxPoolSize = AppSettings.GetInt("DbHelper.MaxPoolSize");
            DbHelper.IsSaveDebugSql = AppSettings.GetBool("LogModule.SaveDebugSql");
            DbHelper.IsGlobalTrackingUsage = AppSettings.GetBool("DbHelper.UsageTracking");
            DbHelper.CheckIdParamAsUlong = AppSettings.GetBool("DbHelper.CheckIdParamAsUlong");
            DbHelper.mysqlConnectionInfos = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            DbHelper.dbHelperTrackThread = new System.Collections.Generic.Dictionary<int, System.Collections.IList>();
            string @string = AppSettings.GetString("DbHelper.CloudProviderType");
            if (!string.IsNullOrEmpty(@string))
            {
                System.Type type = System.Type.GetType(@string, false);
                if (type == null)
                {
                    throw new System.InvalidOperationException("DbHelper.CloudProviderType 指定的类不存在：" + @string);
                }
                DbHelper.CloudProvider = (IDbCloudProvider)System.Activator.CreateInstance(type);
            }
        }

        public DbHelper(DbConnection connection, bool fieldNameToLower, bool isTrackingUsage)
        {
            this.isTrackingUsage = isTrackingUsage;
            this.TrackCreate();
            this.connection = connection;
            this.connectionString = connection.ConnectionString;
            this.dbType = DbHelper.GetDbType(this.connectionString);
            this.command = connection.CreateCommand();
            if (DbHelper.DefaultCommandTimeout > 0)
            {
                this.command.CommandTimeout = DbHelper.DefaultCommandTimeout;
            }
            this.fieldNameToLower = fieldNameToLower;
            bool flag = false;


        }

        public DbHelper(DbConnection connection, bool fieldNameToLower)
            : this(connection, fieldNameToLower, true)
        {
        }

        public DbHelper(DbConnection connection)
            : this(connection, false)
        {
        }

        public DbHelper(string connectionString, bool fieldNameToLower, bool isTrackingUsage)
            : this(DbHelper.CreateConnection(connectionString), fieldNameToLower, isTrackingUsage)
        {
        }

        public DbHelper(string connectionString, bool fieldNameToLower)
            : this(connectionString, fieldNameToLower, true)
        {
        }

        public DbHelper(string connectionString)
            : this(connectionString, false)
        {
        }

        public void Dispose()
        {
            this.Connection.Close();
            this.isDispose = true;
            this.Dispose(true);
           
            System.GC.SuppressFinalize(this);
            this.TrackDispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.connection.Close();
            }
        }

        public DbParameter AddParameter(string parameterName, object value)
        {
            return this.InternalAddParameter(parameterName, value, true);
        }

        public void AddInParameters<T>(string parameterName, System.Collections.Generic.IList<T> valueList)
        {
            parameterName = parameterName.TrimStart(new char[]
			{
				'@'
			}).TrimEnd(new char[]
			{
				' '
			});
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < valueList.Count; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(",");
                }
                string text = string.Format("@_{0}_{1}", parameterName, i);
                stringBuilder.Append(text);
                this.AddParameter(text, valueList[i]);
            }
            this.SaveSpeciaParameter(parameterName, stringBuilder.ToString());
        }

        private void SaveSpeciaParameter(string parameterName, string values)
        {
            if (this.specialParameterNames == null)
            {
                this.specialParameterNames = new System.Collections.Generic.Dictionary<string, string>();
            }
            this.specialParameterNames.Add(string.Format("@{0}", parameterName), values);
        }

        public DbParameter AddLikeParamter(string parameterName, object value)
        {
            return this.InternalAddParameter(parameterName, string.Format("%{0}%", value), false);
        }

        public DbParameter AddLeftLikeParamter(string parameterName, object value)
        {
            return this.InternalAddParameter(parameterName, string.Format("%{0}", value), false);
        }

        public DbParameter AddRightLikeParamter(string parameterName, object value)
        {
            return this.InternalAddParameter(parameterName, string.Format("{0}%", value), false);
        }

        public DbParameter AddParameterNoCheckIdParamAsUlong(string parameterName, object value)
        {
            return this.InternalAddParameter(parameterName, value, false);
        }

        private DbParameter InternalAddParameter(string parameterName, object value, bool checkIdParamAsUlong = true)
        {
            DbParameter result;
            try
            {
                DbParameter dbParameter = this.command.CreateParameter();
                dbParameter.ParameterName = parameterName;
                this.SetParameterValue(dbParameter, value, checkIdParamAsUlong);
                this.command.Parameters.Add(dbParameter);
                result = dbParameter;
            }
            catch (System.Exception var_1_36)
            {
                this.command.Parameters.Clear();
                throw;
            }
            return result;
        }

        private void SetParameterValue(DbParameter param, object value, bool checkIdParamAsUlong = true)
        {
            bool flag = false;
            if (value == null)
            {
                value = System.DBNull.Value;
            }
            else
            {
                System.Type type = value.GetType();
                if (type == typeof(decimal) || type == typeof(int))
                {
                    int maxIntLength = Settings.MaxIntLength;
                    if (maxIntLength > 0)
                    {
                        if (System.Math.Truncate(System.Math.Abs(System.Convert.ToDecimal(value))).ToString().Length > maxIntLength)
                        {
                            throw new System.Exception(string.Format("数字“{0}”太大，超出系统允许范围！", value));
                        }
                    }
                }
                else if (type == typeof(string))
                {
                    string text = (string)value;
                    if (this.DbType == DbType.Sql2000)
                    {
                        flag = (text.Length > 4000);
                    }
                    else if (this.DbType == DbType.MySql)
                    {
                        bool flag2 = this.idParamToUlong && checkIdParamAsUlong;
                        ulong num;
                        if ((flag2 || (flag2 && DbHelper.CheckIdParamAsUlong)) && param.ParameterName.IndexOf("id", System.StringComparison.OrdinalIgnoreCase) >= 0 && ulong.TryParse(text, out num))
                        {
                            value = num;
                        }
                        else
                        {
                            value = DbHelper.CheckReplaceNonGbkChar(text);
                        }
                    }
                }
                else if (type == typeof(ulong) && this.DbType == DbType.Sql2000)
                {
                    ulong num2 = (ulong)value;
                    value = (long)num2;
                }
                else if (type == typeof(DataTable) && this.DbType == DbType.Sql2000)
                {
                    ((SqlParameter)param).SqlDbType = SqlDbType.Structured;
                }
            }
            param.Value = value;
            if (flag)
            {
                param.DbType = System.Data.DbType.AnsiString;
                param.Size = 2147483647;
            }
        }

        public static string CheckParameter(string value)
        {
            value = value.Replace("’'", "＇'");
            value = value.Replace("‘'", "＇'");
            return value;
        }

        private static string CheckReplaceNonGbkChar(string value)
        {
            value = value.Replace("", "\u3000");
            value = value.Replace("", "\u3000");
            value = value.Replace("", "？");
            return value;
        }

        public DbParameter AddOutputParameter(string parameterName, object value)
        {
            return this.DoAddOutputParameter(parameterName, value, ParameterDirection.Output);
        }

        public DbParameter AddInputOutputParameter(string parameterName, object value)
        {
            return this.DoAddOutputParameter(parameterName, value, ParameterDirection.InputOutput);
        }

        private DbParameter DoAddOutputParameter(string parameterName, object value, ParameterDirection direction)
        {
            DbParameter dbParameter = this.AddParameter(parameterName, value);
            dbParameter.Direction = direction;
            if (value != null)
            {
                System.Type type = value.GetType();
                if (!DbHelper.IsPrimitiveType(type))
                {
                    throw new System.InvalidOperationException(string.Format("输出参数“{0}”不支持类型 {1}", parameterName, type.Name));
                }
                if (type == typeof(string))
                {
                    dbParameter.Size = 4000;
                }
                else if (type == typeof(decimal))
                {
                    ((IDbDataParameter)dbParameter).Scale = (byte)Settings.DecimalScale;
                }
            }
            return dbParameter;
        }

        private static bool IsPrimitiveType(System.Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(System.DateTime) || type == typeof(decimal);
        }

        public void BeginTransaction()
        {
            this.CheckTransaction();
            try
            {
                this.command.Transaction = this.connection.BeginTransaction();
            }
            catch (System.Exception innerException)
            {
                throw new System.InvalidOperationException(("启动事务失败，上次执行的非事务SQL：" + this.lastNonTransactionSql) ?? "无", innerException);
            }
        }

        private void CheckTransaction()
        {
            if (this.HasBegunTransaction)
            {
                throw new System.InvalidOperationException("已经存在活动的事务，不能开始事务！");
            }
            this.DoOpenConnection(DbHelperCommandKind.BeginTransaction, null);
        }

        public void CommitTransaction()
        {
            if (this.command.Transaction == null)
            {
                throw new System.InvalidOperationException("没有活动的事务需要提交！");
            }
            this.command.Transaction.Commit();
            this.command.Transaction = null;
        }

        public void RollbackTransaction()
        {
            if (this.command.Transaction == null)
            {
                throw new System.InvalidOperationException("没有活动的事务需要回滚！");
            }
            this.command.Transaction.Rollback();
            this.command.Transaction = null;
        }

        public DataTable ExecuteProcedure(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteProcedure(false);
        }

        public DataTable ExecuteProcedure(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedure(true);
        }

        public DataTable ExecuteProcedure(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedure(true);
        }

        public DataTable ExecuteProcedure(string procedureName, out IHashMap outputParameterValues, out int returnValue)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            outputParameterValues = new HashMap();
            return this.DoExecuteProcedure(outputParameterValues, out returnValue, true);
        }

        public DataTable[] ExecuteProcedureEx(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteProcedureEx(false);
        }

        public DataTable[] ExecuteProcedureEx(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedureEx(true);
        }

        public DataTable[] ExecuteProcedureEx(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedureEx(true);
        }

        public DataTable[] ExecuteProcedureEx(string procedureName, string[] tableNames)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteProcedureEx(false, tableNames);
        }

        public DataTable[] ExecuteProcedureEx(string procedureName, string[] tableNames, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedureEx(true, tableNames);
        }

        public DataTable[] ExecuteProcedureEx(string procedureName, string[] tableNames, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedureEx(true, tableNames);
        }

        public IHashMapList ProcedureSelect(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoProcedureSelect(false);
        }

        public IHashMapList ProcedureSelect(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoProcedureSelect(true);
        }

        public IHashMapList ProcedureSelect(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoProcedureSelect(true);
        }

        public DataTable ExecuteProcedureAndOutputParameter(string procedureName, out IHashMap outputParameterValues)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            outputParameterValues = new HashMap();
            return this.DoExecuteProcedureAndOutputParameter(outputParameterValues, false);
        }

        public DataTable ExecuteProcedureAndOutputParameter(string procedureName, out IHashMap outputParameterValues, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            outputParameterValues = new HashMap();
            return this.DoExecuteProcedureAndOutputParameter(outputParameterValues, true);
        }

        public DataTable ExecuteProcedureAndOutputParameter(string procedureName, out IHashMap outputParameterValues, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            outputParameterValues = new HashMap();
            return this.DoExecuteProcedureAndOutputParameter(outputParameterValues, true);
        }

        public IHashMapList ProcedureSelectAndOutputParameter(string procedureName, out IHashMap outputParameterValues)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoProcedureSelectAndOutputParameter(out outputParameterValues, false);
        }

        public IHashMapList ProcedureSelectAndOutputParameter(string procedureName, out IHashMap outputParameterValues, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoProcedureSelectAndOutputParameter(out outputParameterValues, true);
        }

        public IHashMapList ProcedureSelectAndOutputParameter(string procedureName, out IHashMap outputParameterValues, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoProcedureSelectAndOutputParameter(out outputParameterValues, true);
        }

        public DataTable ExecuteProcedureAndReturnValue(string procedureName, out int returnValue)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteProcedureAndReturnValue(out returnValue);
        }

        public DataTable ExecuteProcedureAndReturnValue(string procedureName, out int returnValue, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedureAndReturnValue(out returnValue);
        }

        public DataTable ExecuteProcedureAndReturnValue(string procedureName, out int returnValue, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteProcedureAndReturnValue(out returnValue);
        }

        public IHashMapList ProcedureSelectAndReturnValue(string procedureName, out int returnValue)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoProcedureSelectAndReturnValue(out returnValue);
        }

        public IHashMapList ProcedureSelectAndReturnValue(string procedureName, out int returnValue, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoProcedureSelectAndReturnValue(out returnValue);
        }

        public IHashMapList ProcedureSelectAndReturnValue(string procedureName, out int returnValue, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoProcedureSelectAndReturnValue(out returnValue);
        }

        public int ExecuteNonQueryProcedure(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteNoQueryProcedure(false);
        }

        public int ExecuteNonQueryProcedure(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteNoQueryProcedure(true);
        }

        public int ExecuteNonQueryProcedure(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteNoQueryProcedure(true);
        }

        public object ExecuteScalerProcedure(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteScalarProcedure();
        }

        public object ExecuteScalerProcedure(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteScalarProcedure();
        }

        public object ExecuteScalerProcedure(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteScalarProcedure();
        }

        public object ExecuteOutputLastParameterProcedure(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteOutLastParameterProcedure();
        }

        public object ExecuteOutputLastParameterProcedure(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteOutLastParameterProcedure();
        }

        public object ExecuteOutputLastParameterProcedure(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteOutLastParameterProcedure();
        }

        public int ExecuteReturnValueProcedure(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteReturnValueProcedure();
        }

        public void ExecuteReturnValueProcedure(string procedureName, out int value)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            this.DoExecuteReturnValueProcedure(out value);
        }

        public int ExecuteReturnValueProcedure(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteReturnValueProcedure();
        }

        public int ExecuteReturnValueProcedure(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteReturnValueProcedure();
        }

        public int ExecuteOutputParameterProcedureAndReturnValue(string procedureName, out IHashMap outputParameterValues)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            outputParameterValues = new HashMap();
            return this.DoExecuteOutputParameterProcedureAndReturnValue(outputParameterValues, false);
        }

        public int ExecuteOutputParameterProcedureAndReturnValue(string procedureName, out IHashMap outputParameterValues, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            outputParameterValues = new HashMap();
            return this.DoExecuteOutputParameterProcedureAndReturnValue(outputParameterValues, true);
        }

        public int ExecuteOutputParameterProcedureAndReturnValue(string procedureName, out IHashMap outputParameterValues, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            outputParameterValues = new HashMap();
            return this.DoExecuteOutputParameterProcedureAndReturnValue(outputParameterValues, true);
        }

        public IHashMap ExecuteOutputParameterProcedure(string procedureName)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            return this.DoExecuteOutputManyParameterProcedure(false);
        }

        public IHashMap ExecuteOutputParameterProcedure(string procedureName, params object[] paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteOutputManyParameterProcedure(true);
        }

        public IHashMap ExecuteOutputParameterProcedure(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecuteProcedure(procedureName, paramValues);
            return this.DoExecuteOutputManyParameterProcedure(true);
        }

        private DbDataReader ExecuteReader(string sql)
        {
           
            this.DoBeginQuery(sql);
            DbDataReader result;
            try
            {
                DbDataReader dbDataReader = this.command.ExecuteReader();
                this.DoAfterExecute();
                result = dbDataReader;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            return result;
        }

        public DataTable ExecuteSQL(string sql, System.Collections.Generic.IDictionary<string, object> paramValues)
        {
            this.AddParameters(paramValues);
            return this.ExecuteSQL(sql);
        }

        private void DoBeginQuery(string sql)
        {
            this.DoBeginExecute(sql, CommandType.Text);
            if (this.DbType == DbType.MySql)
            {
                foreach (DbParameter dbParameter in this.command.Parameters)
                {
                    object value = dbParameter.Value;
                    if (value != null && value.GetType() == typeof(string))
                    {
                        string text = (string)value;
                        text = text.Replace("\\", "\\\\");
                        text = text.Replace("\\\\%", "\\%");
                        text = text.Replace("\\\\_", "\\_");
                        text = text.Replace("\\\\", "\\");
                        dbParameter.Value = text;
                    }
                }
            }
            else
            {

                this.Command.CommandText = sql;
            }
        }

        public DataTable ExecuteSQL(string sql)
        {
            this.DoBeginQuery(sql);
            DataTable result;
            try
            {
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                DataTable dataTable = DbHelper.ReaderToDataTable(reader, this.fieldNameToLower);
                this.DoLogRowCount(dataTable.Rows.Count);
                result = dataTable;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        public DataTable[] ExecuteSQLEx(string sql)
        {
            this.DoBeginQuery(sql);
            DataTable[] result;
            try
            {
                DataSet dataSet = new DataSet();
                DbDataAdapter dbDataAdapter = DbHelper.CreateDataAdapter(this.command);
                dbDataAdapter.Fill(dataSet);
                this.DoAfterExecute();
                DataTable[] array = new DataTable[dataSet.Tables.Count];
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    array[i] = dataSet.Tables[i];
                }
                result = array;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        public DataTable[] ExecuteSQLEx(string sql, string[] tableNames)
        {
            DataTable[] array = this.ExecuteSQLEx(sql);
            this.ResetTableNames(array, tableNames);
            return array;
        }

        internal static DbDataAdapter CreateDataAdapter(DbCommand dbCommand)
        {
            DbType dbType = DbHelper.GetDbType(dbCommand.Connection.ConnectionString);
            DbDataAdapter result;
            if (dbType == DbType.MySql)
            {
                result = DbHelper.CreateMysqlDataAdapter(dbCommand);
            }
            else if (dbType == DbType.Sql2000)
            {
                result = new SqlDataAdapter((SqlCommand)dbCommand);
            }
            else
            {
                result = DbHelper.CreateOleDbDataAdapter(dbCommand);
            }

            return result;
        }

        private static DbDataAdapter CreateMysqlDataAdapter(DbCommand dbCommand)
        {
            return null;
        }

        private static DbDataAdapter CreateOleDbDataAdapter(DbCommand dbCommand)
        {
            return new OleDbDataAdapter((OleDbCommand)dbCommand);
        }


        public DataRow ExecuteFirstRowSQL(string sql)
        {
            DataTable dataTable = this.ExecuteSQL(sql);
            int count = dataTable.Rows.Count;
            if (count >= 2)
            {
                throw new System.InvalidOperationException(string.Format("执行SQL“{0}”时期待返回一行，现在返回了 {1} 行", sql, count));
            }
            return (count > 0) ? dataTable.Rows[0] : null;
        }

        public DataRow ExecuteSingleRowSQL(string sql)
        {
            DataTable dataTable = this.ExecuteSQL(sql);
            int count = dataTable.Rows.Count;
            if (count != 1)
            {
                throw new System.InvalidOperationException(string.Format("执行SQL“{0}”时期待返回一行，现在返回了 {1} 行", sql, count));
            }
            return dataTable.Rows[0];
        }

        public int ExecuteNonQuerySQL(string sql)
        {
            this.DoBeginExecute(sql, CommandType.Text);
            this.command.CommandText = sql;
            int result;
            try
            {
                int num = this.command.ExecuteNonQuery();
                this.DoLogRowCount(-1);
                result = num;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        public int ExecuteIntSQL(string sql)
        {
            object obj = this.ExecuteScalerSQL(sql);
            return (obj != System.DBNull.Value) ? System.Convert.ToInt32(obj) : 0;
        }

        public object ExecuteScalerSQL(string sql)
        {
            this.DoBeginQuery(sql);
            object result;
            try
            {
                object obj = this.command.ExecuteScalar();
                this.DoAfterExecute();
                if (obj != null)
                {
                    System.Type type = obj.GetType();
                    if (type == typeof(int) || type == typeof(uint) || type == typeof(short))
                    {
                        try
                        {
                            this.DoLogRowCount(-1);
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }
                }
                result = obj;
            }
            catch (System.Exception ex)
            {
                this.DoLogErrorSQL(ex);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        public IHashMapList Select(string sql)
        {
            IHashMapList result;
            try
            {
                DbDataReader reader = this.ExecuteReader(sql);
                this.DoAfterExecute();
                IHashMapList HashMapList = DbHelper.ReaderToObjectList(reader, this.fieldNameToLower);
                this.DoLogRowCount(HashMapList.Count);
                result = HashMapList;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        public IHashMapList Select(string sql, System.Collections.Generic.IDictionary<string, object> paramValues)
        {
            this.AddParameters(paramValues);
            return this.Select(sql);
        }

        private void AddParameters(System.Collections.Generic.IDictionary<string, object> paramValues)
        {
            if (paramValues != null && paramValues.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, object> current in paramValues)
                {
                    this.AddParameter(current.Key, current.Value);
                }
            }
        }

        public IHashMap SelectFirstRow(string sql)
        {
            IHashMapList HashMapList = this.Select(sql);
            return (HashMapList.Count > 0) ? HashMapList[0] : null;
        }

        public IHashMap SelectSingleRow(string sql)
        {
            IHashMapList HashMapList = this.Select(sql);
            int count = HashMapList.Count;
            if (count != 1)
            {
                throw new System.InvalidOperationException(string.Format("执行SQL“{0}”时期待返回一行，现在返回了 {1} 行", sql, count));
            }
            return HashMapList[0];
        }

        private static string[] GetFieldNames(System.Collections.Generic.IDictionary<string, object> data)
        {
            string[] array = new string[data.Count];
            data.Keys.CopyTo(array, 0);
            return array;
        }

        private static string[] GetFieldNames(string keyField, System.Collections.Generic.IDictionary<string, object> data)
        {
            string[] array = new string[data.Count];
            array[0] = keyField;
            int num = 1;
            foreach (string current in data.Keys)
            {
                if (current != keyField)
                {
                    array[num] = current;
                    num++;
                }
            }
            return array;
        }

        public int Insert(string tableName, string[] fieldNames, System.Collections.Generic.IDictionary<string, object> data)
        {
            string sql = SqlHelper.BuildInsertSql(tableName, fieldNames);
            this.BuildParameters(fieldNames, data);
            return this.ExecuteNonQuerySQL(sql);
        }

        public int Insert(string tableName, System.Collections.Generic.IDictionary<string, object> data)
        {
            string[] fieldNames = DbHelper.GetFieldNames(data);
            return this.Insert(tableName, fieldNames, data);
        }

        public int Insert(string tableName, string[] fieldNames, params object[] fieldValues)
        {
            string sql = SqlHelper.BuildInsertSql(tableName, fieldNames);
            this.BuildParameters(fieldNames, fieldValues);
            return this.ExecuteNonQuerySQL(sql);
        }

        public int Update(string tableName, string[] fieldNames, System.Collections.Generic.IDictionary<string, object> data)
        {
            string sql = SqlHelper.BuildUpdateSql(tableName, fieldNames);
            this.BuildParameters(fieldNames, data);
            return this.ExecuteNonQuerySQL(sql);
        }

        public int Update(string tableName, string keyField, System.Collections.Generic.IDictionary<string, object> data)
        {
            string[] fieldNames = DbHelper.GetFieldNames(keyField, data);
            return this.Update(tableName, fieldNames, data);
        }

        public int UpdateByKeys(string tableName, string[] keyFieldNames, System.Collections.Generic.IDictionary<string, object> data)
        {
            string[] fieldNames = DbHelper.GetFieldNames(data);
            string sql = SqlHelper.BuildUpdateSql(tableName, keyFieldNames, fieldNames);
            this.BuildParameters(fieldNames, data);
            return this.ExecuteNonQuerySQL(sql);
        }

        private void BuildParameters(string[] fieldNames, System.Collections.Generic.IDictionary<string, object> data)
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                string text = fieldNames[i];
                object value = data.ContainsKey(text) ? data[text] : null;
                this.AddParameter(text, value);
            }
        }

        private void BuildParameters(string[] fieldNames, object[] fieldValues)
        {
            if (fieldNames.Length != fieldValues.Length)
            {
                throw new System.InvalidOperationException("fieldNames 与 fieldValues 长度不同");
            }
            for (int i = 0; i < fieldNames.Length; i++)
            {
                this.AddParameter(fieldNames[i], fieldValues[i]);
            }
        }

        public int Delete(string tableName, string keyField, object keyValue)
        {
            string sql = SqlHelper.BuildDeleteSql(tableName, keyField);
            this.AddParameter(keyField, keyValue);
            return this.ExecuteNonQuerySQL(sql);
        }

        public int DeleteByKeys(string tableName, string[] keyFieldNames, System.Collections.Generic.IDictionary<string, object> data)
        {
            string sql = SqlHelper.BuildDeleteSql(tableName, keyFieldNames);
            this.BuildParameters(keyFieldNames, data);
            return this.ExecuteNonQuerySQL(sql);
        }

        public static System.Guid NewGuidComb()
        {
            byte[] array = System.Guid.NewGuid().ToByteArray();
            System.DateTime dateTime = new System.DateTime(1900, 1, 1);
            System.DateTime now = System.DateTime.Now;
            System.TimeSpan timeSpan = new System.TimeSpan(now.Ticks - dateTime.Ticks);
            System.TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes = System.BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = System.BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            System.Array.Reverse(bytes);
            System.Array.Reverse(bytes2);
            System.Array.Copy(bytes, bytes.Length - 2, array, array.Length - 6, 2);
            System.Array.Copy(bytes2, bytes2.Length - 4, array, array.Length - 4, 4);
            return new System.Guid(array);
        }

        public static System.Guid ParseGuid(string str)
        {
            System.Guid result;
            if (string.IsNullOrEmpty(str))
            {
                result = System.Guid.Empty;
            }
            else
            {
                result = new System.Guid(str);
            }
            return result;
        }

        public static ulong ParseCuid(string str)
        {
            ulong result;
            if (string.IsNullOrEmpty(str))
            {
                result = 0uL;
            }
            else
            {
                result = ulong.Parse(str);
            }
            return result;
        }

        public static ulong ParseCuid(object obj)
        {
            return (obj != null) ? DbHelper.ParseCuid(obj.ToString()) : 0uL;
        }



        public static DbConnection CreateConnection(string connectionString)
        {
            DbType dbType = DbHelper.GetDbType(connectionString);
            DbConnection result;
            if (dbType == DbType.MySql)
            {
                result = DbHelper.CreateMysqlConnection(connectionString);
            }
            else if (dbType == DbType.Sql2000)
            {
                if (DbHelper.DefaultConnectTimeout > 0)
                {
                    DbHelper.AppendConnectionString(ref connectionString, "connect timeout", DbHelper.DefaultConnectTimeout);
                }
                if (DbHelper.MinPoolSize > 0)
                {
                    DbHelper.AppendConnectionString(ref connectionString, "min pool size", DbHelper.MinPoolSize);
                }
                if (DbHelper.MaxPoolSize > 0)
                {
                    DbHelper.AppendConnectionString(ref connectionString, "max pool size", DbHelper.MaxPoolSize);
                }
                result = new SqlConnection(connectionString);
            }
            else
            {
                result = DbHelper.CreateOleDbConnection(connectionString);
            }

            return result;
        }

        private static void AppendConnectionString(ref string connectionString, string key, int value)
        {
            if (connectionString != null && connectionString.IndexOf(key, System.StringComparison.OrdinalIgnoreCase) < 0)
            {
                connectionString = connectionString.Trim();
                if (connectionString[connectionString.Length - 1] != ';')
                {
                    connectionString += ';';
                }
                connectionString += string.Format("{0}={1}", key, value);
            }
        }

        internal static DbType GetDbType(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new System.InvalidOperationException("没有指定连接字符串");
            }
            DbType result;
            if (connectionString.IndexOf("OraOLEDB", System.StringComparison.OrdinalIgnoreCase) >= 0 || connectionString.IndexOf("MSDAORA", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = DbType.Oracle;
            }
            else if (connectionString.IndexOf("Jet.OLEDB", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = DbType.Access;
            }
            else if (connectionString.IndexOf(".fdb", System.StringComparison.OrdinalIgnoreCase) > 0)
            {
                result = DbType.Firebird;
            }
            else if (connectionString.Contains("Data Source=") || connectionString.Contains("server="))
            {
                result = DbType.Sql2000;
            }
            else
            {
                if (connectionString.IndexOf("Server=", System.StringComparison.OrdinalIgnoreCase) < 0)
                {
                    throw new System.InvalidOperationException("不支持的连接字符串：" + connectionString);
                }
                result = DbType.MySql;
            }
            return result;
        }

        private static DbConnection CreateMysqlConnection(string connectionString)
        {
            return null;
        }

        private static DbConnection CreateOleDbConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }



        private static string GetColumnName(DataRow row, bool fieldNameToLower, System.Collections.Generic.Dictionary<string, int> columnCountMap)
        {
            string text = row["ColumnName"] as string;
            if (fieldNameToLower)
            {
                text = text.ToLower();
            }
            if (columnCountMap.ContainsKey(text))
            {
                int value = columnCountMap[text] + 1;
                columnCountMap[text] = value;
                text += value.ToString();
            }
            else
            {
                columnCountMap.Add(text, 1);
            }
            return text;
        }

        private void DoLogRowCount(int value)
        {

        }

        private void DoLog(string sql, long elapsedMilliseconds)
        {

        }

        private void GetSqlStack()
        {

        }

        internal DbHelper._ScriptDbItemList ExecuteItemList(string sql)
        {
            DbHelper._ScriptDbItemList result;
            try
            {
                DbDataReader reader = this.ExecuteReader(sql);
                DbHelper._ScriptDbItemList scriptDbItemList = DbHelper.ReaderToItemList(reader, this.fieldNameToLower);
                this.DoLogRowCount(scriptDbItemList.rows.Length);
                result = scriptDbItemList;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private static DbHelper._ScriptDbItemList ReaderToItemList(DbDataReader reader, bool fieldNameToLower)
        {
            DataRowCollection rows = reader.GetSchemaTable().Rows;
            int count = rows.Count;
            DbHelper._ScriptDbItemList scriptDbItemList = new DbHelper._ScriptDbItemList();
            scriptDbItemList.fields = new string[count];
            scriptDbItemList.fieldSizes = new int[count];
            System.Collections.Generic.Dictionary<string, int> columnCountMap = new System.Collections.Generic.Dictionary<string, int>(count);
            for (int i = 0; i < count; i++)
            {
                DataRow dataRow = rows[i];
                string columnName = DbHelper.GetColumnName(dataRow, fieldNameToLower, columnCountMap);
                scriptDbItemList.fields[i] = columnName;
                int fieldSize = DbHelper.GetFieldSize((int)dataRow["ColumnSize"]);
                scriptDbItemList.fieldSizes[i] = fieldSize;
            }
            try
            {
                System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
                while (reader.Read())
                {
                    object[] array = new object[count];
                    reader.GetValues(array);
                    arrayList.Add(array);
                }
                scriptDbItemList.rows = arrayList.ToArray();
            }
            finally
            {
                reader.Close();
            }
            return scriptDbItemList;
        }

        private static IHashMapList ReaderToObjectList(DbDataReader reader, bool fieldNameToLower)
        {
            return DbHelper.ReaderToObjectList(reader, fieldNameToLower, true);
        }

        private static IHashMapList ReaderToObjectList(DbDataReader reader, bool fieldNameToLower, bool throwIfNoDataTable)
        {
            DataTable dataTable = DbHelper.CheckSchemaTable(reader, throwIfNoDataTable);
            IHashMapList result;
            if (dataTable == null)
            {
                result = null;
            }
            else
            {
                DataRowCollection rows = dataTable.Rows;
                int count = rows.Count;
                System.Collections.Generic.Dictionary<string, int> columnCountMap = new System.Collections.Generic.Dictionary<string, int>(count);
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(count);
                for (int i = 0; i < count; i++)
                {
                    string columnName = DbHelper.GetColumnName(rows[i], fieldNameToLower, columnCountMap);
                    list.Add(columnName);
                }
                HashMapList HashMapList = new HashMapList();
                object[] array = new object[count];
                try
                {
                    while (reader.Read())
                    {
                        reader.GetValues(array);
                        HashMap HashMap = new HashMap();
                        for (int i = 0; i < count; i++)
                        {
                            string key = list[i];
                            HashMap[key] = array[i];
                        }
                        HashMapList.Add(HashMap);
                    }
                }
                finally
                {
                    reader.Close();
                }
                result = HashMapList;
            }
            return result;
        }

        private static DataTable CheckSchemaTable(DbDataReader reader, bool throwIfNoDataTable)
        {
            DataTable result;
            if (reader == null)
            {
                if (throwIfNoDataTable)
                {
                    throw new System.InvalidOperationException("没取到查询结果");
                }
                result = null;
            }
            else
            {
                DataTable schemaTable = reader.GetSchemaTable();
                if (schemaTable == null)
                {
                    if (throwIfNoDataTable)
                    {
                        reader.Close();
                        throw new System.InvalidOperationException("取不到查询结果的定义");
                    }
                    reader.Close();
                }
                result = schemaTable;
            }
            return result;
        }

        public static DataTable ReaderToDataTable(DbDataReader reader, bool fieldNameToLower)
        {
            return DbHelper.ReaderToDataTable(reader, fieldNameToLower, true, true);
        }

        private static DataTable ReaderToDataTable(DbDataReader reader, bool fieldNameToLower, bool throwIfNoDataTable)
        {
            return DbHelper.ReaderToDataTable(reader, fieldNameToLower, throwIfNoDataTable, true);
        }

        private static DataTable ReaderToDataTable(DbDataReader reader, bool fieldNameToLower, bool throwIfNoDataTable, bool closeReader)
        {
            DataTable dataTable = DbHelper.CheckSchemaTable(reader, throwIfNoDataTable);
            DataTable result;
            if (dataTable == null)
            {
                result = null;
            }
            else
            {
                DataTable dataTable2 = new DataTable();
                DataRowCollection rows = dataTable.Rows;
                int count = rows.Count;
                System.Collections.Generic.Dictionary<string, int> columnCountMap = new System.Collections.Generic.Dictionary<string, int>(count);
                int num = -1;
                for (int i = 0; i < count; i++)
                {
                    DataColumn dataColumn = new DataColumn();
                    DataRow dataRow = rows[i];
                    string columnName = DbHelper.GetColumnName(dataRow, fieldNameToLower, columnCountMap);
                    dataColumn.ColumnName = columnName;
                    dataColumn.DataType = (System.Type)dataRow["DataType"];
                    if (dataColumn.DataType == typeof(string))
                    {
                        int num2 = (int)dataRow["ColumnSize"];
                        if (num2 == 10 && columnName == "type")
                        {
                            num2 = 20;
                        }
                        else if (num2 > 0 && columnName == "Slave_IO_State")
                        {
                            num = 1024;
                            num2 = num;
                        }
                        else if (num > 0)
                        {
                            num2 = System.Math.Max(num2, num);
                        }
                        if (num2 > 0)
                        {
                            dataColumn.MaxLength = num2;
                        }
                    }
                    dataTable2.Columns.Add(dataColumn);
                }
                object[] values = new object[count];
                try
                {
                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        dataTable2.Rows.Add(values);
                    }
                }
                finally
                {
                    if (closeReader)
                    {
                        reader.Close();
                    }
                }
                result = dataTable2;
            }
            return result;
        }

        private static DataTable[] ReaderToDataTables(DbDataReader reader, bool fieldNameToLower)
        {
            System.Collections.Generic.IList<DataTable> list = new System.Collections.Generic.List<DataTable>();
            try
            {
                do
                {
                    DataTable item = DbHelper.ReaderToDataTable(reader, fieldNameToLower, true, false);
                    list.Add(item);
                }
                while (reader.NextResult());
            }
            finally
            {
                reader.Close();
            }
            DataTable[] array = new DataTable[list.Count];
            list.CopyTo(array, 0);
            return array;
        }

        private static int GetFieldSize(int maxLength)
        {
            return (maxLength <= 8192) ? maxLength : 4096;
        }

        private static DataType GetFieldType(System.Type type)
        {
            DataType result;
            if (type == typeof(string))
            {
                result = DataType.String;
            }
            else if (type == typeof(decimal) || type == typeof(int) || type == typeof(long) || type == typeof(ulong) || type == typeof(double) || type == typeof(short))
            {
                result = DataType.Number;
            }
            else if (type == typeof(System.DateTime))
            {
                result = DataType.DateTime;
            }
            else
            {
                result = DataType.Unknown;
            }
            return result;
        }

        public static object DataTableToItemList(DataTable table, int first, int count)
        {
            DataView defaultView = table.DefaultView;
            if (count < 0)
            {
                count = defaultView.Count;
            }
            DbHelper._ScriptDbItemList scriptDbItemList = new DbHelper._ScriptDbItemList();
            int count2 = table.Columns.Count;
            scriptDbItemList.fields = new string[count2];
            for (int i = 0; i < count2; i++)
            {
                string columnName = table.Columns[i].ColumnName;
                scriptDbItemList.fields[i] = columnName;
            }
            if (first == 0)
            {
                scriptDbItemList.fieldSizes = new int[count2];
                scriptDbItemList.fieldTypes = new int[count2];
                for (int i = 0; i < count2; i++)
                {
                    DataColumn dataColumn = table.Columns[i];
                    int fieldSize = DbHelper.GetFieldSize(dataColumn.MaxLength);
                    scriptDbItemList.fieldSizes[i] = fieldSize;
                    DataType fieldType = DbHelper.GetFieldType(dataColumn.DataType);
                    scriptDbItemList.fieldTypes[i] = (int)fieldType;
                }
            }
            int num = System.Math.Min(count, (first < defaultView.Count) ? (defaultView.Count - first) : defaultView.Count);
            if (first < defaultView.Count)
            {
                num = System.Math.Min(count, defaultView.Count - first);
            }
            else
            {
                num = System.Math.Min(count, defaultView.Count);
                first = 0;
            }
            scriptDbItemList.rows = new object[(num > 0) ? num : 0];
            for (int i = 0; i < num; i++)
            {
                scriptDbItemList.rows[i] = defaultView[first + i].Row.ItemArray;
            }
            return scriptDbItemList;
        }

        public static IHashMap DataRowToObject(DataTable table, DataRow row)
        {
            HashMap HashMap = new HashMap();
            object[] itemArray = row.ItemArray;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string columnName = table.Columns[i].ColumnName;
                HashMap[columnName] = itemArray[i];
            }
            return HashMap;
        }

        public static IHashMap DataTableToEmptyObject(DataTable table)
        {
            HashMap HashMap = new HashMap();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                DataColumn dataColumn = table.Columns[i];
                object value = null;
                if (dataColumn.DataType == typeof(string))
                {
                    value = string.Empty;
                }
                else if (dataColumn.DataType == typeof(decimal) || dataColumn.DataType == typeof(double) || dataColumn.DataType == typeof(int) || dataColumn.DataType == typeof(short))
                {
                    value = 0m;
                }
                HashMap[dataColumn.ColumnName] = value;
            }
            return HashMap;
        }

        public static IHashMap DataRowToObject(DataRow row)
        {
            return DbHelper.DataRowToObject(row.Table, row);
        }

        public static IHashMap DataTableFirstRowToObject(DataTable table)
        {
            return (table.Rows.Count > 0) ? DbHelper.DataRowToObject(table, table.Rows[0]) : null;
        }

        public static IHashMapList DataTableToObjectList(DataTable table, int first, int count)
        {
            int num = System.Math.Min(count, table.Rows.Count - first);
            HashMapList HashMapList = new HashMapList(num);
            for (int i = 0; i < num; i++)
            {
                HashMapList.Add(DbHelper.DataRowToObject(table, table.Rows[first + i]));
            }
            return HashMapList;
        }

        public static IHashMapList DataTableToObjectList(DataTable table)
        {
            return DbHelper.DataTableToObjectList(table, 0, table.Rows.Count);
        }

        public static DataTable CloneDataTable(DataTable src)
        {
            DataTable dataTable = src.Clone();
            foreach (DataRow dataRow in src.Rows)
            {
                DataRow dataRow2 = dataTable.NewRow();
                dataRow2.ItemArray = dataRow.ItemArray;
                dataTable.Rows.Add(dataRow2);
            }
            dataTable.AcceptChanges();
            return dataTable;
        }

        public static int IndexOfRow(DataTable table, string keyField, System.IComparable keyValue)
        {
            if (string.IsNullOrEmpty(keyField))
            {
                throw new System.ArgumentNullException("keyField");
            }
            if (keyValue == null)
            {
                throw new System.ArgumentNullException("keyValue");
            }
            DataView defaultView = table.DefaultView;
            int result;
            for (int i = 0; i < defaultView.Count; i++)
            {
                if (keyValue.Equals(defaultView[i].Row[keyField]))
                {
                    result = i;
                    return result;
                }
            }
            result = -1;
            return result;
        }

        private void DoBeginExecute(string commandText, CommandType commandType)
        {
            this.Connection.Open();
        }

        private string DealCommandText(string commandText)
        {
            if (this.specialParameterNames != null && this.specialParameterNames.Count > 0)
            {
                foreach (string current in this.specialParameterNames.Keys)
                {
                    commandText = commandText.Replace(current, this.specialParameterNames[current]);
                }
                this.specialParameterNames.Clear();
            }
            return commandText;
        }

        private void LogMySqlConnection(System.DateTime accessTime, string commandText, string paramsText)
        {
            try
            {
                object obj = this.connection.GetType().InvokeMember("driver", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField, null, this.connection, null);
                if (obj != null)
                {
                    DbHelper.mysqlConnectionInfos[obj.GetHashCode()] = new MysqlConnectionInfo
                    {
                        AccessTime = accessTime,
                        CommandText = commandText,
                        ParamsText = paramsText
                    };
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void DoAfterExecute()
        {

        }

        private void DoBeginExecuteProcedure(string procedureName, object[] paramValues)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            DbParameterHelper.DeriveParamters(this);
            if (paramValues.Length > this.command.Parameters.Count)
            {
                throw new System.InvalidOperationException("传入的参数个数超出实际需要的个数！");
            }
            for (int i = 0; i < paramValues.Length; i++)
            {
                this.SetParameterValue(this.command.Parameters[i], paramValues[i], true);
            }
        }

        private void DoBeginExecuteProcedure(string procedureName, IHashMap paramValues)
        {
            this.DoBeginExecute(procedureName, CommandType.StoredProcedure);
            DbParameterHelper.DeriveParamters(this);
            bool flag = true;
            if (flag)
            {
                paramValues = DbHelper.KeysToLower(paramValues);
            }
            for (int i = 0; i < this.command.Parameters.Count; i++)
            {
                DbParameter dbParameter = this.command.Parameters[i];
                string text = dbParameter.ParameterName;
                if (text[0] == '@')
                {
                    text = text.Substring(1);
                }
                if (flag)
                {
                    text = text.ToLower();
                }
                object value;
                if (!paramValues.TryGetValue(text, out value))
                {
                    if (dbParameter.Direction != ParameterDirection.InputOutput && dbParameter.Direction != ParameterDirection.Output)
                    {
                        throw new System.InvalidOperationException(string.Format("没有传存储过程“{0}”参数“{1}”", procedureName, text));
                    }
                }
                else
                {
                    this.SetParameterValue(dbParameter, value, true);
                }
            }
        }

        private static IHashMap KeysToLower(IHashMap paramValues)
        {
            IHashMap HashMap = new HashMap();
            foreach (System.Collections.Generic.KeyValuePair<string, object> current in paramValues)
            {
                HashMap[current.Key.ToLower()] = current.Value;
            }
            return HashMap;
        }

        private void DoOpenConnection(DbHelperCommandKind commandKind, string commandText)
        {
            this.Connection.Open();
        }

        private void DoEndExecute()
        {
            if (this.hasMysqlTimeoutError)
            {
                this.DoFixMysqlTimeoutErrorOnEndExecute();
            }
            if (!this.hasError && (this.isLoggingSql || this.isDebuggingSql) && this.stopwatch != null)
            {
                long elapsedMilliseconds = this.stopwatch.ElapsedMilliseconds;
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                this.FormatSql(stringBuilder);
                string sql = stringBuilder.ToString().Trim();
                bool flag = DbHelper.IsSelectSql(sql);


                this.DealDebugSql(elapsedMilliseconds, stringBuilder, sql, flag);
            }
            this.command.Parameters.Clear();
        }

        private void DealDebugSql(long elapsedMilliseconds, System.Text.StringBuilder sb, string sql, bool isSelectSql)
        {
            DataTable dataTable = null;
            if (this.DbType == DbType.MySql && isSelectSql && (this.isDebuggingSql || DbHelper.IsSaveDebugSql))
            {
                dataTable = this.GetExplain(sql);
            }
            if (this.isDebuggingSql)
            {
                if (dataTable != null)
                {
                    sb.Append("<div style='padding-left: 20px; padding-bottom:10px;'><table border=0 cellspacing=1 cellpadding=1 bgcolor=#dddddd><tr>");
                    DbHelper.FormatExplainHeader(sb, "序号");
                    DbHelper.FormatExplainHeader(sb, "Select类型");
                    DbHelper.FormatExplainHeader(sb, "表名");
                    DbHelper.FormatExplainHeader(sb, "计划类型");
                    DbHelper.FormatExplainHeader(sb, "可能的索引");
                    DbHelper.FormatExplainHeader(sb, "使用的索引");
                    DbHelper.FormatExplainHeader(sb, "索引长");
                    DbHelper.FormatExplainHeader(sb, "引用");
                    DbHelper.FormatExplainHeader(sb, "读取行数");
                    DbHelper.FormatExplainHeader(sb, "其它信息");
                    sb.Append("</tr>");
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        sb.Append("<tr>");
                        DbHelper.FormatExplainData(sb, dataRow["id"], "right");
                        DbHelper.FormatExplainData(sb, dataRow["select_type"], "center");
                        DbHelper.FormatExplainData(sb, dataRow["table"], null);
                        DbHelper.FormatExplainData(sb, dataRow["type"], "center");
                        DbHelper.FormatExplainData(sb, dataRow["possible_keys"], null);
                        DbHelper.FormatExplainData(sb, dataRow["key"], null);
                        DbHelper.FormatExplainData(sb, dataRow["key_len"], "right");
                        DbHelper.FormatExplainData(sb, dataRow["ref"], null);
                        DbHelper.FormatExplainData(sb, dataRow["rows"], "right");
                        DbHelper.FormatExplainData(sb, dataRow["Extra"], null);
                        sb.Append("</tr>");
                    }
                    sb.AppendLine("</table></div>");
                }
                else if (isSelectSql)
                {

                }

            }
            if (DbHelper.IsSaveDebugSql && dataTable != null)
            {
                this.SaveDebugSql(dataTable, sql);
            }
        }

        private void SaveDebugSql(DataTable explain, string sql)
        {
            HttpContext current = HttpContext.Current;
            if (current != null && explain != null && explain.Rows.Count != 0)
            {
                object obj = null;
                if (current.Session != null)
                {

                }
                string userId = (obj == null) ? string.Empty : obj.ToString();

            }
        }



        private DataTable GetExplain(string sql)
        {
            string commandText = "explain " + sql;
            DataTable result;
            try
            {
                DbCommand dbCommand = this.connection.CreateCommand();
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = commandText;
                result = DbHelper.ReaderToDataTable(dbCommand.ExecuteReader(), false);
            }
            catch (System.Exception)
            {
                result = null;
            }
            return result;
        }

        private void DoFixMysqlTimeoutErrorOnEndExecute()
        {
            try
            {
              
            }
            catch (System.Exception ex)
            {

            }
        }

        private static bool IsSelectSql(string sql)
        {
            return sql.StartsWith("select ", System.StringComparison.OrdinalIgnoreCase);
        }

        private static void FormatExplainHeader(System.Text.StringBuilder sb, string caption)
        {
            sb.Append("<td bgcolor=#f4f4f4><font color=#999999>").Append(caption).Append("</font></td>");
        }

        private static void FormatExplainData(System.Text.StringBuilder sb, object value, string align)
        {
            sb.Append("<td bgcolor=#ffffff").Append((align != null) ? (" align=" + align) : null).Append("><font color=#999999>").Append(value).Append("</font></td>");
        }

        private void DoLogErrorSQL(System.Exception e)
        {
            if (this.DbType == DbType.Sql2000 && e is SqlException)
            {
                this.HandleMSSQLError(e as SqlException);
            }
            this.hasError = true;
            if (this.DbType == DbType.MySql)
            {
                this.CheckHasMysqlTimeoutError(e);
            }
            long elapsedMilliseconds = (this.stopwatch != null) ? this.stopwatch.ElapsedMilliseconds : 0L;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            this.FormatError(stringBuilder, e);
            stringBuilder.AppendLine(this.command.CommandText);
            stringBuilder.AppendLine();
            foreach (DbParameter dbParameter in this.command.Parameters)
            {
                object value = dbParameter.Value;
                stringBuilder.Append("  ").Append(dbParameter.ParameterName).Append('=').Append(SqlHelper.DbParameterToString(value));
                if (dbParameter.Direction != ParameterDirection.Input)
                {
                    stringBuilder.Append(" [").Append(System.Enum.GetName(typeof(ParameterDirection), dbParameter.Direction)).Append(']');
                }
                if (value != null)
                {
                    System.Type type = value.GetType();
                    if (type.IsSubclassOf(typeof(System.Enum)))
                    {
                        stringBuilder.Append(" (").Append(type.Name).Append('.').Append(value.ToString()).Append(")");
                    }
                }
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine();
            this.FormatSql(stringBuilder);
            stringBuilder.AppendLine();

            if (this.isLoggingSql)
            {
                System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
                this.FormatError(stringBuilder2, e);

                System.Text.StringBuilder stringBuilder3 = new System.Text.StringBuilder();
                this.FormatSql(stringBuilder3);
                this.DoLog(stringBuilder3.ToString(), elapsedMilliseconds);
            }
            if (this.isDebuggingSql)
            {
                stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("<font color=\"red\">");
                this.FormatError(stringBuilder, e);
                stringBuilder.AppendLine("</font><br />");
                this.FormatSql(stringBuilder);

            }
        }

        private void HandleMSSQLError(SqlException e)
        {
            if (e.Number == 50000 && !string.IsNullOrEmpty(e.Procedure) && !string.IsNullOrEmpty(e.Message))
            {
                throw new System.Exception(e.Message);
            }
        }

        private void CheckHasMysqlTimeoutError(System.Exception e)
        {
            if (e.InnerException != null)
            {
                e = e.InnerException;
            }
            if (e.Message.IndexOf("timeout", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                this.hasMysqlTimeoutError = true;
            }
        }

        private void FormatError(System.Text.StringBuilder sb, System.Exception e)
        {
            sb.Append("执行");
            if (this.command.CommandType == CommandType.StoredProcedure)
            {
                sb.Append("存储过程");
            }
            else
            {
                sb.Append("SQL");
            }
            sb.Append("出错: ").AppendLine((e != null) ? e.Message : null);
        }

        private void FormatSqlParamsText(System.Text.StringBuilder sb)
        {
            bool flag = true;
            foreach (DbParameter dbParameter in this.command.Parameters)
            {
                if (flag)
                {
                    sb.Append(' ');
                    flag = false;
                }
                else
                {
                    sb.Append(',');
                }
                string parameterName = dbParameter.ParameterName;
                string value = DbHelper.DbParameterToString(dbParameter);
                sb.Append((parameterName[0] == '@') ? null : "@").Append(parameterName).Append('=').Append(value);
                if (dbParameter.Direction == ParameterDirection.Output || dbParameter.Direction == ParameterDirection.InputOutput)
                {
                    sb.Append(" output");
                }
            }
        }

        private void FormatSql(System.Text.StringBuilder sb)
        {
            if (this.command.CommandType == CommandType.StoredProcedure)
            {
                sb.Append("exec ").Append(this.command.CommandText);
                this.FormatSqlParamsText(sb);
            }
            else
            {
                string text = this.command.CommandText;
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                foreach (DbParameter dbParameter in this.command.Parameters)
                {

                    string text2 = dbParameter.ParameterName;
                    list.Add(text2);
                }
                list.Sort();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    string text2 = list[i];
                    DbParameter dbParameter = this.command.Parameters[text2];
                    string text3 = DbHelper.DbParameterToString(dbParameter);
                    if (text2[0] != '@')
                    {
                        text2 = '@' + text2;
                    }
                    text = StringUtils.Replace(text, text2, text3, System.StringComparison.OrdinalIgnoreCase);
                }
                sb.Append(text);
            }
        }

        private static string DbParameterToString(DbParameter param)
        {
            return SqlHelper.DbParameterToString(param.Value);
        }

        private DataTable DoExecuteProcedure(bool convertParameterName)
        {
            DataTable result;
            try
            {
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                DataTable dataTable = DbHelper.ReaderToDataTable(reader, this.fieldNameToLower);
                this.GetContextOutputParameter(convertParameterName);
                this.DoLogRowCount(dataTable.Rows.Count);
                result = dataTable;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private DataTable DoExecuteProcedure(System.Collections.Generic.IDictionary<string, object> outputParameterValues, out int returnValue, bool convertParameterName)
        {
            DataTable result;
            try
            {
                this.AddReturnParameter();
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                DataTable dataTable = DbHelper.ReaderToDataTable(reader, this.fieldNameToLower);
                this.GetOutputParameterValues(outputParameterValues, convertParameterName);
                returnValue = this.GetReturnValue();
                this.DoLogRowCount(dataTable.Rows.Count);
                result = dataTable;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private DataTable[] DoExecuteProcedureEx(bool convertParameterName)
        {
            DataTable[] result;
            try
            {
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                DataTable[] array = DbHelper.ReaderToDataTables(reader, this.fieldNameToLower);
                this.GetContextOutputParameter(convertParameterName);
                this.DoLogRowCount(array[array.Length - 1].Rows.Count);
                result = array;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private DataTable[] DoExecuteProcedureEx(bool convertParameterName, string[] tableNames)
        {
            DataTable[] array = this.DoExecuteProcedureEx(convertParameterName);
            this.ResetTableNames(array, tableNames);
            return array;
        }

        private void ResetTableNames(DataTable[] tables, string[] tableNames)
        {
            if (tables.Length != tableNames.Length)
            {
                throw new System.InvalidOperationException("返回的DataTable个数与传入的表名称个数不对应");
            }
            for (int i = 0; i < tables.Length; i++)
            {
                string text = tableNames[i].Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    tables[i].TableName = text;
                }
            }
        }

        private void GetContextOutputParameter(bool convertParameterName)
        {
            this.outputParameters = new HashMap();
            this.GetOutputParameterValues(this.outputParameters, convertParameterName);
        }

        private IHashMapList DoProcedureSelect(bool convertParameterName)
        {
            IHashMapList result;
            try
            {
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                IHashMapList HashMapList = DbHelper.ReaderToObjectList(reader, this.fieldNameToLower);
                this.GetContextOutputParameter(convertParameterName);
                this.DoLogRowCount(HashMapList.Count);
                result = HashMapList;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private DataTable DoExecuteProcedureAndOutputParameter(System.Collections.Generic.IDictionary<string, object> outputParameterValues, bool convertParameterName)
        {
            DataTable result;
            try
            {
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                DataTable dataTable = DbHelper.ReaderToDataTable(reader, this.fieldNameToLower);
                this.GetOutputParameterValues(outputParameterValues, convertParameterName);
                this.DoLogRowCount(dataTable.Rows.Count);
                result = dataTable;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private IHashMapList DoProcedureSelectAndOutputParameter(out IHashMap outputParameterValues, bool convertParameterName)
        {
            IHashMapList result;
            try
            {
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                IHashMapList HashMapList = DbHelper.ReaderToObjectList(reader, this.fieldNameToLower);
                outputParameterValues = new HashMap();
                this.GetOutputParameterValues(outputParameterValues, convertParameterName);
                this.DoLogRowCount(HashMapList.Count);
                result = HashMapList;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private void GetOutputParameterValues(System.Collections.Generic.IDictionary<string, object> outputParameterValues, bool convertParameterName)
        {
            foreach (DbParameter dbParameter in this.command.Parameters)
            {
                if (dbParameter.Direction == ParameterDirection.Output || dbParameter.Direction == ParameterDirection.InputOutput)
                {
                    string text = dbParameter.ParameterName;
                    if (convertParameterName && text[0] == '@')
                    {
                        text = text.Substring(1);
                    }
                    outputParameterValues.Add(text, dbParameter.Value);
                }
            }
        }

        private void AddReturnParameter()
        {
            DbParameter dbParameter = this.command.CreateParameter();
            dbParameter.Direction = ParameterDirection.ReturnValue;
            this.command.Parameters.Add(dbParameter);
        }

        private int GetReturnValue()
        {
            return (int)this.command.Parameters[this.command.Parameters.Count - 1].Value;
        }

        private DataTable DoExecuteProcedureAndReturnValue(out int returnValue)
        {
            DataTable result;
            try
            {
                this.AddReturnParameter();
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                DataTable dataTable = DbHelper.ReaderToDataTable(reader, this.fieldNameToLower, false);
                returnValue = this.GetReturnValue();
                this.DoLogRowCount((dataTable != null) ? dataTable.Rows.Count : 0);
                result = dataTable;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private IHashMapList DoProcedureSelectAndReturnValue(out int returnValue)
        {
            IHashMapList result;
            try
            {
                this.AddReturnParameter();
                DbDataReader reader = this.command.ExecuteReader();
                this.DoAfterExecute();
                IHashMapList HashMapList = DbHelper.ReaderToObjectList(reader, this.fieldNameToLower, false);
                returnValue = this.GetReturnValue();
                this.DoLogRowCount((HashMapList != null) ? HashMapList.Count : 0);
                result = HashMapList;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private int DoExecuteNoQueryProcedure(bool convertParameterName)
        {
            int result;
            try
            {
                int num = this.command.ExecuteNonQuery();
                this.GetContextOutputParameter(convertParameterName);
                this.DoLogRowCount(-1);
                result = num;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private object DoExecuteScalarProcedure()
        {
            object result;
            try
            {
                result = this.command.ExecuteScalar();
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private object DoExecuteOutLastParameterProcedure()
        {
            object result;
            try
            {
                this.command.ExecuteNonQuery();
                for (int i = this.command.Parameters.Count - 1; i > 0; i--)
                {
                    DbParameter dbParameter = this.command.Parameters[i];
                    if (dbParameter.Direction == ParameterDirection.Output || dbParameter.Direction == ParameterDirection.InputOutput)
                    {
                        result = dbParameter.Value;
                        return result;
                    }
                }
                result = null;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        private int DoExecuteReturnValueProcedure()
        {
            int returnValue;
            try
            {
                this.AddReturnParameter();
                this.command.ExecuteNonQuery();
                returnValue = this.GetReturnValue();
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return returnValue;
        }

        private void DoExecuteReturnValueProcedure(out int value)
        {
            try
            {
                this.AddReturnParameter();
                this.command.ExecuteNonQuery();
                value = this.GetReturnValue();
            }
            catch (System.Exception e)
            {
                value = this.GetReturnValue();
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
        }

        private int DoExecuteOutputParameterProcedureAndReturnValue(System.Collections.Generic.IDictionary<string, object> outputParameterValues, bool convertParameterName)
        {
            int returnValue;
            try
            {
                this.AddReturnParameter();
                this.command.ExecuteNonQuery();
                this.GetOutputParameterValues(outputParameterValues, convertParameterName);
                returnValue = this.GetReturnValue();
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return returnValue;
        }

        private IHashMap DoExecuteOutputManyParameterProcedure(bool convertParameterName)
        {
            IHashMap result;
            try
            {
                this.command.ExecuteNonQuery();
                IHashMap HashMap = new HashMap();
                this.GetOutputParameterValues(HashMap, convertParameterName);
                result = HashMap;
            }
            catch (System.Exception e)
            {
                this.DoLogErrorSQL(e);
                throw;
            }
            finally
            {
                this.DoEndExecute();
            }
            return result;
        }

        public bool DropTable(string tableName)
        {
            bool result;
            if (this.DbType == DbType.MySql)
            {
                result = (this.ExecuteNonQuerySQL("drop table if exists " + tableName) == 0);
            }
            else if (this.DbType == DbType.Sql2000)
            {
                result = (this.ExecuteNonQuerySQL(string.Format("if exists (select * from sysobjects where xtype='u' and name='{0}') drop table {0}", tableName)) == -1);
            }
            else
            {
                if (this.DbType != DbType.Firebird)
                {
                    throw new System.InvalidOperationException();
                }
                result = (!this.TableExists(tableName) || this.ExecuteNonQuerySQL("drop table " + tableName) == -1);
            }
            return result;
        }

        public bool TableExists(string tableName)
        {
            bool result;
            if (this.DbType == DbType.MySql)
            {
                this.AddParameter("database", this.Connection.Database);
                this.AddParameter("tableName", tableName);
                result = (this.ExecuteScalerSQL("select TABLE_NAME from information_schema.tables where table_schema=@database and TABLE_NAME=@tableName") != null);
            }
            else if (this.DbType == DbType.Sql2000)
            {
                this.AddParameter("tableName", tableName);
                result = (this.ExecuteScalerSQL("select name from sysobjects where xtype='u' and name=@tableName") != null);
            }
            else
            {
                if (this.DbType != DbType.Firebird)
                {
                    throw new System.InvalidOperationException();
                }
                this.AddParameter("tableName", tableName.ToUpper());
                int num = (int)this.ExecuteScalerSQL("SELECT COUNT(RDB$RELATION_NAME) FROM RDB$RELATIONS WHERE (RDB$RELATION_NAME=@tableName)\r\n AND RDB$VIEW_SOURCE IS NULL");
                result = (num > 0);
            }
            return result;
        }

        public int GetCount(string tableName)
        {
            return this.ExecuteIntSQL("select count(*) from " + tableName);
        }

        public int BatchExecute(string sql)
        {
            return this.BatchExecute(sql, null);
        }

        public int BatchExecute(string sql, DbHelper.SqlStatementExecutedEventHandler onStatementExecuted)
        {
            return -1;
        }

     

        public static DbHelper GetContextDbHelper()
        {
            HttpContext current = HttpContext.Current;
            DbHelper result;
            if (current == null)
            {
                if (DbHelper.testDbHelper == null)
                {
                    DbHelper.testDbHelper = DbHelper.CreateContextDbHelper();
                    string friendlyName = System.AppDomain.CurrentDomain.FriendlyName;
                    if (friendlyName.IndexOf("nunit", System.StringComparison.OrdinalIgnoreCase) < 0 && friendlyName.IndexOf("test", System.StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        throw new System.InvalidOperationException("不支持的AppDomain：" + friendlyName);
                    }
                    System.AppDomain.CurrentDomain.DomainUnload += new System.EventHandler(DbHelper.DoDomainUnload);
                }
                result = DbHelper.testDbHelper;
            }
            else if (!current.Items.Contains("__ContextDbHelper"))
            {
                DbHelper dbHelper = DbHelper.CreateContextDbHelper();
                current.Items["__ContextDbHelper"] = dbHelper;
                result = dbHelper;
            }
            else
            {
                result = (DbHelper)current.Items["__ContextDbHelper"];
            }
            return result;
        }

        private static void DoDomainUnload(object sender, System.EventArgs e)
        {
            DbHelper.testDbHelper.Dispose();
            DbHelper.testDbHelper = null;
        }

        private void TrackCreate()
        {
            if (this.IsTrackingUsage)
            {
                HttpContext current = HttpContext.Current;
                if (current == null)
                {


                    {
                        lock (DbHelper.dbHelperTrackThread)
                        {
                            int managedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                            System.Collections.IList list;
                            if (!DbHelper.dbHelperTrackThread.TryGetValue(managedThreadId, out list))
                            {
                                list = new System.Collections.ArrayList();
                                DbHelper.dbHelperTrackThread[managedThreadId] = list;
                            }
                            list.Add(this);
                        }
                    }
                }
                else
                {
                    System.Collections.IList list = DbHelper.GetDbHelperTrackList(current);
                    if (list == null)
                    {
                        list = new System.Collections.ArrayList();
                        current.Items["__DbHelperTrackList"] = list;
                    }
                    list.Add(this);
                }
            }
        }

        private void TrackDispose()
        {
            if (this.IsTrackingUsage)
            {
                HttpContext current = HttpContext.Current;
                if (current == null)
                {

                    {
                        lock (DbHelper.dbHelperTrackThread)
                        {
                            int managedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                            System.Collections.IList dbHelperTrackList;
                            if (DbHelper.dbHelperTrackThread.TryGetValue(managedThreadId, out dbHelperTrackList))
                            {
                                dbHelperTrackList.Remove(this);
                            }
                        }
                    }
                }
                else
                {
                    System.Collections.IList dbHelperTrackList = DbHelper.GetDbHelperTrackList(current);
                    if (dbHelperTrackList != null)
                    {
                        dbHelperTrackList.Remove(this);
                    }
                }
            }
        }

        private static System.Collections.IList GetDbHelperTrackList(HttpContext context)
        {
            System.Collections.IList result;
            if (context.Items.Contains("__DbHelperTrackList"))
            {
                result = (System.Collections.IList)context.Items["__DbHelperTrackList"];
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static void CheckThreadDispose(string methodName)
        {
            if (DbHelper.IsGlobalTrackingUsage)
            {
                string fullMethodName = InternalUtils.FormatFullMethodName(System.Threading.Thread.CurrentThread, methodName);
                int managedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                System.Collections.IList list;
                if (DbHelper.dbHelperTrackThread.TryGetValue(managedThreadId, out list))
                {
                    DbHelper.CheckDbHelperList(list, fullMethodName);
                }
            }
        }

        public static void DisposeContextDbHelper(string fullMethodName)
        {
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                if (current.Items.Contains("__ContextDbHelper"))
                {
                    DbHelper dbHelper = (DbHelper)current.Items["__ContextDbHelper"];
                    dbHelper.Dispose();
                    current.Items.Remove("__ContextDbHelper");
                }
                if (DbHelper.IsGlobalTrackingUsage)
                {
                    System.Collections.IList dbHelperTrackList = DbHelper.GetDbHelperTrackList(current);
                    if (dbHelperTrackList != null)
                    {
                        current.Items.Remove("__DbHelperTrackList");
                        DbHelper.CheckDbHelperList(dbHelperTrackList, fullMethodName);
                    }
                }
            }
        }

        private static void CheckDbHelperList(System.Collections.IList list, string fullMethodName)
        {
            if (list.Count == 0)
            {
                return;
            }
            string text = string.Format("{0} 代码中存在 {1} 个没有通过using释放的DbHelper实例，最后执行SQL分别如下：", fullMethodName, list.Count);
            foreach (DbHelper dbHelper in list)
            {
                text = text + "\r\n\r\n" + dbHelper.command.CommandText;
            }
            System.Collections.ArrayList arrayList = new System.Collections.ArrayList(list);
            foreach (DbHelper dbHelper in list)
            {
                dbHelper.Dispose();
            }
            throw new System.InvalidOperationException(text);
        }

        private static DbHelper CreateContextDbHelper()
        {
            IDbHelperCreator dbHelperCreator = Settings.DbHelperCreator;
            if (dbHelperCreator == null)
            {
                throw new System.InvalidOperationException("请先设置全局属性 Settings.DbHelperCreator");
            }
            DbHelper dbHelper = dbHelperCreator.CreateDbHelper();
            if (dbHelper == null)
            {
                throw new System.InvalidOperationException("IDbHelperCreator.CreateDbHelper 不能返回 null");
            }
            return dbHelper;
        }
    }

    public enum DbType
    {
        Sql2000,
        MySql,
        Oracle,
        Access,
        Firebird
    }

    public static class SqlHelper
    {
        private const string selectTemplate = "select {1} from {0}";

        private const string insertTemplate = "insert into {0} ({1}) values({2})";

        private const string updateTemplate = "update {0} set {1} where {2}=@{2}";

        private const string updateMultiTemplate = "update {0} set {1} where {2}";

        private const string deleteTemplate = "delete from {0} where {1}=@{1}";

        private const string deleteMultiTemplate = "delete from {0} where {1}";

        public static string StringListToCommaText(string[] items)
        {
            return SqlHelper.StringListToCommaText(items, string.Empty, string.Empty);
        }

        public static string StringListToCommaText(string[] items, string prefix)
        {
            return SqlHelper.StringListToCommaText(items, prefix, string.Empty);
        }

        public static string StringListToCommaText(string[] items, string prefix, string postfix)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < items.Length; i++)
            {
                string field = items[i];
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.Append(',');
                }
                stringBuilder.Append(prefix);
                stringBuilder.Append(SqlHelper.FilterField(field));
                stringBuilder.Append(postfix);
            }
            return stringBuilder.ToString();
        }

        public static string BuildSelectSql(string tableName, params string[] fieldNames)
        {
            string arg = SqlHelper.StringListToCommaText(fieldNames);
            return string.Format("select {1} from {0}", SqlHelper.FilterField(tableName), arg);
        }

        public static string BuildInsertSql(string tableName, params string[] fieldNames)
        {
            string arg = SqlHelper.StringListToCommaText(fieldNames);
            string arg2 = SqlHelper.StringListToCommaText(fieldNames, "@");
            return string.Format("insert into {0} ({1}) values({2})", SqlHelper.FilterField(tableName), arg, arg2);
        }

        public static string BuildUpdateSql(string tableName, string keyField, params string[] otherFieldNames)
        {
            System.Text.StringBuilder partsByPrefix = SqlHelper.GetPartsByPrefix(otherFieldNames, ",");
            return string.Format("update {0} set {1} where {2}=@{2}", SqlHelper.FilterField(tableName), partsByPrefix.ToString(), SqlHelper.FilterField(keyField));
        }

        private static System.Text.StringBuilder GetPartsByPrefix(string[] otherFieldNames, string prefix = ",")
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < otherFieldNames.Length; i++)
            {
                string field = otherFieldNames[i];
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.Append(prefix);
                }
                stringBuilder.AppendFormat("{0}=@{0}", SqlHelper.FilterField(field));
            }
            return stringBuilder;
        }

        public static string BuildUpdateSql(string tableName, params string[] fieldNames)
        {
            string keyField = fieldNames[0];
            string[] array = new string[fieldNames.Length - 1];
            for (int i = 1; i < fieldNames.Length; i++)
            {
                array[i - 1] = fieldNames[i];
            }
            return SqlHelper.BuildUpdateSql(tableName, keyField, array);
        }

        public static string BuildUpdateSql(string tableName, string[] keyFieldNames, params string[] fieldNames)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(keyFieldNames);
            System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
            for (int i = 0; i < fieldNames.Length; i++)
            {
                string item = fieldNames[i];
                if (!list.Contains(item))
                {
                    list2.Add(item);
                }
            }
            System.Text.StringBuilder partsByPrefix = SqlHelper.GetPartsByPrefix(list2.ToArray(), ",");
            System.Text.StringBuilder partsByPrefix2 = SqlHelper.GetPartsByPrefix(keyFieldNames, " and ");
            return string.Format("update {0} set {1} where {2}", SqlHelper.FilterField(tableName), partsByPrefix.ToString(), partsByPrefix2.ToString());
        }

        public static string BuildDeleteSql(string tableName, string keyField)
        {
            return string.Format("delete from {0} where {1}=@{1}", SqlHelper.FilterField(tableName), SqlHelper.FilterField(keyField));
        }

        public static string BuildDeleteSql(string tableName, string[] keyFieldNames)
        {
            System.Text.StringBuilder partsByPrefix = SqlHelper.GetPartsByPrefix(keyFieldNames, " and ");
            return string.Format("delete from {0} where {1}", SqlHelper.FilterField(tableName), partsByPrefix.ToString());
        }

        public static string GetQuotedStr(string str)
        {
            string str2 = "'";
            if (str != null)
            {
                str2 += str.Replace("'", "''");
            }
            return str2 + "'";
        }

        internal static string DbParameterToString(object value)
        {
            string result;
            if (value == null || value == System.DBNull.Value)
            {
                result = "NULL";
            }
            else
            {
                System.Type type = value.GetType();
                if (type == typeof(ulong) || type == typeof(long) || type == typeof(decimal) || type == typeof(double) || type == typeof(int) || type == typeof(short))
                {
                    result = value.ToString();
                }
                else if (type == typeof(System.DateTime))
                {
                    result = SqlHelper.GetQuotedStr(((System.DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (type == typeof(string))
                {
                    result = SqlHelper.GetQuotedStr((string)value);
                }
                else if (type.IsSubclassOf(typeof(System.Enum)))
                {
                    result = ((int)value).ToString();
                }
                else
                {
                    result = SqlHelper.GetQuotedStr(value.ToString());
                }
            }
            return result;
        }

        public static string FilterField(string field)
        {
            return field.Replace(" ", "");
        }
    }

    internal sealed class MysqlConnectionInfo
    {
        public System.DateTime AccessTime;

        public string CommandText;

        public string ParamsText;
    }

    public static class InternalUtils
    {
        internal const string DefaultEncodingName = "gb2312";

        internal const string SysDir = "_Sys";

        internal const string SysAssemblyName = "Carpa.Web";

        internal static readonly System.Text.Encoding DefaultEncoding = System.Text.Encoding.GetEncoding("gb2312");

        private static System.Collections.Hashtable assemblyInfoCache = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

        private static System.DateTime minVersionDateTime = new System.DateTime(2000, 1, 1, 0, 0, 0);

        private static readonly long minModifiedTimeTicks = new System.DateTime(2008, 5, 12, 0, 0, 0).Ticks;

        private static readonly string[] cMods = new string[]
		{
			"Ctrl",
			"Shift",
			"Alt"
		};

        internal static string GetPageParams(HttpRequest request)
        {
            string text = request.QueryString["__Params"];
            if (string.IsNullOrEmpty(text))
            {
                text = request.Form["__Params"];
            }
            return text;
        }

        internal static string FormatPageParams(System.Collections.Generic.IDictionary<string, object> pageParams)
        {
            string result;
            if (pageParams == null)
            {
                result = string.Empty;
            }
            else
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                bool flag = true;
                foreach (System.Collections.Generic.KeyValuePair<string, object> current in pageParams)
                {
                    if (flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        stringBuilder.Append(',');
                    }
                    string value = SqlHelper.DbParameterToString(current.Value);
                    stringBuilder.Append(current.Key).Append('=').Append(value);
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        internal static bool IsDebuggingSql(HttpContext context)
        {
            bool result = false;
            bool flag = false;
            InternalUtils.InitSqlDebug(context, ref result, ref flag);
            return result;
        }

        internal static void InitSqlDebug(ref bool isDebuggingSql, ref bool isNoLoggingSql)
        {
            InternalUtils.InitSqlDebug(HttpContext.Current, ref isDebuggingSql, ref isNoLoggingSql);
        }

        private static void InitSqlDebug(HttpContext context, ref bool isDebuggingSql, ref bool isNoLoggingSql)
        {
            if (context != null)
            {
                HttpSessionState session = context.Session;
                if (session != null)
                {
                    isDebuggingSql = (context.Session["__DebugSql"] != null);
                    isNoLoggingSql = (context.Session["__NoLogSql"] != null);
                }
            }
        }

        internal static System.DateTime GetAssemblyModifiedTime(System.Reflection.Assembly assembly)
        {
            object obj = InternalUtils.assemblyInfoCache[assembly];
            System.DateTime result;
            if (obj == null)
            {
                lock (InternalUtils.assemblyInfoCache)
                {
                    obj = InternalUtils.assemblyInfoCache[assembly];
                    if (obj == null)
                    {
                        System.DateTime dateTime = InternalUtils.LogAssemblyDateTime(assembly);
                        InternalUtils.assemblyInfoCache[assembly] = dateTime;
                        result = dateTime;
                        return result;
                    }
                    result = (System.DateTime)obj;
                    return result;
                }
            }
            result = (System.DateTime)obj;
            return result;
        }

        internal static System.DateTime LogAssemblyDateTime(System.Reflection.Assembly assembly)
        {
            System.Reflection.AssemblyName name = assembly.GetName();
            string localPath = new Uri(name.CodeBase).LocalPath;
            System.DateTime lastWriteTime = System.IO.File.GetLastWriteTime(localPath);
            System.DateTime result = new System.DateTime(lastWriteTime.Year, lastWriteTime.Month, lastWriteTime.Day, lastWriteTime.Hour, lastWriteTime.Minute, lastWriteTime.Second);
            string text = "文件时间 " + result.ToString("yyyy-MM-dd HH:mm:ss");
            int build = name.Version.Build;
            if (build >= 3210)
            {
                result = InternalUtils.minVersionDateTime.AddDays((double)build).AddSeconds((double)(name.Version.Revision * 2));
                text = string.Concat(new object[]
				{
					"版本 ",
					name.Version,
					"，内部时间 ",
					result.ToString("yyyy-MM-dd HH:mm:ss"),
					"，",
					text
				});
            }

            return result;
        }

        internal static int GetVersionTag(System.DateTime modifiedTime)
        {
            return (int)((modifiedTime.Ticks - InternalUtils.minModifiedTimeTicks) / 10000000L);
        }

        internal static bool IsRequestNotModified(HttpContext context, System.DateTime lastModifiedTime)
        {
            string text = context.Request.Headers["If-Modified-Since"];
            bool result;
            if (text != null)
            {
                int num = text.IndexOf(';');
                if (num > 0)
                {
                    text = text.Substring(0, num);
                }
                System.DateTime t;
                if (System.DateTime.TryParse(text, out t))
                {
                    if (t >= lastModifiedTime)
                    {
                        context.Response.StatusCode = 304;
                        result = true;
                        return result;
                    }
                }
            }
            result = false;
            return result;
        }

        internal static void SetCacheLastModified(HttpContext context, System.DateTime lastModifiedTime, bool neverExpires)
        {
            if (lastModifiedTime.ToUniversalTime() < System.DateTime.UtcNow)
            {
                HttpCachePolicy cache = context.Response.Cache;
                cache.SetCacheability(HttpCacheability.Public);
                cache.SetLastModified(lastModifiedTime);
                if (neverExpires)
                {
                    cache.SetExpires(System.DateTime.Now.AddYears(1));
                    cache.SetValidUntilExpires(true);
                }
            }
        }


        internal static bool GetIsMessageException(System.Exception ex)
        {
            return false;
        }

        internal static bool NeedLog(System.Exception ex)
        {
            return !InternalUtils.GetIsMessageException(ex);
        }

        internal static void LogError(string message, System.Exception ex, HttpRequest request)
        {
            InternalUtils.LogError(message, ex, request, null);
        }

        internal static void LogError(string message, System.Exception ex, HttpRequest request, HttpSessionState session)
        {

        }

        internal static string DictionaryToQueryString(System.Collections.Generic.IDictionary<string, object> dict)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            foreach (System.Collections.Generic.KeyValuePair<string, object> current in dict)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('&');
                }
                string key = current.Key;
                if (!string.IsNullOrEmpty(key))
                {
                    stringBuilder.Append(HttpUtility.UrlEncodeUnicode(key)).Append('=');
                }
                if (current.Value != null)
                {
                    stringBuilder.Append(HttpUtility.UrlEncodeUnicode(current.Value.ToString()));
                }
            }
            return stringBuilder.ToString();
        }

        internal static void CheckDispose(object instance, string methodName)
        {
            string fullMethodName = InternalUtils.FormatFullMethodName(instance, methodName);
            DbHelper.DisposeContextDbHelper(fullMethodName);
        }

        internal static string FormatFullMethodName(object instance, string methodName)
        {
            return (instance != null) ? string.Format("{0}.{1}", instance.GetType().FullName, methodName) : null;
        }

        public static string GetSessionVarStr(string varName)
        {
            string result;
            if (!string.IsNullOrEmpty(varName))
            {
                object obj = HttpContext.Current.Session[varName];
                result = ((obj != null) ? obj.ToString() : null);
            }
            else
            {
                result = null;
            }
            return result;
        }

        internal static string GetLoginUserGuid()
        {
            string sessionVarStr = InternalUtils.GetSessionVarStr(Settings.ProfileKeySessionVarName);
            string profileId = InternalUtils.GetProfileId(sessionVarStr);
            string str = null;
            if (!string.IsNullOrEmpty(profileId))
            {
                str = profileId + "_";
            }
            string sessionVarStr2 = "";
            return str + sessionVarStr2;
        }

        public static string GetProfileId(string profileKey)
        {
            string result;
            if (!string.IsNullOrEmpty(profileKey))
            {
                profileKey = InternalUtils.ExtractConnectInfo(profileKey);
                result = StringUtils.GetHashCode(profileKey);
            }
            else
            {
                result = null;
            }
            return result;
        }

        private static string ExtractConnectInfo(string connectionString)
        {
            string result;
            if (connectionString.Contains("Data Source="))
            {
                try
                {
                    DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
                    dbConnectionStringBuilder.ConnectionString = connectionString;
                    object obj;
                    object obj2;
                    if (dbConnectionStringBuilder.TryGetValue("data source", out obj) && dbConnectionStringBuilder.TryGetValue("initial catalog", out obj2))
                    {
                        string text = (obj != null) ? obj.ToString().ToLower() : null;
                        string text2 = (obj2 != null) ? obj2.ToString().ToLower() : null;
                        if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
                        {
                            if (text == "localhost" || text == "127.0.0.1")
                            {
                                text = "(local)";
                            }
                            result = text + "_" + text2;
                            return result;
                        }
                    }
                }
                catch (System.ArgumentException)
                {
                }
            }
            result = connectionString.ToLower();
            return result;
        }

        internal static string GetBrowserNameVersion()
        {
            HttpRequest request = HttpContext.Current.Request;
            string userAgent = request.UserAgent;
            string arg = request.Browser.Browser;
            float num = (float)request.Browser.MajorVersion;
            bool flag = userAgent.Contains(" MSIE ");
            if (flag || userAgent.Contains("Trident/"))
            {
                arg = "IE";
                string pattern = flag ? "MSIE\\s(\\d+\\.\\d+)" : "rv:(\\d+\\.\\d+)";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = regex.Match(userAgent);
                float.TryParse(match.Groups[1].Value, out num);
            }
            return string.Format("{0}_{1}", arg, num);
        }
    }
}
