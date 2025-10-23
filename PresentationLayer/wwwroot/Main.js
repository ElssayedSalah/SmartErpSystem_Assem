//-------------------------------------------------------- Forms Functions -----------------------------
//var currentLanguage = getLanguageFromCookie();

function RemoveArrayDuplicates(arr) {
    var uniqueArr = [];
    $.each(arr, function (i, el) {
        if ($.inArray(el, uniqueArr) === -1) uniqueArr.push(el);
    });
    return uniqueArr;
}

function ReOrderModelListItems(selector, rowSelector, PropName) {
    PropName = PropName ? PropName : "";
    $(selector).find(rowSelector).each(function (i, q) {
        $(q).find("input ,textarea,select").each(function (j, c) {
            var id = $(c).attr("id");
            var name = $(c).attr("name");
            if (name) {
                //name = name.replace(new RegExp(PropName | /\[([0-9]+)\]/g), i);

                var Matches = name.match(new RegExp(/\[([0-9]+)\]/g), i);
                $(Matches).each(function (index, match) {
                    name = name.replace(PropName + match, PropName + "[" + i + "]");
                });
                $(c).attr("name", name);
            }
            if (id) {
                var Matches = id.match(new RegExp(/_([0-9]+)_/g), i);
                $(Matches).each(function (index, match) {
                    id = id.replace(PropName + match, PropName + "_" + i + "_");
                });
                //id = id.replace(new RegExp(PropName | /_([0-9]+)_/g), i);
                $(c).attr("id", id);
            }
        });
    });
}

function ReOrderModelListItems2(selector, rowSelector, PropName) {
    PropName = PropName ? PropName : "";
    selector.find(rowSelector).each(function (i, q) {
        $(q).find("input ,textarea,select").each(function (j, c) {
            var id = $(c).attr("id");
            var name = $(c).attr("name");
            if (name) {
                //name = name.replace(new RegExp(PropName | /\[([0-9]+)\]/g), i);

                var Matches = name.match(new RegExp(/\[([0-9]+)\]/g), i);
                $(Matches).each(function (index, match) {
                    name = name.replace(PropName + match, PropName + "[" + i + "]");
                });
                $(c).attr("name", name);
            }
            if (id) {
                var Matches = id.match(new RegExp(/_([0-9]+)_/g), i);
                $(Matches).each(function (index, match) {
                    id = id.replace(PropName + match, PropName + "_" + i + "_");
                });
                //id = id.replace(new RegExp(PropName | /_([0-9]+)_/g), i);
                $(c).attr("id", id);
            }
        });
    });
}


function removeFromArray(arr, value) {
    var index = arr.indexOf(value);
    if (index > -1) {
        arr.splice(index, 1);
    }
    return arr;
}
function objectifyForm(frmId) {//serialize data function
    formArray = document.getElementById(frmId);
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}
function convertFormToObject(formArray) {//serialize data function
    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        
        if (formArray[i]['name'] && formArray[i]['name'].trim() != "")
            returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    
    return returnArray;
}
function DeleteWithConfirm(id, WithRedirect, DeleteAction, Query, SuccessCallBack) {
    if (!DeleteAction) {
        DeleteAction = Actions.Delete;
    }
    if (Query == undefined)
        Query = "";
    ShowErrorAlertWithCallBack(Locales.DeleteConfirm, "", function () {
        $.ajax({
            url: DeleteAction + "?id=" + id + Query,
            type: "get",
            datatype: "json",
            data: {},
            success: function (data) {
                if (data.Error) {
                    ShowSideError("", data.Message)
                }
                else {
                    ShowSideDone("", data.Message)
                    if (WithRedirect) {
                        window.location.href = Actions.List;
                    }
                    else {
                        try {
                            $("[id*='-grid']").data('kendoGrid').dataSource.read();
                            $("[id*='-grid']").data('kendoGrid').refresh();
                        } catch (e) {

                        }

                        if (SuccessCallBack) {
                            SuccessCallBack();
                        }
                    }
                }

            }, error: function () {
                ShowErrorAlert(Locales.DeleteFailed);
            }
        });
    }, undefined, Locales.Yes, Locales.No)

}
function BindForm(data) {
    Object.keys(data).forEach(function (key, index) {
        var value = Object.values(data)[index];
        $("#" + key).val(value);
        try {
            $("#" + key).data("kendoNumericTextBox").value(value != "" && value != null ? value : 0);
        } catch (e) {

        }

    });
}
function setDropDownOptions(selector, data, removeOldOptions, SelectLastOption) {
    var LastId = "";
    if (removeOldOptions) { $(selector + " option").slice(1).remove(); }
    $(data).each(function (i, c) {
        $(selector).append("<option value='" + c.Id + "'>" + c.Name + "</option>");
        if (SelectLastOption) { $(selector).val(c.Id); }
        LastId = c.Id;
    });
    InitKendoDropDown(selector);
    return LastId;
}

function CloseSideBar() {
    if (!$("body").hasClass("page-sidebar-closed")) { $(".menu-toggler.sidebar-toggler").click(); }
}
//------------------------------------------------------------------------------------------------------


//----------------------------------------------------------- Controls ---------------------------------
function InitLocalesEditor(selector) {
    try {
        $(selector).kendoTabStrip(
            {
                animation: {
                    open: {
                        effects: "fadeIn"
                    }
                }
            });
    } catch (e) {

    }
    //Copy from first tab to standard tab
    $(".FirstLocaleTab input ,.FirstLocaleTab textarea").on("input", function (e) {
        try {
            //Get Name Of control
            var controlName = $(e.target).attr("name").split('[0].')[1];
            //Find Control With Same Name In Standard Tab
            $(e.target).closest(".localizer").find(".StandardTab [name*='" + controlName + "']").val($(e.target).val());

        } catch (ex) {

        }

    });
}
function InitKendoDropDown(selector) {
    InitKendoDropDownWithElement($(selector));
}
function InitKendoDropDownWithElement(element) {
    element.not(".multiple").not(".nosearch").addClass('edited').kendoDropDownList({
        filter: "contains",
    });
}
function InitMultiSelect(selector, placeHolder) {
    $(selector).kendoMultiSelect({
        select: function (e) {
            var current = this.value();
            if (this.dataSource.view()[e.item.index()].value === "0") {
                this.value("");
            }
            else if (current.indexOf("0") !== -1) {
                current = $.grep(current, function (value) {
                    return value !== "0";
                });

                this.value(current);
            }
        },
        change: function (e) {
            if (this.value().length === 0)
                this.value(["0"]);
        }, filter: "contains"
        , placeholder: placeHolder,
        autoClose: false
    }).data("kendoMultiSelect")
}
function CreateNumericUpDown(element, decimals, change, readonly) {
    
    var format = "#,#.";
    if (decimals == 0)
        format = "#";
    if (decimals == undefined)
        decimals = 2;
    for (var i = 0; i < decimals; i++) {
        format += "0"
    }

    element.kendoNumericTextBox({
        change: change,
        /* value: element.val(),*/
        format: format,
        min: "0",
        decimals: decimals, //always display 2 digits

    });   
    
   
    if (element.length > 0 && readonly == true)
        element.data('kendoNumericTextBox').readonly();
    //element.bind("focus", function (e) {
    //    if (!$(this).attr("readonly"))
    //        $(this).data("kendoNumericTextBox").open();
    //});
    try {
        element.rules("remove", "number");
    } catch (e) {

    }

}

function CreateNumericUpDownNigativeValue(element, decimals, change, readonly) {
    var format = "#,#.";
    if (decimals == 0)
        format = "#";
    if (decimals == undefined)
        decimals = 2;
    for (var i = 0; i < decimals; i++) {
        format += "0"
    }

    element.kendoNumericTextBox({
        change: change,       
        format: format,       
        decimals: decimals, 

    });


    if (element.length > 0 && readonly == true)
        element.data('kendoNumericTextBox').readonly();    
    try {
        element.rules("remove", "number");
    } catch (e) {

    }

}


function DestroyNumericUpDown(element) {
    element.data("kendoNumericTextBox").destroy();
    element.val("0");
    element.removeAttr("role").removeAttr("data-val-required").removeAttr("data-val-number").removeAttr("data-role").removeAttr("aria-valuenow").removeAttr("aria-disabled").removeAttr("style");
    element.removeClass("k-input");
    var elementHtml = element.clone();
    var helpblock = element.closest(".form-group").find(".help-block");
    element.closest(".k-widget").remove();
    elementHtml.insertBefore(helpblock);
}
function CreateDatePicker(selector, disable, start, end) {
    CreateDatePickerWithElment($(selector), disable, start, end)
}
function FillDropDown(select, data, SelectLocale, val) {
    var AllowSearch = false;
    try {
        select.data("kendoDropDownList").destroy();
        AllowSearch = true;
    } catch (e) {

    }
    select.removeClass("nosearch");
    select.html('<option value="">' + SelectLocale + '</option>');
    $(data).each(function (i, c) {
        select.append("<option value='" + c.Id + "'>" + c.Name + "</option>");
    });
    if (val)
        select.val(val);
    if (AllowSearch) {
        InitKendoDropDownWithElement(select);
    }
}
//------------------------------------------------------------------------------------------------------


//------------------------------------------------------- Custom Functions -----------------------------

function ShowTimePicker(selector) {
    $(selector).parent().addClass('DateTimePicker')
    $(selector).siblings(".k-widget.k-timepicker").show();

}
function HideTimePicker(selector) {
    $(selector).parent().removeClass('DateTimePicker')
    $(selector).siblings(".k-widget.k-timepicker").hide();
}
function CreateMonthDatePickerWithElment(element, disable, start, end, format) {
    format = format ?? "MM/yyyy"; // set the format to show only month and year
    element.kendoDatePicker({
       /* culture: CultureName,*/
        format: format,
        start: "year", // set the start view to show years only
        depth: "year", // restrict the selection to the year level
        min: start ? new Date(start) : undefined,
        max: end ? new Date(end) : undefined,
    });
    if (disable == 'True')
        element.data('kendoDatePicker').enable(false);
    element.bind("focus", function (e) {
        if (!$(this).attr("readonly"))
            $(this).data("kendoDatePicker").open();
    });
}
function CreateDatePickerWithElment(element, disable, start, end, format, defaultDateTime) {
    format = format ?? "yyyy-MM-dd";
    element.kendoDatePicker({
        culture: currentLanguage,
        format: format,
        min: start ? new Date(start) : undefined,
        max: end ? new Date(end) : undefined,
        value: defaultDateTime ? new Date(defaultDateTime) : new Date()

    });
    if (disable == 'True')
        element.data('kendoDatePicker').enable(false);
    element.bind("focus", function (e) {
        if (!$(this).attr("readonly"))
            $(this).data("kendoDatePicker").open();
    });
}

function CreatekendoDateTimePickert(element, disable, start, end, format, defaultDateTime) {
    
    format = format ?? "yyyy-MM-dd hh:mm tt";
    element.kendoDateTimePicker({
        culture: currentLanguage,
        format: format,
        min: start ? new Date(start) : undefined,
        max: end ? new Date(end) : undefined,
        value: defaultDateTime ? new Date(defaultDateTime) : new Date()
    });
    if (element.length > 0 && disable === 'True')
        element.data('kendoDateTimePicker').enable(false);
    element.bind("focus", function (e) {
        if (!$(this).attr("readonly"))
            $(this).data("kendoDateTimePicker").open();
    });
}

function CheckValidationDisplay() {
    var HasError = false;
    $(".validationError").each(function (i, c) {
        if ($(c).text().trim() != "") {
            HasError = true;
        }
    });
    if (HasError)
        $(".validationError").addClass('validationErrorVisible');
}
function CreateTimePicker(selector, disable) {
    $(selector).kendoTimePicker({
       /* culture: CultureName,*/
    });
    if (element.length > 0 && disable == 'True')
        $(selector).data('kendoTimePicker').enable(false);
}
function createGuid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + s4() + s4();
}

function GetPopCreate(e, CreateUrl, ModalSize, PopUpDropDownId, HidePopUpSaveButton) {
    $(e).attr("disabled", "disabled");
    var icon = $(e).find("i").attr('class');
    $(e).find('i').removeAttr('class');
    $(e).find('i').addClass("fa fa-spin fa-spinner")
    $.ajax({
        url: CreateUrl,
        type: "Get",
        datatype: "html",
        data: { IsPopUp: true, ModalSize: ModalSize, PopUpDropDownId: PopUpDropDownId, HidePopUpSaveButton: HidePopUpSaveButton },
        success: function (result) {
            $("#PopUpHolder").html(result);
            $(e).find('i').removeAttr('class');
            $(e).find('i').addClass(icon);
            $("#PopUpModal").modal('show');
            $(e).removeAttr("disabled");
        },
        error: function () {
            ShowSideError("", Locales.PopUpGetError);
            $(e).removeAttr("disabled");
            $(e).find('i').removeAttr('class');
            $(e).find('i').addClass(icon);
        }
    });
}
function HideAllNotifications() {
    PNotify.removeAll();
}
function Round(num) {
    return Math.round(num * 100) / 100;
}
function GetDateDiff(From, To) {
    if (From != "" & To != "") {
        var ApproxDaysPerMonth = 30.4375;
        var ApproxDaysPerYear = 365.25;
        From = moment(From, "DD/MM/YYYY");
        To = moment(To, "DD/MM/YYYY");
        var iDays = From.diff(To, 'days');
        var iYear = parseInt(iDays / ApproxDaysPerYear);
        iDays -= parseInt(iYear * ApproxDaysPerYear);
        var iMonths = parseInt(iDays / ApproxDaysPerMonth);
        iDays -= parseInt(iMonths * ApproxDaysPerMonth);

        return { Days: iDays, Months: iMonths, Years: iYear, TotalDays: From.diff(To, 'days') };
    }
    return { Days: "", Months: "", Years: "", TotalDays };
}
function SavePopUp(e) {
    var frm = $(e).closest(".modal-dialog").find("form")[0];
    var SaveUrl = $(frm).attr("action");
    try {
        BeforeSavePopUp();
    } catch (e) { }
    if ($(frm).valid()) {
        $.ajax({
            url: SaveUrl,
            type: "Post",
            datatype: "json",
            data: convertFormToObject(frm),
            success: function (data) {
                if (data.IsSaved) {
                    var DropDownName = $("#PopUpDropDownId").val();
                    if (DropDownName) {
                        try {

                            $("#" + DropDownName).append("<option selected='selected' value='" + data.Value + "'>" + data.Text + "</option>");
                            if ($("#" + DropDownName).data('kendoMultiSelect')) {
                                $("#" + DropDownName).data('kendoMultiSelect').destroy();
                                $("#" + DropDownName).closest('.k-widget').html(document.getElementById(DropDownName));
                                $("#" + DropDownName).closest('.k-widget').attr('class', '');
                                InitMultiSelect("#" + DropDownName);
                            } else {
                                $("#" + DropDownName).data("kendoDropDownList").destroy();
                                InitKendoDropDown("#" + DropDownName);
                            }
                        } catch (e) {

                        }
                    }
                    $("#PopUpModal").modal("hide");
                    ShowSideDone("", data.Message);
                    try {
                        if (SavePopUpCallBack != undefined || SavePopUpCallBack != null)
                            SavePopUpCallBack(data, SaveUrl);
                    } catch (e) {

                    }

                }
                else {
                    ShowSideError("", data.Message);
                }
            },
            error: function () {
                ShowSideError("", Locales.PopUpSaveError);
                $(e).find(".fa-spin").removeClass("fa-spin").removeClass("fa-spinner").addClass("fa-plus");
            }
        });
    }

}
//------------------------------------------------------------------------------------------------------

//------------------------------------------------------- Document Ready -------------------------------
document.addEventListener('DOMContentLoaded', function () {
   /* kendo.culture(CultureName);*/
    CheckValidationDisplay();
    initSidebarClick();
    InitKendoDropDown("select");
    $(".k-dropdown").each(function (i, c) {
        if ($(c).find("select").attr("readonly")) {
            $(c).addClass("disabledDropDown");
        }
    });

    $(".k-text").each(function (i, c) {        
        CreateNumericUpDown($(c),0, '',false);
    });
    $(".k-text-negative").each(function (i, c) {
        CreateNumericUpDownNigativeValue($(c), 0, '', false);
    });

    //Handle Control S Button
    $(window).bind('keydown', function (event) {
        if (event.ctrlKey || event.metaKey) {
            switch (String.fromCharCode(event.which).toLowerCase()) {
                case 's':
                    event.preventDefault();
                    //Click Save Button
                    $('[name="save-continue"]').click();
                    break;

            }
        }
    });

    //Alert Before Leave If Not Saved
    var unsaved = false;

    $(":input").change(function () { //trigers change in all input fields including text type
        unsaved = true;
    });
    $("form").submit(function () {
        CheckValidationDisplay();
        unsaved = false;
    });
    function unloadPage() {
        //todo check
        //if (unsaved && $("form").length > 0 && !$("form").hasClass('nowarn')) {
        //    return Locales.SaveBeforeLeave;
        //}
    }

    window.onbeforeunload = unloadPage;
   /* setCurrencyRateFinder();*/
    //Finance

});
//function setCurrencyRateFinder() {
//    $("#CurrencyId").change(function (e) {
//        var CurrencyId = $(e.target).val();
//        if (CurrencyId) {
//            $.ajax({
//                url: "/admin/Finance/Currency/GetCurrencyRate",
//                type: "Get",
//                datatype: "json",
//                data: { CurrencyId: CurrencyId },
//                success: function (data) {
//                    $("#CurrencyRate").val(data);
//                }
//            });
//        }
//    }).change();
//}

//------------------------------------------------------------------------------------------------------
//Sidebar
function initSidebarClick() {
    $(".dropdownlink ").click(function (e) {
        var slideSpeed = 200;
        var slideOffeset = -200;
        let arrowParent = e.target.closest("li");
        var currentMenuLink = $(e.target);
        var OldOppendMenu = $(".openmenu").not(arrowParent).last();
        if (OldOppendMenu.length > 0 && $(OldOppendMenu).has(currentMenuLink).length == 0) {
            var oldSub = OldOppendMenu.children("ul");
            oldSub.slideUp(slideSpeed, function () {
                if ($('body').hasClass('page-sidebar-fixed')) {
                    menu.slimScroll({
                        'scrollTo': (oldSub.position()).top
                    });
                } else {
                    App.scrollTo(oldSub, slideOffeset);
                }
                OpenCurrentMenu(currentMenuLink, slideOffeset, slideSpeed);
            });
        } else {
            OpenCurrentMenu(currentMenuLink, slideOffeset, slideSpeed);
        }
    });
}
function OpenCurrentMenu(currentMenuLink, slideOffeset, slideSpeed) {
    var sub = currentMenuLink.closest("li").children("ul");
    if (sub.is(":visible")) {
        sub.slideUp(slideSpeed, function () {
            if ($('body').hasClass('page-sidebar-fixed')) {
                menu.slimScroll({
                    'scrollTo': (currentMenuLink.position()).top
                });
            } else {
                App.scrollTo(currentMenuLink, slideOffeset);
            }
            currentMenuLink.closest("li").removeClass("openmenu");
        });
    }
    else {
        App.scrollTo(currentMenuLink, slideOffeset);
        sub.slideDown(slideSpeed, function () {
            if ($('body').hasClass('page-sidebar-fixed')) {
                menu.slimScroll({
                    'scrollTo': (currentMenuLink.position()).top
                });
            } else {
                App.scrollTo(currentMenuLink, slideOffeset);
            }
            $(".openmenu").removeClass("openmenu");
            currentMenuLink.closest("li").addClass("openmenu");
        });
    }
}

function getLanguageFromCookie() {
    var cookies = document.cookie.split(';');

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].trim();

        if (cookie.startsWith('.AspNetCore.Culture=')) {
            var encodedLanguage = cookie.substring('AspNetCore.Culture='.length);
            var decodedLanguage = decodeURIComponent(encodedLanguage);
            var language = decodedLanguage.split('|')[1].split('=')[1];
            if (language == 'ar') {
                language = 'ar-EG'
            }
            else {
                language = 'en-US'
            }
            return language;
        }
    }
    return null;
}


function DeleteWithConfirmationDailog(Id, controler, deleteSource) {
    var confirmMsg = 'هل تريد حذف العنصر الحالي ؟'
    var deleteErorrMsg = 'حدث خطأ أثناء الحذف'
    var titleMsg = 'تأكيد الحذف'
    var deleteSuccessMsg = 'تم الحذف بنجاح'
    var okButtonText = 'موافق'
    var cancelButtonText = 'غير موافق'

    //var currentLanguage = getLanguageFromCookie();
    if (currentLanguage != 'ar-EG') {
        confirmMsg = 'Do You Want To Delete This Item ?'
        deleteErorrMsg = 'An Erorr Happend'
        titleMsg = 'Confirm Delation'
        deleteSuccessMsg = 'Item Deleted Successflly'
        okButtonText = 'Ok'
        cancelButtonText = 'Cancel'
    }

    Swal.fire({
        title: titleMsg,
        text: confirmMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: cancelButtonText,
        confirmButtonText: okButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            debugger
            $.ajax({
                url: '/' + controler + '/Delete/',
                data: { Id: Id },
                success: function (data) {
                    debugger
                    if (data.type == "success") {
                        //refresh grid if delete action came from grid else redirect to list if delete action came from edit form
                        if (deleteSource == 'grid') {

                            Swal.fire(deleteSuccessMsg, '', 'success');

                            var grid = $("#datatable").data("kendoGrid");
                            var dataSource = grid.dataSource;
                            dataSource.read();

                        } else {

                            window.location.href = '/' + controler + '/Index';
                        }

                    }
                    else if (data.type == "faild") {
                        Swal.fire(data.msg, '', 'error');
                    }
                    else {
                        Swal.fire(deleteErorrMsg, '', 'error');
                    }
                }
            });

        }
    });   
}

function DeleteAccountWithConfirmationDailog(Id, controler,action) {
    var confirmMsg = 'هل تريد حذف العنصر الحالي ؟'
    var deleteErorrMsg = 'حدث خطأ أثناء الحذف'
    var titleMsg = 'تأكيد الحذف'
    var deleteSuccessMsg = 'تم الحذف بنجاح'
    var okButtonText = 'موافق'
    var cancelButtonText = 'غير موافق'

    if (currentLanguage != 'ar-EG') {
        confirmMsg = 'Do You Want To Delete This Item ?'
        deleteErorrMsg = 'An Erorr Happend'
        titleMsg = 'Confirm Delation'
        deleteSuccessMsg = 'Item Deleted Successflly'
        okButtonText = 'Ok'
        cancelButtonText = 'Cancel'
    }

    Swal.fire({
        title: titleMsg,
        text: confirmMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: cancelButtonText,
        confirmButtonText: okButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            debugger
            $.ajax({
                url: '/' + controler + '/' + action + '/',
                data: { Id: Id },
                success: function (data) {
                    debugger
                    if (data.type == "success") {
                        Swal.fire(deleteSuccessMsg, '', 'success');
                        var tree = $("#treelist").data("kendoTreeList");
                        tree.dataSource.read();

                    }
                    else if (data.type == "faild") {
                        Swal.fire(data.msg, '', 'error');
                    }
                    else {
                        Swal.fire(deleteErorrMsg, '', 'error');
                    }
                }
            });

        }
    });
}

function SendEInvoiceWithConfirmation(Id, controler) {
    var confirmMsg = 'هل تريد إرسال الفاتورة للمصلحة الضرائب ؟'
    var deleteErorrMsg = 'حدث خطأ أثناء إرسال الفاتورة'
    var titleMsg = 'تأكيد الإرسال'
    //var deleteSuccessMsg = 'تم إرسال الفاتورة بنجاح'
    var okButtonText = 'موافق'
    var cancelButtonText = 'غير موافق'

    if (currentLanguage != 'ar-EG') {
        confirmMsg = 'Do You Want To Send This Document ?'
        deleteErorrMsg = 'An Erorr Happend'
        titleMsg = 'Confirm Send'
        //deleteSuccessMsg = 'Item Send Successflly'
        okButtonText = 'Ok'
        cancelButtonText = 'Cancel'
    }

    Swal.fire({
        title: titleMsg,
        text: confirmMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: cancelButtonText,
        confirmButtonText: okButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            debugger
            $.ajax({
                url: '/' + controler + '/SendEInvoice/',
                data: { Id: Id },
                success: function (data) {
                    debugger
                    if (data.type == "success") {
                        Swal.fire(data.msg, '', 'success');

                    }
                    else if (data.type == "faild") {
                        Swal.fire(data.msg, '', 'error');
                    }
                    else {
                        Swal.fire(deleteErorrMsg, '', 'error');
                    }
                }
            });

        }
    });
}


function SendEInvoiceWithConfirmation2(Id, controler,DocType) {
    var confirmMsg = 'هل تريد إرسال الفاتورة للمصلحة الضرائب ؟'
    var deleteErorrMsg = 'حدث خطأ أثناء إرسال الفاتورة'
    var titleMsg = 'تأكيد الإرسال'
    //var deleteSuccessMsg = 'تم إرسال الفاتورة بنجاح'
    var okButtonText = 'موافق'
    var cancelButtonText = 'غير موافق'

    if (currentLanguage != 'ar-EG') {
        confirmMsg = 'Do You Want To Send This Document ?'
        deleteErorrMsg = 'An Erorr Happend'
        titleMsg = 'Confirm Send'
        //deleteSuccessMsg = 'Item Send Successflly'
        okButtonText = 'Ok'
        cancelButtonText = 'Cancel'
    }

    Swal.fire({
        title: titleMsg,
        text: confirmMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: cancelButtonText,
        confirmButtonText: okButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            debugger
            $.ajax({
                url: '/' + controler + '/SendEInvoice/',
                data: { Id: Id, DocType: DocType },
                success: function (data) {
                    debugger
                    if (data.type == "success") {
                        Swal.fire(data.msg, '', 'success');

                    }
                    else if (data.type == "faild") {
                        Swal.fire(data.msg, '', 'error');
                    }
                    else {
                        Swal.fire(deleteErorrMsg, '', 'error');
                    }
                }
            });

        }
    });
}

function CancelEInvoiceWithConfirmation(Id, controler) {
    var confirmMsg = 'هل تريد إرسال إلغاء  الفاتورة ؟'
    var deleteErorrMsg = 'حدث خطأ أثناء إلغاء الفاتورة'
    var titleMsg = 'تأكيد الإلغاء'
    var okButtonText = 'موافق'
    var cancelButtonText = 'غير موافق'

    if (currentLanguage != 'ar-EG') {
        confirmMsg = 'Do You Want To Cancel This Document ?'
        deleteErorrMsg = 'An Erorr Happend'
        titleMsg = 'Confirm Cancelation'
        okButtonText = 'Ok'
        cancelButtonText = 'Cancel'
    }

    Swal.fire({
        title: titleMsg,
        text: confirmMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: cancelButtonText,
        confirmButtonText: okButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            debugger
            $.ajax({
                url: '/' + controler + '/CancelEInvoice/',
                data: { Id: Id },
                success: function (data) {
                    debugger
                    if (data.type == "success") {
                        Swal.fire(data.msg, '', 'success');

                    }
                    else if (data.type == "faild") {
                        Swal.fire(data.msg, '', 'error');
                    }
                    else {
                        Swal.fire(deleteErorrMsg, '', 'error');
                    }
                }
            });

        }
    });
}

function ShowSuccessMessege(msg)
{
    Swal.fire(msg, '', 'success');
}
function ShowErrorMessege(msg) {
    Swal.fire(msg, '', 'error');
}


function ShowConfirmationMessege(confirmMsg, sender) {
    var confirmMsg = confirmMsg    
    var okButtonText = 'موافق'
    var cancelButtonText = 'غير موافق'

    if (currentLanguage != 'ar-EG') {       
        okButtonText = 'Ok'
        cancelButtonText = 'Cancel'
    }

    Swal.fire({
        text: confirmMsg,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: cancelButtonText,
        confirmButtonText: okButtonText
    }).then((result) => {
        if (result.isConfirmed) {            
           $(sender).submit();

        }
    });
}

//------------------------------------------------------------------------------------------------------
//serializeObject
(function ($) {
    $.fn.serializeObject = function () {
        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push": /^$/,
                "fixed": /^\d+$/,
                "named": /^[a-zA-Z0-9_]+$/
            };
        this.build = function (base, key, value) {
            base[key] = value;
            return base;
        };

        this.push_counter = function (key) {
            if (push_counters[key] === undefined) {
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };

        $.each($(this).serializeArray(), function () {
            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;
            while ((k = keys.pop()) !== undefined) {
                // Adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

                // Push
                if (k.match(patterns.push)) {
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }

                // Fixed
                else if (k.match(patterns.fixed)) {
                    merge = self.build([], k, merge);
                }
                // Named
                else if (k.match(patterns.named)) {
                    merge = self.build({}, k, merge);
                }
            }
            json = $.extend(true, json, merge);
        });
        return json;
    };
})(jQuery);