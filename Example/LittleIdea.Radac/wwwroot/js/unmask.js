$(function() {
    $(".masked-data").click(function() {
        var me = $(this);
        var path = me.attr("data-route");

        $.ajax({
            type: "GET",
            data: {},
            url: window.location.origin + path,
            success: function(data) {
                if (data.status === "success") {
                    me.text(data.result);
                    me.removeAttr("data-route");
                    me.removeClass("masked-data");
                    me.addClass("unmasked-data");
                    me.off("click");
                } else {
                    alert(data.message);
                }
            },
            statusCode: {
                403: function() {
                    alert("Your activity is deemed too risky.");
                }
            }
        });
    });
});