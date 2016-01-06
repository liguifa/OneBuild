$(document).ready(function ()
{
    $("#main-panel").panel(
    {
        fit: true,
        title: 'One build',
        border: true,
        minimizable: true,
        maximizable: true,
        closable: true,
        onBeforeClose: function ()
        {
            $.messager.confirm('警告', '您确认退出吗？', function (r)
            {
                if (r)
                {
                    window.external.Exit();
                }
            });
            return false;
        },
        onMaximize: function ()
        {
            window.external.Maximize();
        },
        onMinimize: function ()
        {
            window.external.Minimize();
        }
    });

    $('#layout').tabs(
	{
	    border: true,
	    fit: true
	});
    $('#layout').tabs('add',
	{
	    title: '欢迎',
	    content: 'Tab Body',
	    closable: false
	});
    $('#menu-file').menubutton(
	{
	    menu: '#menu-file-sub'
	});
    $('#menu-edit').menubutton(
	{
	    menu: '#menu-edit-sub'
	});
    $('#menu-setting').menubutton(
	{
	    menu: '#menu-setting-sub'
	});
    $('#menu-about').menubutton(
	{
	    menu: '#menu-about-sub'
	});
    $('.menu-sub').menu(
	{
	    onClick: function (item)
	    {
	        InvokeMenuMethod(item.id);
	    }
	});
});


function WindowMaximize()
{
    $("#main-panel").panel("maximize");
}

function InvokeMenuMethod(id)
{
    switch (id)
    {
        case "menu-about-author":
            {
                AboutAuthor();
                break;
            }
        case "menu-file-open":
            {
                OpenFile();
                break;
            }
        case "menu-file-new":
            {
                NewProject();
                break;
            }
    }
}

function AboutAuthor()
{
    $("#body").append("<div id='aboutAuthor'></div>");
    $('#aboutAuthor').window(
	{
	    width: 600,
	    height: 400,
	    modal: true,
	    title: "关于作者",
	    collapsible: false,
	    minimizable: false,
	    maximizable: false,
	    closable: true
	});
}

function OpenFile()
{

    $("body").append("<div id='fileWindow'></div>");
    $('#fileWindow').window(
	{
	    width: 600,
	    height: 400,
	    modal: true,
	    title: "打开文件",
	    collapsible: false,
	    minimizable: false,
	    maximizable: false,
	    closable: true,
	    content: "<ul id='files'></ul>"
	});
    $("#files").tree(
	{
	    data: eval("(" + window.external.GetFilesInfo() + ")"),
	    animate: true,
	    lines: true,
	    onBeforeExpand: function (node)
	    {
	        var dataNode = eval("(" + node.attributes + ")");
	        if (dataNode.isLoad == "False")
	        {
	            var nodeChildren = eval("(" + window.external.GetFilesInfo(dataNode.path) + ")");
	            $('#files').tree('append', {
	                parent: node.target,
	                data: nodeChildren
	            });
	            var newAttributes = "{'isLoad':'true','path':'" + dataNode.path + "'}";
	            $('#files').tree('update', {
	                target: node.target,
	                attributes: newAttributes
	            });
	        }
	    }
	});
}


//New Project
var step = ["views/project/new/projectSet.html", "views/project/new/solutionSet.html", "views/project/new/planSet.html", "views/project/new/backView.html"];
var currentStep = 0;
function NewProject()
{
    $("#layout").tabs("add",
    {
        title: '新建项目',
        closable: true,
        fit: true,
        content: window.external.Navigate("views/project/new/layout.html")
    });
    $("#newProjectLeft").panel(
    {
        fit: true
    })
    $("#newProjectRight").panel(
    {
        title: "项目设置",
        content: window.external.Navigate("views/project/new/projectSet.html")
    });
    $("#stepNext").click(function ()
    {
        currentStep++;
        $("#newProjectRight").panel(
        {
            title: "项目设置",
            content: window.external.Navigate(step[currentStep])
        });
    });

}