// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//==================================================================================================
// import $ from '.././lib/jquery/dist/jquery.js';
// $ = require('.././lib/jquery/dist/jquery.js');
// Write your JavaScript code.
// The main class

var AlertBox = function (id, option) {
  this.show = function (msg) {
    if (msg === '' || typeof msg === 'undefined' || msg === null) {
      throw '"msg parameter is empty"';
    }
    else {
      var alertArea = document.querySelector(id);
      var alertBox = document.createElement('DIV');
      var alertContent = document.createElement('DIV');
      var alertClose = document.createElement('A');
      var alertClass = this;
      alertContent.classList.add('alert-content');
      alertContent.innerHTML = msg;
      alertClose.classList.add('alert-close');
      alertClose.setAttribute('href', '#');
      alertBox.classList.add('alert-box');
      alertBox.appendChild(alertContent);
      if (!option.hideCloseButton || typeof option.hideCloseButton === 'undefined') {
        alertBox.appendChild(alertClose);
      }
      alertArea.appendChild(alertBox);
      alertClose.addEventListener('click', function (event) {
        event.preventDefault();
        alertClass.hide(alertBox);
      });
      if (!option.persistent) {
        var alertTimeout = setTimeout(function () {
          alertClass.hide(alertBox);
          clearTimeout(alertTimeout);
        }, option.closeTime);
      }
    }
  };

  this.hide = function (alertBox) {
    alertBox.classList.add('hide');
    var disperseTimeout = setTimeout(function () {
      alertBox.parentNode.removeChild(alertBox);
      clearTimeout(disperseTimeout);
    }, 500);
  };
};

// Sample invoke
// var alertNonPersistent = document.querySelector('#alertNonPersistent');
// var alertPersistent = document.querySelector('#alertPersistent');
// var alertShowMessage = document.querySelector('#alertShowMessage');
// var alertHiddenClose = document.querySelector('#alertHiddenClose');
// var alertMessageBox = document.querySelector('#alertMessageBox');
var alertbox = new AlertBox('#alert-area', {
  closeTime: 5000,
  persistent: false,
  hideCloseButton: false
});
var alertboxPersistent = new AlertBox('#alert-area', {
  closeTime: 5000,
  persistent: true,
  hideCloseButton: false
});
var alertNoClose = new AlertBox('#alert-area', {
  closeTime: 5000,
  persistent: false,
  hideCloseButton: true
});

// alertNonPersistent.addEventListener('click', function () {
//   alertbox.show(alertMessageBox.value);
//   alertMessageBox.value = '';
// });

// alertPersistent.addEventListener('click', function () {
//   alertboxPersistent.show(alertMessageBox.value);
//   alertMessageBox.value = '';
// });

// alertShowMessage.addEventListener('click', function () {
//   alertbox.show('Hello! This is a message.');
// });

// alertHiddenClose.addEventListener('click', function () {
//   alertNoClose.show('Hello! I have hidden my close button.');
// });


//===================================================================
$(document).ready(function () {
  $("#new-post").off("click").on("click", function () {
    if ($("#profile-create-post").is(":visible")) {
      $("#profile-create-post").slideUp();
    } else {
      $("#profile-create-post").slideDown();
    }
  });

  "use strict";

  const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

  connection.start().then(function () {
  }).catch(function (err) {
    return console.error(err.toString());
  });

  connection.on("AddToChat", (userRealName, userName, userAvatarUrl) => {
    if ($(`#${userName}`).length === 0) {
      $(".sidebar-nav").append(
        `<li class="sidebar-nav-item" id="${userName}">` +
        ` <a href="javascript:register_popup('${userName}', '${userName});" class="sidebar-nav-link">` +
        ` <img width="45" height="45" src="${userAvatarUrl}" ` +
        ` aria-hidden="true" ` +
        ` focusable="false" ` +
        ` data-prefix="fad" ` +
        ` role="img" ` +
        ` xmlns="http://www.w3.org/2000/svg" ` +
        ` viewBox="0 0 576 512" ` +
        ` class="svg-inline">` +
        ` <span class="link-text">${userRealName}</span>` +
        ` </a>` +
        `</li>`
      );
    }

    // $(`#${userName}`).click(function () {
    //   register_popup(userName, userName);
    // });
  });

  //Disable send button until connection is established
  // document.getElementById("sendButton").disabled = true;
  connection.on("ReceiveFriendRequest", (user) => {
    if ($(".alert-box").length === 0) {
      alertbox.show("You received a friend request from " + user + `. <a href="/Friends/Requests">View Now</a>`);
    }
  });

  connection.on("ReceiveVideoInvite", (fromUser, inviteUrl) => {
    if ($(".alert-box").length === 0) {
      //alertboxPersistent.show("You received a video call invite from " + fromUser + ": " + inviteUrl);
      alertboxPersistent.show(`You received a video call invite from ${fromUser}: <a href="${inviteUrl}">${inviteUrl}</a>`);
    }
  });

  connection.on("ReceiveMessage", function (user, username, message) {
    var existing = $(".chat-messages-area").html();
    var previous = $(".chat-message").last().text();
    var location = window.location.href;
    var chattingWith = $(".card-header").text();
    if (!location.includes("Chat") && $(".alert-box").length === 0) {
      //alertbox.show("You received a friend message from " + user);
      alertbox.show(`You received a friend message from <a href="/Chat/${username}#${username}">${user}</a>`);
    }
    if (message != previous && chattingWith.includes(user)) {
      //$(".chat-messages-area").html(existing + "<span class='chat-message'>" + message + "</span><br>");
      $(".chat-messages-area").append("<span class='chat-message chat-msg-received'>" + message + "</span><br><br>");
      $(".chat-messages-area").animate({ scrollTop: $(".chat-messages-area")[0].scrollHeight }, 100);
    }
  });


  $("#chat-form-submit").off("click").on("click", function (event) {
    event.preventDefault();
    var toUser = location.hash;
    toUser = toUser.replace("#", "");
    var existing = $(".chat-messages-area").html();
    var fromUser = $("#chat-form-from").val();
    var message = $("#chat-form-message").val();
    var fromRealName = $("#chat-form-from-realname").val();
    $("#chat-form-message").val("");
    //$(".chat-messages-area").html(existing + "<span class='chat-message'>" + fromRealName + ": " + message + "</span><br>");
    $(".chat-messages-area").append("<span class='chat-message chat-msg-sent'>" + fromRealName + ": " + message + "</span><br><br>");
    $(".chat-messages-area").animate({ scrollTop: $(".chat-messages-area")[0].scrollHeight }, 100);
    connection.invoke("SendPrivateMessage", toUser, fromUser, message);
  });

  $(".send-friend-request-values").on("click", function () {
    var rawInput = $(this).attr("value");
    var userNames = rawInput.split(",");
    connection.invoke("NotifyFriendRequest", userNames[0], userNames[1]).catch(function (err) {
      return console.error(err.toString());
    });
  });

  $("#sendVideoChatInvite").on("click", function () {
    var videoToUser = $("#videoToUser").val();
    var videoFromUser = $("#videoFromUser").val();
    var videoUrl = window.location.href;

    connection.invoke("SendVideoInvite", videoToUser.toString(), videoFromUser.toString(), videoUrl);
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
  element = element + '<div style="clear: both"></div></div><div class="popup-messages">Spooked ya!</div><div class="chat-controls">';
  element = element + '<input type="text" class="form-control-chat"><button class="btn btn-primary send-message">Send</button></div></div>';

  document.getElementsByTagName("body")[0].innerHTML = document.getElementsByTagName("body")[0].innerHTML + element;

  popups.unshift(id);

  // $(".send-message").on("click", function () {
  //   alert("Hi");
  // });
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

//========================================================================================================
