<%@ Page Language="C#" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no, width=device-width" />
    <title></title>
    <script type="text/javascript" src="../JS/extjs/ext-all.js"></script>
    <link rel="Stylesheet" type="text/css" href="../JS/extjs/resources/css/ext-all.css" />
    <link rel="stylesheet" href="http://cache.amap.com/lbs/static/main.css?v=1.0" />
    <script type="text/javascript" src="../JS/extjs/ext-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../JS/json.js"></script>
    <script type="text/javascript" src="../JS/cb.js"></script>
    <script type="text/javascript" src="../JS/fun.js"></script>
    <script type="text/javascript" src="../JS/jquery-1.7.1.min.js"></script>
    <style type="text/css">
        body,html,#container{
            height: 100%;
            margin: 0px;
        }
        ul, li {
            margin: 0px;
            padding: 0px;
        }
        .Title {
            color:red;
            font-size:20px;
            padding-top:5px;
            float:none;
        }
        .menu {
            
        }
        .menu li{
            list-style-type:none;
            font-size:10px;
            color:#3448a1;
            padding:10px;
            cursor:pointer;
            text-align:center;
        }
        .menu li:hover {
            background-color:#ffcc66;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="height:100%;">
        <div id="container"></div>
    </form>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.4.1&key=4de8fc02064b304db7e4ec8ca9c18b50"></script>
    <script type="text/javascript">
        //初始化地图对象，加载地图
        ////初始化加载地图时，若center及level属性缺省，地图默认显示用户当前城市范围

        var map = new AMap.Map('container', {
            resizeEnable: true,
            zoom: 5,
            center: [109.335937, 34.189085]
        });

        var clickEventListener = map.on('dblclick', function (e) {
            //document.getElementById("lnglat").value = e.lnglat.getLng() + ',' + e.lnglat.getLat()
            parent.Ext.getCmp("longitude").setValue(e.lnglat.getLng());
            parent.Ext.getCmp("latitude").setValue(e.lnglat.getLat());
            Ext.Msg.alert("提示", "抓取成功！", function () {
                parent.Ext.getCmp("MapWin").close();
            });
        });

    </script>
</body>
</html>
