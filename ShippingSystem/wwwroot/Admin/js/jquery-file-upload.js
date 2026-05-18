(function($) {
  'use strict';
  if ($("#fileuploader").length) {
    $("#fileuploader").uploadFile({
      url: "~/Admin/images/",
      fileName: "myfile"
    });
  }
})(jQuery);