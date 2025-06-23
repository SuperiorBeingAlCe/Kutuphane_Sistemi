import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

/**
 * Kullanıcı ekleme formu bileşeni.
 * Kullanıcıdan ad soyad, email ve telefon bilgilerini alır ve API'ye gönderir.
 */
const AddUser: React.FC = () => {
    // Kullanıcı bilgileri için state'ler
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');

    // Hata ve başarı durumları için state'ler
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);

    const navigate = useNavigate();

    /**
     * Form gönderildiğinde çalışır.
     * Kullanıcı bilgilerini API'ye gönderir.
     * @param e Form submit olayı
     */
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            // JWT token'ı localStorage'dan al
            const token = localStorage.getItem('token');

            // Kartın son kullanma tarihini 1 yıl sonrası olarak ayarla
            const expireDate = new Date();
            expireDate.setFullYear(expireDate.getFullYear() + 1);
            const expireAt = expireDate.toISOString();

            /**
             * API'ye gönderilecek payload.
             * @type {{ fullName: string, email: string, phoneNumber: string, cardNumber: string, expireAt: string | null }}
             */
            const payload = {
                fullName,
                email,
                phoneNumber,
                cardNumber: '',
                expireAt: expireAt || null
            };

            // API isteği gönder
            const response = await axios.post(
                'https://localhost:7195/api/User/add',
                payload,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );

            console.log('Kullanıcı oluşturuldu:', response.data);
            setSuccess(true);
            setError(null);

            // Başarıyla eklenince yönlendir
            setTimeout(() => {
                navigate('/Users');
            }, 1500);
        } catch (err) {
            setSuccess(false);
            if (axios.isAxiosError(err) && err.response?.data) {
                const backendError = (err.response.data as { detail?: string }).detail;
                setError(backendError || 'Kullanıcı eklenemedi.');
            } else {
                setError('Sunucuya bağlanılamadı.');
            }
        }
    };

    return (
        <div style={wrapperStyle}>
            <form onSubmit={handleSubmit} style={formStyle}>
                <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Kullanıcı Ekle</h2>

                <div style={fieldStyle}>
                    <label htmlFor="fullName">Ad Soyad *</label>
                    <input
                        id="fullName"
                        type="text"
                        value={fullName}
                        onChange={e => setFullName(e.target.value)}
                        required
                        placeholder="Ad Soyad"
                        style={inputStyle}
                    />
                </div>

                <div style={fieldStyle}>
                    <label htmlFor="email">Email *</label>
                    <input
                        id="email"
                        type="email"
                        value={email}
                        onChange={e => setEmail(e.target.value)}
                        required
                        placeholder="Email"
                        style={inputStyle}
                    />
                </div>

                <div style={fieldStyle}>
                    <label htmlFor="phoneNumber">Telefon *</label>
                    <input
                        id="phoneNumber"
                        type="tel"
                        value={phoneNumber}
                        onChange={e => setPhoneNumber(e.target.value)}
                        required
                        placeholder="Telefon"
                        style={inputStyle}
                    />
                </div>

                {error && <p style={{ color: 'red', marginBottom: '12px' }}>{error}</p>}
                {success && <p style={{ color: 'lightgreen', marginBottom: '12px' }}>Kullanıcı başarıyla eklendi!</p>}

                <button type="submit" style={btnStyle}>
                    Kaydet
                </button>
            </form>
        </div>
    );
};

/**
 * Sayfa dış sarmalayıcı stil ayarları
 */
const wrapperStyle: React.CSSProperties = {
    width: '100vw',
    height: '100vh',
    backgroundColor: '#121212',
    color: '#fff',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    padding: '20px',
    boxSizing: 'border-box',
};

/**
 * Form stil ayarları
 */
const formStyle: React.CSSProperties = {
    backgroundColor: '#1e1e1e',
    padding: '20px',
    borderRadius: '8px',
    boxShadow: '0 0 15px rgba(0,0,0,0.7)',
    width: '320px',
};

/**
 * Form alanı stil ayarları
 */
const fieldStyle: React.CSSProperties = {
    marginBottom: '16px',
    display: 'flex',
    flexDirection: 'column',
};

/**
 * Girdi kutusu stil ayarları
 */
const inputStyle: React.CSSProperties = {
    width: '100%',
    padding: '8px',
    borderRadius: '4px',
    border: '1px solid #555',
    backgroundColor: '#2a2a2a',
    color: '#fff',
    boxSizing: 'border-box',
    marginTop: '4px',
};

/**
 * Buton stil ayarları
 */
const btnStyle: React.CSSProperties = {
    width: '100%',
    padding: '10px',
    backgroundColor: '#0b74de',
    border: 'none',
    color: '#fff',
    fontWeight: 'bold',
    borderRadius: '5px',
    cursor: 'pointer',
};

export default AddUser;