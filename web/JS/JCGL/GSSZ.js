var pageSize = 15;

var UserStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'UserName', 'Company'
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
                        store: UserStore,
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserName',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '手机'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Company',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '公司名称'
                            },
                            {
                                xtype: 'gridcolumn',
                                width: 180,
                                sortable: false,
                                menuDisabled: true,
                                text: '操作',
                                renderer: function (value,cellmeta,record,rowIndex,columnIndex,store) {
                                    return "<a href='javascript:void(0);' onClick='Del(\"" + record.data.UserName + "\");'>删除</a>";
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
                                        width: 180,
                                        labelWidth: 50,
                                        id: 'UserName',
                                        fieldLabel: '手机号'
                                    },
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
                                            var win = new AddUser();
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
                                store: UserStore,
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

Ext.define('AddUser', {
    extend: 'Ext.window.Window',

    height: 400,
    width: 350,
    layout: {
        type: 'fit'
    },
    title: '新增关联',

    modal: true,
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
                            fieldLabel: '手机号',
                            labelWidth: 70,
                            allowDecimals: false,
                            columnWidth: 1,
                            id: 'UserNameAdd',
                            padding: '50 10 50 10'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: '公司名称',
                            labelWidth: 70,
                            columnWidth: 1,
                            id: 'CompanyAdd',
                            padding: '0 10 50 10'
                        }
                    ],
                    buttonAlign: 'center',
                    buttons: [
                        {
                            text: '确认关联',
                            iconCls: 'enable',
                            handler: function () {
                                if (Ext.getCmp("UserNameAdd").getValue() == "" || Ext.getCmp("UserNameAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "手机号不能为空！");
                                    return false;
                                }
                                if (Ext.getCmp("CompanyAdd").getValue() == "" || Ext.getCmp("CompanyAdd").getValue() == null) {
                                    Ext.Msg.alert("提示", "公司名称不能为空！");
                                    return false;
                                }
                                CS('CZCLZ.Model.AddUserAndCompany', function (retVal) {
                                    if (retVal) {
                                        Ext.Msg.alert("提示", "保存成功！", function () {
                                            DataBind(1);
                                            me.close();
                                        });
                                    } else {
                                        Ext.Msg.alert("提示", "已存在相同手机号或公司名称，不可添加！");
                                        return false;
                                    }
                                }, CS.onError, Ext.getCmp("UserNameAdd").getValue(), Ext.getCmp("CompanyAdd").getValue());
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

function DataBind(cp) {
    CS('CZCLZ.Model.GetUserAndCompanyList', function (retVal) {
        if (retVal) {
            UserStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('UserName').getValue(), Ext.getCmp('Company').getValue());
}

function Del(UserName)
{
    Ext.Msg.confirm("提示", "是否删除？", function (btn) {
        if (btn == "yes") {
            CS('CZCLZ.Model.DeleteUserAndCompany', function (retVal) {
                if (retVal) {
                    Ext.Msg.alert("提示", "删除成功！", function () {
                        DataBind(1);
                    });
                }
            }, CS.onError, UserName)
        }
    });
}
