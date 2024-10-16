# MediaServer
A simple .NET-based server for streaming local media to connected devices. Stream your personal media library to any compatible device in your network.

## Configuration
Enter the location of your media files in the MediaSettings section in appsettings.json:
```javascript ()
"MediaSettings": {
    "MediaDirectory": "C:\\PATH\\TO\\YOUR\\MEDIA"
}
```

## Requests
Retrieve all music files
```
curl http://{ServerIP}:{ServerPORT}/api/media/music
```

Retrieve music files by a specific artist
```
curl http://{ServerIP}:{ServerPORT}/api/media/music/artist/Left%20Boy
```

Retrieve music files from a specific album
```
curl http://{ServerIP}:{ServerPORT}/api/media/music/album/Guns%20Bitches%20and%Weed
```

Play a specific music track
```
curl http://{ServerIP}:{ServerPORT}/api/media/music/play/Health%20Ego
```

Retrieve all video files
```
curl http://{ServerIP}:{ServerPORT}/api/media/video
```

Retrieve video files by a specific director
```
curl http://{ServerIP}:{ServerPORT}/api/media/video/director/Stanley%20Kubrick
```

Retrieve video files by a specific genre
```
curl http://{ServerIP}:{ServerPORT}/api/media/video/genre/Dark%20Comedy
```

Play a specific video title
```
curl http://{ServerIP}:{ServerPORT}/api/media/video/play/Dr.%20Strangelove%20or%20How%20I%20Learned%20to%20Stop%20Worrying%20and%20Love%20the%20Bomb
```
