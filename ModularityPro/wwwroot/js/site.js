// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//==================================================================================================
// import $ from '.././lib/jquery/dist/jquery.js';
// $ = require('.././lib/jquery/dist/jquery.js');
// Write your JavaScript code.
$(document).ready(function () {
  $("#new-post").click(function () {
    if ($("#profile-create-post").is(":visible")) {
      $("#profile-create-post").slideUp();
    } else {
      $("#profile-create-post").slideDown();
    }
  });

  function setHasNewRequestsDefault(fn, context) {
    var result;

    return function () {
      if (fn) {
        result = fn.apply(context || this, arguments);
        fn = null;
      }

      return false;
    }
  }

  var hasNewRequests = setHasNewRequestsDefault();

  function viewFriendRequests() {
    hasNewRequests = false;
  }

  if (hasNewRequests === true) {
    $("#friend-request-header").css("color", "red");
  } else {
    $("#friend-request-header").css("color", "black");
  }

  "use strict";

  const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

  //Disable send button until connection is established
  // document.getElementById("sendButton").disabled = true;
  connection.on("ReceiveFriendRequest", function (user) {
    hasNewRequests = true;
  });

  // connection.on("ReceiveMessage", function (user, message) {
  //   var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
  //   var encodedMsg = user + " says " + msg;
  //   var li = document.createElement("li");
  //   li.textContent = encodedMsg;
  //   document.getElementById("messagesList").appendChild(li);
  // });
  // connection.start().then(function () {
  //   document.getElementById("sendButton").disabled = false;
  // }).catch(function (err) {
  //   return console.error(err.toString());
  // });

  // document.getElementById("sendButton").addEventListener("click", function (event) {
  //   var fromUser = document.getElementById("userInput").value;
  //   var message = document.getElementById("messageInput").value;
  //   var toUser = document.getElementById("sendToUser").value;


  //   // var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
  //   var encodedMsg = fromUser + " says " + message;
  //   var li = document.createElement("li");
  //   li.textContent = encodedMsg;
  //   document.getElementById("messagesList").appendChild(li);



  //   connection.invoke("SendPrivateMessage", toUser, fromUser, message).catch(function (err) {
  //     return console.error(err.toString());
  //   });
  //   event.preventDefault();
  // });

  connection.start().then(function () {
  }).catch(function (err) {
    return console.error(err.toString());
  });

  $("#send-friend-request").off("click").on("click", function () {
    var rawInput = $("#send-friend-request-values").attr("value");
    var userNames = rawInput.split(",");
    connection.invoke("NotifyFriendRequest", userNames[0], userNames[1]).catch(function (err) {
      return console.error(err.toString());
    });
  });



});

// POPUP STUFFS

//this function can remove a array element.
Array.remove = function (array, from, to) {
  var rest = array.slice((to || from) + 1 || array.length);
  array.length = from < 0 ? array.length + from : from;
  return array.push.apply(array, rest);
};

//this variable represents the total number of popups can be displayed according to the viewport width
var total_popups = 0;

//arrays of popups ids
var popups = [];

//this is used to close a popup
function close_popup(id) {
  for (var iii = 0; iii < popups.length; iii++) {
    if (id == popups[iii]) {
      Array.remove(popups, iii);

      document.getElementById(id).style.display = "none";

      calculate_popups();

      return;
    }
  }
}

//displays the popups. Displays based on the maximum number of popups that can be displayed on the current viewport width
function display_popups() {
  var right = 220;

  var iii = 0;
  for (iii; iii < total_popups; iii++) {
    if (popups[iii] != undefined) {
      var element = document.getElementById(popups[iii]);
      element.style.right = right + "px";
      right = right + 320;
      element.style.display = "block";
    }
  }

  for (var jjj = iii; jjj < popups.length; jjj++) {
    var element = document.getElementById(popups[jjj]);
    element.style.display = "none";
  }
}

//creates markup for a new popup. Adds the id to popups array.
function register_popup(id, name) {

  for (var iii = 0; iii < popups.length; iii++) {
    //already registered. Bring it to front.
    if (id == popups[iii]) {
      Array.remove(popups, iii);

      popups.unshift(id);

      calculate_popups();


      return;
    }
  }

  var element = '<div class="popup-box chat-popup" id="' + id + '">';
  element = element + '<div class="popup-head">';
  element = element + '<div class="popup-head-left">' + name + '</div>';
  element = element + '<div class="popup-head-right"><a href="javascript:close_popup(\'' + id + '\');">&#10005;</a></div>';
  element = element + '<div style="clear: both"></div></div><div class="popup-messages"></div></div>';

  document.getElementsByTagName("body")[0].innerHTML = document.getElementsByTagName("body")[0].innerHTML + element;

  popups.unshift(id);

  calculate_popups();

}

//calculate the total number of popups suitable and then populate the toatal_popups variable.
function calculate_popups() {
  var width = window.innerWidth;
  if (width < 540) {
    total_popups = 0;
  }
  else {
    width = width - 200;
    //320 is width of a single popup box
    total_popups = parseInt(width / 320);
  }

  display_popups();

}

//recalculate when window is loaded and also when window is resized.
window.addEventListener("resize", calculate_popups);
window.addEventListener("load", calculate_popups);