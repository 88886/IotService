#若无必要，勿增实体

#当前支持接口
   [Description("打印机列表")]
        printerList = 0,

        [Description("打印任务")]
        printJob = 1,

        [Description("烧录下载")]
        BurnSoft = 2,

        [Description("获取本机IP")]
        httpServer = 3,

        [Description("获取电子秤重量")]
        com_weighting = 4,

        [Description("获取串口列表")]
        comList = 5,

        [Description("配置串口参数")]
        comSetting = 6,

        [Description("打开串口")]
        openCom = 7,

        [Description("关闭串口")]
        closeCom = 8,

        [Description("向串口发送指令（十六进制）")]
        sendComCommand = 9,

        [Description("删除目录")]
        rmDir = 10,

        [Description("创建目录")]
        mkDir = 11,

        [Description("重命名目录或者移动目录")]
        mvDir = 12,

        [Description("删除文件")]
        deleteFile = 13,

        [Description("创建文件")]
        createFile = 14,

        [Description("重命名文件")]
        renameFile = 15,

        [Description("监控目录")]
        watchDir = 16,

        [Description("配置访问FTP服务器需要的参数")]
        ftpSetting = 17,

        [Description("解析文件配置")]
        resolveFile = 18,

        [Description("通过modbus协议从plc从机设备的某个寄存器地址读取值")]
        modbusInteract = 19,

        [Description("启动三菱PLC")]
        StartSLPLC = 20,

        [Description("停用三菱PLC")]
        stopSLPlc = 21,


下一步增加接口为：
西门子PLC支持
RFID自动扫描枪支持

支持动态启动TCP服务器
支持动态发送TCP请求

支持动态启动HTTP服务器
支持动态发送HTTP请求