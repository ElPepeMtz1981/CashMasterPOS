# 💸 ChangeCalculator Utility – CashMasterPOS

This utility provides a precise and modular algorithm to calculate monetary change based on a given price, tendered cash, and configured currency denominations.

## 🔧 Class Overview

`ChangeCalculator` is a static class designed to perform accurate change breakdowns given a transaction scenario. It follows clean architecture principles and throws meaningful exceptions when calculation is not possible.

---

## 📦 Method: `CalculateChange(decimal price, Dictionary<decimal, int> customerTender)`

### Description

Calculates the optimal change to return based on configured denominations (`Currency.Denominations`). Ensures the input is valid, sufficient, and solvable with the available currency structure.

### Parameters

| Name            | Type                            | Description                                      |
|-----------------|----------------------------------|--------------------------------------------------|
| `price`         | `decimal`                       | Total amount to be paid for the transaction.     |
| `customerTender`| `Dictionary<decimal, int>`      | Keys represent the denominations tendered, values their counts. |

### Returns

- `Dictionary<decimal, int>`: breakdown of change to return (denomination → quantity).

---

## 🚨 Exceptions

| Exception                     | Thrown When                                                   |
|------------------------------|----------------------------------------------------------------|
| `ChangeCalculationException` | - Price is 0 or less<br>- Tender is null or empty<br>- Insufficient payment<br>- Denominations not configured<br>- Exact change can't be returned |

Each exception includes:
- `Price`: the product’s price
- `Payment`: the total tendered by the customer
- A meaningful message string

---

## 🔁 Configuration: `Currency.Denominations`

Before calling the method, ensure the `Currency.Denominations` list is populated and ordered **descendingly** (largest to smallest). Example:

```csharp
Currency.SetDenominations(new List<decimal>
{
    100.00m, 50.00m, 20.00m, 10.00m, 5.00m,
     1.00m, 0.50m, 0.25m, 0.10m, 0.05m, 0.01m
});
```
---

### 🧩 Extensible Design: Separate Validation with `TenderValidator`

As an additional improvement beyond the code challenge requirements, a helper class named `TenderValidator` was implemented to encapsulate the validation of the customer's payment input (`tender`).

This design choice follows the Single Responsibility Principle (SRP) and provides several benefits:

- Reusable, decoupled validation logic independent from business rules
- Clear, traceable error messages with optional context
- Focused unit testing on edge-case validation scenarios
- Improved long-term maintainability and test coverage

The inclusion of `TenderValidator` reflects a proactive mindset centered on quality, flexibility, and architecture-first thinking — even within a limited challenge scope.

---