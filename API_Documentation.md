# Consultation Platform API Documentation

## Overview
This document provides comprehensive documentation for the Consultation Platform API, including all endpoints, request/response formats, authentication, and examples.

## Base URL
```
https://your-domain.com/api
```

## Authentication
The API uses JWT Bearer token authentication. Include the token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## Common Response Format
All API responses follow this standardized format:

```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {},
  "errors": [],
  "statusCode": 200
}
```

## Error Responses
```json
{
  "success": false,
  "message": "Error description",
  "errors": ["Detailed error 1", "Detailed error 2"],
  "statusCode": 400
}
```

---

## 1. Authentication Endpoints

### 1.1 User Login
**POST** `/authentication/login`

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "refresh-token-here",
    "expiresIn": 3600,
    "user": {
      "id": "user-id",
      "email": "user@example.com",
      "fullName": "John Doe",
      "role": "Client"
    }
  }
}
```

### 1.2 User Registration
**POST** `/authentication/register`

**Request Body:**
```json
{
  "fullName": "John Doe",
  "userName": "johndoe",
  "email": "john@example.com",
  "phoneNumber": "+1234567890",
  "password": "password123"
}
```

### 1.3 Advisor Registration
**POST** `/authentication/register/advisor`

**Request Body:**
```json
{
  "fullName": "Dr. Jane Smith",
  "specialty": "Family Counseling",
  "description": "Experienced family counselor with 10+ years",
  "zoomRoomUrl": "https://zoom.us/j/123456789",
  "consultationId": 1,
  "email": "jane@example.com",
  "userName": "janesmith",
  "phoneNumber": "+1234567890",
  "address": "123 Main St, City, State",
  "password": "password123"
}
```

### 1.4 Admin Registration
**POST** `/authentication/register/admin`

**Request Body:**
```json
{
  "fullName": "Admin User",
  "email": "admin@example.com",
  "userName": "admin",
  "phoneNumber": "+1234567890",
  "address": "123 Admin St, City, State",
  "password": "admin123"
}
```

### 1.5 Get Current User Profile
**GET** `/authentication/MyProfile`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "user-id",
    "email": "user@example.com",
    "fullName": "John Doe",
    "role": "Client",
    "phoneNumber": "+1234567890",
    "address": "123 Main St"
  }
}
```

---

## 2. User Management Endpoints

### 2.1 Get All Users (Admin Only)
**GET** `/user`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "user-id",
      "email": "user@example.com",
      "fullName": "John Doe",
      "role": "Client",
      "phoneNumber": "+1234567890",
      "address": "123 Main St"
    }
  ]
}
```

### 2.2 Get User by ID
**GET** `/user/{id}`

### 2.3 Update User
**PUT** `/user/{id}`

**Request Body:**
```json
{
  "fullName": "John Updated",
  "phoneNumber": "+1234567890",
  "address": "456 New St"
}
```

### 2.4 Delete User (Admin Only)
**DELETE** `/user/{id}`

---

## 3. Consultation Management Endpoints

### 3.1 Get All Consultations
**GET** `/consultation`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "consultationName": "Family Counseling",
      "description": "Professional family counseling services",
      "isActive": true,
      "advisorCount": 5,
      "requestCount": 25
    }
  ]
}
```

### 3.2 Get Consultation with Related Data
**GET** `/consultation/with-related-data`

### 3.3 Get Consultation by ID
**GET** `/consultation/{id}`

### 3.4 Create Consultation (Admin Only)
**POST** `/consultation`

**Request Body:**
```json
{
  "consultationName": "New Consultation Type",
  "description": "Description of the consultation type"
}
```

### 3.5 Update Consultation
**PUT** `/consultation/{id}`

**Request Body:**
```json
{
  "id": 1,
  "consultationName": "Updated Name",
  "description": "Updated description",
  "isActive": true
}
```

### 3.6 Delete Consultation (Admin Only)
**DELETE** `/consultation/{id}`

---

## 4. Advisor Management Endpoints

### 4.1 Get All Advisors
**GET** `/advisor`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "userId": "advisor-user-id",
      "fullName": "Dr. Jane Smith",
      "specialty": "Family Counseling",
      "description": "Experienced counselor",
      "zoomRoomUrl": "https://zoom.us/j/123456789",
      "consultationId": 1,
      "consultationName": "Family Counseling",
      "isActive": true,
      "totalConsultations": 50,
      "pendingRequests": 3,
      "averageRating": 4.8
    }
  ]
}
```

### 4.2 Get Advisor by ID
**GET** `/advisor/{id}`

### 4.3 Get Advisors by Consultation Type
**GET** `/advisor/consultation/{consultationId}`

### 4.4 Create Advisor (Admin Only)
**POST** `/advisor`

**Request Body:**
```json
{
  "fullName": "Dr. New Advisor",
  "specialty": "Mental Health",
  "description": "Specialized in mental health counseling",
  "zoomRoomUrl": "https://zoom.us/j/987654321",
  "consultationId": 1,
  "email": "advisor@example.com",
  "userName": "newadvisor",
  "phoneNumber": "+1234567890",
  "address": "123 Advisor St",
  "password": "password123"
}
```

### 4.5 Update Advisor
**PUT** `/advisor/{id}`

**Request Body:**
```json
{
  "fullName": "Updated Name",
  "specialty": "Updated Specialty",
  "description": "Updated description",
  "zoomRoomUrl": "https://zoom.us/j/updated",
  "consultationId": 2,
  "isActive": true
}
```

### 4.6 Delete Advisor (Admin Only)
**DELETE** `/advisor/{id}`

---

## 5. Advisor Availability Endpoints

### 5.1 Get Advisor Availability
**GET** `/advisor/{id}/availability`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "advisorId": 1,
      "date": "2024-01-15",
      "startTime": "09:00:00",
      "endTime": "10:00:00",
      "isBooked": false,
      "advisorName": "Dr. Jane Smith"
    }
  ]
}
```

### 5.2 Create Availability (Advisor Only)
**POST** `/advisor/availability`

**Request Body:**
```json
{
  "advisorId": 1,
  "date": "2024-01-15",
  "startTime": "09:00:00",
  "endTime": "10:00:00"
}
```

### 5.3 Create Bulk Availability (Advisor Only)
**POST** `/advisor/availability/bulk`

**Request Body:**
```json
{
  "advisorId": 1,
  "startDate": "2024-01-15",
  "endDate": "2024-01-31",
  "timeSlots": [
    {
      "startTime": "09:00:00",
      "endTime": "10:00:00"
    },
    {
      "startTime": "14:00:00",
      "endTime": "15:00:00"
    }
  ],
  "excludeDays": ["Saturday", "Sunday"]
}
```

### 5.4 Delete Availability (Advisor Only)
**DELETE** `/advisor/availability/{id}`

---

## 6. Consultation Request Endpoints

### 6.1 Get Advisor Requests (Advisor Only)
**GET** `/advisor/{id}/requests`

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "userId": "client-user-id",
      "userName": "John Doe",
      "advisorId": 1,
      "advisorName": "Dr. Jane Smith",
      "consultationId": 1,
      "consultationName": "Family Counseling",
      "appointmentTime": "2024-01-15T09:00:00",
      "notes": "Need help with family issues",
      "status": "Pending",
      "createdAt": "2024-01-10T10:00:00"
    }
  ]
}
```

### 6.2 Update Request Status (Advisor Only)
**PUT** `/advisor/requests/{requestId}/status`

**Request Body:**
```json
"Confirmed"
```

**Status Options:** `Pending`, `Confirmed`, `Completed`, `Cancelled`, `NoShow`

---

## 7. Complaint Management Endpoints

### 7.1 Get All Complaints (Admin Only)
**GET** `/complaint`

### 7.2 Get User Complaints
**GET** `/complaint/user`

### 7.3 Get Complaint by ID
**GET** `/complaint/{id}`

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "userId": "user-id",
    "userName": "John Doe",
    "userEmail": "john@example.com",
    "title": "Service Issue",
    "status": "InProgress",
    "createdAt": "2024-01-10T10:00:00",
    "messageCount": 3,
    "lastMessage": "We're working on it",
    "lastMessageAt": "2024-01-11T15:00:00",
    "messages": [
      {
        "id": 1,
        "complaintId": 1,
        "senderId": "user-id",
        "senderName": "John Doe",
        "messageText": "I have an issue with the service",
        "sentAt": "2024-01-10T10:00:00",
        "isFromAdmin": false
      }
    ]
  }
}
```

### 7.4 Create Complaint
**POST** `/complaint`

**Request Body:**
```json
{
  "title": "Service Issue",
  "message": "I'm experiencing problems with the consultation booking"
}
```

### 7.5 Update Complaint (Admin Only)
**PUT** `/complaint/{id}`

**Request Body:**
```json
{
  "title": "Updated Title",
  "status": "Resolved"
}
```

### 7.6 Delete Complaint (Admin Only)
**DELETE** `/complaint/{id}`

### 7.7 Get Complaint Messages
**GET** `/complaint/{id}/messages`

### 7.8 Add Complaint Message
**POST** `/complaint/{id}/messages`

**Request Body:**
```json
{
  "messageText": "Thank you for your patience. We're working on resolving this issue."
}
```

### 7.9 Update Complaint Status (Admin Only)
**PUT** `/complaint/{id}/status`

**Request Body:**
```json
"Resolved"
```

**Status Options:** `Pending`, `InProgress`, `Resolved`, `Closed`

### 7.10 Get Complaint Statistics (Admin Only)
**GET** `/complaint/statistics`

---

## 8. Volunteer Application Endpoints

### 8.1 Get All Applications (Admin Only)
**GET** `/volunteer`

### 8.2 Get User Application
**GET** `/volunteer/user`

### 8.3 Get Application by ID (Admin Only)
**GET** `/volunteer/{id}`

### 8.4 Create Application
**POST** `/volunteer`

**Request Body:**
```json
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "phoneNumber": "+1234567890",
  "areaOfInterest": "Community Service",
  "notes": "I want to help the community"
}
```

### 8.5 Update Application
**PUT** `/volunteer/{id}`

**Request Body:**
```json
{
  "fullName": "John Updated",
  "email": "john.updated@example.com",
  "phoneNumber": "+1234567890",
  "areaOfInterest": "Updated Interest",
  "notes": "Updated notes"
}
```

### 8.6 Delete Application (Admin Only)
**DELETE** `/volunteer/{id}`

### 8.7 Review Application (Admin Only)
**PUT** `/volunteer/{id}/review`

**Request Body:**
```json
{
  "status": "Approved",
  "reviewNotes": "Great candidate, approved for volunteer position"
}
```

**Status Options:** `Pending`, `Approved`, `Rejected`, `UnderReview`

### 8.8 Get Applications by Status (Admin Only)
**GET** `/volunteer/status/{status}`

### 8.9 Get Volunteer Statistics (Admin Only)
**GET** `/volunteer/statistics`

---

## 9. News Management Endpoints

### 9.1 Get All News
**GET** `/news`

### 9.2 Get Active News
**GET** `/news/active`

### 9.3 Get News by ID
**GET** `/news/{id}`

### 9.4 Create News (Admin Only)
**POST** `/news`

**Request Body:**
```json
{
  "title": "New Service Available",
  "content": "We're excited to announce our new counseling service...",
  "imageUrl": "https://example.com/image.jpg",
  "publishDate": "2024-01-15T10:00:00",
  "isActive": true
}
```

### 9.5 Update News (Admin Only)
**PUT** `/news/{id}`

**Request Body:**
```json
{
  "title": "Updated Title",
  "content": "Updated content...",
  "imageUrl": "https://example.com/updated-image.jpg",
  "publishDate": "2024-01-15T10:00:00",
  "isActive": true
}
```

### 9.6 Delete News (Admin Only)
**DELETE** `/news/{id}`

### 9.7 Increment View Count
**PUT** `/news/{id}/view`

### 9.8 Get News Statistics (Admin Only)
**GET** `/news/statistics`

---

## 10. Service Offering Endpoints

### 10.1 Get All Services
**GET** `/service`

### 10.2 Get Active Services
**GET** `/service/active`

### 10.3 Get Service by ID
**GET** `/service/{id}`

### 10.4 Create Service (Admin Only)
**POST** `/service`

**Request Body:**
```json
{
  "title": "Family Counseling Service",
  "description": "Professional family counseling services",
  "imageUrl": "https://example.com/service-image.jpg",
  "redirectUrl": "https://example.com/family-counseling",
  "isActive": true
}
```

### 10.5 Update Service (Admin Only)
**PUT** `/service/{id}`

**Request Body:**
```json
{
  "title": "Updated Service Title",
  "description": "Updated service description",
  "imageUrl": "https://example.com/updated-image.jpg",
  "redirectUrl": "https://example.com/updated-service",
  "isActive": true
}
```

### 10.6 Delete Service (Admin Only)
**DELETE** `/service/{id}`

### 10.7 Increment Click Count
**PUT** `/service/{id}/click`

---

## HTTP Status Codes

- **200** - Success
- **201** - Created
- **400** - Bad Request
- **401** - Unauthorized
- **403** - Forbidden
- **404** - Not Found
- **500** - Internal Server Error

## Rate Limiting

The API implements rate limiting:
- **100 requests per minute** for authenticated users
- **20 requests per minute** for unauthenticated users

## Pagination

For endpoints that return lists, pagination is supported:

**Query Parameters:**
- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 10, max: 100)

**Response Format:**
```json
{
  "success": true,
  "data": {
    "items": [...],
    "totalCount": 100,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 10,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

## Error Handling

The API provides detailed error messages:

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    "Email is required",
    "Password must be at least 6 characters"
  ],
  "statusCode": 400
}
```

## Security Considerations

1. **JWT Tokens** - All authenticated endpoints require valid JWT tokens
2. **Role-Based Access** - Different endpoints require specific user roles
3. **Input Validation** - All inputs are validated server-side
4. **HTTPS Only** - API should be accessed over HTTPS in production
5. **Rate Limiting** - Prevents abuse and ensures fair usage

## Testing

Use the provided `Charity_BE.http` file for testing endpoints with tools like VS Code REST Client or Postman.

## Support

For API support and questions, contact the development team or refer to the project documentation. 