﻿var pageSize = 15;

var MbStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'ID', 'Name', 'ShowName', 'Addtime'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

var yglwlStore = Ext.create('Ext.data.Store', {
    fields: [
        "MX_ID", "Company", "MID", "ShowCompany"
    ]
});

var SelStore = Ext.create('Ext.data.Store', {
    fields: [
        "ID", "Name"
    ]
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
                                dataIndex: 'Name',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '公司名称'
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
                                    return "<a href='javascript:void(0);' onClick='Xg(\"" + value + "\");'>修改名称</a>　<a href='javascript:void(0);' onClick='GL(\"" + value + "\",\"" + record.data.Name + "\");'>关联物流</a>　<a href='javascript:void(0);' onClick='HQ(\"" + value + "\");'>获取URL</a>";
                                }
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                hidden: true,
                                items: [
                                    {
                                        xtype: 'textfield',
                                        width: 230,
                                        labelWidth: 60,
                                        id: 'Company',
                                        fieldLabel: '公司名称'
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

Ext.define('SelectWL', {
    extend: 'Ext.window.Window',

    height: 447,
    width: 216,
    layout: {
        type: 'fit'
    },
    title: '选择物流',
    modal: true,
    id: 'SelWLlist',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gridpanel',
                    columnLines: 1,
                    border: 1,
                    store: SelStore,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Name',
                            flex: 1,
                            sortable: false,
                            menuDisabled: true,
                            text: '企业'
                        },
                        {
                            xtype: 'gridcolumn',
                            flex: 1,
                            dataIndex: 'ID',
                            sortable: false,
                            menuDisabled: true,
                            text: '操作',
                            renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                return "<a href='javascript:void(0);' onClick='CheckWL(\"" + me.MID + "\",\"" + record.data.Name + "\",\"" + value + "\");'>选择</a>";
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
                                    fieldLabel: '公司',
                                    id: 'SearchCompany',
                                    labelWidth: 40,
                                    width: 120
                                },
                                {
                                    xtype: 'button',
                                    text: '查询',
                                    iconCls: 'search',
                                    handler: function () {
                                        CheckWLData(me.MID, Ext.getCmp("SearchCompany").getValue());
                                    }
                                }
                            ]
                        }
                    ]
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

Ext.define('AddModelDetail', {
    extend: 'Ext.window.Window',

    height: 459,
    width: 492,
    layout: {
        type: 'border'
    },
    title: '关联企业',
    modal: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    region: 'north',
                    height: 100,
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
                                    xtype: 'button',
                                    id: 'SelWL',
                                    text: '选择',
                                    iconCls: 'search',
                                    margin: 20,
                                    handler: function () {
                                        var win = new SelectWL({ "MID": me.MID });
                                        win.show(null, function () {
                                            CheckWLData(me.MID, "");
                                        });
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    region: 'center',
                    title: '已关联企业列表',
                    store: yglwlStore,
                    autoScroll: true,
                    columnLines: 1,
                    border: 1,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ShowCompany',
                            width: 180,
                            sortable: false,
                            menuDisabled: true,
                            text: '企业显示名称'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'MID',
                            width: 180,
                            sortable: false,
                            menuDisabled: true,
                            hidden: true,
                            text: '模板id'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'MX_ID',
                            width: 180,
                            sortable: false,
                            menuDisabled: true,
                            text: '操作',
                            renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                return "<a href='javascript:void(0);' onClick='DelYGLWL(\"" + value + "\",\"" + record.data.MID + "\");'>删除</a>　<a href='javascript:void(0);' onClick='XGYGLWL(\"" + value + "\",\"" + record.data.MID + "\");'>修改名称</a>";
                            }
                        }
                    ]
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

Ext.define('AddMb', {
    extend: 'Ext.window.Window',

    height: 400,
    width: 350,
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
                            fieldLabel: '公司名称',
                            labelWidth: 70,
                            columnWidth: 1,
                            id: 'NameAdd',
                            padding: '50 10 50 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '模板标题',
                            labelWidth: 70,
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
                                    Ext.Msg.alert("提示", "公司名称不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp("ShowNameAdd").getValue() == "" || Ext.getCmp("ShowNameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "模板标题不能为空！");
                                    return false;
                                }
                                var NameAdd = Ext.getCmp("NameAdd").getValue();
                                CS('CZCLZ.Model.AddModelByCHB', function (retVal) {
                                    if (retVal) {
                                        DataBind(1);
                                        Ext.Msg.confirm("提示", "是否立即新增关联物流？", function (btn) {
                                            if (btn == "yes") {
                                                var win = new AddModelDetail({ "MID": retVal, "Company": Ext.getCmp("NameAdd").getValue() });
                                                win.show(null, function () {
                                                    Ext.getCmp("AddMbWin").close();
                                                    WLBind(retVal);
                                                    Ext.getCmp("GSName").setText(NameAdd);
                                                });
                                            }
                                        });
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
    width: 350,
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
                            fieldLabel: '公司名称',
                            labelWidth: 70,
                            columnWidth: 1,
                            id: 'NameAdd',
                            padding: '50 10 50 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '模板标题',
                            labelWidth: 70,
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
                                    Ext.Msg.alert("提示", "公司名称不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp("ShowNameAdd").getValue() == "" || Ext.getCmp("ShowNameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "模板标题不能为空！");
                                    return false;
                                }
                                CS('CZCLZ.Model.XGModelByCHB', function (retVal) {
                                    if (retVal) {
                                        Ext.Msg.alert("提示", "修改成功！", function (btn) {
                                            DataBind(1);
                                            me.close();
                                        });
                                    }
                                }, CS.onError, me.MID, Ext.getCmp("NameAdd").getValue(), Ext.getCmp("ShowNameAdd").getValue());
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('ShowYGLWL', {
    extend: 'Ext.window.Window',

    height: 200,
    width: 350,
    layout: {
        type: 'fit'
    },
    title: '修改显示公司名称',

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
                            fieldLabel: '显示名称',
                            labelWidth: 70,
                            columnWidth: 1,
                            id: 'NameAdd',
                            padding: '50 10 50 10'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认',
                            iconCls: 'enable',
                            handler: function () {
                                if (Ext.getCmp("NameAdd").getValue() == "" || Ext.getCmp("NameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "公司名称不能为空！");
                                    return false;
                                }
                                CS('CZCLZ.Model.XGShowNameByCHB', function (retVal) {
                                    if (retVal) {
                                        Ext.Msg.alert("提示", "修改成功！", function (btn) {
                                            WLBind(me.mid);
                                            me.close();
                                        });
                                    }
                                }, CS.onError, me.MID, me.MX_ID, Ext.getCmp("NameAdd").getValue());
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('URLWin', {
    extend: 'Ext.window.Window',

    height: 130,
    width: 565,
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

function CheckWLData(mid, SearchCompany) {
    CS('CZCLZ.Model.SelectWLByCHB', function (retVal) {
        if (retVal) {
            SelStore.loadData(retVal);
        }
    }, CS.onError, mid, SearchCompany);
}

function CheckWL(mid, Company, modelid) {
    CS('CZCLZ.Model.CheckWLByCHB', function (retVal) {
        if (retVal) {
            Ext.Msg.alert("提示", "选择成功！", function () {
                WLBind(mid);
                Ext.getCmp("SelWLlist").close();
            });
        }
    }, CS.onError, mid, Company, modelid);
}

function WLBind(mid) {
    CS('CZCLZ.Model.WLBindByCHB', function (retVal) {
        if (retVal) {
            yglwlStore.loadData(retVal);
        }
    }, CS.onError, mid);
}

function DelYGLWL(MX_ID, mid) {
    Ext.Msg.confirm("提示", "是否删除？", function (btn) {
        if (btn == "yes") {
            CS('CZCLZ.Model.DeleteYGLWLByCHB', function (retVal) {
                if (retVal) {
                    WLBind(mid);
                }
            }, CS.onError, MX_ID);
        }
    });
}

function XGYGLWL(MX_ID, mid)
{
    var win = new ShowYGLWL({ mid: mid, MX_ID: MX_ID });
    win.show(null, function () {
        CS("CZCLZ.Model.ShowYGLWLByCHB", function (retVal) {
            if (retVal)
            {
                Ext.getCmp("NameAdd").setValue(retVal);
            }
        }, CS.onError, MX_ID, mid)
    });
}

function DataBind(cp) {
    CS('CZCLZ.Model.GetModelListByCHB', function (retVal) {
        if (retVal) {
            MbStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('Company').getValue());
}

function Xg(mid) {
    CS('CZCLZ.Model.GetModelDetailByCHB', function (retVal) {
        if (retVal) {
            var win = new XGMb({ "MID": mid });
            win.show(null, function () {
                Ext.getCmp("NameAdd").setValue(retVal[0]["Name"]);
                Ext.getCmp("ShowNameAdd").setValue(retVal[0]["ShowName"]);
            });
        }
    }, CS.onError, mid);
}

function GL(mid, name) {
    var win = new AddModelDetail({ "MID": mid, "Company": name });
    win.show(null, function () {
        WLBind(mid);
        Ext.getCmp("GSName").setText(name);
    });
}

function HQ(mid) {
    var win = new URLWin();
    win.show(null, function () {
        Ext.getCmp("Url").setText("http://templet.chahuobao.net/MbView/CHBView.aspx?id=" + mid);
    });
}

function Del(id) {
    Ext.Msg.confirm("提示", "是否删除？", function (btn) {
        if (btn == "yes") {
            CS('CZCLZ.Model.DeleteModel', function (retVal) {
                if (retVal) {
                    Ext.Msg.alert("提示", "删除成功！", function () {
                        DataBind(1);
                    });
                }
            }, CS.onError, id);
        }
    });
}
