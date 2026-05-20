
(function ($) {
    "use strict";

    var current_fs, next_fs, previous_fs;
    var opacity;
    var stepsForm = $(".steps");
    var fieldsets = stepsForm.find("fieldset");

    fieldsets.hide().first().show();
    $("#progressbar li").removeClass("active").first().addClass("active");

    stepsForm.validate({
        errorClass: "invalid",
        errorElement: "span",
        ignore: ":hidden",
        errorPlacement: function (error, element) {
            error.insertAfter(element);
        },
        highlight: function (element) {
            $(element).addClass("invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("invalid");
        }
    });

    $(".next").click(function () {
        if (!stepsForm.valid()) {
            return false;
        }

        current_fs = $(this).parent();
        next_fs = $(this).parent().next();
        $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");
        next_fs.show();
        current_fs.animate({
            opacity: 0
        }, {
            step: function (now) {
                opacity = 1 - now;
                next_fs.css({
                    opacity: opacity
                });
            },
            duration: 1,
            complete: function () {
                current_fs.hide();

                if (typeof window.onShipmentStepChanged === "function") {
                    window.onShipmentStepChanged(next_fs, $("fieldset").index(next_fs));
                }
            }
        });

        return false;
    });

    $(".submit").click(function () {
        if (!stepsForm.valid()) {
            return false;
        }

        current_fs = $(this).parent();
        next_fs = $(this).parent().next();
        $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");
        next_fs.show();
        current_fs.animate({
            opacity: 0
        }, {
            step: function (now) {
                opacity = 1 - now;
                next_fs.css({
                    opacity: opacity
                });
            },
            duration: 1,
            complete: function () {
                current_fs.hide();
            }
        });

        return false;
    });

    $(".previous").click(function () {
        current_fs = $(this).parent();
        previous_fs = $(this).parent().prev();
        $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");
        previous_fs.show();
        current_fs.animate({
            opacity: 0
        }, {
            step: function (now) {
                opacity = 1 - now;
                previous_fs.css({
                    opacity: opacity
                });
            },
            duration: 1,
            complete: function () {
                current_fs.hide();
            }
        });

        return false;
    });

    window.resetShipmentWizard = function (formSelector) {
        var form = $(formSelector);
        var fieldsets = form.find("fieldset");

        fieldsets.hide().first().show();
        $("#progressbar li").removeClass("active").first().addClass("active");
        form.validate().resetForm();
        form.find(".invalid").removeClass("invalid");
    };

    window.goToShipmentStep = function (index) {
        var form = $(".steps");
        var fieldsets = form.find("fieldset");

        if (index < 0 || index >= fieldsets.length) {
            return;
        }

        fieldsets.hide();
        fieldsets.eq(index).show();

        $("#progressbar li").removeClass("active");
        $("#progressbar li").each(function (stepIndex) {
            if (stepIndex <= index) {
                $(this).addClass("active");
            }
        });

        if (typeof window.onShipmentStepChanged === "function") {
            window.onShipmentStepChanged(fieldsets.eq(index), index);
        }
    };
}(jQuery));
