// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
  $("#new-post").click(function () {
    if ($("#profile-create-post").is(":visible")) {
      $("#profile-create-post").slideUp();
    } else {
      $("#profile-create-post").slideDown();
    }
  });
});