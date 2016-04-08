function initExchangeEdit(sourceCurrencyId, destCurrencyId){
    var idPairs = { 
        "#SourceAccountId" : "#SourceCurrencyName",
        "#DestAccountId" : "#DestCurrencyName" 
        };

    var currencies = {
        "#SourceAccountId" : sourceCurrencyId,
        "#DestAccountId" : destCurrencyId
    }; 

    var destAmount;
    var updateEditors = function() {
        var $el = $("#DestAmount");
        if(currencies["#SourceAccountId"] == currencies["#DestAccountId"]) {
            $("#DestAmountGroup").hide();
            destAmount = $el.val();
            $el.val("0");
        } else {
            $el.val(destAmount);
            $("#DestAmountGroup").show();
        }
    };

    var onAccountChange = function(e) {
        $.ajax({
            type: "POST", contentType: "application/json; charset=utf-8", dataType: "json",
            url: "/Exchange/GetCurrency",
            data: "{ accountId: " + $(e.target).val() + " }",
            success: function(data, status) {     
                var selectId = "#"+$(e.target).attr("id");
                $(idPairs[selectId]).empty().text(data.name);
                currencies[selectId] = data.id;
                updateEditors();
            }
        });
    };

    for(var id in idPairs)
        $(id).change(onAccountChange);

    updateEditors();

    $("#SourceAccountId").focus();
};
