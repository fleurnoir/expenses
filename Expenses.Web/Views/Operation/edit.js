function initOperationEdit (btnCancelText) {
    var errorFun = function(jqXHR, textStatus, errorThrown) {
        console.log(textStatus, errorThrown);
    };

    $("#CategoryId").change(function(e) {
        $.ajax({
            type: "POST",
            url: "/Operation/GetSubcategories",
            data: "{ categoryId: " + $(e.target).val() + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(data, status) {     
                var $el = $("#SubcategoryId");
                $el.empty(); // remove old options
                var len = data.length;
                for(var i=0; i < len; i++) {
                    $el.append($("<option></option>")
                        .attr("value", data[i].id).text(data[i].name));
                }                    
            },
            error: errorFun 
        });
    });

    $("#AccountId").change(function(e) {
        $.ajax({
            type: "POST",
            url: "/Operation/GetCurrencyName",
            data: "{ accountId: " + $(e.target).val() + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(data, status) {     
                $("#CurrencyName").empty().text(data.currencyName);
            },
            error: errorFun
        });
    });

    var dlgOptions = {
        autoOpen: false,
        modal: true,  
        buttons: {
            OK: function() {
                $.ajax({
                    type: "POST",
                    url: "/Operation/AddCategory",
                    data: "{ categoryName: '" + $("#add-category-name").val() + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data, status) {     
                        var $el = $("#CategoryId");
                        $el.append($("<option></option>")
                            .attr("value", data.id).text(data.name));
                        $el.val(data.id);
                        $("#SubcategoryId").empty();                    
                        $("#add-category-dialog").dialog("close");
                    },
                    error: errorFun
                });
            }
        }            
    };

    dlgOptions.buttons[btnCancelText] = function() {$(this).dialog("close");}

    $( "#add-category-dialog" ).dialog(dlgOptions);

    dlgOptions = {
        autoOpen: false,
        modal: true,  
        buttons: {
            OK: function() {
                $.ajax({
                    type: "POST",
                    url: "/Operation/AddSubcategory",
                    data: "{ subcategoryName: '" + $("#add-sub-name").val() + "', categoryId: " + $("#CategoryId").val() + " }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data, status) {     
                        var $el = $("#SubcategoryId");
                        $el.append($("<option></option>")
                            .attr("value", data.id).text(data.name));
                        $el.val(data.id);
                        $("#add-sub-dialog").dialog("close");
                    },
                    error: errorFun
                });
            }
        }            
    };

    dlgOptions.buttons[btnCancelText] = function() {$(this).dialog("close");}

    $( "#add-sub-dialog" ).dialog(dlgOptions);

    $( "#add-category" ).click(function() {
       $( "#add-category-dialog" ).dialog( "open" );
    });

    $( "#add-sub" ).click(function() {
       $( "#add-sub-dialog" ).dialog( "open" );
    });

    $("#CategoryId").focus();
};
