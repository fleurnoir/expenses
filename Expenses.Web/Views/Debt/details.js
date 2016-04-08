function initDebtDetails (debtId, strCancel, strCreate, strEdit) {
   var showError = function(message) {
      $("#error-text").text(message);
      $("#error-dialog").dialog("open");
   };

   var onAjaxSuccess = function($dlg, data) {
        $dlg.dialog("close");
        if(data.ok == true)
            window.location="/Debt/Details/"+debtId;
        else
            showError(data.error);
   };

   var onAjaxError = function($dlg, error) {
        $dlg.dialog("close");
        showError(error);
   };

   var dlgButtons = {};

   dlgButtons["OK"] = function() {
                var $dlg = $("#edit-dialog");
                var data = "{ debtId: '" + debtId + "', " +
                           "id: '"       + $("#edit-id").val() + "', " +
                           "amount: '"   + $("#edit-amount").val() + "', " +
                           "comment: '"  + $("#edit-comment").val() + "' }";

                var url = ($dlg.dialog("option","title") == strCreate) ? 
                             "/Debt/AddRepayment" : "/Debt/EditRepayment";

                $.ajax({
                    type: "POST", contentType: "application/json; charset=utf-8", dataType: "json",
                    url: url,
                    data: data,
                    success: function(data, status) { onAjaxSuccess($dlg, data); },
                    error: function(jqXHR, textStatus, errorThrown) { onAjaxError($dlg, errorThrown); } 
                });
            };

   dlgButtons[strCancel] = function() {$(this).dialog("close");};

   $( "#edit-dialog" ).dialog({
        autoOpen: false,
        modal: true,  
        buttons: dlgButtons            
    });

    dlgButtons = {};

    dlgButtons["OK"] = function() {
                var $dlg = $("#delete-dialog");
                $.ajax({
                    type: "POST", contentType: "application/json; charset=utf-8", dataType: "json",
                    url: "/Debt/DeleteRepayment",
                    data: "{ debtId: " + debtId + ", " + 
                            "id: " + $("#delete-id").val() + " }",
                    success: function(data, status) { onAjaxSuccess($dlg, data); },
                    error: function(jqXHR, textStatus, errorThrown) { onAjaxError($dlg, errorThrown); }  
                });
            };

     dlgButtons[strCancel] = function() {$(this).dialog("close");};

    $( "#delete-dialog" ).dialog({
        autoOpen: false,
        modal: true,  
        buttons: dlgButtons            
    });

    $( "#error-dialog" ).dialog({
        autoOpen: false,
        modal: true,  
        buttons: {
            OK: function() { $( "#error-dialog" ).dialog("close"); }
        }            
    });

    $( "#create-link" ).click(function() {
       var $dialog = $( "#edit-dialog" );
       $dialog.dialog("option", "title", strCreate);
       $("#edit-amount").val("");
       $("#edit-comment").val("");
       $dialog.dialog( "open" );
    });

    $( ".edit-link" ).click( function (e) {
       var $dialog = $( "#edit-dialog" );
       $dialog.dialog("option", "title", strEdit);

       var id = $(e.currentTarget).attr("data-item-id");
       $("#edit-id").val(id);
       $("#edit-amount").val($("#amount-"+id).text());
       $("#edit-comment").val($("#comment-"+id).text());
       $dialog.dialog( "open" );
    });

    $( ".delete-link" ).click(function (e) {
       var id = $(e.currentTarget).attr("data-item-id");
       $("#delete-id").val(id);
       $("#delete-amount").text($("#amount-"+id).text());
       $("#delete-dialog").dialog( "open" );
    });

}