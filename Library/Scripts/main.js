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
    if (window.innerWidth <= 1200) {
        $('#overlay').removeClass("d-none");
        $('#overlay').width($('body').width() + 30);
        $('#overlay').height($('body').height() + 30);
        //$('#overlay').css("top", "25px");
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
                $("#content-area").css("maxWidth", (window.innerWidth - 280) + "px");
            }
            //$.fn.dataTable
            //    .tables({ visible: true, api: true })
            //    .columns.adjust();
        }, 3);
    }
}

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