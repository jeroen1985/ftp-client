var app = {};

app.tree = {};

app.tree.local = function () {
    var path;
    var refresh;

    var genLocalTree = function () {
        $.ajax({
            method: 'GET',
            url: '/Tree/Local?path=' + path,
            data: {
                format: 'json'
            },
            error: function (xhr, statusText) {
                alert("Error: " + xhr.statusText);
            },
            success: function (data) {
                createLocalTree(data);
            }
        });
    }

    var createLocalTree = function (data) {
        if (refresh == true) {
            $('#jstree1').jstree(true).settings.core.data = data;
            $('#jstree1').jstree(true).refresh(false, true);
            return;
        }

        $('#jstree1').jstree({
            'core': {
                'data': data,
            }
        });
    };

    return {
        generateLocalTree: function (p,r) {
            path = p;
            refresh = r;
            genLocalTree();
        },
        localTreeOnChange: function (data) {          
            var regex = /node_(\d+)_(\w+)_(\d+)_(.*)/gi;
            var match = regex.exec(data.selected);
            var type = match[2];
            if (type == 'm') {
                path = match[4];
                refresh = true;
                genLocalTree()
            }
        }
    }
};

app.tree.remote = function () {
    var path;
    var refresh;

    var connect = function () {
        $.ajax({
            method: 'POST',
            url: '/SFTP/Connect',
            data: $('#connectionString').serialize(),
            error: function (xhr, statusText) {
                alert("Error: " + xhr.statusText);
            },
            success: function (data) {
                generateRemoteBuildTree();
            }
        });
    }
    
    var generateRemoteBuildTree = function () {
        $.ajax({
            method: 'GET',
            url: '/Sftp/GetRemoteTree?path=' + path,
            data: {
                format: 'json'
            },
            error: function (xhr, statusText) {
                alert("Error: " + xhr.statusText);
            },
            success: function (data) {
                createRemoteTree(data);
            }
        });
    }

    var createRemoteTree = function (data) {
        if (refresh == true) {
            $('#jstree2').jstree(true).settings.core.data = data;
            $('#jstree2').jstree(true).refresh(false, true);
            return;
        }
        $('#jstree2').jstree({
            'core': {
                'data': data,
            }
        });

    };

    return {
        connect: function (p, r) {
            path = p;
            refresh = r;
            connect();
        },
        remoteTreeOnChange: function (data) {
            var regex = /node_(\d+)_(\w+)_(\d+)_(.*)/gi;
            var match = regex.exec(data.selected);
            var type = match[2];
            if (type == 'm') {
                path = match[4];
                refresh = true;
                generateRemoteBuildTree();
            }
        },
        generateRemoteTree: function (p, r) {
            path = p;
            refresh = r;
            generateRemoteBuildTree();
        }
    }

}

app.tree.transferFilesLocal = function () {
    var localToRemote = {
        SourcePath: "",
        DestinationPath: ""
    }
    var source = $('#jstree1').jstree('get_selected');
    var destination = $('#jstree2').jstree('get_selected');

    var regexs = /node_(\d+)_(\w+)_(\d+)_(.*)/gi;
    var matchs = regexs.exec(source);
    var type = matchs[2];
    var regexd = /node_(\d+)_(\w+)_(\d+)_(.*)/gi;
    var matchd = regexd.exec(destination);
    
    var moveLocalToRemote = function () {
        if (type == 'f') {
            localToRemote.SourcePath = matchs[4];
            localToRemote.DestinationPath = matchd[4];
            $.ajax({
                method: 'POST',
                url: '/SFTP/CopyLocalToRemote',
                data: localToRemote,
                error: function (xhr, statusText) {
                    alert("Error: " + xhr.statusText);
                },
                success: function (data) {                    
                }
            });
        }

    }

    return {
        getDestinationPath: function () {
            return localToRemote.DestinationPath;
        },
        transferLocalToRemote: function () {
            moveLocalToRemote();
        }
    }
}

app.tree.transferFilesRemote = function () {
    var remoteToLocal = {
        DestinationPath: "",
        SourcePath: ""
    }
    
    var source = $('#jstree2').jstree('get_selected');
    var destination = $('#jstree1').jstree('get_selected');

    var regexs = /node_(\d+)_(\w+)_(\d+)_(.*)/gi;
    var matchs = regexs.exec(source);
    var type = matchs[2];
    var regexd = /node_(\d+)_(\w+)_(\d+)_(.*)/gi;
    var matchd = regexd.exec(destination);

    var moveRemoteToLocal = function (){
        if (type == 'f') {
            remoteToLocal.SourcePath = matchs[4];
            remoteToLocal.DestinationPath = matchd[4];

            $.ajax({
                method: 'POST',
                url: '/SFTP/CopyRemoteToLocal',
                data: remoteToLocal,
                error: function (xhr, statusText) {
                    alert("Error: " + xhr.statusText);
                },
                success: function (data) {
                }
            });
        }
    }

    return {
        getDestinationPath: function () {
            return remoteToLocal.DestinationPath;
        },
        transferRemoteToLocal: function () {
            moveRemoteToLocal();
        }
    }

}


app.tree.eventsLogs = function () {
    var execEventLog = function () {
        $.ajax({
            method: 'GET',
            url: '/EventLog',
            data: {
                format: 'json'
            },
            error: function (xhr, statusText) {
                alert("Error: " + xhr.statusText);
            },
            success: function (data) {
                updateEventLog(data);
                $('#EventLog').animate({ scrollTop: $('#EventLog').prop("scrollHeight") }, 500);
            }
        });
    }

    var updateEventLog = function (data) {
        if (data.EventsLog.length > 0) {
            var newline = String.fromCharCode(13, 10);
            for (var i = 0; i < data.EventsLog.length; i++) {
                switch (data.EventsLog[i]) {
                    case 0:
                        $('#EventLog').val($('#EventLog').val() + "Connecting to " + data.UserName + "\u0040" + data.Host + ":" + data.Port + "" + newline);
                        break;
                    case 1:
                        $('#EventLog').val($('#EventLog').val() + "Connected to " + data.UserName + "\u0040" + data.Host + ":" + data.Port + "" + newline);
                        break;
                    case 2:
                        $('#EventLog').val($('#EventLog').val() + "Retrieving directory list" + newline);
                        break;
                    case 3:
                        $('#EventLog').val($('#EventLog').val() + "Starting file transfer" + newline);
                        break;
                    case 4:
                        $('#EventLog').val($('#EventLog').val() + "File in transfer" + newline);
                        break;
                    case 5:
                        $('#EventLog').val($('#EventLog').val() + "Completed file transfer" + newline);
                        break;

                }
            }
        }
    }

    return {
        execEventLog: function () {
            execEventLog();
        }
    }
}

$(function () {
    var localTree = app.tree.local();
    localTree.generateLocalTree("C:\\", false);
    var remoteTree = app.tree.remote();
        
    $('#jstree1').on("changed.jstree", function (e, data) {
        localTree.localTreeOnChange(data);
    });

    $("#connectionString").submit(function (event) {
        event.preventDefault();
        remoteTree.connect("/", false);
    });
   
    $('#jstree2').on("changed.jstree", function (e, data) {
        remoteTree.remoteTreeOnChange(data);     
    });

    if ($('#jstree2').html().length == 0) {
        $("#transferLocalToRemote").on('click', function () {
            var transferLocalFiles = app.tree.transferFilesLocal();
            transferLocalFiles.transferLocalToRemote();
            var destination = transferLocalFiles.getDestinationPath();
            remoteTree.generateRemoteTree(destination, true)
        });
        $("#transferRemoteToLocal").on('click', function () {
            var transferRemoteFiles = app.tree.transferFilesRemote();
            transferRemoteFiles.transferRemoteToLocal();
            var destination = transferRemoteFiles.getDestinationPath();
            localTree.generateLocalTree(destination, true);
        });
    }

    var eventsLog = app.tree.eventsLogs();
    setInterval(eventsLog.execEventLog, 3000);
});