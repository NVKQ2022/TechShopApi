# 🛍️ TechShopApi

[![.NET CI](https://github.com/NVKQ2022/TechShopApi/actions/workflows/CI.yml/badge.svg)](https://github.com/NVKQ2022/TechShopApi/actions/workflows/CI.yml)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/SQL%20Server-EF%20Core-CC292B?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![NoSQL](https://img.shields.io/badge/MongoDB-Document%20DB-47A248?logo=mongodb)](https://www.mongodb.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)

A scalable, high-performance e-commerce RESTful API backend engineered with **ASP.NET Core (.NET 8)** for modern electronics and gadget retail platforms. 

TechShopApi features a **polyglot persistence architecture** leveraging Microsoft SQL Server for relational identities and transactional security alongside MongoDB for flexible product cataloging, order tracking, and high-throughput document storage.

---

## 📑 Table of Contents

- [Architectural Overview](#-architectural-overview)
- [Key Features](#-key-features)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Prerequisites](#-prerequisites)
- [Configuration](#-configuration)
- [Getting Started](#-getting-started)
- [Running with Docker](#-running-with-docker)
- [API Endpoints Overview](#-api-endpoints-overview)
- [Recommendation Engine](#-recommendation-engine)
- [CI/CD & Quality Assurance](#-cicd--quality-assurance)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🏛️ Architectural Overview

TechShopApi is structured around a decoupled, layered architecture employing the **Repository Pattern** and **Polyglot Persistence**:

- **Microsoft SQL Server (EF Core)**: Houses identity data, password hashes, salts, OAuth provider bindings, email verification OTPs, and FCM device tokens.
- **MongoDB**: Houses unstructured and high-volume data: catalog products, dynamic category schemas, customer cart and wishlist states, orders, customer reviews, promotional sales events, and notifications.
- **Asynchronous Co-Purchase Recommendation Engine**: Computes item-item association frequencies to serve intelligent cross-selling recommendations in real time.
- **External Integrations**: VNPay & VietQR payment workflows, ImageKit media CDN, Firebase Cloud Messaging (FCM), and Google OAuth.

```
                    ┌────────────────────────┐
                    │   Client Applications  │
                    │  (Web, Mobile, Admin)  │
                    └───────────┬────────────┘
                                │ HTTPS / REST / JWT
                                ▼
                    ┌────────────────────────┐
                    │   ASP.NET Core Web API │
                    │   (Routing, Rate Limit,│
                    │    JWT Auth, Swagger)  │
                    └───────────┬────────────┘
                                │
        ┌───────────────────────┼────────────────────────┐
        ▼                       ▼                        ▼
┌──────────────┐        ┌──────────────┐         ┌──────────────┐
│  SQL Server  │        │   MongoDB    │         │  3rd Parties │
│  (EF Core)   │        │  (Driver)    │         │  & Services  │
├──────────────┤        ├──────────────┤         ├──────────────┤
│ - Users      │        │ - Products   │         │ - VNPay      │
│ - Auth & OTP │        │ - Orders     │         │ - VietQR     │
│ - Providers  │        │ - Carts/Wish │         │ - ImageKit   │
│ - FCM Tokens │        │ - Reviews    │         │ - Firebase   │
└──────────────┘        └──────────────┘         └──────────────┘
```

---

## ✨ Key Features

### 🔐 Authentication & Security
- **JWT Authentication**: Stateless authentication with role-based access control (`Admin`, `User`).
- **Cryptographic Password Protection**: SHA-256 password hashing with unique per-user salt and application pepper.
- **Email Verification & OTP**: One-Time Passwords for account registration and password reset verification.
- **Social Login**: Support for Google OAuth and Firebase token verification.
- **Rate Limiting**: Built-in ASP.NET Core `FixedWindowLimiter` on authentication endpoints to defend against brute-force attacks.
- **IDOR Protection & Stock Guards**: Strict server-side validation ensuring users only access their own data, with server-side price computation to prevent tampering.

### 📦 Product & Catalog Management
- **Full Product CRUD**: Categorized products with attributes, technical specifications, variants (color), pricing, and real-time stock levels.
- **Dynamic Sales & Events**: Scheduled promotional sales, percentage discounts, and flash sales management.
- **Search & Categorization**: Fast category-based querying and full-text keyword search.

### 🛒 Shopping Cart & Wishlist
- **Persistent Server-Side Cart**: Real-time stock validation prior to cart modifications and checkout.
- **Wishlist Integration**: Save favorite items with one-tap transfer from wishlist to cart (`move/{productId}`).

### 💳 Order Processing & Payments
- **Multi-Step Checkout Pipeline**: Address validation, order preparation, real-time inventory decrement, and automatic stock restoration upon cancellation.
- **VNPay Integration**: Seamless checkout via the VNPay gateway.
- **VietQR Dynamic Payment**: Generates dynamic banking QR codes (e.g. BIDV) containing specific order references and amounts for instant bank transfers.

### 🧠 Intelligent Recommendations
- **Item-Item Co-Purchase Matrix**: Real-time collaborative filtering algorithm computing product affinity from completed order histories.
- **Administrative Controls**: Endpoints to trigger manual or scheduled similarity matrix rebuilds.

### 🔔 Notifications & Media
- **Push Notifications (FCM)**: Push notification dispatch to mobile/web clients via `FirebaseAdmin`.
- **In-App Notification Center**: Track read and unread messages per user.
- **Image CDN Integration**: Secure image uploads via `ImageKit` with MIME-type and file size validation (max 10MB).

---

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| **Framework** | ASP.NET Core 8.0 (.NET 8) |
| **Language** | C# 12 |
| **Relational Database** | Microsoft SQL Server (via Entity Framework Core 9) |
| **NoSQL Database** | MongoDB (Official MongoDB C# Driver 3.5.2) |
| **Authentication** | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`), Google Auth |
| **Push Notifications** | Firebase Admin SDK (`FirebaseAdmin`) |
| **Payment Gateways** | VNPay (`VNPAY.NET`), VietQR API |
| **Image Hosting** | ImageKit API |
| **API Documentation** | Swagger / OpenAPI (`Swashbuckle.AspNetCore`) |
| **Containerization** | Docker (Multi-stage build) |
| **CI/CD** | GitHub Actions, Trivy Security Scanner, Qodana |

---

## 📂 Project Structure

```
TechShopApi/
├── Controllers/
│   └── Api/
│       ├── AdminController.cs            # Analytics, product/user administration, sales events
│       ├── AuthenticateController.cs     # Auth, registration, OTP, Google/Firebase login
│       ├── AVersionController.cs         # API build & version metadata
│       ├── CartController.cs             # User shopping cart operations
│       ├── NotificationController.cs     # Notifications and FCM registration
│       ├── OrderController.cs            # Order checkout, lifecycle & status tracking
│       ├── ProductController.cs          # Catalog querying, details, and search
│       ├── PurchaseController.cs         # VNPay and VietQR payment processing
│       ├── RecommendationController.cs   # Co-purchase product recommendations
│       ├── ReviewController.cs           # Product ratings & user reviews
│       ├── UserController.cs             # User profile, shipping info, password update
│       └── WishlistController.cs         # Wishlist management
├── Data/
│   ├── Authenticate/                     # SQL Server repositories (User, OTP, FCM)
│   ├── Context/                          # EF Core AuthenticateDbContext
│   └── *.Repository.cs                   # MongoDB repositories (Product, Order, etc.)
├── DTOs/                                 # Data Transfer Objects & validation models
├── Helpers/                              # SecurityHelper, ConverterHelper, VersionHelper
├── Models/                               # Domain entities (SQL Server & MongoDB BSON models)
├── Service/                              # Business services (Email, Payment, FCM, ImageKit, Recommendation)
├── wwwroot/Email-templates/              # Responsive HTML templates for OTP emails
├── .github/workflows/                    # CI workflows (build, test, Trivy scan)
├── Dockerfile                            # Optimized multi-stage Docker build
├── Program.cs                            # Application entry point & DI configuration
├── appsettings.json                      # Base configuration template
└── TechShopApi.csproj                    # Project file & NuGet dependencies
```

---

## 📋 Prerequisites

Ensure you have the following installed locally:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB / Docker container)
- [MongoDB Community Server](https://www.mongodb.com/try/download/community) (or MongoDB Atlas cluster)
- [Docker](https://www.docker.com/) *(optional, for containerized run)*

---

## ⚙️ Configuration

Copy or update `appsettings.json` (or `appsettings.Development.json`) with your credentials:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "UserDatabase": "Server=localhost;Database=TechShopAuthDb;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True;"
  },
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "webdev_project",
    "ProductCollectionName": "Product",
    "OrderCollectionName": "Order",
    "CategoryCollectionName": "Category",
    "UserDetailCollectionName": "UserDetail",
    "ReviewCollectionName": "Reviews",
    "NotificationCollectionName": "Notification",
    "ProductSaleEventCollectionName": "SaleEvents"
  },
  "JWT": {
    "Key": "YourSuperSecretKeyWithAtLeast32Characters!"
  },
  "JwtConfig": {
    "Issuer": "TechShopApi",
    "Audience": "TechShopApiUser",
    "ExpireMinutes": 60
  },
  "Security": {
    "Pepper": "YourStaticPepperString",
    "Email": "your-smtp-email@gmail.com",
    "Password": "your-smtp-app-password"
  },
  "GoogleOAuth": {
    "ClientId": "your-google-client-id.apps.googleusercontent.com"
  },
  "ImageKit": {
    "PublicKey": "your_imagekit_public_key",
    "PrivateKey": "your_imagekit_private_key",
    "UrlEndpoint": "https://ik.imagekit.io/your_endpoint"
  },
  "VietQR": {
    "MerchantId": "MOMO",
    "AccountNumber": "7011084307",
    "AccountName": "TECHSHOP",
    "Username": "techshop_username",
    "Password": "techshop_password"
  },
  "VnPay": {
    "BaseUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
    "ReturnUrl": "http://localhost:5079/api/purchase/vnpay-return",
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET"
  },
  "BaseUrl": "http://localhost:5079"
}
```

> **Security Note**: `appsettings.json` and `appsettings.Development.json` contain sample/placeholder values for repository sharing. For local development, real credentials should be placed in **.NET User Secrets** (`dotnet user-secrets`) or in `appsettings.Development.local.json` (which is gitignored). For production, supply sensitive secrets via environment variables (e.g. `ConnectionString__UserDatabase`, `JWT__Key`, `ImageKit__PrivateKey`).

---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/NVKQ2022/TechShopApi.git
cd TechShopApi
```

### 2. Restore dependencies
```bash
dotnet restore
```

### 3. Build the Database (Code First Migration)

You can build and initialize the SQL Server database schema from code using any of the following methods:

**Option A — Programmatic Migration (No extra tools needed):**
```bash
dotnet run -- --migrate
```

**Option B — EF Core CLI Tool:**
```bash
# Install EF tool if not already installed
dotnet tool install --global dotnet-ef

# Apply migrations
dotnet ef database update
```

**Option C — Standalone SQL Script (SSMS / Azure Data Studio / Docker):**
Execute [`Migrations/InitialCreate.sql`](file:///home/quan/projects/techshop/TechShopApi/Migrations/InitialCreate.sql) directly on your SQL Server instance. It is fully idempotent and creates all required tables, indexes, and seed data safely.

### 4. Build and Run
```bash
dotnet build
dotnet run
```

The application will start listening on the configured URLs (typically `http://localhost:5079` or `https://localhost:7079`).

### 5. Access Interactive Swagger Documentation
Navigate to:
```
http://localhost:5079/swagger
```
Authorize requests by clicking **Authorize** and supplying your JWT token:
```
Bearer <your_token_here>
```

---

## 🐳 Running with Docker

### Build the Docker image
```bash
docker build -t techshopapi:latest .
```

### Run the container
```bash
docker run -d \
  -p 8080:8080 \
  -e ConnectionString__UserDatabase="Server=host.docker.internal;Database=TechShopAuthDb;..." \
  -e MongoDbSettings__ConnectionString="mongodb://host.docker.internal:27017" \
  --name techshop-api \
  techshopapi:latest
```

The API will be accessible at `http://localhost:8080/swagger`.

---

## 📡 API Endpoints Overview

| Area | Method | Endpoint | Description | Auth Required |
|---|---|---|---|:---:|
| **Auth** | `POST` | `/api/Authenticate/register` | Register new user account | No |
| | `POST` | `/api/Authenticate/login` | Authenticate and obtain JWT token | No |
| | `POST` | `/api/Authenticate/google` | Google OAuth token authentication | No |
| | `POST` | `/api/Authenticate/firebase` | Firebase authentication | No |
| | `POST` | `/api/Authenticate/EmailVerify/Send` | Send email verification OTP | No |
| | `GET` | `/api/Authenticate/Email/verify` | Verify OTP code | No |
| | `PUT` | `/api/Authenticate/ResetPassword` | Reset password using verified OTP | No |
| **Catalog** | `GET` | `/api/Product/Fetch` | Get products list (curated/random) | No |
| | `GET` | `/api/Product/Fetch/{category}/{number}` | Filter products by category | No |
| | `GET` | `/api/Product/Search/{keyword}` | Search products by keyword | No |
| | `GET` | `/api/Product/Details/{id}` | Get product details & specs | No |
| | `GET` | `/api/Product/All/Category` | List all available categories | No |
| **Cart** | `GET` | `/api/Cart` | Retrieve user shopping cart | Yes |
| | `POST` | `/api/Cart/add` | Add product to cart with quantity | Yes |
| | `PUT` | `/api/Cart/update` | Update item quantity in cart | Yes |
| | `DELETE` | `/api/Cart/remove/{productId}` | Remove item from cart | Yes |
| **Wishlist** | `GET` | `/api/Wishlist` | Get user wishlist items | Yes |
| | `POST` | `/api/Wishlist/add` | Add product to wishlist | Yes |
| | `POST` | `/api/Wishlist/move/{productId}` | Move item from wishlist to cart | Yes |
| **Orders** | `POST` | `/api/Order/prepare` | Create initial order draft | Yes |
| | `GET` | `/api/Order/my` | List current user order history | Yes |
| | `GET` | `/api/Order/{id}` | Get order details | Yes |
| | `DELETE` | `/api/Order/{orderId}` | Cancel order & restore product stock | Yes |
| **Payments** | `POST` | `/api/Purchase/confirm/{orderId}` | Confirm order & choose payment | Yes |
| | `GET` | `/api/Purchase/payment-qr/{orderId}` | Get VietQR payment image & info | No |
| **Recommendations** | `GET` | `/api/recommend/product/{productId}` | Get related products co-purchased | No |
| | `POST` | `/api/recommend/rebuild` | Trigger co-purchase matrix rebuild | Admin |
| **Admin** | `GET` | `/api/Admin/overview` | Platform-wide sales & KPI summary | Admin |
| | `GET` | `/api/Admin/top-sellers` | Top-performing products | Admin |
| | `POST` | `/api/Admin/products` | Create new catalog item | Admin |
| | `PUT` | `/api/Admin/products/{id}/sale` | Apply sale discounts to a product | Admin |
| | `GET` | `/api/Admin/sales-events` | Retrieve active & scheduled events | Admin |
| **System** | `GET` | `/api/AVersion` | API version, commit hash, build date | No |

---

## 🧠 Recommendation Engine

TechShopApi features an **Item-Item Collaborative Filtering** model based on historical co-purchase patterns:

1. **Matrix Construction**:
   The engine iterates across all completed orders in MongoDB and maps pairwise item associations:
   $$\text{Similarity}(A, B) = \text{Count of orders containing both } A \text{ and } B$$
2. **Real-time Querying**:
   When querying `/api/recommend/product/{productId}`, the top-$N$ most correlated products are extracted from an in-memory concurrent dictionary (`SimilarityMatrix`).
3. **Resilience**:
   The matrix automatically builds on application startup asynchronously in a background thread and can be refreshed on demand by administrators without downtime.

---

## 🛡️ CI/CD & Quality Assurance

This repository includes continuous integration pipelines defined in [`.github/workflows/CI.yml`](.github/workflows/CI.yml):

- **Build & Test**: Automated restore and Release build verification on Ubuntu runners using .NET 8.
- **Docker Container Build**: Verified build using Docker Buildx with GitHub Actions caching.
- **Vulnerability Scanning**: Automated container security scanning using **Aqua Security Trivy** to identify OS and package vulnerabilities.
- **Code Quality**: Automated static analysis using JetBrains Qodana (`.github/workflows/qodana_code_quality.yml`).

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create your feature branch (`git checkout -b feature/amazing-feature`).
3. Commit your changes (`git commit -m 'feat: add amazing feature'`).
4. Push to the branch (`git push origin feature/amazing-feature`).
5. Open a Pull Request.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE) (or as designated by the project maintainers).
