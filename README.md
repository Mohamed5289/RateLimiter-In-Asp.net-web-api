# **ASP.NET Core Rate Limiting API**  

## **Project Overview**  
This project is a **RESTful API** built using **ASP.NET Core** that implements **rate limiting** to manage incoming requests and protect the server from excessive traffic. The project utilizes `Microsoft.AspNetCore.RateLimiting` to apply different rate-limiting strategies based on **IP address, user identity, and concurrency limits**.  

---

## **Key Features**  

### **1. Rate Limiting Strategies Implemented**  
- **IP-based Limiting** – Limits requests per IP address using a **Fixed Window Limiter**.  
- **User-based Limiting** – Limits requests per user identity to prevent abuse from authenticated users.  
- **Concurrency Limiting** – Controls the number of concurrent requests allowed at a given time.  
- **Token Bucket Limiting** – Uses a **token-based system**, replenishing tokens over time to ensure smooth traffic flow.  
- **Fixed Window Limiting** – Restricts a set number of requests within a fixed time window.  
- **Sliding Window Limiting** – Similar to fixed window but spreads requests more evenly over time.  

### **2. API Documentation & Testing**  
- **Swagger UI** is integrated for API documentation and testing in development mode.  

### **3. Security & Performance Enhancements**  
- **HTTPS enforcement** to secure communication.  
- **Authorization middleware** to control access based on authentication.  
- **Rejection Handling** – If a request exceeds the limit, the server returns **HTTP 429 (Too Many Requests)**.  

---

## **How It Works**  

1. **Rate Limiter Policies Are Defined**  
   - Each policy is registered in `builder.Services.AddRateLimiter(options => {...})`.  

2. **Middleware Is Activated**  
   - The `app.UseRateLimiter();` middleware applies the policies globally to the API.  

3. **Requests Are Managed Efficiently**  
   - Based on the **IP address, user identity, or concurrency limits**, requests are either allowed or rejected.  

---

## **Use Cases**  
✅ **API Protection** – Prevents abuse by rate-limiting high-frequency requests.  
✅ **Fair Resource Allocation** – Ensures all users get a fair share of API resources.  
✅ **DDoS Mitigation** – Protects against potential **DDoS attacks** by limiting request rates.  
✅ **Scalable API Design** – Helps manage API traffic efficiently without overloading the server.  

This project is ideal for **building secure and scalable APIs** while maintaining **server performance and stability**. 🚀
