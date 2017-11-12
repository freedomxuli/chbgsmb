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
            color:orange;
            font-size:15px;
            padding-top:5px;
            float:none;
        }
        .menu {
            
        }
        .menu li{
            list-style-type:none;
            font-size:10px;
            color:#3448a1;
            padding:1px;
            cursor:pointer;
        }
        .menu li:hover {
            background-color:#ffcc66;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" height="100%" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td colspan="2" style="border-bottom:2px solid #808080;" align="center"><div class="Title"></div><div style="float:right;"><img src="../Images/sm1.png" width="10" height="5"/>&nbsp;<span style="font-size:8px;">在途</span>&nbsp;<img src="../Images/sm2.png" width="10" height="5"/>&nbsp;<span style="font-size:8px;">到达</span>&nbsp;<img src="../Images/sm3.png" width="10" height="5"/>&nbsp;<span style="font-size:8px;">返程</span>&nbsp;</div></td>
        </tr>
        <tr>
            <td width="10%" style="border-right:2px solid #808080;background-color:#ffffcc;" valign="top">
                <ul class="menu">
                    
                </ul>
            </td>
            <td width="90%" height="350px"><div id="container"></div></td>
        </tr>
    </table>
    </form>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.4.1&key=4de8fc02064b304db7e4ec8ca9c18b50"></script>
    <script type="text/javascript">
        var gsid = "";
        $(function () {
            $.getUrlParam = function (name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return unescape(r[2]); return null;
            }
            gsid = $.getUrlParam('id');
            GetData(gsid, "");
        });

        //var map = new AMap.Map('container', {
        //    resizeEnable: true,
        //    zoom: 10,
        //    center: [116.480983, 40.0958]
        //});

        function GetData(id,username)
        {
            CS('CZCLZ.Model.GetDetailsByView', function (retVal) {
                if (retVal) {
                    $(".Title").html(retVal.dt_model[0]["ShowName"]);
                    var menu_html = "";
                    menu_html += "<li><div data-username='' >全部</div></li>";
                    for (var i = 0; i < retVal.dt_child.length; i++)
                    {
                        menu_html += "<li><div data-username='" + retVal.dt_child[i]["UserName"] + "'>" + retVal.dt_child[i]["Company"] + "</div></li>";
                    }
                    $(".menu").html(menu_html);

                    $(".menu li").click(function () {
                        GetData(id, $(this).children().data("username"));
                    });
                    cleatmarks();
                    for (var i = 0; i < retVal.dt.length; i++)
                    {
                        addmaker(retVal.dt[i]["jingweidu"].split(","), "", retVal.dt[i]["markinfo"], retVal.dt[i]["ZT"]);
                    }
                }
            }, CS.onError, id, username);
        }
    </script>
    <script type="text/javascript">
       //初始化地图对象，加载地图
       ////初始化加载地图时，若center及level属性缺省，地图默认显示用户当前城市范围
       var cluster, markers = [];

       var map = new AMap.Map('container', {
            resizeEnable: true,
            zoom: 5,
        });
        var infoWindow = new AMap.InfoWindow({ offset: new AMap.Pixel(0, -30) });
        //        var markers = [];
        //图层切换控件

        //地图中添加地图操作ToolBar插件
        map.plugin(['AMap.ToolBar'], function () {
            //设置地位标记为自定义标记
            var toolBar = new AMap.ToolBar();
            map.addControl(toolBar);
        });

        function cleatmarks() {
            map.remove(markers);
        }
        //p:经纬度 l:标签 i：信息窗口
        function addmaker(p, l, i,z) {
            var iconUrl = "../Images/zaitu.png";
            if (z == "1")
                iconUrl = "../Images/zaitu2.png";
            else if (z == "2")
                iconUrl = "../Images/zaitu3.png";
            var marker = new AMap.Marker({
                position: p,
                map: map,
                extData: false,
                icon: new AMap.Icon({
                    size: new AMap.Size(29, 20),  //图标大小
                    image: iconUrl,
                    imageOffset: new AMap.Pixel(0, 0)
                })
            });
            markers.push(marker);
            marker.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
                offset: new AMap.Pixel(35, 0), //修改label相对于maker的位置
                content: l
            });
            marker.content = i;

            marker.on('click', markerClick);

            markers.push(marker);


        }

        function markerClick(e) {

            infoWindow.setContent(e.target.content);
            infoWindow.open(map, e.target.getPosition());
        }
        function setview() {

            map.setFitView();
        }

        function addCluster() {
            map.plugin(["AMap.MarkerClusterer"], function () {
                cluster = new AMap.MarkerClusterer(map, markers);
            });
        }
    </script>
</body>
</html>
