# منصة الاستشارات الخيرية - Charity Consultation Platform

## نظرة عامة
منصة استشارات خيرية متكاملة تتيح للمستخدمين التواصل مع المستشارين المتخصصين في مجالات مختلفة، مع إدارة شاملة للمحتوى والخدمات.

## الميزات الرئيسية

### 👥 إدارة المستخدمين
- **المديرين (Admins)**: إدارة شاملة للنظام والمستخدمين
- **المستشارين (Advisors)**: تقديم الاستشارات في مجالات متخصصة
- **العملاء (Clients)**: طلب الاستشارات والتفاعل مع المستشارين

### 📋 الاستشارات
- إدارة أنواع الاستشارات المختلفة
- جدولة مواعيد المستشارين
- تتبع حالة الطلبات
- تقييم الاستشارات

### 🎥 المحاضرات
- رفع وإدارة المحاضرات المرئية
- تصنيف المحاضرات حسب النوع
- إحصائيات المشاهدة والتحميل
- دعم أنواع مختلفة من الملفات

### 📰 الأخبار والخدمات
- نشر وإدارة الأخبار
- عرض الخدمات المقدمة
- إحصائيات التفاعل

### 📞 الشكاوى والطلبات
- نظام شكاوى متكامل
- طلبات النصيحة
- تتبع حالة الطلبات

### 🤝 التطوع
- طلبات التطوع
- مراجعة وإدارة الطلبات
- إحصائيات التطوع

## التقنيات المستخدمة

### Backend
- **ASP.NET Core 7.0** - إطار العمل الرئيسي
- **Entity Framework Core** - ORM للتعامل مع قاعدة البيانات
- **Identity Framework** - إدارة المستخدمين والصلاحيات
- **JWT Authentication** - المصادقة والتفويض
- **AutoMapper** - تحويل البيانات
- **Swagger/OpenAPI** - توثيق API

### Database
- **SQL Server** - قاعدة البيانات الرئيسية
- **Entity Framework Migrations** - إدارة التحديثات

### Architecture
- **Repository Pattern** - نمط المستودع
- **Service Layer** - طبقة الخدمات
- **DTO Pattern** - نقل البيانات
- **Dependency Injection** - حقن التبعيات

## هيكل المشروع

```
Charity_BE/
├── BLL/                          # Business Logic Layer
│   ├── Service/                  # Service Implementations
│   ├── ServiceAbstraction/       # Service Interfaces
│   └── Mapping/                  # AutoMapper Profiles
├── DAL/                          # Data Access Layer
│   ├── Data/
│   │   ├── Models/               # Entity Models
│   │   ├── Migrations/           # EF Migrations
│   │   └── DataSeed/             # Seed Data
│   └── Repositories/             # Repository Pattern
├── Shared/                       # Shared DTOs
│   └── DTOS/                     # Data Transfer Objects
└── Charity_BE/                   # Web API Project
    ├── Controllers/              # API Controllers
    ├── Program.cs                # Application Configuration
    └── appsettings.json          # Configuration Settings
```

## API Endpoints

### Authentication
- `POST /api/authentication/login` - تسجيل الدخول
- `POST /api/authentication/register` - تسجيل مستخدم جديد
- `POST /api/authentication/register-admin` - تسجيل مدير
- `POST /api/authentication/register-advisor` - تسجيل مستشار
- `GET /api/authentication/me` - معلومات المستخدم الحالي

### Users Management
- `GET /api/user` - جميع المستخدمين
- `GET /api/user/{id}` - مستخدم محدد
- `PUT /api/user/{id}` - تحديث مستخدم
- `DELETE /api/user/{id}` - حذف مستخدم
- `POST /api/user/{id}/activate` - تفعيل مستخدم
- `POST /api/user/{id}/deactivate` - إلغاء تفعيل مستخدم

### Advisors
- `GET /api/advisor` - جميع المستشارين
- `GET /api/advisor/{id}` - مستشار محدد
- `POST /api/advisor` - إنشاء مستشار
- `PUT /api/advisor/{id}` - تحديث مستشار
- `DELETE /api/advisor/{id}` - حذف مستشار
- `GET /api/advisor/{id}/availability` - أوقات توفر المستشار
- `POST /api/advisor/availability` - إضافة وقت توفر

### Consultations
- `GET /api/consultation` - جميع الاستشارات
- `GET /api/consultation/active` - الاستشارات النشطة
- `GET /api/consultation/{id}` - استشارة محددة
- `POST /api/consultation` - إنشاء استشارة
- `PUT /api/consultation/{id}` - تحديث استشارة
- `DELETE /api/consultation/{id}` - حذف استشارة

### Lectures
- `GET /api/lecture` - جميع المحاضرات
- `GET /api/lecture/published` - المحاضرات المنشورة
- `GET /api/lecture/{id}` - محاضرة محددة
- `POST /api/lecture` - إنشاء محاضرة
- `POST /api/lecture/upload` - رفع فيديو
- `PUT /api/lecture/{id}` - تحديث محاضرة
- `DELETE /api/lecture/{id}` - حذف محاضرة
- `PUT /api/lecture/{id}/publish` - نشر محاضرة
- `GET /api/lecture/search` - البحث في المحاضرات

### News
- `GET /api/news` - جميع الأخبار
- `GET /api/news/active` - الأخبار النشطة
- `GET /api/news/{id}` - خبر محدد
- `POST /api/news` - إنشاء خبر
- `PUT /api/news/{id}` - تحديث خبر
- `DELETE /api/news/{id}` - حذف خبر
- `PUT /api/news/{id}/view` - زيادة عدد المشاهدات

### Services
- `GET /api/serviceoffering` - جميع الخدمات
- `GET /api/serviceoffering/active` - الخدمات النشطة
- `GET /api/serviceoffering/{id}` - خدمة محددة
- `POST /api/serviceoffering` - إنشاء خدمة
- `PUT /api/serviceoffering/{id}` - تحديث خدمة
- `DELETE /api/serviceoffering/{id}` - حذف خدمة
- `PUT /api/serviceoffering/{id}/click` - زيادة عدد النقرات

### Complaints
- `GET /api/complaint` - جميع الشكاوى (للمديرين)
- `GET /api/complaint/user` - شكاوى المستخدم
- `GET /api/complaint/{id}` - شكوى محددة
- `POST /api/complaint` - إنشاء شكوى
- `PUT /api/complaint/{id}` - تحديث شكوى
- `DELETE /api/complaint/{id}` - حذف شكوى
- `GET /api/complaint/{id}/messages` - رسائل الشكوى
- `POST /api/complaint/{id}/messages` - إضافة رسالة

### Advice Requests
- `GET /api/advicerequest` - جميع الطلبات (للمديرين)
- `GET /api/advicerequest/user` - طلبات المستخدم
- `GET /api/advicerequest/{id}` - طلب محدد
- `POST /api/advicerequest` - إنشاء طلب
- `PUT /api/advicerequest/{id}` - تحديث طلب
- `DELETE /api/advicerequest/{id}` - إلغاء طلب
- `PUT /api/advicerequest/{id}/confirm` - تأكيد طلب
- `PUT /api/advicerequest/{id}/complete` - إكمال طلب

### Volunteers
- `GET /api/volunteer` - جميع طلبات التطوع (للمديرين)
- `GET /api/volunteer/user` - طلب التطوع للمستخدم
- `GET /api/volunteer/{id}` - طلب تطوع محدد
- `POST /api/volunteer` - إنشاء طلب تطوع
- `PUT /api/volunteer/{id}` - تحديث طلب تطوع
- `DELETE /api/volunteer/{id}` - حذف طلب تطوع
- `PUT /api/volunteer/{id}/review` - مراجعة طلب تطوع

### Admins
- `GET /api/admin` - جميع المديرين
- `GET /api/admin/{id}` - مدير محدد
- `POST /api/admin` - إنشاء مدير
- `PUT /api/admin/{id}` - تحديث مدير
- `DELETE /api/admin/{id}` - حذف مدير
- `GET /api/admin/statistics` - إحصائيات النظام
- `GET /api/admin/dashboard` - بيانات لوحة التحكم

## التثبيت والتشغيل

### المتطلبات
- .NET 7.0 SDK
- SQL Server 2019 أو أحدث
- Visual Studio 2022 أو VS Code

### خطوات التثبيت

1. **استنساخ المشروع**
```bash
git clone [repository-url]
cd Charity_BE
```

2. **تحديث قاعدة البيانات**
```bash
# في Package Manager Console
Update-Database

# أو في Terminal
dotnet ef database update
```

3. **تشغيل المشروع**
```bash
dotnet run
```

4. **الوصول إلى Swagger**
```
https://localhost:7121/swagger
```

### إعدادات قاعدة البيانات

تأكد من تحديث connection string في `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CharityDataBase;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

### إعدادات JWT

يتم تكوين JWT في `appsettings.json`:

```json
{
  "JWT": {
    "Key": "your-secret-key-here",
    "Issuer": "https://localhost:7121/",
    "Audience": "My audience",
    "DurationInDays": 30
  }
}
```

## الأدوار والصلاحيات

### Admin
- إدارة جميع المستخدمين
- إدارة الاستشارات والمحاضرات
- مراجعة الشكاوى وطلبات التطوع
- إحصائيات النظام

### Advisor
- إدارة أوقات التوفر
- تأكيد وإكمال طلبات الاستشارة
- عرض طلبات الاستشارة المخصصة

### Client
- إنشاء طلبات استشارة
- رفع شكاوى
- طلب التطوع
- مشاهدة المحاضرات والأخبار

## الأمان

- **JWT Authentication** - مصادقة آمنة
- **Role-based Authorization** - تفويض حسب الأدوار
- **Input Validation** - التحقق من المدخلات
- **Error Handling** - معالجة الأخطاء
- **SQL Injection Protection** - حماية من حقن SQL

## الاختبار

### Unit Testing
```bash
dotnet test
```

### Integration Testing
```bash
dotnet test --filter "Category=Integration"
```

## النشر

### Docker
```bash
docker build -t charity-api .
docker run -p 8080:80 charity-api
```

### Azure
```bash
az webapp up --name charity-api --resource-group myResourceGroup --runtime "DOTNETCORE:7.0"
```

## المساهمة

1. Fork المشروع
2. إنشاء branch جديد (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add some AmazingFeature'`)
4. Push إلى Branch (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

## الترخيص

هذا المشروع مرخص تحت رخصة MIT - انظر ملف [LICENSE](LICENSE) للتفاصيل.

## الدعم

للدعم والاستفسارات:
- إنشاء Issue في GitHub
- التواصل عبر البريد الإلكتروني
- مراجعة التوثيق في Swagger

## التحديثات القادمة

- [ ] تطبيق موبايل (React Native)
- [ ] نظام إشعارات في الوقت الفعلي
- [ ] دعم المدفوعات
- [ ] تحليلات متقدمة
- [ ] نظام تقييم متطور
- [ ] دعم اللغات المتعددة 