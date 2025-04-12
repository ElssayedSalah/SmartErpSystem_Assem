

//var currentLanguage = getLanguageFromCookie();
function intializeKendoGrid(url) {
    var grid = new kendo.data.DataSource({
       
        type: "ajax",
        transport: {
            read: {
                url: url ,
                type: 'POST',
                dataType: 'json',

            }
        },
        schema: {
            data: 'Data',
            model: {},
            total: 'Total',
            errors: 'Errors'
        },
        error: function (e) {
            display_kendoui_grid_error(e);
            this.cancelChanges();
        },
        pageSize: 10,
    });
    return grid;
}

//function ConfirmationDailog(Id, controler, deleteSource)
//{    
//    var confirmMsg ='هل تريد حذف العنصر الحالي ؟'
//    var deleteErorrMsg ='حدث خطأ أثناء الحذف'
//    var titleMsg ='تأكيد الحذف'
//    var deleteSuccessMsg ='تم الحذف بنجاح'
//    var okButtonText ='موافق'
//    var cancelButtonText ='غير موافق'

//    //var currentLanguage = getLanguageFromCookie();
//    if (currentLanguage != 'ar-EG')
//    {
//        confirmMsg = 'Do You Want To Delete This Item ?'
//        deleteErorrMsg = 'An Erorr Happend'
//        titleMsg = 'Confirm Delation'
//        deleteSuccessMsg = 'Item Deleted Successflly'
//        okButtonText = 'Ok'
//        cancelButtonText = 'Cancel'
//    }

//    Swal.fire({
//        title: titleMsg,
//        text: confirmMsg,
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        cancelButtonText: cancelButtonText,
//        confirmButtonText: okButtonText
//    }).then((result) => {
//        if (result.isConfirmed) { 
//            debugger         
//            $.ajax({
//                url: '/' + controler + '/Delete/',
//                data: { Id: Id },
//                success: function (data) {
//                    debugger
//                    if (data.type == "success") {
//                        //refresh grid if delete action came from grid else redirect to list if delete action came from edit form
//                        if (deleteSource == 'grid') {

//                            Swal.fire(deleteSuccessMsg, '', 'success');

//                            var grid = $("#datatable").data("kendoGrid");
//                            var dataSource = grid.dataSource;
//                            dataSource.read();

//                        } else {

//                            window.location.href = '/' + controler + '/Index';
//                        }

//                    }
//                    else if (data.type == "faild")
//                    {
//                        Swal.fire(data.msg, '','error');
//                    }
//                    else
//                    {
//                        Swal.fire(deleteErorrMsg, '','error');
//                    }
//                }
//            });
        
//        }
//    });


//    //kendo.confirm(confirmMsg).then(function () {
//    //    $.ajax({
//    //        url: '/' + controler +'/Delete/',
//    //        data: { Id: Id},
//    //        success: function (data) {
//    //            debugger
//    //            if (data.type == "success") {
//    //                //refresh grid if delete action came from grid else redirect to list if delete action came from edit form
//    //                if (deleteSource == 'grid') {
//    //                    kendo.alert(data.msg);
//    //                    var grid = $("#datatable").data("kendoGrid");
//    //                    var dataSource = grid.dataSource;
//    //                    dataSource.read();

//    //                } else {
                       
//    //                    window.location.href = '/' + controler + '/Index';
//    //                    /*kendo.alert(data.msg);*/
//    //                }
     
//    //            } else if (data.type == "faild")
//    //            {
//    //                kendo.alert(data.msg);
//    //            }
//    //            else {                   
//    //                kendo.alert(deleteErorrMsg);
//    //            }             
//    //        }
//    //    });      
//    //}, function () {
      
//    //});


//}

function GridOperationButtons(ControlerName)
{    
    var columns = [
        {         
            template: '<a href="/' + ControlerName+'/Details/#=Id#" class="btn btn-info"><span class="k-icon k-i-eye k-button-icon"></span><span class="k-button-text"></span></a>',
            width: 50,           
        },        
        {
            width: 50,
            template: '<a href="/' + ControlerName +'/Edit/#=Id#" class="btn btn-primary"><span class="k-icon k-i-edit k-button-icon"></span><span class=""></span></a>',

        },
        {
            width: 50,
            template: '<button type="button" onclick="DeleteWithConfirmationDailog(\'#=Id#\',\'' + ControlerName + '\',\'grid\')" class="btn btn-danger" title="{DeleteTitle}"><span class="k-icon k-i-close k-button-icon"></span><span class=""></span> </button>',
        }
    ];
    return columns;
}

//function getLanguageFromCookie() {
//    var cookies = document.cookie.split(';');

//    for (var i = 0; i < cookies.length; i++) {
//        var cookie = cookies[i].trim();

//        if (cookie.startsWith('.AspNetCore.Culture=')) {
//            var encodedLanguage = cookie.substring('AspNetCore.Culture='.length);
//            var decodedLanguage = decodeURIComponent(encodedLanguage);
//            var language = decodedLanguage.split('|')[1].split('=')[1];
//            if (language == 'ar') {
//                language = 'ar-EG'
//            }
//            else {
//                language = 'en-US'
//            }
//            return language;
//        }
//    }
//    return null;
//}





