# Lecture Management API Documentation

## Overview
This document provides comprehensive documentation for the Lecture Management API, including all endpoints for managing lectures, video uploads, and external links.

## Base URL
```
https://your-domain.com/api/lecture
```

## Authentication
All endpoints require JWT Bearer token authentication except where noted:
```
Authorization: Bearer <your-jwt-token>
```

## Lecture Types
- **ExternalLink (0)**: External video links (YouTube, Vimeo, etc.)
- **UploadedVideo (1)**: Videos uploaded directly to the platform
- **YouTubeVideo (2)**: YouTube video links
- **VimeoVideo (3)**: Vimeo video links

---

## 1. Get All Lectures
**GET** `/api/lecture`

**Description:** Retrieve all lectures (admin only)

**Authorization:** Admin

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "title": "مقدمة في الاستشارات الأسرية",
      "description": "محاضرة شاملة عن أساسيات الاستشارات الأسرية",
      "type": 1,
      "externalUrl": null,
      "videoFilePath": "lecture_1.mp4",
      "videoFileName": "lecture_1.mp4",
      "videoFileSize": 52428800,
      "thumbnailUrl": "/images/thumbnails/lecture_1.jpg",
      "durationMinutes": 45,
      "consultationId": 1,
      "consultationName": "الاستشارات الأسرية",
      "createdBy": "admin-id",
      "createdByName": "المدير",
      "isActive": true,
      "isPublished": true,
      "createdAt": "2024-01-15T10:00:00Z",
      "updatedAt": null,
      "publishedAt": "2024-01-15T10:00:00Z",
      "viewCount": 150,
      "downloadCount": 25,
      "tags": "استشارات,أسرة,تربية",
      "parentLectureId": null,
      "parentLectureTitle": null,
      "childLectures": []
    }
  ]
}
```

---

## 2. Get Published Lectures
**GET** `/api/lecture/published`

**Description:** Retrieve only published lectures (public access)

**Authorization:** None required

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "title": "مقدمة في الاستشارات الأسرية",
      "description": "محاضرة شاملة عن أساسيات الاستشارات الأسرية",
      "type": 1,
      "thumbnailUrl": "/images/thumbnails/lecture_1.jpg",
      "durationMinutes": 45,
      "viewCount": 150,
      "isPublished": true,
      "createdAt": "2024-01-15T10:00:00Z"
    }
  ]
}
```

---

## 3. Get Lecture by ID
**GET** `/api/lecture/{id}`

**Description:** Retrieve a specific lecture by ID

**Authorization:** None required

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "title": "مقدمة في الاستشارات الأسرية",
    "description": "محاضرة شاملة عن أساسيات الاستشارات الأسرية",
    "type": 1,
    "externalUrl": null,
    "videoFilePath": "lecture_1.mp4",
    "videoFileName": "lecture_1.mp4",
    "videoFileSize": 52428800,
    "thumbnailUrl": "/images/thumbnails/lecture_1.jpg",
    "durationMinutes": 45,
    "consultationId": 1,
    "consultationName": "الاستشارات الأسرية",
    "createdBy": "admin-id",
    "createdByName": "المدير",
    "isActive": true,
    "isPublished": true,
    "createdAt": "2024-01-15T10:00:00Z",
    "updatedAt": null,
    "publishedAt": "2024-01-15T10:00:00Z",
    "viewCount": 150,
    "downloadCount": 25,
    "tags": "استشارات,أسرة,تربية",
    "parentLectureId": null,
    "parentLectureTitle": null,
    "childLectures": []
  }
}
```

---

## 4. Create Lecture (External Link)
**POST** `/api/lecture`

**Description:** Create a new lecture with external link

**Authorization:** Admin

**Request Body:**
```json
{
  "title": "محاضرة عن التربية الإيجابية",
  "description": "محاضرة شاملة عن أساليب التربية الإيجابية للأطفال",
  "type": 0,
  "externalUrl": "https://www.youtube.com/watch?v=example",
  "thumbnailUrl": "https://img.youtube.com/vi/example/maxresdefault.jpg",
  "durationMinutes": 60,
  "consultationId": 1,
  "tags": "تربية,أطفال,إيجابية",
  "parentLectureId": null,
  "isPublished": true
}
```

**Response:**
```json
{
  "success": true,
  "message": "تم إنشاء المحاضرة بنجاح",
  "data": {
    "id": 2,
    "title": "محاضرة عن التربية الإيجابية",
    "type": 0,
    "externalUrl": "https://www.youtube.com/watch?v=example",
    "isPublished": true,
    "createdAt": "2024-01-15T11:00:00Z"
  }
}
```

---

## 5. Upload Video Lecture
**POST** `/api/lecture/upload`

**Description:** Upload a video file and create a lecture

**Authorization:** Admin

**Request Body:** (multipart/form-data)
```
videoFile: [video file]
title: "محاضرة عن حل المشاكل الأسرية"
description: "محاضرة شاملة عن كيفية حل المشاكل الأسرية"
thumbnailUrl: "https://example.com/thumbnail.jpg"
durationMinutes: 90
consultationId: 1
tags: "مشاكل,أسرة,حلول"
parentLectureId: null
isPublished: false
```

**Supported Video Formats:**
- MP4 (.mp4)
- AVI (.avi)
- MOV (.mov)
- WMV (.wmv)
- FLV (.flv)
- WebM (.webm)

**Maximum File Size:** 500MB

**Response:**
```json
{
  "success": true,
  "message": "تم رفع الفيديو بنجاح",
  "data": {
    "id": 3,
    "title": "محاضرة عن حل المشاكل الأسرية",
    "type": 1,
    "videoFilePath": "guid_filename.mp4",
    "videoFileName": "lecture_video.mp4",
    "videoFileSize": 104857600,
    "isPublished": false,
    "createdAt": "2024-01-15T12:00:00Z"
  }
}
```

---

## 6. Update Lecture
**PUT** `/api/lecture/{id}`

**Description:** Update an existing lecture

**Authorization:** Admin

**Parameters:**
- `id` (int): Lecture ID

**Request Body:**
```json
{
  "title": "محاضرة محدثة عن التربية الإيجابية",
  "description": "وصف محدث للمحاضرة",
  "externalUrl": "https://www.youtube.com/watch?v=updated",
  "thumbnailUrl": "https://example.com/updated-thumbnail.jpg",
  "durationMinutes": 75,
  "consultationId": 2,
  "tags": "تربية,أطفال,إيجابية,محدث",
  "isPublished": true
}
```

**Response:**
```json
{
  "success": true,
  "message": "تم تحديث المحاضرة بنجاح",
  "data": {
    "id": 2,
    "title": "محاضرة محدثة عن التربية الإيجابية",
    "updatedAt": "2024-01-15T13:00:00Z"
  }
}
```

---

## 7. Delete Lecture
**DELETE** `/api/lecture/{id}`

**Description:** Delete a lecture and its associated files

**Authorization:** Admin

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "message": "تم حذف المحاضرة بنجاح",
  "data": true
}
```

---

## 8. Publish Lecture
**PUT** `/api/lecture/{id}/publish`

**Description:** Publish a draft lecture

**Authorization:** Admin

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "message": "تم نشر المحاضرة بنجاح",
  "data": {
    "id": 3,
    "isPublished": true,
    "publishedAt": "2024-01-15T14:00:00Z"
  }
}
```

---

## 9. Unpublish Lecture
**PUT** `/api/lecture/{id}/unpublish`

**Description:** Unpublish a published lecture

**Authorization:** Admin

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "message": "تم إلغاء نشر المحاضرة بنجاح",
  "data": {
    "id": 3,
    "isPublished": false,
    "publishedAt": null
  }
}
```

---

## 10. Search Lectures
**GET** `/api/lecture/search`

**Description:** Search and filter lectures with pagination

**Authorization:** None required

**Query Parameters:**
- `searchTerm` (string, optional): Search in title, description, and tags
- `consultationId` (int, optional): Filter by consultation type
- `type` (int, optional): Filter by lecture type
- `isPublished` (bool, optional): Filter by publication status
- `tags` (string, optional): Filter by tags
- `fromDate` (datetime, optional): Filter from date
- `toDate` (datetime, optional): Filter to date
- `pageNumber` (int, default: 1): Page number
- `pageSize` (int, default: 10): Items per page

**Example Request:**
```
GET /api/lecture/search?searchTerm=تربية&consultationId=1&isPublished=true&pageNumber=1&pageSize=5
```

**Response:**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": 1,
        "title": "محاضرة عن التربية الإيجابية",
        "type": 0,
        "thumbnailUrl": "/images/thumbnails/lecture_1.jpg",
        "durationMinutes": 60,
        "viewCount": 150,
        "isPublished": true,
        "createdAt": "2024-01-15T10:00:00Z"
      }
    ],
    "totalCount": 25,
    "pageNumber": 1,
    "pageSize": 5,
    "totalPages": 5,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

---

## 11. Get Lectures by Consultation
**GET** `/api/lecture/consultation/{consultationId}`

**Description:** Get all lectures for a specific consultation type

**Authorization:** None required

**Parameters:**
- `consultationId` (int): Consultation type ID

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "title": "محاضرة عن التربية الإيجابية",
      "type": 0,
      "consultationId": 1,
      "consultationName": "الاستشارات الأسرية",
      "isPublished": true
    }
  ]
}
```

---

## 12. Get Lectures by Type
**GET** `/api/lecture/type/{type}`

**Description:** Get all lectures of a specific type

**Authorization:** None required

**Parameters:**
- `type` (int): Lecture type (0=ExternalLink, 1=UploadedVideo, 2=YouTubeVideo, 3=VimeoVideo)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 2,
      "title": "محاضرة على يوتيوب",
      "type": 2,
      "externalUrl": "https://www.youtube.com/watch?v=example",
      "isPublished": true
    }
  ]
}
```

---

## 13. Get Related Lectures
**GET** `/api/lecture/{id}/related`

**Description:** Get lectures related to a specific lecture

**Authorization:** None required

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 4,
      "title": "محاضرة ذات صلة",
      "type": 1,
      "consultationId": 1,
      "viewCount": 75,
      "isPublished": true
    }
  ]
}
```

---

## 14. Increment View Count
**PUT** `/api/lecture/{id}/view`

**Description:** Increment the view count for a lecture

**Authorization:** None required

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "message": "تم تحديث عدد المشاهدات",
  "data": true
}
```

---

## 15. Increment Download Count
**PUT** `/api/lecture/{id}/download`

**Description:** Increment the download count for a lecture

**Authorization:** None required

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "message": "تم تحديث عدد التحميلات",
  "data": true
}
```

---

## 16. Get Video Stream URL
**GET** `/api/lecture/{id}/stream`

**Description:** Get the streaming URL for an uploaded video

**Authorization:** None required

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "data": "/uploads/lectures/guid_filename.mp4"
}
```

---

## 17. Get Lecture Statistics
**GET** `/api/lecture/statistics`

**Description:** Get overall lecture statistics

**Authorization:** Admin

**Response:**
```json
{
  "success": true,
  "data": {
    "totalLectures": 50,
    "publishedLectures": 35,
    "draftLectures": 15,
    "externalLinks": 20,
    "uploadedVideos": 30,
    "publishedPercentage": 70.0
  }
}
```

---

## 18. Get Lecture Statistics by Consultation
**GET** `/api/lecture/statistics/consultation/{consultationId}`

**Description:** Get lecture statistics for a specific consultation type

**Authorization:** Admin

**Parameters:**
- `consultationId` (int): Consultation type ID

**Response:**
```json
{
  "success": true,
  "data": {
    "totalLectures": 15,
    "publishedLectures": 12,
    "draftLectures": 3,
    "totalViews": 2500,
    "totalDownloads": 450,
    "averageViews": 166.67,
    "averageDownloads": 30.0
  }
}
```

---

## 19. Generate Thumbnail
**POST** `/api/lecture/{id}/thumbnail`

**Description:** Generate a thumbnail for a video lecture

**Authorization:** Admin

**Parameters:**
- `id` (int): Lecture ID

**Response:**
```json
{
  "success": true,
  "message": "تم إنشاء الصورة المصغرة بنجاح",
  "data": "/images/thumbnails/lecture_1.jpg"
}
```

---

## 20. Update Thumbnail
**PUT** `/api/lecture/{id}/thumbnail`

**Description:** Update the thumbnail for a lecture

**Authorization:** Admin

**Parameters:**
- `id` (int): Lecture ID

**Request Body:**
```json
"https://example.com/new-thumbnail.jpg"
```

**Response:**
```json
{
  "success": true,
  "message": "تم تحديث الصورة المصغرة بنجاح",
  "data": true
}
```

---

## Error Responses

### Validation Error
```json
{
  "success": false,
  "message": "بيانات غير صحيحة",
  "errors": [
    "عنوان المحاضرة مطلوب",
    "يرجى إدخال رابط صحيح"
  ],
  "statusCode": 400
}
```

### File Upload Error
```json
{
  "success": false,
  "message": "ملف الفيديو غير صالح",
  "statusCode": 400
}
```

### Not Found Error
```json
{
  "success": false,
  "message": "المحاضرة برقم 1 غير موجودة",
  "statusCode": 404
}
```

### Unauthorized Error
```json
{
  "success": false,
  "message": "المدير غير مصادق عليه",
  "statusCode": 401
}
```

---

## File Upload Guidelines

### Supported Video Formats
- **MP4** (.mp4) - Recommended
- **AVI** (.avi)
- **MOV** (.mov)
- **WMV** (.wmv)
- **FLV** (.flv)
- **WebM** (.webm)

### File Size Limits
- **Maximum Size:** 500MB per video
- **Recommended Size:** Under 200MB for better streaming

### Video Quality Recommendations
- **Resolution:** 720p or 1080p
- **Frame Rate:** 24-30 fps
- **Codec:** H.264 for MP4
- **Audio:** AAC or MP3

### Thumbnail Guidelines
- **Format:** JPG or PNG
- **Size:** 1280x720 pixels (16:9 aspect ratio)
- **File Size:** Under 2MB

---

## Best Practices

### For External Links
1. Use official embed URLs from YouTube/Vimeo
2. Include proper thumbnails
3. Set accurate duration
4. Add relevant tags for search

### For Uploaded Videos
1. Compress videos before upload
2. Use MP4 format for best compatibility
3. Generate thumbnails automatically
4. Set proper metadata

### For Content Management
1. Organize lectures by consultation types
2. Use consistent naming conventions
3. Add comprehensive descriptions
4. Tag lectures appropriately
5. Review content before publishing

---

## Security Considerations

1. **File Upload Security**
   - Validate file types and sizes
   - Scan uploaded files for malware
   - Store files outside web root
   - Use secure file naming

2. **Access Control**
   - Admin-only upload and management
   - Public read access for published content
   - Role-based permissions

3. **Content Protection**
   - Watermark videos if needed
   - Implement download restrictions
   - Monitor usage patterns

---

## Performance Optimization

1. **Video Streaming**
   - Use adaptive bitrate streaming
   - Implement video compression
   - Cache frequently accessed videos

2. **Thumbnail Generation**
   - Generate thumbnails on upload
   - Cache generated thumbnails
   - Use CDN for static assets

3. **Database Optimization**
   - Index frequently queried fields
   - Implement pagination
   - Use efficient queries

This comprehensive API provides full lecture management capabilities with support for both external links and uploaded videos, complete with search, statistics, and content management features. 