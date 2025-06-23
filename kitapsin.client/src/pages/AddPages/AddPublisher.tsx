import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const AddPublisher: React.FC = () => {
  const [name, setName] = useState('');
  const [address, setAddress] = useState('');
  const [phone, setPhone] = useState('');
  const [email, setEmail] = useState('');

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const token = localStorage.getItem('token');
      const response = await axios.post(
        'https://localhost:7195/api/Publisher',
        {
          name,
          address,
          phone,
          email,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        }
      );

      console.log('Yayınevi oluşturuldu:', response.data);
      setSuccess(true);
      setError(null);

      setTimeout(() => {
        navigate('/publishers'); 
      }, 1500);
    } catch (err) {
      setSuccess(false);
      if (axios.isAxiosError(err) && err.response && err.response.data) {
        setError((err.response.data as { message?: string }).message || 'Yayınevi eklenemedi.');
      } else {
        setError('Sunucuya bağlanılamadı.');
      }
    }
  };

  return (
    <div
      style={{
        width: '100vw',
        height: '100vh',
        backgroundColor: '#121212',
        color: '#fff',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        padding: '20px',
        boxSizing: 'border-box',
      }}
    >
      <form
        onSubmit={handleSubmit}
        style={{
          backgroundColor: '#1e1e1e',
          padding: '20px',
          borderRadius: '8px',
          boxShadow: '0 0 15px rgba(0,0,0,0.7)',
          width: '320px',
        }}
      >
        <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Yayınevi Ekle</h2>

        <div style={{ marginBottom: '16px' }}>
          <label htmlFor="name" style={{ display: 'block', marginBottom: '8px' }}>
            İsim *
          </label>
          <input
            type="text"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
            placeholder="Yayınevi adı"
            style={inputStyle}
          />
        </div>

        <div style={{ marginBottom: '16px' }}>
          <label htmlFor="address" style={{ display: 'block', marginBottom: '8px' }}>
            Adres
          </label>
          <input
            type="text"
            id="address"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
            placeholder="Adres"
            style={inputStyle}
          />
        </div>

        <div style={{ marginBottom: '16px' }}>
          <label htmlFor="phone" style={{ display: 'block', marginBottom: '8px' }}>
            Telefon
          </label>
          <input
            type="tel"
            id="phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
            placeholder="Telefon"
            style={inputStyle}
          />
        </div>

        <div style={{ marginBottom: '16px' }}>
          <label htmlFor="email" style={{ display: 'block', marginBottom: '8px' }}>
            Email
          </label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Email"
            style={inputStyle}
          />
        </div>

        {error && <p style={{ color: 'red', marginBottom: '12px' }}>{error}</p>}
        {success && <p style={{ color: 'lightgreen', marginBottom: '12px' }}>Yayınevi başarıyla eklendi!</p>}

        <button type="submit" style={btnStyle}>
          Kaydet
        </button>
      </form>
    </div>
  );
};

const inputStyle: React.CSSProperties = {
  width: '100%',
  padding: '8px',
  borderRadius: '4px',
  border: '1px solid #555',
  backgroundColor: '#2a2a2a',
  color: '#fff',
  boxSizing: 'border-box',
};

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

export default AddPublisher;