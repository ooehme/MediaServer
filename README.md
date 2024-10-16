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
curl http://{ServerIP}:{ServerPORT}/api/media/music/artist/TheBeatles
```

Retrieve music files from a specific album
```
curl http://{ServerIP}:{ServerPORT}/api/media/music/album/AbbeyRoad
```

Play a specific music track
```
curl http://{ServerIP}:{ServerPORT}/api/media/music/play/LetItBe
```

Retrieve all video files
```
curl http://{ServerIP}:{ServerPORT}/api/media/video
```

Retrieve video files by a specific director
```
curl http://{ServerIP}:{ServerPORT}/api/media/video/director/ChristopherNolan
```

Retrieve video files by a specific genre
```
curl http://{ServerIP}:{ServerPORT}/api/media/video/genre/Action
```

Play a specific video title
```
curl http://{ServerIP}:{ServerPORT}/api/media/video/play/Inception
```
