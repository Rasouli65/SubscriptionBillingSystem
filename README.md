# Subscription Billing System

A scalable Subscription and Billing management system built with **.NET 8**, following **Clean Architecture**, **Domain-Driven Design (DDD)**, and **CQRS** principles.

---

# Architecture Overview

The solution is organized into four layers following the Onion Architecture:

## Domain Layer (Core)
The heart of the application, containing high-level business rules:
- **Aggregates:** `Subscription`, `Customer`, `Invoice`.
- **Value Objects:** `Money` (with logic for negative amounts and currency codes).
- **Domain Events:** `SubscriptionCreatedEvent`, `SubscriptionActivatedEvent`.
- **Exceptions:** Custom `DomainException` for business rule violations.

## Application Layer
Orchestrates the flow of data:
- **MediatR:** Handles Commands (state changes) and Queries (data retrieval).
- **Domain Event Handlers:** Reacts to internal events like auto-generating invoices.

## Infrastructure Layer
Technical implementations:
- **Persistence:** EF Core with In-Memory provider.
- **Background Jobs:** Hangfire integration for recurring expiration checks.
- **Repositories:** Implementation of data access logic.

## Api Layer (Presentation)
The entry point of the system:
- **Middleware:** Global Exception Handling for converting Domain Exceptions to HTTP 400.
- **Swagger:** Fully documented REST endpoints.

---

# Technical Highlights

- **Encapsulated Domain:** Entities have private setters; state changes occur only through well-defined methods.
- **Primitive Obsession Avoidance:** Using Value Objects like `Money` instead of just `decimal`.
- **Automatic Invoicing:** Fully decoupled logic using Domain Events.
- **Global Exception Handling:** Custom middleware prevents technical stack traces from leaking to the client.
- **Background Worker:** Fully automated subscription lifecycle management using Hangfire.

---

# End-to-End System Walkthrough (Testing the Flow)

Follow these steps in Swagger to test the complete lifecycle:

### 1. Create a Customer
- **Endpoint:** `POST /api/customers`
- **Validation:** Email must contain '@' and Address cannot be empty.
- **Result:** Returns a `CustomerId`.

### 2. Create a Subscription
- **Endpoint:** `POST /api/subscriptions`
- **Logic:** Provide `CustomerId` and specify `amount` (must be > 0) and `currency` (3-letter code).
- **Internal Action:** A `SubscriptionCreatedEvent` triggers the automatic creation of a **Pending Invoice**.

### 3. Retrieve & Pay Invoice
- **Endpoint:** `GET /api/subscriptions/{id}/invoices` to find the invoice.
- **Endpoint:** `POST /api/invoices/{invoiceId}/pay`.
- **Automatic State Change:** Once paid, the system automatically switches the Subscription to **Active**.

### 4. Background Expiration
- The system runs a **Hangfire Recurring Job** every minute.
- Set a short duration for a subscription (e.g., 1 minute).
- After 1 minute, the job detects the expiration and transitions the status to **Expired**.
- **Hangfire Dashboard:** Monitor jobs at `https://localhost:{port}/hangfire`.
- To check Subscription Change status to expired run this endpoint after 1 minute
- **Endpoint:** `GET /api/Customers/{customerId}/subscriptions`
---

# Global Exception Handling

The system ensures that business rule violations are returned as clean, readable errors:

**Example Request:** Creating a subscription with `amount: -10`.
**Response (400 Bad Request):**
```json
{
  "error": "Amount cannot be negative.",
  "status": 400
}