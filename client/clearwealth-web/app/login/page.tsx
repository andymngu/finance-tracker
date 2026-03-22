// client/clearwealth-web/app/login/page.tsx
'use client';
import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { saveToken } from '@/lib/auth';

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

async function handleLogin(e: React.FormEvent) {
  e.preventDefault();
  
  try {
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5057'}/api/auth/login`,
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
      }
    );

    if (!res.ok) {
      alert('Invalid credentials');
      return;
    }

    const data = await res.json();
    saveToken(data.token);
    router.push('/dashboard');
  } catch (err) {
    alert('Login failed: ' + (err instanceof Error ? err.message : 'Unknown error'));
  }
}

  return (
    <main style={{
      minHeight: '100vh', display: 'flex', alignItems: 'center',
      justifyContent: 'center', background: '#0a0a0a', fontFamily: "'DM Sans', sans-serif"
    }}>
      <link href="https://fonts.googleapis.com/css2?family=DM+Sans:wght@300;400;500&family=Instrument+Serif:ital@0;1&display=swap" rel="stylesheet"/>
      <div style={{ width: 400, padding: '48px 40px', background: '#111', borderRadius: 16, border: '0.5px solid #222' }}>
        <p style={{ fontFamily: "'Instrument Serif', serif", fontSize: 28, color: '#fff', marginBottom: 8 }}>
          Welcome back
        </p>
        <p style={{ fontSize: 13, color: '#666', marginBottom: 32 }}>Sign in to your ClearWealth account</p>

        <form onSubmit={handleLogin}>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontSize: 11, color: '#555', letterSpacing: '0.08em', textTransform: 'uppercase', display: 'block', marginBottom: 6 }}>Email</label>
            <input value={email} onChange={e => setEmail(e.target.value)}
              type="email" placeholder="you@example.com"
              style={{ width: '100%', padding: '10px 14px', background: '#1a1a1a', border: '0.5px solid #333', borderRadius: 8, color: '#fff', fontSize: 14, outline: 'none' }}/>
          </div>
          <div style={{ marginBottom: 24 }}>
            <label style={{ fontSize: 11, color: '#555', letterSpacing: '0.08em', textTransform: 'uppercase', display: 'block', marginBottom: 6 }}>Password</label>
            <input value={password} onChange={e => setPassword(e.target.value)}
              type="password" placeholder="••••••••"
              style={{ width: '100%', padding: '10px 14px', background: '#1a1a1a', border: '0.5px solid #333', borderRadius: 8, color: '#fff', fontSize: 14, outline: 'none' }}/>
          </div>
          <button type="submit" style={{
            width: '100%', padding: '12px', background: '#1D9E75', border: 'none',
            borderRadius: 8, color: '#fff', fontSize: 14, fontWeight: 500, cursor: 'pointer'
          }}>Sign in</button>
        </form>

        <p style={{ fontSize: 11, color: '#333', textAlign: 'center', marginTop: 16 }}>
          Dev mode — any credentials accepted
        </p>
      </div>
    </main>
  );
}