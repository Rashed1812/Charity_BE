# Best Practices and Recommendations for Consultation Platform

## 1. Security Best Practices

### 1.1 Authentication & Authorization
- **JWT Token Security**
  - Use short-lived access tokens (15-30 minutes)
  - Implement refresh token rotation
  - Store refresh tokens securely (hashed in database)
  - Include user roles in JWT claims
  - Validate token signature and expiration

- **Password Security**
  - Enforce strong password policies (minimum 8 characters, complexity requirements)
  - Use bcrypt or Argon2 for password hashing
  - Implement account lockout after failed attempts
  - Require password change on first login

- **Role-Based Access Control (RBAC)**
  - Define clear role hierarchies
  - Use attribute-based authorization for fine-grained control
  - Implement resource-level permissions
  - Audit access logs regularly

### 1.2 API Security
- **HTTPS Only**
  - Force HTTPS in production
  - Use HSTS headers
  - Implement certificate pinning for mobile apps

- **Input Validation**
  - Validate all inputs server-side
  - Use parameterized queries to prevent SQL injection
  - Sanitize user inputs before storage
  - Implement rate limiting

- **CORS Configuration**
  - Restrict CORS to specific domains
  - Don't use wildcard (*) for production
  - Implement proper preflight handling

### 1.3 Data Protection
- **Sensitive Data**
  - Encrypt sensitive data at rest
  - Use encryption in transit (TLS 1.3)
  - Implement data masking for logs
  - Regular security audits

- **GDPR Compliance**
  - Implement data retention policies
  - Provide data export functionality
  - Support right to be forgotten
  - Document data processing activities

## 2. Validation and Error Handling

### 2.1 Input Validation
```csharp
// Use Data Annotations for validation
public class CreateUserDTO
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")]
    public string Password { get; set; }
}
```

### 2.2 Custom Validation
```csharp
public class CustomValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Custom validation logic
        if (value == null)
            return new ValidationResult("Value is required");

        return ValidationResult.Success;
    }
}
```

### 2.3 Global Error Handling
```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "An error occurred",
            StatusCode = 500
        };

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = 400;
                response.Message = "Validation failed";
                response.Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = 404;
                response.Message = notFoundEx.Message;
                break;

            case UnauthorizedAccessException:
                response.StatusCode = 401;
                response.Message = "Unauthorized access";
                break;
        }

        httpContext.Response.StatusCode = response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
```

## 3. Database Design and Optimization

### 3.1 Indexing Strategy
```sql
-- Primary indexes
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Advisors_ConsultationId ON Advisors(ConsultationId);
CREATE INDEX IX_AdviceRequests_AdvisorId ON AdviceRequests(AdvisorId);
CREATE INDEX IX_AdviceRequests_UserId ON AdviceRequests(UserId);
CREATE INDEX IX_AdviceRequests_Status ON AdviceRequests(Status);

-- Composite indexes for common queries
CREATE INDEX IX_AdviceRequests_AdvisorId_Status ON AdviceRequests(AdvisorId, Status);
CREATE INDEX IX_AdvisorAvailability_AdvisorId_Date ON AdvisorAvailability(AdvisorId, Date);
CREATE INDEX IX_Complaints_UserId_Status ON Complaints(UserId, Status);
```

### 3.2 Data Types and Constraints
```sql
-- Use appropriate data types
ALTER TABLE Users ADD CONSTRAINT CK_Users_Email CHECK (Email LIKE '%@%.%');
ALTER TABLE AdvisorAvailability ADD CONSTRAINT CK_StartTime_Before_EndTime CHECK (StartTime < EndTime);
ALTER TABLE AdviceRequests ADD CONSTRAINT CK_AppointmentTime_Future CHECK (AppointmentTime > GETDATE());
```

### 3.3 Soft Delete Pattern
```csharp
public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    string DeletedBy { get; set; }
}

// In queries, always filter out soft-deleted records
public async Task<List<T>> GetAllAsync()
{
    return await _context.Set<T>()
        .Where(e => !e.IsDeleted)
        .ToListAsync();
}
```

## 4. Performance Optimization

### 4.1 Caching Strategy
```csharp
// Redis caching for frequently accessed data
public class CacheService
{
    private readonly IDistributedCache _cache;
    
    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
    {
        var cached = await _cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<T>(cached);

        var result = await factory();
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30)
        };
        
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(result), options);
        return result;
    }
}
```

### 4.2 Pagination and Filtering
```csharp
public class PaginationParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;
    
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string SearchTerm { get; set; }
    public string SortBy { get; set; }
    public bool IsDescending { get; set; }
}

public async Task<PaginatedResponse<T>> GetPaginatedAsync<T>(
    IQueryable<T> query, 
    PaginationParameters parameters)
{
    var totalCount = await query.CountAsync();
    
    if (!string.IsNullOrEmpty(parameters.SearchTerm))
        query = ApplySearch(query, parameters.SearchTerm);
    
    if (!string.IsNullOrEmpty(parameters.SortBy))
        query = ApplySorting(query, parameters.SortBy, parameters.IsDescending);
    
    var items = await query
        .Skip((parameters.PageNumber - 1) * parameters.PageSize)
        .Take(parameters.PageSize)
        .ToListAsync();
    
    return new PaginatedResponse<T>
    {
        Items = items,
        TotalCount = totalCount,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize,
        TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
        HasPreviousPage = parameters.PageNumber > 1,
        HasNextPage = parameters.PageNumber < (int)Math.Ceiling(totalCount / (double)parameters.PageSize)
    };
}
```

### 4.3 Async/Await Best Practices
```csharp
// Always use async/await for I/O operations
public async Task<AdvisorDTO> GetAdvisorByIdAsync(int id)
{
    var advisor = await _context.Advisors
        .Include(a => a.Consultation)
        .Include(a => a.User)
        .FirstOrDefaultAsync(a => a.Id == id);
    
    return _mapper.Map<AdvisorDTO>(advisor);
}

// Use ConfigureAwait(false) for library code
public async Task<List<AdvisorDTO>> GetAllAdvisorsAsync()
{
    var advisors = await _context.Advisors
        .Include(a => a.Consultation)
        .ToListAsync()
        .ConfigureAwait(false);
    
    return _mapper.Map<List<AdvisorDTO>>(advisors);
}
```

## 5. Logging and Monitoring

### 5.1 Structured Logging
```csharp
public class LoggingService
{
    private readonly ILogger<LoggingService> _logger;
    
    public void LogUserAction(string userId, string action, object data)
    {
        _logger.LogInformation(
            "User {UserId} performed {Action} with data {@Data}",
            userId, action, data);
    }
    
    public void LogError(Exception ex, string context)
    {
        _logger.LogError(ex, "Error in {Context}: {Message}", context, ex.Message);
    }
}
```

### 5.2 Health Checks
```csharp
public class HealthCheckService : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check database connectivity
            await _context.Database.CanConnectAsync(cancellationToken);
            
            // Check external services
            var redisHealth = await _redis.PingAsync();
            
            return HealthCheckResult.Healthy("All services are healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Health check failed", ex);
        }
    }
}
```

## 6. Testing Strategy

### 6.1 Unit Testing
```csharp
[TestClass]
public class AdvisorServiceTests
{
    private Mock<IAdvisorRepository> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private AdvisorService _service;
    
    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IAdvisorRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new AdvisorService(_mockRepository.Object, _mockMapper.Object);
    }
    
    [TestMethod]
    public async Task GetAdvisorById_ValidId_ReturnsAdvisor()
    {
        // Arrange
        var advisorId = 1;
        var advisor = new Advisor { Id = advisorId, FullName = "Test Advisor" };
        var advisorDto = new AdvisorDTO { Id = advisorId, FullName = "Test Advisor" };
        
        _mockRepository.Setup(r => r.GetByIdAsync(advisorId))
            .ReturnsAsync(advisor);
        _mockMapper.Setup(m => m.Map<AdvisorDTO>(advisor))
            .Returns(advisorDto);
        
        // Act
        var result = await _service.GetAdvisorByIdAsync(advisorId);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(advisorId, result.Id);
        Assert.AreEqual("Test Advisor", result.FullName);
    }
}
```

### 6.2 Integration Testing
```csharp
[TestClass]
public class AdvisorControllerIntegrationTests
{
    private TestServer _server;
    private HttpClient _client;
    
    [TestInitialize]
    public void Setup()
    {
        var builder = new WebHostBuilder()
            .UseStartup<TestStartup>();
        _server = new TestServer(builder);
        _client = _server.CreateClient();
    }
    
    [TestMethod]
    public async Task GetAdvisors_ReturnsOkResult()
    {
        // Act
        var response = await _client.GetAsync("/api/advisor");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<List<AdvisorDTO>>>(content);
        Assert.IsTrue(result.Success);
    }
}
```

## 7. Deployment and DevOps

### 7.1 Docker Configuration
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Charity_BE/Charity_BE.csproj", "Charity_BE/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Charity_BE/Charity_BE.csproj"
COPY . .
WORKDIR "/src/Charity_BE"
RUN dotnet build "Charity_BE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Charity_BE.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Charity_BE.dll"]
```

### 7.2 Environment Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db;Database=CharityDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-with-at-least-32-characters",
    "Issuer": "CharityPlatform",
    "Audience": "CharityUsers",
    "ExpirationMinutes": 30,
    "RefreshTokenExpirationDays": 7
  },
  "Redis": {
    "ConnectionString": "redis:6379"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

### 7.3 CI/CD Pipeline
```yaml
name: Build and Deploy

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish -c Release -o ./publish
    
    - name: Build Docker image
      run: docker build -t charity-platform .
    
    - name: Deploy to staging
      if: github.ref == 'refs/heads/main'
      run: |
        # Deploy to staging environment
```

## 8. Frontend Integration

### 8.1 API Client Configuration
```typescript
// api-client.ts
class ApiClient {
  private baseUrl: string;
  private token: string | null;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
    this.token = localStorage.getItem('token');
  }

  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    const url = `${this.baseUrl}${endpoint}`;
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
      ...options.headers,
    };

    if (this.token) {
      headers.Authorization = `Bearer ${this.token}`;
    }

    const response = await fetch(url, {
      ...options,
      headers,
    });

    if (!response.ok) {
      if (response.status === 401) {
        // Handle token refresh or redirect to login
        this.handleUnauthorized();
      }
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return response.json();
  }

  async get<T>(endpoint: string): Promise<ApiResponse<T>> {
    return this.request<T>(endpoint);
  }

  async post<T>(endpoint: string, data: any): Promise<ApiResponse<T>> {
    return this.request<T>(endpoint, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  async put<T>(endpoint: string, data: any): Promise<ApiResponse<T>> {
    return this.request<T>(endpoint, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  }

  async delete<T>(endpoint: string): Promise<ApiResponse<T>> {
    return this.request<T>(endpoint, {
      method: 'DELETE',
    });
  }

  private handleUnauthorized(): void {
    localStorage.removeItem('token');
    window.location.href = '/login';
  }
}

export const apiClient = new ApiClient(process.env.REACT_APP_API_URL);
```

### 8.2 Error Handling
```typescript
// error-handler.ts
export class ErrorHandler {
  static handle(error: any): void {
    if (error.response) {
      const { status, data } = error.response;
      
      switch (status) {
        case 400:
          this.showValidationErrors(data.errors);
          break;
        case 401:
          this.handleUnauthorized();
          break;
        case 403:
          this.showError('Access denied');
          break;
        case 404:
          this.showError('Resource not found');
          break;
        case 500:
          this.showError('Server error occurred');
          break;
        default:
          this.showError('An unexpected error occurred');
      }
    } else {
      this.showError('Network error occurred');
    }
  }

  private static showValidationErrors(errors: string[]): void {
    // Display validation errors to user
    console.error('Validation errors:', errors);
  }

  private static handleUnauthorized(): void {
    localStorage.removeItem('token');
    window.location.href = '/login';
  }

  private static showError(message: string): void {
    // Show error message to user (toast, alert, etc.)
    console.error(message);
  }
}
```

## 9. Monitoring and Analytics

### 9.1 Application Insights
```csharp
// Configure Application Insights
services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = Configuration["ApplicationInsights:ConnectionString"];
});

// Custom telemetry
public class TelemetryService
{
    private readonly TelemetryClient _telemetryClient;
    
    public void TrackUserAction(string userId, string action, Dictionary<string, string> properties = null)
    {
        _telemetryClient.TrackEvent("UserAction", new Dictionary<string, string>
        {
            ["UserId"] = userId,
            ["Action"] = action,
            ...properties ?? new Dictionary<string, string>()
        });
    }
    
    public void TrackException(Exception ex, string context)
    {
        _telemetryClient.TrackException(ex, new Dictionary<string, string>
        {
            ["Context"] = context
        });
    }
}
```

### 9.2 Performance Monitoring
```csharp
// Performance counters
public class PerformanceService
{
    private readonly ILogger<PerformanceService> _logger;
    
    public async Task<T> MeasureAsync<T>(string operation, Func<Task<T>> operationFunc)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var result = await operationFunc();
            stopwatch.Stop();
            
            _logger.LogInformation(
                "Operation {Operation} completed in {ElapsedMs}ms",
                operation, stopwatch.ElapsedMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Operation {Operation} failed after {ElapsedMs}ms",
                operation, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
```

## 10. Security Checklist

- [ ] Implement HTTPS everywhere
- [ ] Use secure JWT configuration
- [ ] Implement rate limiting
- [ ] Validate all inputs
- [ ] Use parameterized queries
- [ ] Implement proper CORS
- [ ] Enable security headers
- [ ] Regular security audits
- [ ] Monitor for suspicious activity
- [ ] Keep dependencies updated
- [ ] Implement proper logging
- [ ] Use environment variables for secrets
- [ ] Implement backup strategies
- [ ] Test disaster recovery procedures

## 11. Performance Checklist

- [ ] Implement caching strategy
- [ ] Use database indexing
- [ ] Optimize queries
- [ ] Implement pagination
- [ ] Use async/await properly
- [ ] Monitor response times
- [ ] Implement CDN for static content
- [ ] Optimize images and assets
- [ ] Use compression
- [ ] Monitor memory usage
- [ ] Implement health checks
- [ ] Set up alerting

## 12. Testing Checklist

- [ ] Unit tests for all services
- [ ] Integration tests for controllers
- [ ] API endpoint tests
- [ ] Database migration tests
- [ ] Security tests
- [ ] Performance tests
- [ ] Load testing
- [ ] User acceptance tests
- [ ] Automated deployment tests
- [ ] Monitoring and alerting tests

This comprehensive guide provides the foundation for building a robust, secure, and scalable consultation platform. Follow these best practices to ensure your application meets enterprise-grade standards. 