masterserver/test/ - Returns "NO"
masterserver/Test/GetTemplate - Returns a sample json of the layout to make a server
masterserver/simple/secret/{secret}/ - Returns simple info on a server from master server
masterserver/simple/code/{code}/ - Returns simple info on a server from master server
masterserver/advanced/secret/{secret}/ - Returns advanced info on a server from dedicated server
masterserver/GetList/Secret/all/{AccessToken}/ - Returns a list of every servers secret
masterserver/GetList/Secret/public/ - Returns a list of every public servers secret
masterserver/GetList/simpleserver/all/{AccessToken} - Returns a list of every servers simple info
masterserver/GetList/simpleserver/public/ - Returns a list of every public servers simple info
masterserver/GetPublicServerCount/ - Returns the amount of public servers
masterserver/GetServerCount/ - Returns the amount of servers
masterserver/Getserver/advanced/secret/{secret}/players/simple/ - Returns a list of simple player infomation from a server
masterserver/Getserver/advanced/secret/{secret}/players/advanced/{AccessToken}/ - Returns a list of advanced player infomation from a server
masterserver/Getserver/advanced/secret/{secret}/players/advanced/{PlayerId}/{AccessToken}/ - Returns advanced player infomation of a single player from a server
masterserver/Getserver/advanced/secret/{secret}/players/kick/{PlayerId}/{AccessToken}/ - Kicks the player from the selected server
masterserver/CreateServer/ - Creates a server - CANNOT BE PERMANENT - TIMEOUT CANNOT BE OVER 15 MIN
masterserver/CreateAdvancedServer/{AccessToken}/ - Creates a server - CAN BE PERMANENT - TIMEOUT CAN BE ANY VALUE ABOVE 0 - CAN HAVE CUSTOM CODE AND SECRET
masterserver/RemoveServer/code/{AccessToken}/ - Will close a server
masterserver/RemoveServer/secret/{AccessToken}/ - Will close a server
masterserver/Getserver/{secret}/SetBeatmap/{AccessToken}/ - Will set the beatmap, modifiers and countdown on a server(This is required to be used on a tournament server as the players cannot choose)

status/ - Returns status
status/status/mp_override.json - Returns Mp overrides
