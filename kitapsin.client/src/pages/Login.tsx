import React, { useState, useEffect } from 'react';
import { useAuth } from '../components/AuthContext';
import { useNavigate } from 'react-router-dom';

const Login: React.FC = () => {
    const { isAuthenticated, isLoading, login } = useAuth();
    const navigate = useNavigate();

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [regUsername, setRegUsername] = useState('');
    const [regEmail, setRegEmail] = useState('');
    const [regPassword, setRegPassword] = useState('');
    const [isRegisterOpen, setIsRegisterOpen] = useState(false);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (!isLoading && isAuthenticated) {
            navigate('/', { replace: true });
        }
    }, [isAuthenticated, isLoading, navigate]);

    const handleLoginSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        try {
            // 1. Login isteği gönder, JSON olarak token al
            const res = await fetch('https://localhost:7195/api/Auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password }),
            });
            if (!res.ok) {
                const txt = await res.text();
                throw new Error(txt || 'Giriş başarısız.');
            }
            const data = await res.json();
            const token = data.token;  // Burada token JSON içinden çıkarılıyor

            // 2. Token'ı kaydet
            login(token);
            localStorage.setItem('token', token);

            // 3. Token ile admin bilgilerini username üzerinden çek
            const adminRes = await fetch(`https://localhost:7195/api/admin/username/${username}`, {
                headers: { Authorization: `Bearer ${token}` },
            });
            if (!adminRes.ok) throw new Error('Admin bilgisi alınamadı.');
            const adminData = await adminRes.json();

            // 4. Admin ID'yi localStorage'a kaydet
            localStorage.setItem('adminId', adminData.id.toString());

            // 5. Ana sayfaya yönlendir
            navigate('/', { replace: true });
        } catch (err) {
            alert(err instanceof Error ? err.message : 'Bir hata oluştu.');
        } finally {
            setLoading(false);
        }
    };

    // handleRegisterSubmit ve JSX kısmı değişmediği için aynen kaldı

    return (
        <div style={{
            display: 'flex',
            width: 950,
            height: 400,
            margin: '100px auto',
            border: '1px solid #ccc',
            borderRadius: 8,
            overflow: 'hidden',
            boxShadow: '0 0 10px rgba(0,0,0,0.1)',
            backgroundColor: 'black',
            position: 'relative'
        }}>
            {/* SOL BİLGİ PANELİ */}
            <div style={{
                flex: 1,
                backgroundColor: '#222',
                color: 'white',
                padding: 30,
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
            }}>
                <h3>Örnek Hesap</h3>
                <p><strong>admin</strong> / <strong>1234</strong></p>
                <p>Hemen deneyin!</p>
            </div>

            {/* SAĞ PANEL: GİRİŞ VE KAYIT */}
            <div style={{ flex: 1, position: 'relative', overflow: 'hidden' }}>
                {/* Giriş Formu */}
                <form
                    onSubmit={handleLoginSubmit}
                    style={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        width: '100%',
                        height: '100%',
                        padding: 20,
                        boxSizing: 'border-box',
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'center',
                        backgroundColor: 'black',
                        color: 'white',
                        transition: 'transform 0.6s ease-in-out',
                        transform: isRegisterOpen ? 'translateX(-100%)' : 'translateX(0)',
                    }}
                >
                    <h2 style={{ marginBottom: 20 }}>Giriş Yap</h2>
                    <input
                        placeholder="Kullanıcı Adı"
                        value={username}
                        onChange={e => setUsername(e.target.value)}
                        style={{ marginBottom: 15, padding: 8, borderRadius: 4, border: 'none', outline: 'none' }}
                        disabled={loading}
                        required
                    />
                    <input
                        type="password"
                        placeholder="Şifre"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        style={{ marginBottom: 15, padding: 8, borderRadius: 4, border: 'none', outline: 'none' }}
                        disabled={loading}
                        required
                    />
                    <button
                        type="submit"
                        style={{
                            padding: 10, borderRadius: 4, border: 'none',
                            backgroundColor: '#1e90ff', color: 'white',
                            cursor: loading ? 'not-allowed' : 'pointer',
                            fontWeight: 'bold'
                        }}
                        disabled={loading}
                    >
                        {loading ? 'Bekleyiniz...' : 'Giriş Yap'}
                    </button>
                    <p style={{ marginTop: 20 }}>
                        Hesabınız yok mu?{' '}
                        <button
                            type="button"
                            onClick={() => setIsRegisterOpen(true)}
                            style={{ color: '#1e90ff', background: 'none', border: 'none', cursor: 'pointer', padding: 0 }}
                            disabled={loading}
                        >
                            Kayıt Ol
                        </button>
                    </p>
                </form>

                {/* Kayıt Formu */}
                <form
                    onSubmit={async e => {
                        e.preventDefault();
                        setLoading(true);
                        try {
                            const res = await fetch('https://localhost:7195/api/Admin', {
                                method: 'POST',
                                headers: { 'Content-Type': 'application/json' },
                                body: JSON.stringify({
                                    username: regUsername,
                                    email: regEmail,
                                    password: regPassword,
                                }),
                            });
                            if (!res.ok) {
                                const txt = await res.text();
                                throw new Error(txt || 'Kayıt işlemi başarısız.');
                            }
                            const created = await res.json();
                            alert(`Kayıt başarılı! ID: ${created.id}`);
                            setIsRegisterOpen(false);
                        } catch (err) {
                            alert(err instanceof Error ? err.message : 'Bir hata oluştu.');
                        } finally {
                            setLoading(false);
                        }
                    }}
                    style={{
                        position: 'absolute',
                        top: 0,
                        right: 0,
                        width: '100%',
                        height: '100%',
                        padding: 20,
                        boxSizing: 'border-box',
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'center',
                        backgroundColor: '#f9f9f9',
                        color: 'black',
                        transition: 'transform 0.6s ease-in-out',
                        transform: isRegisterOpen ? 'translateX(0)' : 'translateX(100%)',
                    }}
                >
                    <h2 style={{ marginBottom: 20 }}>Kayıt Ol</h2>
                    <input
                        placeholder="Kullanıcı Adı"
                        value={regUsername}
                        onChange={e => setRegUsername(e.target.value)}
                        style={{ marginBottom: 15, padding: 8, borderRadius: 4, border: '1px solid #ccc' }}
                        disabled={loading}
                        required
                    />
                    <input
                        type="email"
                        placeholder="Email"
                        value={regEmail}
                        onChange={e => setRegEmail(e.target.value)}
                        style={{ marginBottom: 15, padding: 8, borderRadius: 4, border: '1px solid #ccc' }}
                        disabled={loading}
                        required
                    />
                    <input
                        type="password"
                        placeholder="Şifre"
                        value={regPassword}
                        onChange={e => setRegPassword(e.target.value)}
                        style={{ marginBottom: 15, padding: 8, borderRadius: 4, border: '1px solid #ccc' }}
                        disabled={loading}
                        required
                    />
                    <button
                        type="submit"
                        style={{
                            padding: 10, borderRadius: 4, border: 'none',
                            backgroundColor: '#1e90ff', color: 'white',
                            cursor: loading ? 'not-allowed' : 'pointer',
                            fontWeight: 'bold'
                        }}
                        disabled={loading}
                    >
                        {loading ? 'Bekleyiniz...' : 'Kayıt Ol'}
                    </button>
                    <p style={{ marginTop: 20 }}>
                        Zaten hesabınız var mı?{' '}
                        <button
                            type="button"
                            onClick={() => setIsRegisterOpen(false)}
                            style={{ color: '#1e90ff', background: 'none', border: 'none', cursor: 'pointer', padding: 0 }}
                            disabled={loading}
                        >
                            Giriş Yap
                        </button>
                    </p>
                </form>
            </div>
        </div>
    );
};

export default Login;