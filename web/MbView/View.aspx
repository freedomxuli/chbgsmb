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
    <form id="form1" runat="server">
    <table width="100%" height="100%" cellspacing="0" cellpadding="0" border="0">
        <tr class="wintitle">
            <td colspan="2" style="border-bottom:2px solid #808080;" align="center"><div class="Title ParentTitle"></div><div class="Title SonTitle">&nbsp;</div><div style="float:right;"><img src="../Images/sm1.png" width="10" height="5"/>&nbsp;<span style="font-size:12px;">在途</span>&nbsp;<span style="font-size:12px;" class="zt"></span>&nbsp;<img src="../Images/sm2.png" width="10" height="5"/>&nbsp;<span style="font-size:12px;">到达</span>&nbsp;<span style="font-size:12px;" class="dd"></span>&nbsp;<img src="../Images/sm3.png" width="10" height="5"/>&nbsp;<span style="font-size:12px;">返程</span>&nbsp;<span style="font-size:12px;" class="fc"></span>&nbsp;</div></td>
        </tr>
        <tr>
            <td width="10%" style="border-right:2px solid #808080;background-color:#ffffcc;" valign="top">
                <ul class="menu">
                    
                </ul>
            </td>
            <td class="wincontent" width="90%">
                <div id="container"></div>
                <div class="button-group">
                    <input type="button" class="button" id="mapshow" value="刷新地图" />
                </div>
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.4.1&key=4de8fc02064b304db7e4ec8ca9c18b50"></script>
    <script type="text/javascript">
        var gsid = "";
        var user = "";
        var isautofresh = true;
        var num = 0;
        var jsnum = 0;
        $(function () {
            var wheight = $(window).height();
            wincontent = wheight - 78;
            $(".wincontent").height(wincontent);
            $(".wintitle").height(78);
            $.getUrlParam = function (name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return unescape(r[2]); return null;
            }
            gsid = $.getUrlParam('id');
            GetData(gsid, user);
            setInterval("freshlable()", "1000");
            //var StartGetData = setInterval(function () {
            //    GetData(gsid, user);
            //}, 60000);
        });

        //var map = new AMap.Map('container', {
        //    resizeEnable: true,
        //    zoom: 10,
        //    center: [116.480983, 40.0958]
        //});

        function freshlable() {
            if (isautofresh == false) {
                return;
            }
            num = num + 1
            if (num >= 60) {
                if (jsnum > 20) {
                    window.location.reload();
                } else {
                    jsnum++;
                    num = 0;
                    GetData(gsid, user);
                }
            }
            $("#mapshow").val("自动刷新(" + (60 - num) + ")秒");
        }

        function GetData(id, username) {
            ACS('CZCLZ.Model.GetDetailsByView', function (retVal) {
                if (retVal) {
                    $(".ParentTitle").html(retVal.dt_model[0]["ShowName"]);
                    $(".zt").html(retVal.zt + "单");
                    $(".dd").html(retVal.dd + "单");
                    $(".fc").html(retVal.fc + "单");
                    var menu_html = "";
                    menu_html += "<li><div data-username='' >全部</div></li>";
                    for (var i = 0; i < retVal.dt_child.length; i++) {
                        menu_html += "<li><div data-username='" + retVal.dt_child[i]["UserName"] + "' data-company='" + retVal.dt_child[i]["Company"] + "'>" + retVal.dt_child[i]["Company"] + "</div></li>";
                    }
                    $(".menu").html(menu_html);

                    $(".menu li").click(function () {
                        user = $(this).children().data("username");
                        GetData(id, $(this).children().data("username"));
                        if ($(this).children().data("company") != "undefined" && $(this).children().data("company") != undefined) {
                            $(".SonTitle").html($(this).children().data("company"));
                        } else
                            $(".SonTitle").html("&nbsp;");
                    });
                    cleatmarks();
                    for (var i = 0; i < retVal.dt.length; i++) {
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
            center: [109.335937, 34.189085]
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
        function addmaker(p, l, i, z) {
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
                    size: new AMap.Size(26, 39),  //图标大小
                    image: iconUrl,
                    imageOffset: new AMap.Pixel(0, 0)
                })
            });
            markers.push(marker);
            //alert(l);
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
            setTimeout(function () {
                $(".amap-info-close").remove();
            }, 10);
        }

        //关闭信息窗体
        function closeInfoWindow() {
            map.clearInfoWindow();
        }

        function setview() {

            map.setFitView();
        }

        function addCluster() {
            map.plugin(["AMap.MarkerClusterer"], function () {
                cluster = new AMap.MarkerClusterer(map, markers);
            });
        }

        AMap.event.addDomListener(document.getElementById('mapshow'), 'click', function () {
            if (isautofresh == true) {
                isautofresh = false;
                $("#mapshow").val("停止自动刷新");
            }
            else {
                isautofresh = true;
                i = 0;
            }
        }, false);
    </script>
</body>
</html>
