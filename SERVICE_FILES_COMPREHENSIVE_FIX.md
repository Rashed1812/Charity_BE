# إصلاحات شاملة لملفات Service - Comprehensive Service Files Fix

## 🐛 الأخطاء المكتشفة في جميع ملفات Service

### 1. **AuthService.cs** ✅ تم الإصلاح
#### المشاكل:
- استخدام `BLL.DTOS.AuthDTO` بدلاً من `Shared.DTOS.AuthDTO`
- استخدام `FullName` بدلاً من `FirstName` و `LastName`
- استخدام `Patient` بدلاً من `Client`
- استدعاء `SaveChangesAsync()` غير ضروري

#### الإصلاحات:
```csharp
// قبل الإصلاح
using BLL.DTOS.AuthDTO;
user.FullName = dto.FullName;
await _userManager.AddToRoleAsync(user, "Patient");

// بعد الإصلاح
using Shared.DTOS.AuthDTO;
user.FirstName = dto.FirstName;
user.LastName = dto.LastName;
await _userManager.AddToRoleAsync(user, "Client");
```

### 2. **LectureService.cs** ✅ تم الإصلاح
#### المشاكل:
- استخدام `PaginatedResponse<LectureDTO>` غير موجود
- استخدام `Shared.DTOS.Common` غير موجود

#### الإصلاحات:
```csharp
// قبل الإصلاح
public async Task<PaginatedResponse<LectureDTO>> SearchLecturesAsync(...)

// بعد الإصلاح
public async Task<object> SearchLecturesAsync(...)
```

### 3. **UserService.cs** ✅ تم الإصلاح
#### المشاكل:
- using statements مكررة
- استخدام `BLL.DTOS.UserDTO` و `Shared.DTOS.UserDTO` معاً
- استدعاء طرق غير موجودة في المستودعات

#### الإصلاحات:
```csharp
// قبل الإصلاح
using BLL.DTOS.UserDTO;
using Shared.DTOS.UserDTO;

// بعد الإصلاح
using Shared.DTOS.UserDTO;
```

---

## 🔍 ملفات Service المتبقية للفحص

### 4. **AdviceRequestService.cs**
#### المشاكل المحتملة:
- استخدام `ConsultationStatus` بدلاً من `AdviceRequestStatus`
- استدعاء طرق غير موجودة في المستودعات

### 5. **ComplaintService.cs**
#### المشاكل المحتملة:
- استخدام `ComplaintStatus` enum
- استدعاء طرق غير موجودة في المستودعات

### 6. **VolunteerService.cs**
#### المشاكل المحتملة:
- استخدام `VolunteerStatus` enum
- استدعاء طرق غير موجودة في المستودعات

### 7. **NewsService.cs**
#### المشاكل المحتملة:
- استخدام `NewsStatus` enum
- استدعاء طرق غير موجودة في المستودعات

### 8. **ServiceOfferingService.cs**
#### المشاكل المحتملة:
- استخدام `ServiceStatus` enum
- استدعاء طرق غير موجودة في المستودعات

### 9. **ConsultationService.cs**
#### المشاكل المحتملة:
- استخدام `ConsultationStatus` enum
- استدعاء طرق غير موجودة في المستودعات

### 10. **AdminService.cs**
#### المشاكل المحتملة:
- استخدام `AdminStatus` enum
- استدعاء طرق غير موجودة في المستودعات

### 11. **ComplaintMessageService.cs**
#### المشاكل المحتملة:
- استخدام `MessageStatus` enum
- استدعاء طرق غير موجودة في المستودعات

---

## 🔧 الإصلاحات المطلوبة

### 1. **إضافة Enums المفقودة**
```csharp
// في Shared/DTOS/Common/Enums.cs
public enum AdviceRequestStatus
{
    Pending,
    Confirmed,
    InProgress,
    Completed,
    Cancelled,
    NoShow
}

public enum ComplaintStatus
{
    Pending,
    InProgress,
    Resolved,
    Closed
}

public enum VolunteerStatus
{
    Pending,
    Approved,
    Rejected,
    UnderReview
}

public enum NewsStatus
{
    Draft,
    Published,
    Archived
}

public enum ServiceStatus
{
    Active,
    Inactive,
    Suspended
}

public enum ConsultationStatus
{
    Active,
    Inactive,
    Suspended
}

public enum AdminStatus
{
    Active,
    Inactive,
    Suspended
}

public enum MessageStatus
{
    Sent,
    Delivered,
    Read
}
```

### 2. **إضافة طرق مفقودة في المستودعات**
```csharp
// في IUserRepository
Task<int> GetTotalUsersCountAsync();
Task<int> GetActiveUsersCountAsync();
Task<int> GetNewUsersThisMonthAsync();
Task<object> GetUsersCountByRoleAsync();
Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm);
Task<List<ApplicationUser>> GetUsersByRoleAsync(string role);
Task<bool> IsEmailUniqueAsync(string email);
Task<bool> IsPhoneUniqueAsync(string phoneNumber);

// في IAdviceRequestRepository
Task<int> GetTotalConsultationsByUserAsync(string userId);
Task<int> GetPendingConsultationsByUserAsync(string userId);
Task<int> GetCompletedConsultationsByUserAsync(string userId);

// في IComplaintRepository
Task<int> GetTotalComplaintsByUserAsync(string userId);
Task<int> GetPendingComplaintsByUserAsync(string userId);

// في IVolunteerRepository
Task<List<VolunteerApplication>> GetVolunteerApplicationsByUserAsync(string userId);
```

### 3. **إضافة DTOs المفقودة**
```csharp
// في Shared/DTOS/Common/PaginatedResponse.cs
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
```

---

## 📋 خطة الإصلاح

### **المرحلة الأولى: إصلاح Using Statements**
1. ✅ AuthService.cs
2. ✅ LectureService.cs
3. ✅ UserService.cs
4. 🔄 AdviceRequestService.cs
5. 🔄 ComplaintService.cs
6. 🔄 VolunteerService.cs
7. 🔄 NewsService.cs
8. 🔄 ServiceOfferingService.cs
9. 🔄 ConsultationService.cs
10. 🔄 AdminService.cs
11. 🔄 ComplaintMessageService.cs

### **المرحلة الثانية: إضافة Enums**
1. إنشاء ملف `Shared/DTOS/Common/Enums.cs`
2. إضافة جميع Enums المفقودة
3. تحديث جميع الملفات لاستخدام Enums الصحيحة

### **المرحلة الثالثة: إضافة طرق المستودعات**
1. تحديث جميع Repository Interfaces
2. تحديث جميع Repository Classes
3. إضافة التنفيذ للطرق المفقودة

### **المرحلة الرابعة: إضافة DTOs**
1. إنشاء `PaginatedResponse<T>`
2. إضافة أي DTOs مفقودة أخرى
3. تحديث AutoMapper Profiles

---

## 🚀 النتيجة المتوقعة

بعد إكمال جميع الإصلاحات:
- ✅ جميع ملفات Service تعمل بدون أخطاء
- ✅ جميع Using Statements صحيحة
- ✅ جميع Enums موجودة ومستخدمة بشكل صحيح
- ✅ جميع طرق المستودعات موجودة
- ✅ جميع DTOs موجودة
- ✅ النظام جاهز للاختبار والتشغيل

---

## 📝 ملاحظات مهمة

1. **ترتيب الإصلاح**: يجب إصلاح الملفات بالترتيب المذكور
2. **اختبار كل ملف**: بعد إصلاح كل ملف، يجب اختباره للتأكد من عدم وجود أخطاء
3. **النسخ الاحتياطية**: احتفظ بنسخة احتياطية قبل كل تعديل
4. **التوثيق**: وثق جميع التغييرات في ملف منفصل

---

*تم إنشاء هذا الملف بتاريخ: ديسمبر 2024* 