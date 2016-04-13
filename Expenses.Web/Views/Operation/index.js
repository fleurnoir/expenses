function initOperationIndex (strDateFormat, strDateFrom, strDateTo, strShowFilter, strHideFilter) {

    var getDate = function(selector) {
        return $.datepicker.formatDate("yy-mm-dd", $(selector).datepicker("getDate"))
    }

    var setDate = function(selector, dateString) {
        $(selector).datepicker("setDate", $.datepicker.parseDate("yy-mm-dd", dateString));
    }

    var options = { dateFormat: strDateFormat };
    $("#date-from").datepicker(options);
    setDate("#date-from", strDateFrom);
    $("#date-to").datepicker(options);
    setDate("#date-to", strDateTo);

    $("#apply-filter").click(function(e){
        $a = $(e.target);
        $a.attr("href","/Operation/Index?dateFrom=" + getDate("#date-from") + 
                        "&dateTo=" + getDate("#date-to") +
                        "&categoryId=" + $("#CategoryId").val() +
                        "&subcategoryId=" + $("#SubcategoryId").val() +
                        "&accountId=" + $("#AccountId").val() );
    });

    $("#show-filter").click(function(e){
        $filter = $("#filter");
        if($filter.is(":visible")){
            $filter.hide();
            $(e.target).text(strShowFilter);
        }
        else {
            $filter.show();
            $(e.target).text(strHideFilter);
        }
    });

    $("#CategoryId").change(function(e) {
        $.ajax({
            type: "POST", contentType: "application/json; charset=utf-8", dataType: "json",
            url: "/Operation/GetFilterSubcategories",
            data: "{ categoryId: " + $(e.target).val() + " }",
            success: function(data, status) {     
                var $el = $("#SubcategoryId");
                $el.empty();
                var len = data.length;
                for(var i=0; i < len; i++) {
                    $el.append($("<option></option>")
                       .attr("value", data[i].id).text(data[i].name));
                }                    
            }
        });
    });

    $("#create-new").focus();
}