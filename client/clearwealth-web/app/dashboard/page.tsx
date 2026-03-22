// client/clearwealth-web/app/dashboard/page.tsx
'use client';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { isLoggedIn, removeToken } from '@/lib/auth';
import { api, Account, NetWorthSummary, CategorySummary, CashFlowSummary } from '@/lib/api';

const fmt = (n: number) => new Intl.NumberFormat('en-CA', { style: 'currency', currency: 'CAD', maximumFractionDigits: 0 }).format(n);

const ACCOUNT_COLORS: Record<string, string> = {
  Checking: '#1D9E75', Savings: '#0F6E56',
  CreditCard: '#E24B4A', AutoLoan: '#D85A30', StudentLoan: '#BA7517',
};

export default function Dashboard() {
  const router = useRouter();
  const [accounts, setAccounts]   = useState<Account[]>([]);
  const [netWorth, setNetWorth]   = useState<NetWorthSummary | null>(null);
  const [spending, setSpending]   = useState<CategorySummary[]>([]);
  const [cashFlow, setCashFlow]   = useState<CashFlowSummary | null>(null);
  const [loading, setLoading]     = useState(true);

  useEffect(() => {
    if (!isLoggedIn()) { router.push('/login'); return; }

    // Fetch all data in parallel
    Promise.all([
      api.getAccounts(),
      api.getNetWorth(),
      api.getSpendingByCategory(),
      api.getCashFlow(),
    ]).then(([accs, nw, sp, cf]) => {
      setAccounts(accs);
      setNetWorth(nw);
      setSpending(sp);
      setCashFlow(cf);
      setLoading(false);
    }).catch(() => setLoading(false));
  }, [router]);

  const maxSpend = Math.max(...spending.map(s => s.totalSpent), 1);

  if (loading) return (
    <div style={{ minHeight: '100vh', background: '#0a0a0a', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <p style={{ color: '#444', fontFamily: "'DM Sans', sans-serif", fontSize: 14 }}>Loading your finances...</p>
    </div>
  );

  return (
    <main style={{ minHeight: '100vh', background: '#0a0a0a', color: '#fff', fontFamily: "'DM Sans', sans-serif" }}>
      <link href="https://fonts.googleapis.com/css2?family=DM+Sans:wght@300;400;500&family=Instrument+Serif:ital@0;1&display=swap" rel="stylesheet"/>

      {/* Top bar */}
      <div style={{ borderBottom: '0.5px solid #1a1a1a', padding: '0 32px', height: 56, display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
          <div style={{ width: 8, height: 8, borderRadius: '50%', background: '#1D9E75' }}/>
          <span style={{ fontSize: 15, fontWeight: 500, letterSpacing: '-0.3px' }}>ClearWealth</span>
        </div>
        <button onClick={() => { removeToken(); router.push('/login'); }}
          style={{ fontSize: 12, color: '#555', background: 'none', border: 'none', cursor: 'pointer' }}>
          Sign out
        </button>
      </div>

      <div style={{ maxWidth: 1000, margin: '0 auto', padding: '32px 24px' }}>

        {/* Header */}
        <div style={{ marginBottom: 32 }}>
          <p style={{ fontFamily: "'Instrument Serif', serif", fontSize: 32, fontWeight: 400, letterSpacing: '-0.5px', marginBottom: 4 }}>
            Good morning
          </p>
          <p style={{ fontSize: 13, color: '#555' }}>
            {new Date().toLocaleDateString('en-CA', { weekday: 'long', month: 'long', day: 'numeric' })}
          </p>
        </div>

        {/* Net worth + cash flow row */}
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr 1fr', gap: 12, marginBottom: 24 }}>
          {[
            { label: 'Net worth',    value: fmt(netWorth?.netWorth ?? 0),  sub: 'Total position',      color: (netWorth?.netWorth ?? 0) >= 0 ? '#1D9E75' : '#E24B4A' },
            { label: 'Total assets', value: fmt(netWorth?.assets ?? 0),    sub: 'Banks & savings',     color: '#1D9E75' },
            { label: 'Total debt',   value: fmt(netWorth?.debts ?? 0),     sub: 'Loans & cards',       color: '#E24B4A' },
            { label: 'Monthly net',  value: fmt(cashFlow?.net ?? 0),       sub: 'Income minus spend',  color: (cashFlow?.net ?? 0) >= 0 ? '#1D9E75' : '#E24B4A' },
          ].map(card => (
            <div key={card.label} style={{ background: '#111', border: '0.5px solid #1e1e1e', borderRadius: 12, padding: '16px 18px' }}>
              <p style={{ fontSize: 11, color: '#555', letterSpacing: '0.06em', textTransform: 'uppercase', marginBottom: 8 }}>{card.label}</p>
              <p style={{ fontSize: 22, fontWeight: 500, color: card.color, letterSpacing: '-0.5px', marginBottom: 2 }}>{card.value}</p>
              <p style={{ fontSize: 11, color: '#444' }}>{card.sub}</p>
            </div>
          ))}
        </div>

        <div style={{ display: 'grid', gridTemplateColumns: '1fr 360px', gap: 16 }}>

          {/* Accounts list */}
          <div>
            <p style={{ fontSize: 11, color: '#444', letterSpacing: '0.08em', textTransform: 'uppercase', marginBottom: 12 }}>Linked accounts</p>
            <div style={{ background: '#111', border: '0.5px solid #1e1e1e', borderRadius: 12, overflow: 'hidden' }}>
              {accounts.map((acc, i) => (
                <div key={acc.id} style={{
                  display: 'flex', alignItems: 'center', justifyContent: 'space-between',
                  padding: '14px 18px',
                  borderBottom: i < accounts.length - 1 ? '0.5px solid #1a1a1a' : 'none',
                }}>
                  <div style={{ display: 'flex', alignItems: 'center', gap: 12 }}>
                    <div style={{
                      width: 36, height: 36, borderRadius: 8,
                      background: `${ACCOUNT_COLORS[acc.type]}22`,
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      fontSize: 13, fontWeight: 600, color: ACCOUNT_COLORS[acc.type]
                    }}>
                      {acc.institutionName[0]}
                    </div>
                    <div>
                      <p style={{ fontSize: 13, fontWeight: 500, marginBottom: 2 }}>{acc.institutionName}</p>
                      <p style={{ fontSize: 11, color: '#555' }}>{acc.accountName} · {acc.type}</p>
                    </div>
                  </div>
                  <p style={{ fontSize: 14, fontWeight: 500, color: acc.balance >= 0 ? '#1D9E75' : '#E24B4A' }}>
                    {fmt(acc.balance)}
                  </p>
                </div>
              ))}
            </div>

            {/* Cash flow breakdown */}
            {cashFlow && (
              <div style={{ marginTop: 16, background: '#111', border: '0.5px solid #1e1e1e', borderRadius: 12, padding: '18px 20px' }}>
                <p style={{ fontSize: 11, color: '#444', letterSpacing: '0.08em', textTransform: 'uppercase', marginBottom: 14 }}>This month</p>
                {[
                  { label: 'Income',   value: cashFlow.income,   color: '#1D9E75' },
                  { label: 'Spending', value: cashFlow.spending, color: '#E24B4A' },
                  { label: 'Net',      value: cashFlow.net,      color: cashFlow.net >= 0 ? '#1D9E75' : '#E24B4A' },
                ].map(row => (
                  <div key={row.label} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 10 }}>
                    <p style={{ fontSize: 13, color: '#666' }}>{row.label}</p>
                    <p style={{ fontSize: 13, fontWeight: 500, color: row.color }}>{fmt(row.value)}</p>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Spending by category */}
          <div>
            <p style={{ fontSize: 11, color: '#444', letterSpacing: '0.08em', textTransform: 'uppercase', marginBottom: 12 }}>Spending breakdown</p>
            <div style={{ background: '#111', border: '0.5px solid #1e1e1e', borderRadius: 12, padding: '18px 20px' }}>
              {spending.length === 0
                ? <p style={{ fontSize: 13, color: '#444' }}>No spending data yet</p>
                : spending.map(cat => (
                  <div key={cat.category} style={{ marginBottom: 16 }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 6 }}>
                      <p style={{ fontSize: 12, color: '#888' }}>{cat.category}</p>
                      <p style={{ fontSize: 12, fontWeight: 500 }}>{fmt(cat.totalSpent)}</p>
                    </div>
                    <div style={{ height: 4, background: '#1a1a1a', borderRadius: 2, overflow: 'hidden' }}>
                      <div style={{
                        height: '100%', borderRadius: 2, background: '#1D9E75',
                        width: `${(cat.totalSpent / maxSpend) * 100}%`,
                        transition: 'width 0.6s ease'
                      }}/>
                    </div>
                  </div>
                ))
              }
            </div>
          </div>
        </div>
      </div>
    </main>
  );
}