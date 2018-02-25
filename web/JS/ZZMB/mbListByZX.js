var pageSize = 15;

var MbStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'ID', 'Addtime', 'ShowName', 'longitude', 'latitude', 'LevelNum', 'UserName', 'UserID'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

Ext.onReady(function () {

    Ext.define('mainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        store: MbStore,
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserName',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '账号（手机号）'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'ShowName',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '模板标题'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'longitude',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '经度'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'latitude',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '纬度'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'LevelNum',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '放大比例'
                            },
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'Addtime',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                format: 'Y-m-d',
                                text: '新增时间'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'ID',
                                width: 240,
                                sortable: false,
                                menuDisabled: true,
                                text: '操作',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    return "<a href='javascript:void(0);' onClick='Del(\"" + value + "\");'>删除</a>　<a href='javascript:void(0);' onClick='Xg(\"" + value + "\");'>修改名称</a>　<a href='javascript:void(0);' onClick='GL(\"" + value + "\",\"" + record.data.UserName + "\");'>设置地图</a>　<a href='javascript:void(0);' onClick='HQ(\"" + value + "\",\"" + record.data.longitude + "\",\"" + record.data.latitude + "\",\"" + record.data.LevelNum + "\");'>获取URL</a>";
                                }
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'textfield',
                                        width: 230,
                                        labelWidth: 60,
                                        id: 'UserName',
                                        fieldLabel: '账号'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询',
                                        handler: function () {
                                            DataBind(1);
                                        }
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'add',
                                        text: '新增',
                                        handler: function () {
                                            var win = new AddMb();
                                            win.show(null, function () {

                                            });
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: MbStore,
                                displayInfo: true
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new mainView();

    DataBind(1);
});

function DataBind(cp) {
    CS('CZCLZ.Model.GetModelListByZX', function (retVal) {
        if (retVal) {
            MbStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('UserName').getValue());
}

Ext.define('AddMb', {
    extend: 'Ext.window.Window',

    height: 400,
    width: 450,
    layout: {
        type: 'fit'
    },
    title: '新增模板',

    modal: true,
    id: 'AddMbWin',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'column'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: '账户（手机号）',
                            labelWidth: 100,
                            columnWidth: 1,
                            id: 'NameAdd',
                            padding: '50 10 50 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '模板标题',
                            labelWidth: 100,
                            columnWidth: 1,
                            id: 'ShowNameAdd',
                            padding: '0 10 50 10'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认',
                            iconCls: 'enable',
                            handler: function () {
                                if (Ext.getCmp("NameAdd").getValue() == "" || Ext.getCmp("NameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "账户不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp("ShowNameAdd").getValue() == "" || Ext.getCmp("ShowNameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "模板标题不能为空！");
                                    return false;
                                }
                                var NameAdd = Ext.getCmp("NameAdd").getValue();
                                CS('CZCLZ.Model.AddModelByZX', function (retVal) {
                                    if (retVal) {
                                        DataBind(1);
                                        //Ext.Msg.confirm("提示", "是否立即新增关联物流？", function (btn) {
                                        //    if (btn == "yes") {
                                        //        var win = new AddModelDetail({ "MID": retVal, "Company": Ext.getCmp("NameAdd").getValue() });
                                        //        win.show(null, function () {
                                        //            Ext.getCmp("AddMbWin").close();
                                        //            WLBind(retVal);
                                        //            Ext.getCmp("GSName").setText(NameAdd);
                                        //        });
                                        //    }
                                        //});
                                    }
                                }, CS.onError, Ext.getCmp("NameAdd").getValue(), Ext.getCmp("ShowNameAdd").getValue());
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('XGMb', {
    extend: 'Ext.window.Window',

    height: 400,
    width: 450,
    layout: {
        type: 'fit'
    },
    title: '修改模板',

    modal: true,
    id: 'XGMbWin',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'column'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: '账户（手机号）',
                            labelWidth: 100,
                            columnWidth: 1,
                            id: 'NameAdd',
                            padding: '50 10 50 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '模板标题',
                            labelWidth: 100,
                            columnWidth: 1,
                            id: 'ShowNameAdd',
                            padding: '0 10 50 10'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认',
                            iconCls: 'enable',
                            handler: function () {
                                if (Ext.getCmp("NameAdd").getValue() == "" || Ext.getCmp("NameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "账户不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp("ShowNameAdd").getValue() == "" || Ext.getCmp("ShowNameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "模板标题不能为空！");
                                    return false;
                                }
                                CS('CZCLZ.Model.XGModelByZX', function (retVal) {
                                    if (retVal) {
                                        Ext.Msg.alert("提示", "修改成功！", function (btn) {
                                            DataBind(1);
                                            me.close();
                                        });
                                    }
                                }, CS.onError, me.ID, Ext.getCmp("NameAdd").getValue(), Ext.getCmp("ShowNameAdd").getValue());
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

function Del(id) {
    Ext.Msg.confirm("提示", "是否删除？", function (btn) {
        if (btn == "yes") {
            CS('CZCLZ.Model.DeleteModelByZX', function (retVal) {
                if (retVal) {
                    Ext.Msg.alert("提示", "删除成功！", function () {
                        DataBind(1);
                    });
                }
            }, CS.onError, id);
        }
    });
}

function Xg(id) {
    CS('CZCLZ.Model.GetModelDetailByZX', function (retVal) {
        if (retVal) {
            var win = new XGMb({ "ID": id });
            win.show(null, function () {
                Ext.getCmp("NameAdd").setValue(retVal[0]["UserName"]);
                Ext.getCmp("ShowNameAdd").setValue(retVal[0]["ShowName"]);
            });
        }
    }, CS.onError, id);
}

function HQ(id,lng,lat,lv) {
    var win = new URLWin();
    win.show(null, function () {
        Ext.getCmp("Url").setText("http://templet.chahuobao.net/MbView/ViewByZX.aspx?id=" + id + "&lng=" + lng + "&lat=" + lat + "&lv=" + lv);
    });
}

Ext.define('URLWin', {
    extend: 'Ext.window.Window',

    height: 130,
    width: 865,
    layout: {
        type: 'fit'
    },
    title: '获取Url',
    modal: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    layout: {
                        type: 'fit'
                    },
                    items: [
                        {
                            xtype: 'label',
                            id: 'Url',
                            margin: '20 0 0 0',
                            padding: 10,
                            text: ''
                        }
                    ]
                }
            ],
            buttonAlign: 'center',
            buttons: [
                {
                    xtype: 'button',
                    algin: 'center',
                    iconCls: 'close',
                    text: '关闭',
                    handler: function () {
                        me.close();
                    }
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('AddModelDetail', {
    extend: 'Ext.window.Window',

    height: 459,
    width: 492,
    layout: {
        type: 'border'
    },
    title: '设置地图',
    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    region: 'north',
                    height: 459,
                    layout: {
                        align: 'center',
                        pack: 'center',
                        type: 'vbox'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            flex: 1,
                            width: 400,
                            height: 60,
                            layout: {
                                type: 'column'
                            },
                            border: 0,
                            items: [
                                {
                                    xtype: 'label',
                                    id: 'GSName',
                                    columnWidth: 1,
                                    padding: 20,
                                    text: ''
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '经度',
                                    labelWidth: 60,
                                    columnWidth: 1,
                                    id: 'longitude',
                                    readOnly: true,
                                    padding: 20
                                },
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '纬度',
                                    labelWidth: 60,
                                    columnWidth: 1,
                                    id: 'latitude',
                                    readOnly: true,
                                    padding: 20
                                },
                                {
                                    xtype: 'button',
                                    id: 'SelWL',
                                    text: '选择经纬度',
                                    iconCls: 'search',
                                    margin: 20,
                                    handler: function () {
                                        var win = new ShowMap();
                                        win.show(null, function () {

                                        });
                                    }
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: '放大等级',
                                    labelWidth: 60,
                                    columnWidth: 1,
                                    allowDecimals: false,
                                    minValue: 3,
                                    maxValue: 18,
                                    id: 'LevelNum',
                                    padding: 20,
                                    value:3
                                }
                            ]
                        }
                    ]
                }
            ],
            buttonAlign: 'center',
            buttons: [
                {
                    text: '确认',
                    iconCls: 'enable',
                    handler: function () {
                        if (Ext.getCmp("longitude").getValue() == "" || Ext.getCmp("longitude").getValue() == null) {
                            Ext.Msg.alert("提示", "经度不能为空！");
                            return false;
                        }
                        if (Ext.getCmp("latitude").getValue() == "" || Ext.getCmp("latitude").getValue() == null) {
                            Ext.Msg.alert("提示", "纬度不能为空！");
                            return false;
                        }
                        if (Ext.getCmp("LevelNum").getValue() == "" || Ext.getCmp("LevelNum").getValue() == null) {
                            Ext.Msg.alert("提示", "放大等级不能为空！");
                            return false;
                        }
                        CS('CZCLZ.Model.UpdateZX', function (retVal) {
                            if (retVal) {
                                Ext.Msg.alert("提示", "设置成功！", function () {
                                    DataBind(1);
                                    me.close();
                                });
                            }
                        }, CS.onError, me.ID, Ext.getCmp("longitude").getValue(), Ext.getCmp("latitude").getValue(), Ext.getCmp("LevelNum").getValue());
                    }
                },
                {
                    text: '关闭',
                    iconCls: 'close',
                    handler: function () {
                        me.close();
                    }
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('ShowMap', {
    extend: 'Ext.window.Window',

    height: 600,
    width: 800,
    layout: {
        type: 'fit'
    },
    title: '抓取地图（双击确认经纬度）',
    modal: true,
    id:'MapWin',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'fit'
                    },
                    border: 0,
                    html: '<iframe id="frame1" src="approot/r/MbView/GetMap.aspx" frameborder="0" width="100%" height="100%"></iframe>'
                }
            ],
            buttonAlign: 'center',
            buttons: [
                {
                    text: '关闭',
                    iconCls: 'close',
                    handler: function () {
                        me.close();
                    }
                }
            ]
        });

        me.callParent(arguments);
    }

});

function GL(id, UserName)
{
    var win = new AddModelDetail({ "ID": id, "UserName": UserName });
    win.show(null, function () {
        CS('CZCLZ.Model.GetZXXQ', function (retVal) {
            Ext.getCmp("longitude").setValue(retVal[0]["longitude"]);
            Ext.getCmp("latitude").setValue(retVal[0]["latitude"]);
            Ext.getCmp("LevelNum").setValue(retVal[0]["LevelNum"] != "" && retVal[0]["LevelNum"] != null ? retVal[0]["LevelNum"] : 5);
            Ext.getCmp("GSName").setText("账户：　　" + UserName);
        }, CS.onError, id);
    });
}