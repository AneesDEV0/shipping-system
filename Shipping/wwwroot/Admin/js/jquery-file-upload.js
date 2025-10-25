(function($) {
  'use strict';
  if ($("#fileuploader").length) {
    $("#fileuploader").uploadFile({
      url: "../../~/admin/images/",
      fileName: "myfile"
    });
  }
})(jQuery);