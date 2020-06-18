<div style="max-width: 100%"><img src="https://i.ibb.co/QJHt9cY/MP-Header.jpg"/></div>

### _Created by Matt Stroud, Dj Sevenbergen, Tyler Bates & KhanSahab Khakwan_

## Description

ModularityPro is a multiplatorm, mobile ready social media network application dynamically connecting users through the use of:

* [Video Calls](#videoCall)
* [Chat Hub](#chatHub) - our built in messenger
* [Friend Requests](#friendRequest)
* [Message Board](#feed) displaying users posts in real time.

Upon entering the home page you are given the options to register or login. Once authenticated you can immediately begin posting to the message board or use the top navigation bar to search for other users, customize your profile, view pending friend requests or enter the video chat room. User searches are based on Name and from the results you can either view that users profile or send them a friend request.

(Keep an eye out for the pop up box notifying you of any new friend requests)
<hr />

## _Setup/Installation Requirements_ 

## To Host the Server:

1. Clone this projects repository into your local directory following [these](https://www.linode.com/docs/development/version-control/how-to-install-git-and-clone-a-github-repository/) instructions.

2. Open the now local project folder with [VSC](https://code.visualstudio.com/Download) or an equivalent

3. Download [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/install/runtime?pivots=os-windows) then enter the following command in the terminal to confirm installation (2.2 or higher)
```sh
dotnet -- version
``` 
4. Still in the command line, enter the following two commands:
```sh
dotnet tool install -g 
```
```
dotnet-script
```
5. Download [ASP.NET Core](https://dotnet.microsoft.com/download) to enable live viewing on a local server

6. Open the repository, navigate to the containing folder of the project & Enter the following command to confirm build stability 

```sh
dotnet build 
```

7. Within that same containing folder enter the following to open a live server w/auto updated viewing
```sh
dotnet watch run
``` 

**The server is now live!**

8. Enter the following url into your browser to view the project:
```
https:localhost.5001/
```

<hr />

<div id="mySql">

## MySQL Installation & Configuration:
1. Download the [MySQL Community Server DMG file](https://dev.mysql.com/downloads/file/?id=484914) with the _"No thanks, just start my download"_ link.
2. On the configuration page of the installer select the following options:
* Use legacy password encryption
* Set your password
3. Open your terminal and enter the command:
```
'export PATH="/usr/local/mysql/bin:$PATH"' >> ~/.bash_profile
```
4. Download the [MySQL Workbench DMG file](https://dev.mysql.com/downloads/file/?id=484391)
5. Open Local Instance 3306 with the password you set.
6. Within the project directory, create a file called "appsettings.json" and populate it with the following code:
```sh
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;database=modularity_pro;uid=root;pwd=[YOUR PASSWORD GOES HERE];"
  },
  "AllowedHosts": "*"
}
```
7. Next, initiate a data migration by entering the following into your terminal:
```
dotnet ef migrations add DATABASE
```
8. Once this successfully employs, enter the following to update the data tables to correlate with the project models:
```
dotnet ef database update
```
9. You should now be able to view the full project database in MySQL Workbench.
<hr />

</div>

## Key Features

<div id="videoCall">

### Video Call

ModularityPro features video conference rooms allowing you to invite friends to join you in face to face connections!
Rooms are accessible from the top navigation bar and display a list of your friends. Choose one from the list and they are sent an alert with a designated url. They simply enter that url into their browser to join your room.

<img src="https://media.giphy.com/media/j5bds4FBI9RZrLXO4E/giphy.gif"/>

</div>

<div id="chatHub">

### Chat Hub

Chat Hub dynamically displays your friend list in an animated navigation bar. Simply hover over it with your cursor and click on the desired friend to open a messenger box.

<img src="https://media.giphy.com/media/XZOEI9sHMgDE9wPNPs/giphy.gif">

</div>

<div id="friendRequest">

### Friend Requests

Connecting with other users has never been easier.<br>
Simply click on the following icon to send a friend request to that user:<p style="height: 35px"><img src="https://i.ibb.co/Wcs97Db/friend-icon.jpg"></p><br>You can find these icons in the users profile or next to their name in the search results.
* Note that searching with an empty searchbar returns all users 

You can check the status of all pending requests from the navigation bar by clicking on "Requests"

<img src ="https://media.giphy.com/media/Kau4vrp2rzrPfhmTyX/giphy.gif">

</div>

<div id="feed">

### Message Board

The homepage of our site features a message board displaying all posts from yourself and your friends in real time!

<img src="https://media.giphy.com/media/dZRxwmVKOvdkLIdUbQ/giphy.gif">

</div>

<hr />

## Technology Used

<div class="img">

|Click icon for info|||
|-----|-----|-----|
|<a href="https://en.wikipedia.org/wiki/C_Sharp_%28programming_language%29"><img src="https://i.ibb.co/yg4msBL/C.png"></a>|<a href="https://en.wikipedia.org/wiki/.NET_Core"><img src="https://i.ibb.co/x67pVGj/NETCORE.png"></a>|<a href="https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-3.1"><img src="https://i.ibb.co/TLJZ0kH/netcoremvc.png"></a>|
|<a href="https://www.mysql.com/products/workbench/"><img src="https://i.ibb.co/02qDMbp/mysql.png"></a>|<a href="https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/entity-sql-language"><img src="https://i.ibb.co/txCy5BK/Entity.png"></a>|<a href="https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio"><img src="https://i.ibb.co/pn6gGCc/Identity.png"></a>|
|<a href="https://webrtc.org/"><img src="https://i.ibb.co/BshZfsT/webrtc.jpg"></a>|<a href="https://dotnet.microsoft.com/apps/aspnet/signalr"><img src="https://i.ibb.co/4ps5ZM4/signalr.png"></a>|<a href="https://www.docker.com/?utm_source=google&utm_medium=cpc&utm_campaign=dockerhomepage&utm_content=namer&utm_term=dockerhomepage&utm_budget=growth&gclid=CjwKCAjw_qb3BRAVEiwAvwq6VsInKX5r2iddiuCtBcRbSBD9JqOUv_jmAPtfvLZKRLJ2qctljUiUfxoCWxcQAvD_BwE"><img src="https://i.ibb.co/C55GVC3/docker.png"></a>|
|<a href="https://www.javascript.com/"><img src="https://i.ibb.co/wBxFph1/JS.jpg"></a>|<a href="https://jquery.com/"><img src="https://i.ibb.co/qmyCqg0/jquery.png"></a>|<a href="https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-3.1"><img src="https://i.ibb.co/kGGf28J/cshtml.png"></a>|
|<a href="https://www.w3.org/Style/CSS/Overview.en.html"><img src="https://i.ibb.co/2y8kcD2/css.png"></a>|<a href="https://getbootstrap.com/"><img src="https://i.ibb.co/HVBt0D2/bootstrap.png"></a>|<a href="https://code.visualstudio.com/"><img src="https://i.ibb.co/TT523dM/vscode.png"></a>|

</div>

<hr />

## Logistics / Specs

|Behavior|Input|Output|
|-----|-----|-----|
|User is greeted at home page|"https://localhost:5001/"|"localhost:5001/index.html"|
|User can register|"register"|"localhost:5001/account/register"|
|User can login|"login"|"localhost:5001/account/login"|
|User can post to message board|"Post"|"localhost:5001/index"|
|User can search users by name|"test"|"localhost:localhost:5001/search?search={parameters}"|
|User can view profiles of users|"clicks on user name"|"localhost:5001/profile/{userid}"|
|User can edit own profile information|"Edit"|"localhost:5001/profile/{user name}/edit"|
|User can send friend request to other user|"clicks add-friend icon"|"localhost:5001/requests/{user name}"|
|User can view all pending friend requests | "clicks 'requests' on navigation bar"|"localhost:5001/requests/{user name}"|
|User can message other users|"clicks user from chathub sidebar"|"localhost:5001/chat/{#user name}"|
|User can video call other users|"clicks 'video call' on navigation bar"|"localhost:5001/video"|

<hr />

## Legal

#### Apache License V2.0

Copyright 2020 Matt Stroud, Dj Sevenbergen, Tyler Bates & KhanSahab Khakwan @ Epicodus

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
