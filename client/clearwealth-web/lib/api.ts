// client/clearwealth-web/lib/api.ts
import { getToken } from './auth';

const BASE = process.env.NEXT_PUBLIC_API_URL;

// Every request goes through this — attaches JWT automatically
async function apiFetch<T>(path: string): Promise<T> {
  const token = getToken();

  const res = await fetch(`${BASE}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      // This is how the JWT reaches your backend on every request
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
  });

  if (res.status === 401) {
    // Token expired or invalid — send user back to login
    window.location.href = '/login';
    throw new Error('Unauthorized');
  }

  if (!res.ok) throw new Error(`API error: ${res.status}`);
  return res.json();
}

// Typed API calls — one function per endpoint
export type Account = {
  id: string;
  institutionName: string;
  accountName: string;
  type: 'Checking' | 'Savings' | 'CreditCard' | 'AutoLoan' | 'StudentLoan';
  balance: number;
  lastSyncedAt: string;
};

export type NetWorthSummary = {
  assets: number;
  debts: number;
  netWorth: number;
};

export type CategorySummary = {
  category: string;
  totalSpent: number;
  count: number;
};

export type CashFlowSummary = {
  income: number;
  spending: number;
  net: number;
};

export const api = {
  getAccounts:          () => apiFetch<Account[]>('/api/accounts'),
  getNetWorth:          () => apiFetch<NetWorthSummary>('/api/accounts/net-worth'),
  getTransactions:      () => apiFetch<any[]>('/api/transactions'),
  getSpendingByCategory:() => apiFetch<CategorySummary[]>('/api/transactions/spending-by-category'),
  getCashFlow:          () => apiFetch<CashFlowSummary>('/api/transactions/cash-flow'),
};