$(document).ready(function () {
    $('#header-menu-btn').addClass('active');

    //螢幕大小改變
    $(window).resize(function () {
        resize_adjust();
    });

    /* 開啟/關閉menu */
    $('.menu-btn').click(function () {
        $('#menu').toggleClass('active');
        $('#overlay').toggleClass('active');
        $('.menu-btn').toggleClass('active');
        resize_adjust();
    })
});

function resize_adjust() {
    if (window.innerWidth <= 768) {
        $('#overlay').removeClass("d-none");
        $('#overlay').width($('body').width());
        $('#overlay').height(($('body').height() - 35) + "px");
        $('#overlay').css("top", "50px");
        $("#content-area").removeClass("ps-xl-2");
        $("#content-area").css("maxWidth", "100%");
    }
    else if (window.innerWidth <= 1200) {
        $('#overlay').removeClass("d-none");
        $('#overlay').width($('body').width());
        $('#overlay').height(($('body').height() - 35) + "px");
        $('#overlay').css("top", "50px");
        $("#content-area").removeClass("ps-xl-2");
        $("#content-area").css("maxWidth", "100%");
    } else {
        $('#overlay').addClass("d-none");
        $('#overlay').width(0);
        $('#overlay').height(0);
        $("#content-area").addClass("ps-xl-2");
        setTimeout(function () {
            if ($('#menu').hasClass('active')) {
                $("#content-area").css("maxWidth", (window.innerWidth - 30) + "px");
            } else {
                $("#content-area").css("maxWidth", (window.innerWidth - 283) + "px");
            }
            //$.fn.dataTable
            //    .tables({ visible: true, api: true })
            //    .columns.adjust();
        }, 3);
    }
}

//--------------------初始化--------------------
//DataTables
var InitDataTables = function (id) {
    const table = $("#" + id);
    var height = table.data("height") ?? "280px";
    //var loadingoff = table.data("loadingoff") ?? false;

    var options = {
        dom: 'Bfrtip',
        "scrollX": true,
        "scrollY": height,
        "scrollCollapse": true,
        "searching": true,
        "info": false,
        "lengthChange": false,
        //"data": dataList ?? [],
        "scroller": true,
        "deferRender": true,
        "destroy": true,
        language: {
            "search": "搜尋",
            "lengthMenu": "顯示 _MENU_ 項結果",
            "zeroRecords": "此搜尋沒有可顯示的資料",
            "emptyTable": "無資料",
            "infoEmpty": "",
            "info": "共 _PAGES_ 頁，目前在第 _PAGE_ 頁",
            "infoFiltered": "(從 _MAX_ 項結果中搜尋)",
            "paginate": {
                "first": '<i class="fas fa-angle-double-left"></i>',
                "last": '<i class="fas fa-angle-double-right"></i>',
                "next": '<i class="fas fa-chevron-right"></i>',
                "previous": '<i class="fas fa-chevron-left"></i>'
            }
        },
        "order": [],
        "columnDefs": [
            {
                "orderable": false,
                "targets": "no-sort"
            }
        ]
    };

    if ($.fn.DataTable.isDataTable(id)) {
        table.addClass("d-none");
        table.DataTable().destroy();
    }

    table.removeClass("d-none");
    table.DataTable(options).draw();
    resize_adjust();

    return;
};
//--------------------END--------------------

//---------------sweetalert共用---------------
//多載:
//錯誤提示
//呼叫方法1: 只傳msg
//呼叫方法2: 傳入title, msg  
var ErrorAlert = function (...args) {
    let title = "錯誤";
    let msg = "";
    if (args.length == 1)
        msg = args[0];
    else {
        title = args[0];
        msg = args[1];
    }

    //這樣外面才能用then接
    let s = swal.fire({
        icon: 'error',
        title: title,
        html: msg,
        allowOutsideClick: false,
        customClass: {
            confirmButton: 'btn btn-secondary'
        }
    });
    return s;
};
//資訊提示
//呼叫方法1: 只傳msg
//呼叫方法2: 傳入title, msg  
var InfoAlert = function (...args) {
    let title = "資訊";
    let msg = "";
    if (args.length == 1)
        msg = args[0];
    else {
        title = args[0];
        msg = args[1];
    }

    let s = swal.fire({
        icon: 'info',
        title: title,
        html: msg,
        allowOutsideClick: false,
        customClass: {
            confirmButton: 'btn btn-secondary'
        }
    });
    return s;
};
//成功提示
function SuccessAlert(title, msg) {
    let s = swal.fire({
        icon: 'success',
        title: title,
        html: msg,
        allowOutsideClick: false,
        customClass: {
            confirmButton: 'btn btn-secondary'
        }
    });
    return s;
}
//錯誤視窗顯示
function ErrorRespAlert(resp) {
    var msg = null;
    if (resp == null)
        return;
    if (resp.data == null)
        return;

    if (!IsEmpty(resp.data.msg)) {
        msg = resp.data.msg.split("\r\n");
        if (msg.length == 2)
            swal.fire({
                icon: 'error',
                title: msg[0],
                html: msg[1],
                allowOutsideClick: false,
                customClass: {
                    confirmButton: 'btn btn-secondary'
                }
            }).then(function () {
                DoErrorResp(resp.status);
            });
        else
            swal.fire({
                icon: 'error',
                title: String(resp.data.msg),
                allowOutsideClick: false,
                customClass: {
                    confirmButton: 'btn btn-secondary'
                }
            }).then(function () {
                DoErrorResp(resp.status);
            });
    } else {
        swal.fire({
            icon: 'error',
            title: String(resp.status),
            html: String(resp.statusText),
            allowOutsideClick: false,
            customClass: {
                confirmButton: 'btn btn-secondary'
            }
        });
    }
}
//--------------------END--------------------

//普遍使用
function AuthPost(url, data) {
    return new Promise((resolve, reject) => {
        axios.post(url, data)
            .then(response => {
                //成功
                //Console(response);
                if (response.status == 200) {
                    resolve(response);
                }
            })
            .catch(error => {
                //失敗
                var resp = error.response;
                Console(resp);
                reject(resp);
            });
    });
}
//視窗提醒
var AuthPostAlert = function (url, data, title) {
    return new Promise((resolve, reject) => {
        swal.fire({
            icon: 'warning',
            title: title ?? '確定?',
            confirmButtonText: '確定',
            cancelButtonText: '取消',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            allowOutsideClick: false,
            preConfirm: function () {
                return AuthPost(url, data).catch((error) => { return error; });
            },
            customClass: {
                confirmButton: 'sweet-custom-confirm',
                cancelButton: 'btn btn-danger'
            }
        }).then((confirmResult) => {
            if (confirmResult.isConfirmed == true) {
                let resp = confirmResult.value;
                if (resp.status == 200) {
                    resolve(resp);
                } else {
                    ErrorRespAlert(resp, true);
                    reject(resp);
                }
            }
        }).catch(() => { });
    });
};